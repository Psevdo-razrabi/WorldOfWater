using System;
using Helpers.Extensions;
using StateMachine;
using UnityEngine;

namespace State
{
    public class PlayerSliding : PlayerBehaviour, IDisposable
    {
        public PlayerSliding(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
            playerStateMachine.AddDispose(this);
        }
        
        public override void OnEnter()
        {
            base.OnEnter();
            OnGroundContactLost();
            Debug.Log("Вход в слайдинг");
        }
        
        public override void OnExit()
        {
            base.OnExit();
        }

        public override bool TrySwapState()
        {
            return StateMachineData.IsGroundForGround() && StateMachineData.IsGroundTooSteep();
        }
        
        public override void OnFixedUpdateBehaviour()
        {
            CalculateFriction();
            base.OnFixedUpdateBehaviour();
            HandleSliding();
            CalculateMomentum();
        }
        
        public override void OnUpdateBehaviour()
        {
            StateMachine.TrySwapState<PlayerMovement>();
            StateMachine.TrySwapState<PlayerIdle>();
            StateMachine.TrySwapState<PlayerRising>();
            StateMachine.TrySwapState<PlayerFalling>();
        }

        private void HandleSliding()
        {
            var pointDownVector = Vector3
                .ProjectOnPlane(StateMachineData._groundHelper.GetGroundNormal(), StateMachineData._transform.up).normalized;

            var movementVelocity = CalculateMovementVelocity();
            movementVelocity = VectorMath.RemoveDotVector(movementVelocity, pointDownVector);
            StateMachineData.horizontalMovement += movementVelocity * Time.fixedDeltaTime;
        }

        private void CalculateMomentum()
        {
            var momentum = StateMachineData._momentum;
            var transform = StateMachineData._transform;
            
            momentum = Vector3.ProjectOnPlane(momentum, StateMachineData._groundHelper.GetGroundNormal());

            if (VectorMath.GetDotProduct(momentum, transform.up) > 0f)
            {
                momentum = VectorMath.RemoveDotVector(momentum, transform.up);
            }

            var slideDirection = 
                Vector3.ProjectOnPlane(-transform.up, StateMachineData._groundHelper.GetGroundNormal())
                .normalized;

            momentum += slideDirection * (StateMachineData.PhysicsConfig.SlideGravity * Time.deltaTime);

            StateMachineData._momentum = momentum;
        }
        
        protected override void CalculateFriction()
        {
            StateMachineData.friction = StateMachineData.PhysicsConfig.AirFriction;
        }

        public void Dispose()
        {
            
        }
    }
}