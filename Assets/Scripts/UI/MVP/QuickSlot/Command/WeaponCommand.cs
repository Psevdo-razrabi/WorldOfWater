using UnityEngine;

namespace QuickSlot.Command
{
    public class WeaponItemCommand : IQuickSlotCommand
    {
        public void Execute()
        {
            Debug.Log("Weapon command executed");
        }
    }
}