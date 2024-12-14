using System;
using Inventory;

namespace QuickSlot.Command
{
    public class ItemCommandInvoker
    {
        public void ExecuteCommand(EItemCommand type)
        {
            IQuickSlotCommand command = type switch
            {
                EItemCommand.Consumables => new ConsumablesCommand(),
                EItemCommand.Instruments => new InstrumentsCommand(),
                EItemCommand.Materials => new MaterialsCommand(),
                EItemCommand.Weapon => new WeaponItemCommand(),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
            
            command.Execute();
        }
    }
}