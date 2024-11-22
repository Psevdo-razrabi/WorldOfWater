using System;
using StateMachine;
using UnityEngine;

namespace State
{
    public class PlayerMovement : PlayerBehaviour, IDisposable
    {
        public PlayerMovement(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
            playerStateMachine.AddDispose(this);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            OnGroundContactRegained();
            Debug.Log("Вход в ходьбу");
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override bool TrySwapState()
        {
            return StateMachineData.IsGroundForGround() && StateMachineData.IsGroundTooSteep() == false &&
                   StateMachineData.IsInputZero() == false && StateMachineData.IsInventoryOpen() == false;
        }
        
        public override void OnFixedUpdateBehaviour()
        {
            CalculateFriction();
            base.OnFixedUpdateBehaviour();
        }
        
        public override void OnUpdateBehaviour()
        {
            base.OnUpdateBehaviour();
            StateMachine.TrySwapState<PlayerIdle>();
            StateMachine.TrySwapState<PlayerRising>();
            StateMachine.TrySwapState<PlayerSliding>();
            StateMachine.TrySwapState<PlayerFalling>();
            StateMachine.TrySwapState<PlayerJumping>();
        }
        
        protected override void CalculateFriction()
        {
            StateMachineData.friction = StateMachineData.PhysicsConfig.GroundFriction;
        }

        public void Dispose()
        {
            
        }
    }
}