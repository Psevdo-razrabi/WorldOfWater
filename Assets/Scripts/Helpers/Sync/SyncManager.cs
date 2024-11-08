using System;
using System.Collections.Generic;
using Loader;
using SceneManagment;
using Sync;
using UniRx;
using UnityEngine;
using Zenject;

namespace Helpers.Sync
{
    public class SyncManager : MonoBehaviour
    {
        private SyncData _syncManager = new();
        private Dictionary<TypeSync, Action> _loadActions;
        private SceneLoader _sceneLoader;

        private void Start()
        {
            ProjectActions.OnTypeLoad.Subscribe(AllReadyLoad).AddTo(this);
            
            _loadActions = new Dictionary<TypeSync, Action>
            {
                { TypeSync.Config, () => _syncManager.ConfigLoad = true },
                { TypeSync.Prefab, () => _syncManager.PrefabLoad = true }
            };
        }

        [Inject]
        public void Construct(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        private async void AllReadyLoad(TypeSync sync)
        {
            if(_syncManager.StartLoad) return;

            if (_loadActions.TryGetValue(sync, out Action action))
            {
                action.Invoke();
            }
            else
            {
                throw new KeyNotFoundException();
            }

            if (_syncManager.IsAllLoaded)
            {
                _syncManager.StartLoad = true;
                await _sceneLoader.LoadScene(TypeScene.MenuScene);
            }
        }
    }
}