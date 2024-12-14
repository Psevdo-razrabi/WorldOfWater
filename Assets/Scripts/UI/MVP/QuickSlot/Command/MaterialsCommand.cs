using UnityEngine;

namespace QuickSlot.Command
{
    public class MaterialsCommand : IQuickSlotCommand
    {
        public void Execute()
        {
            Debug.Log("Materials command executed");
        }
    }
}