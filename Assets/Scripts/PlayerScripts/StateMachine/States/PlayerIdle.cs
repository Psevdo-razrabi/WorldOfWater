using System;
using Helpers.Extensions;
using StateMachine;
using UnityEngine;

namespace State
{
    public class PlayerIdle : PlayerBehaviour, IDisposable
    {
        public PlayerIdle(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
            playerStateMachine.AddDispose(this);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log("Вход в idle");
            OnGroundContactRegained();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override bool TrySwapState()
        {
            return StateMachineData.IsGroundForGround() && StateMachineData.IsGroundTooSteep() == false &&
                   StateMachineData.IsInputZero();
        }

        public override void OnFixedUpdateBehaviour()
        {
            CalculateFriction();
            base.OnFixedUpdateBehaviour();
            HandleMomentumGround();
        }

        public override void OnUpdateBehaviour()
        {
            StateMachine.TrySwapState<PlayerMovement>();
            StateMachine.TrySwapState<PlayerRising>();
            StateMachine.TrySwapState<PlayerSliding>();
            StateMachine.TrySwapState<PlayerFalling>();
            StateMachine.TrySwapState<PlayerJumping>();
        }

        protected override void CalculateFriction()
        {
            StateMachineData.friction = StateMachineData.PhysicsConfig.GroundFriction;
        }

        private void HandleMomentumGround()
        {
            if (VectorMath.GetDotProduct(StateMachineData.verticalMovement, StateMachineData._transform.up) < 0f)
            {
                StateMachineData.verticalMovement = Vector3.zero;
            }
        }

        public void Dispose()
        {
            
        }
    }
}