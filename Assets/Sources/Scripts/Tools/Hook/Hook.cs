using System.Collections.Generic;
using Sources.Scripts.Hook.InputControllers;
using Sources.Scripts.Items;
using Sources.Scripts.Tools.Hook;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody))]
public class Hook : Tool
{
    [SerializeField] private Transform _itemAttachedPoint;
    private bool _onWater;
    private int _waterLayer;
    private InputController _inputController;
    private Transform _pointOfHit;
    private TakedItemsData _takedItemsData;
    private List<GameObject> _capturedItems = new();
    public Transform PointOfHit => _pointOfHit;
    
    [Inject]
    public void Construct(InputController inputController, TakedItemsData takedItemsData)
    {
        _inputController = inputController;
        _takedItemsData = takedItemsData;
    }

    public bool OnWater => _onWater;

    public void ReturnHook()
    {
        foreach (var item in _capturedItems)
        {
            Destroy(item);
        }
        _takedItemsData.AddTakedItems();
        _capturedItems.Clear();
    }
    
    private void Start()
    {
        _waterLayer = LayerMask.NameToLayer("Water");
    }

    private void OnCollisionEnter(Collision other)
    {
        CheckWater(other);
        SettingCapturedItem(other);
    }

    private void OnCollisionExit(Collision other)
    {
        CheckWater(other);
    }

    private void SettingCapturedItem(Collision other)
    {
        if (other.gameObject.TryGetComponent<IDruggable>(out IDruggable druggableItem))
        {
            Transform drugItem = druggableItem.GetTransform();
            Rigidbody rb = drugItem.GetComponent<Rigidbody>();
            _capturedItems.Add(other.gameObject);
            _takedItemsData.AddCaptureItem(drugItem.GetComponent<Item>());
            
            drugItem.SetParent(gameObject.transform);
            rb.isKinematic = true;
            drugItem.position = _itemAttachedPoint.position;
            DisableCollision(drugItem.gameObject);
        }
    }
    
    private void DisableCollision(GameObject item)
    {
        Collider[] colliders = item.GetComponentsInChildren<Collider>();
        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }
    }
    
    private void CheckWater(Collision other)
    {
        if (other.gameObject.layer == _waterLayer)
        {
            _onWater = !_onWater;
            _pointOfHit = other.transform;
            _inputController.SwitchInput();
        }
    }
}
