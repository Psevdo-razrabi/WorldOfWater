using System;
using StateMachine;

namespace State
{
    public class PlayerPicksUpItem : PlayerBehaviour, IDisposable
    {
        public PlayerPicksUpItem(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
            playerStateMachine.AddDispose(this);
        }

        public override bool TrySwapState()
        {
            return StateMachineData.IsPickUpItem();
        }

        public void Dispose()
        {
            
        }
    }
}