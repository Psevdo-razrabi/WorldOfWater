using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Sources.Scripts.DI
{
    public class TestMonobeh : MonoBehaviour,IInitializable
    {
        [SerializeField] private GameLifetimeScope _gameLifetimeScope;
        private Test _test;

        private void Awake()
        {
            _test = _gameLifetimeScope.Container.Resolve<Test>();
        }

        private void Start()
        {
            Debug.Log(_test.a);
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}