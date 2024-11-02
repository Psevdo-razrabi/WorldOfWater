using System;
using StateMachine;

namespace State
{
    public class PlayerIdle : PlayerBehaviour, IDisposable
    {
        public PlayerIdle(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
            playerStateMachine.AddDispose(this);
        }

        public void Dispose()
        {
            
        }
    }
}