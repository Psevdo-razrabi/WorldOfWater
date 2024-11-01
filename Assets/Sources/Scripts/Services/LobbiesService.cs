using System;
using Cysharp.Threading.Tasks;
using Game.MVVM;
using Game.MVVM.Menu;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using VContainer.Unity;

namespace Game.Services
{
    public class LobbiesService
    {
        private ViewsService _viewsService;
        public string LobbyCode { get; private set; }
        private Lobby _lobby;
        
        public LobbiesService(ViewsService viewsService)
        {
            _viewsService = viewsService;
        }

        public async UniTask CreateLobby(string worldName = "NewWorld")
        {
            try
            {
                int maxPlayers = 8;
                CreateLobbyOptions options = new CreateLobbyOptions();
                options.IsPrivate = true;

                _lobby = await LobbyService.Instance.CreateLobbyAsync(worldName, maxPlayers, options);
                LobbyCode = _lobby.LobbyCode;

                await SubscribeToLobbyEvents(_lobby);
                
                Debug.Log($"Lobby created: {_lobby.Name}");
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
                    return;
                }
                await LobbyService.Instance.RemovePlayerAsync(_lobby.Id, playerId);
                Debug.Log($"Lobby leaved {_lobby.Id}");
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
        }
    }
}