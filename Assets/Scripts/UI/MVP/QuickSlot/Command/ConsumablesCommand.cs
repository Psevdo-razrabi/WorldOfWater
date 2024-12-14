using UnityEngine;

namespace QuickSlot.Command
{
    public class ConsumablesCommand : IQuickSlotCommand
    {
        public void Execute()
        {
            Debug.Log("Consumables command executed");
        }
    }
}