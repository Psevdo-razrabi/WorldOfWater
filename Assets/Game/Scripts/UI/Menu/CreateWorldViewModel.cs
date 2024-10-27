using System;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using Game.Services;
using Unity.Services.Authentication;
using Unity.Services.Core;

namespace Game.MVVM.Menu
{
    public class CreateWorldViewModel : ViewModel
    {
        private ViewsService _viewsService;
        private LobbiesService _lobbiesService;

        public string WorldName { get; set; }

        public void Init(IBindable playButton)
        {
            Binder.CreateButtonTrigger<Click>(playButton, OnCreateClicked);
        }

        [Inject]
        private void Construct(ViewsService viewsService, LobbiesService lobbiesService)
        {
            _viewsService = viewsService;
            _lobbiesService = lobbiesService;
        }

        private async void OnCreateClicked()
        {
            await _lobbiesService.CreateLobby(WorldName);

            _viewsService.Close();
            _viewsService.Close();
            _viewsService.Open(ViewId.Lobby);
        }
    }
}
