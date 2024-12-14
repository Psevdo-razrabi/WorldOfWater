using UnityEngine;

namespace QuickSlot.Command
{
    public class InstrumentsCommand : IQuickSlotCommand
    {
        public void Execute()
        {
            Debug.Log("Instruments command executed");
        }
    }
}