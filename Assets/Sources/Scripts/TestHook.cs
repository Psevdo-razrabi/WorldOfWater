using System;
using Sources.Scripts.Items;
using Sources.Scripts.UI.ThrowToolsUI;
using Sources.Scripts.UI.ThrowToolsUI.SpearModel;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Zenject;

namespace Sources.Scripts
{
    public class TestHook : MonoBehaviour
    {
        [SerializeField] private GameObject _hook;
        [SerializeField] private GameObject _spear;
        [SerializeField] private GameObject _rod;
        
        [Inject] private ThrowToolsPresenter _throwToolsPresenter;
        [Inject] private HookModel _hookModel;
        [Inject] private SpearModel _spearModel;
        [Inject] private TakedItemsData _takedItemsData;
        private void Update()
        {
            if (Input.GetKey(KeyCode.Alpha1))
            {
                _hook.SetActive(true);
                _spear.SetActive(false);
                _throwToolsPresenter.GetModel(_hookModel);
            }
            if (Input.GetKey(KeyCode.Alpha2))
            {
                _spear.SetActive(true);
                _hook.SetActive(false);
                _throwToolsPresenter.GetModel(_spearModel);
            }
            if (Input.GetKey(KeyCode.F))
            {
                _takedItemsData.TakeAddedItems();
            }
        }
    }
}