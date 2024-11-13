using System;
using Data;
using Helpers;
using Helpers.Extensions;
using StateMachine;
using StateMachine.Data;
using UnityEngine;

namespace State
{
    public abstract class PlayerBehaviour : IState
    {
        public PlayerStateMachine StateMachine { get; private set; }
        public PlayerData Player { get; private set; }
        public StateMachineData StateMachineData { get; private set; }

        protected event Action<Vector3> OnLand;
        protected event Action<Vector3> OnJump; 

        protected PlayerBehaviour(PlayerStateMachine playerStateMachine)
        {
            Preconditions.CheckNotNull(playerStateMachine);
            StateMachine = playerStateMachine;
            Player = StateMachine.GetData();
            StateMachineData = StateMachine.GetStateMachineData();
            Player.PlayerInputReader.EnablePlayerAction();
        }

        public virtual void OnEnter() => AddActionsCallbacks();

        public virtual void OnExit() => RemoveActionCallbacks();

        public virtual void OnUpdateBehaviour() {}

        public virtual void OnFixedUpdateBehaviour()
        {
            StateMachineData.CheckForGround();
            HandleMomentum();
            
            var velocity = StateMachine.GetStateMachine().currentStates is PlayerIdle or PlayerMovement
                ? CalculateMovementVelocity()
                : Vector3.zero;
            
            velocity += StateMachineData.PhysicsConfig.UseLocalMomentum
                ? StateMachineData._transform.localToWorldMatrix * StateMachineData._momentum
                : StateMachineData._momentum;
            
            StateMachineData._groundHelper.SetExtendSensorRange(StateMachineData.IsGroundForState());
            
            StateMachineData._groundHelper.SetVelocity(velocity);

            StateMachineData._savedVelocity = velocity;
            StateMachineData._savedMovementVelocity = CalculateMovementVelocity();
            
            StateMachineData.ceilingDetector.Reset();
        }

        public virtual bool TrySwapState() => default;
        
        public void ClearDelegates(params Action[] clearActions)
        {
            foreach (var action in clearActions)
            {
                action?.Invoke();
            }

            clearActions = null;
        }
        
        protected virtual void AddActionsCallbacks() { Player.PlayerInputReader.Jump += HandleJumpingInput; }
        protected virtual void RemoveActionCallbacks() { Player.PlayerInputReader.Jump -= HandleJumpingInput; }
        protected abstract void CalculateFriction();

        protected void CheckIsUseLocalMomentum()
        {
            if (StateMachineData.PhysicsConfig.UseLocalMomentum)
                StateMachineData._momentum =
                    StateMachineData._transform.localToWorldMatrix * StateMachineData._momentum;
        }

        private void HandleMomentum()
        {
            CheckIsUseLocalMomentum();

            StateMachineData.verticalMovement =
                VectorMath.ExtractDotVector(StateMachineData._momentum, StateMachineData._transform.up);
            StateMachineData.horizontalMovement = StateMachineData._momentum - StateMachineData.verticalMovement;

            StateMachineData.verticalMovement -= StateMachineData._transform.up *
                                                 (StateMachineData.PhysicsConfig.Gravity * Time.deltaTime);
            
            if (StateMachine.GetStateMachine().currentStates is PlayerIdle or PlayerMovement 
                && VectorMath.GetDotProduct(StateMachineData.verticalMovement, StateMachineData._transform.up) < 0f)
            {
                StateMachineData.verticalMovement = Vector3.zero;
            }

            if (StateMachineData.IsGroundForState() == false)
            {
                AdjustHorizontalMomentum(StateMachineData.horizontalMovement, CalculateMovementVelocity());
            }

            StateMachineData.horizontalMovement = Vector3.MoveTowards(StateMachineData.horizontalMovement, Vector3.zero,
                StateMachineData.friction * Time.deltaTime);

            StateMachineData._momentum = StateMachineData.horizontalMovement + StateMachineData.verticalMovement;
            
            CheckIsUseLocalMomentum();
        }
        
        private void HandleJumpingInput(bool isButtonPressed)
        {
            Debug.Log("сколько раз вызвался");
            if (StateMachineData._jumpKeyIsPressed == false && isButtonPressed)
                StateMachineData._jumpKeyWasPressed = true;

            if (StateMachineData._jumpKeyIsPressed && isButtonPressed == false)
            {
                StateMachineData._jumpInputIsLocked = false;
            }

            StateMachineData._jumpKeyIsPressed = isButtonPressed;
        }

        private void AdjustHorizontalMomentum(Vector3 horizontalMomentum, Vector3 movementVelocity)
        {
            var airControlRate = StateMachineData.MovementConfig.AirControlRate;
            var speed = StateMachineData.MovementConfig.MovementSpeed;
            if (horizontalMomentum.magnitude > speed)
            {
                if (VectorMath.GetDotProduct(movementVelocity, horizontalMomentum.normalized) > 0f)
                {
                    movementVelocity = VectorMath.RemoveDotVector(movementVelocity, horizontalMomentum.normalized);
                }

                horizontalMomentum += movementVelocity * (Time.deltaTime * airControlRate * 0.25f);
            }
            else
            {
                horizontalMomentum += movementVelocity * (Time.deltaTime * airControlRate);
                horizontalMomentum = Vector3.ClampMagnitude(horizontalMomentum, speed);
            }

            StateMachineData.horizontalMovement = horizontalMomentum;
        }

        protected Vector3 CalculateMovementVelocity() =>
            CalculateMovementDirection() * StateMachineData.MovementConfig.MovementSpeed;

        protected void OnGroundContactRegained()
        {
            Vector3 collisionVelocity = StateMachineData.PhysicsConfig.UseLocalMomentum
                ? StateMachineData._transform.localToWorldMatrix * StateMachineData._momentum
                : StateMachineData._momentum;
            InvokeLand(collisionVelocity);
        }

        protected void OnGroundContactLost()
        {
            CheckIsUseLocalMomentum();

            var velocity = StateMachineData.GetMovementVelocity();

            if (velocity.sqrMagnitude >= 0f && StateMachineData._momentum.sqrMagnitude > 0f)
            {
                var projectedMomentum = Vector3.Project(StateMachineData._momentum, velocity.normalized);
                var dot = VectorMath.GetDotProduct(projectedMomentum.normalized, velocity.normalized);

                if (projectedMomentum.sqrMagnitude >= velocity.sqrMagnitude && dot > 0f)
                    velocity = Vector3.zero;
                else if(dot > 0f)
                {
                    velocity -= projectedMomentum;
                }
            }

            StateMachineData._momentum += velocity;
            
            CheckIsUseLocalMomentum();
        }

        private Vector3 CalculateMovementDirection()
        {
            var input = StateMachineData.PlayerInputReader.Direction;
            var cameraTransform = StateMachineData._playerCamera;
            var playerTransform = StateMachineData._transform;
            
            var direction = Vector3.ProjectOnPlane(cameraTransform.right, playerTransform.up).normalized * input.x + 
                            Vector3.ProjectOnPlane(cameraTransform.forward, playerTransform.up).normalized * input.z;
            
            return direction.magnitude > 1f ? direction.normalized : direction;
        }

        protected void InvokeLand(Vector3 vector)
        {
            OnLand?.Invoke(vector);
        }

        protected void InvokeJump(Vector3 vector)
        {
            OnJump?.Invoke(vector);
        }
    }
}