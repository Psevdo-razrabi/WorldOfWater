using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using Game.Services;

namespace Game.MVVM.Menu
{
    public class CreateWorldViewModel : ViewModel
    {
        private ViewsService _viewsService;

        public string WorldName { get; set; }

        public void Init(IBindable playButton)
        {
            Binder.CreateButtonTrigger<Click>(playButton, OnCreateClicked);
        }

        [Inject]
        private void Construct(ViewsService viewsService)
        {
            _viewsService = viewsService;
        }

        private async void OnCreateClicked()
        {
            /*string lobbyName = WorldName;
            int maxPlayers = 8;
            CreateLobbyOptions options = new CreateLobbyOptions();
            options.IsPrivate = true;

            options.Data = new Dictionary<string, DataObject>()
            {
                {
                    "Test1", new DataObject(
                        visibility: DataObject.VisibilityOptions.Public,
                        value: "Test1")
                },
            };

            options.Data = new Dictionary<string, DataObject>()
            {
                {
                    "Test2", new DataObject(
                        visibility: DataObject.VisibilityOptions.Public,
                        value: "Test2",
                        index: DataObject.IndexOptions.S1)
                },
            };

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);*/

            _viewsService.Close();
            _viewsService.Open(ViewId.MainMenu);
        }
    }
}
