using System;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Game.DI;
using UnityEngine;
using VContainer;
using Game.Services;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine.SceneManagement;

namespace Game.MVVM.Menu
{
    public class CreateWorldViewModel : ViewModel
    {
        private ViewsService _viewsService;
        private LobbiesService _lobbiesService;
        private ScenesService _scenesService;

        public string WorldName { get; set; }

        public void Init(IBindable playButton)
        {
            Binder.CreateButtonTrigger<Click>(playButton, OnCreateClicked);
        }

        [Inject]
        private void Construct(ViewsService viewsService, LobbiesService lobbiesService,
            ScenesService scenesService)
        {
            _viewsService = viewsService;
            _lobbiesService = lobbiesService;
            _scenesService = scenesService;
        }

        private async void OnCreateClicked()
        {
            await _lobbiesService.CreateLobby();
        }
    }
}
