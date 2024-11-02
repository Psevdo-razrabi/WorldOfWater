using System;
using StateMachine;

namespace State
{
    public class PlayerMovement : PlayerBehaviour, IDisposable
    {
        public PlayerMovement(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
            playerStateMachine.AddDispose(this);
        }

        public void Dispose()
        {
            
        }
    }
}