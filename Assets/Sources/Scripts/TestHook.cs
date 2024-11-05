using System;
using Sources.Scripts.Items;
using Sources.Scripts.UI.ThrowToolsUI;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Zenject;

namespace Sources.Scripts
{
    public class TestHook : MonoBehaviour
    {
        [Inject] private ThrowToolsPresenter _throwToolsPresenter;
        [Inject] private HookModel _hookModel;
        [Inject] private TakedItemsData _takedItemsData;
        private void Update()
        {
            if (Input.GetKey(KeyCode.Alpha1))
            {
                _throwToolsPresenter.GetModel(_hookModel);
            }

            if (Input.GetKey(KeyCode.F))
            {
                _takedItemsData.TakeAddedItems();
            }
        }
    }
}