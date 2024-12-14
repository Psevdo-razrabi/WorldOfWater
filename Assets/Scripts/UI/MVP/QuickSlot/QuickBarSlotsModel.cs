using System;
using Helpers;
using Inventory;
using Loader;
using QuickSlot.Command;
using QuickSlot.Db;
using R3;
using Sync;
using UnityEngine;

namespace QuickSlot
{
    public class QuickBarSlotsModel : StorageModel, IDisposable
    {
        public DataViewQuickBar DataViewQuickBar { get; private set; }
        public QuickSlotsParameters Parameters { get; private set; }
        private event Action<bool, int> OnSlotSelected;
        private ItemCommandInvoker _itemCommandInvoker = new ();

        private void SetCommand(bool isActive, int index)
        {
            //подписка из presenter и отправка на view
            //получение itemа
            //и что с этим item делать в команде
            //просто предыдущий слот буду делать в !isActive = isActive
            Item item = default;

            if (TryGet(index, out item))
            {
                _itemCommandInvoker.ExecuteCommand(item.ItemDescription.ItemCommand);
            }
            else
            {
                Debug.Log($"Выбранный слот {index + 1} пуст");
            }
        }

        public void Dispose()
        {
            _onModelChange?.Dispose();
            ItemsArray?.Dispose();
            OnSlotSelected -= SetCommand;
        }

        public void Initialize()
        {
            Parameters = ResourceManager.Instance.GetResources<UploadedResources<ScriptableObject>>(
                ResourceManager.Instance.GetOrRegisterKey(ResourcesName.QuickSlotParameters)).resources as QuickSlotsParameters;
            
            ItemsArray = new ObservableArray<Item>(Parameters.Parameters.capacity);
            _onModelChange = new Subject<Item[]>();
            DataViewQuickBar = new DataViewQuickBar(Parameters.Parameters.capacity);
            
            OnSlotSelected += SetCommand;
        }

        public void InvokeSlotSelected(bool isActive, int index)
        {
            OnSlotSelected?.Invoke(isActive, index);
        }
    }
}