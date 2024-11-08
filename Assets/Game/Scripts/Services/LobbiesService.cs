using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Game.MVVM;
using Game.MVVM.Menu;
using R3;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using VContainer.Unity;

namespace Game.Services
{
    public class LobbiesService
    {
        private ViewsService _viewsService;

        public string JoinCode { get; private set; }

        public ReactiveCommand Connected { get; } = new();
        
        public LobbiesService(ViewsService viewsService)
        {
            _viewsService = viewsService;
        }

        public async UniTask CreateLobby()
        {
            NetworkManager.Singleton.NetworkConfig.ConnectionApproval = true;
            NetworkManager.Singleton.ConnectionApprovalCallback = ConnectionApproval;
            var allocation = await RelayService.Instance.CreateAllocationAsync(4);
            JoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            var endpoint = allocation.ServerEndpoints.First(c => c.ConnectionType == "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().
                SetHostRelayData(endpoint.Host, (ushort)endpoint.Port, allocation.AllocationIdBytes,
                    allocation.Key, allocation.ConnectionData, true);

            NetworkManager.Singleton.StartHost();
            
            Connected.Execute();
        }

        public async UniTask JoinLobby(string joinCode)
        {
            var allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            var endpoint = allocation.ServerEndpoints.First(c => c.ConnectionType == "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().
                SetClientRelayData(endpoint.Host, (ushort)endpoint.Port, allocation.AllocationIdBytes,
                    allocation.Key, allocation.ConnectionData, allocation.HostConnectionData, true);

            NetworkManager.Singleton.StartClient();
            
            Connected.Execute();
        }

        public async UniTask LeaveLobby()
        {
            NetworkManager.Singleton.Shutdown();
        }
        
        private void ConnectionApproval(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
        {
            response.Approved = true;
            response.CreatePlayerObject = true;
            response.Pending = false; 
        }

        /*public async UniTask CreateLobby(string worldName = "NewWorld")
        {
            try
            {
                int maxPlayers = 8;
                CreateLobbyOptions options = new()
                {
                    IsPrivate = true
                };

                _lobby = await LobbyService.Instance.CreateLobbyAsync(worldName, maxPlayers, options);
                LobbyCode = _lobby.LobbyCode;

                await SubscribeToLobbyEvents(_lobby);

                //NetworkManager.Singleton;
                //NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(NetworkManager.Singleton.GetComponent<UnityTransport>().);
                if(NetworkManager.Singleton.StartHost())
                {
                    Debug.Log($"Host started!");
                }
                else
                {
                    Debug.Log($"Host failed!");
                }
                
                Debug.Log($"Lobby created: {_lobby.Name}");
                Debug.Log(_lobby.Players.Count);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private async UniTask SubscribeToLobbyEvents(Lobby lobby)
        {
            var callbacks = new LobbyEventCallbacks();
            callbacks.KickedFromLobby += OnKickedFromLobby;
            try {
                await LobbyService.Instance.SubscribeToLobbyEventsAsync(lobby.Id, callbacks);
            }
            catch (LobbyServiceException ex)
            {
                switch (ex.Reason) {
                    case LobbyExceptionReason.AlreadySubscribedToLobby: 
                        Debug.LogWarning($"Already subscribed to lobby[{lobby.Id}]. We did not need to try and subscribe again. Exception Message: {ex.Message}"); break;
                    case LobbyExceptionReason.SubscriptionToLobbyLostWhileBusy: 
                        Debug.LogError($"Subscription to lobby events was lost while it was busy trying to subscribe. Exception Message: {ex.Message}"); throw;
                    case LobbyExceptionReason.LobbyEventServiceConnectionError: 
                        Debug.LogError($"Failed to connect to lobby events. Exception Message: {ex.Message}"); throw;
                    default: throw;
                }
            }
        }

        public async UniTask<bool> TryJoinLobby(string code = "NewCode")
        {
            try
            {
                _lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(code);
                await SubscribeToLobbyEvents(_lobby);
                Debug.Log($"Joined lobby: {_lobby.Name}");
                
                if(NetworkManager.Singleton.StartClient())
                {
                    Debug.Log($"Client started!");
                }
                else
                {
                    Debug.Log($"Client failed!");
                }
                
                return true;
            }
            catch (LobbyServiceException e)
            {
                Debug.Log("Not found lobby!");
                Debug.Log(e);
                return false;
            }
        }

        public async UniTask LeaveLobby()
        {
            try
            {
                var playerId = AuthenticationService.Instance.PlayerId;
                if (_lobby.HostId == AuthenticationService.Instance.PlayerId)
                {
                    await LobbyService.Instance.DeleteLobbyAsync(_lobby.Id);
                    Debug.Log($"Lobby deleted {_lobby.Id}");
                    NetworkManager.Singleton.Shutdown();
                    return;
                }
                await LobbyService.Instance.RemovePlayerAsync(_lobby.Id, playerId);
                Debug.Log($"Lobby leaved {_lobby.Id}");
                NetworkManager.Singleton.Shutdown();
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
        
        private void OnKickedFromLobby()
        {
            _viewsService.Close();
            _viewsService.Open<CreateWorldView>();
            _viewsService.Open<JoinWorldView>();
            Debug.Log("Lobby closed!");
        }*/
    }
}