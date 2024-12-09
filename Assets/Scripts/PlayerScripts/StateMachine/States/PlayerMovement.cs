using System;
using StateMachine;
using StateMachine.Enums;
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
            StateMachineData.InvokeGaitState(GaitState.Walk);
            EnableOrDisableWalk(true);
            OnGroundContactRegained();
            Debug.Log("Вход в ходьбу");
        }

        public override void OnExit()
        {
            base.OnExit();
            EnableOrDisableWalk(false);
        }

        public override bool TrySwapState()
        {
            return StateMachineData.IsGroundForGround() && StateMachineData.IsGroundTooSteep() == false &&
                   StateMachineData.IsInputZero() == false && StateMachineData.IsInventoryOpen() == false && 
                   StateMachineData.IsPickUpItem() == false;
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
            StateMachine.TrySwapState<PlayerPicksUpItem>();
            StateMachine.TrySwapState<PlayerRising>();
            StateMachine.TrySwapState<PlayerSliding>();
            StateMachine.TrySwapState<PlayerFalling>();
            StateMachine.TrySwapState<PlayerJumping>();
        }

        public void Dispose()
        {
            
        }
        
        protected override void CalculateFriction()
        {
            StateMachineData._friction = StateMachineData.PhysicsConfig.GroundFriction;
        }
        
        
        private void EnableOrDisableWalk(bool enable)
        {
            StateMachineData.InvokeWalk(enable);
        }
    }
}