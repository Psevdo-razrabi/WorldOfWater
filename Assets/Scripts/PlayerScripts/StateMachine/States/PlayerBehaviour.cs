﻿using System;
using Data;
using Helpers;
using Helpers.Extensions;
using Inventory;
using StateMachine;
using StateMachine.Data;
using UnityEngine;

namespace State
{
    public abstract class PlayerBehaviour : IState
    { 
        public PlayerStateMachine StateMachine { get; private set; }
        protected PlayerData Player { get; private set; }
        protected StateMachineData StateMachineData { get; private set; }
        
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

        public virtual void OnUpdateBehaviour()
        {
            FindItemView();
            UpdateInputState();
            CheckIfStarting();
            CheckIfStopped();
        }

        private void UpdateInputState()
        {
            var movementInputDetected = StateMachineData.IsInputZero();
            var movementInputDuration = StateMachineData._movementInputDuration;

            if (!movementInputDetected)
            {
                UpdateMovementInputState(movementInputDuration);
                StateMachineData._movementInputDuration += Time.deltaTime;
            }
            else
            {
                ResetMovementInputState();
            }
        }

        private void UpdateMovementInputState(float duration)
        {
            if (duration == 0)
            {
                StateMachineData._movementInputTapped = true;
                StateMachineData._movementInputPressed = false;
                StateMachineData._movementInputHeld = false;
            }
            else if (duration is > 0 and < 0.15f)
            {
                StateMachineData._movementInputTapped = false;
                StateMachineData._movementInputPressed = true;
                StateMachineData._movementInputHeld = false;
            }
            else
            {
                StateMachineData._movementInputTapped = false;
                StateMachineData._movementInputPressed = false;
                StateMachineData._movementInputHeld = true;
            }
        }
        
        private void FaceMoveDirection()
        {
            var transform = StateMachineData._transform;
            var direction = StateMachineData._direction;
            
            Vector3 characterForward = new Vector3(transform.forward.x, 0f, transform.forward.z).normalized;
            Vector3 characterRight = new Vector3(transform.right.x, 0f, transform.right.z).normalized;
            Vector3 directionForward = new Vector3(direction.x, 0f, direction.z).normalized;

            _cameraForward = _cameraController.GetCameraForwardZeroedYNormalised();
            Quaternion strafingTargetRotation = Quaternion.LookRotation(_cameraForward);

            _strafeAngle = characterForward != directionForward ? Vector3.SignedAngle(characterForward, directionForward, Vector3.up) : 0f;

            _isTurningInPlace = false;

            if (_isStrafing)
            {
                if (_moveDirection.magnitude > 0.01)
                {
                    if (_cameraForward != Vector3.zero)
                    {
                        // Shuffle direction values - these are separate from the strafe values as we don't want to lerp, we need to know immediately
                        // what direction to shuffle, and then lock the value so it doesn't return to zero once we lose input (so the blend tree works
                        // to the end of the anim clip)
                        _shuffleDirectionZ = Vector3.Dot(characterForward, directionForward);
                        _shuffleDirectionX = Vector3.Dot(characterRight, directionForward);

                        UpdateStrafeDirection(
                            Vector3.Dot(characterForward, directionForward),
                            Vector3.Dot(characterRight, directionForward)
                        );
                        _cameraRotationOffset = Mathf.Lerp(_cameraRotationOffset, 0f, _rotationSmoothing * Time.deltaTime);

                        float targetValue = _strafeAngle > _forwardStrafeMinThreshold && _strafeAngle < _forwardStrafeMaxThreshold ? 1f : 0f;

                        if (Mathf.Abs(_forwardStrafe - targetValue) <= 0.001f)
                        {
                            _forwardStrafe = targetValue;
                        }
                        else
                        {
                            float t = Mathf.Clamp01(_STRAFE_DIRECTION_DAMP_TIME * Time.deltaTime);
                            _forwardStrafe = Mathf.SmoothStep(_forwardStrafe, targetValue, t);
                        }
                    }

                    transform.rotation = Quaternion.Slerp(transform.rotation, strafingTargetRotation, _rotationSmoothing * Time.deltaTime);
                }
                else
                {
                    UpdateStrafeDirection(1f, 0f);

                    float t = 20 * Time.deltaTime;
                    float newOffset = 0f;

                    if (characterForward != _cameraForward)
                    {
                        newOffset = Vector3.SignedAngle(characterForward, _cameraForward, Vector3.up);
                    }

                    _cameraRotationOffset = Mathf.Lerp(_cameraRotationOffset, newOffset, t);

                    if (Mathf.Abs(_cameraRotationOffset) > 10)
                    {
                        _isTurningInPlace = true;
                    }
                }
            }
            else
            {
                UpdateStrafeDirection(1f, 0f);
                _cameraRotationOffset = Mathf.Lerp(_cameraRotationOffset, 0f, _rotationSmoothing * Time.deltaTime);

                _shuffleDirectionZ = 1;
                _shuffleDirectionX = 0;

                Vector3 faceDirection = new Vector3(_velocity.x, 0f, _velocity.z);

                if (faceDirection == Vector3.zero)
                {
                    return;
                }

                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(faceDirection),
                    _rotationSmoothing * Time.deltaTime
                );
            }
        }

        private void UpdateStrafeDirection(float targetZ, float targetX)
        {
            _strafeDirectionZ = Mathf.Lerp(_strafeDirectionZ, targetZ, _ANIMATION_DAMP_TIME * Time.deltaTime);
            _strafeDirectionX = Mathf.Lerp(_strafeDirectionX, targetX, _ANIMATION_DAMP_TIME * Time.deltaTime);
            _strafeDirectionZ = Mathf.Round(_strafeDirectionZ * 1000f) / 1000f;
            _strafeDirectionX = Mathf.Round(_strafeDirectionX * 1000f) / 1000f;
        }

        private void CheckIfStarting()
        {
            var locomotionStartTimer = StateMachineData._locomotionStartTimer;
            
            locomotionStartTimer = VariableOverrideDelayTimer(locomotionStartTimer);

            bool isStartingCheck = false;

            if (locomotionStartTimer <= 0.0f)
            {
                var speed = StateMachineData.GetVelocity().magnitude;
                if (StateMachineData.GetDirection().magnitude > 0.01 && speed < 1) //todo && !_isStrafing
                {
                    isStartingCheck = true;
                }

                if (isStartingCheck)
                {
                    if (!StateMachineData._isStarting)
                    {
                        StateMachineData._locomotionStartDirection = StateMachineData._newDirectionDifferenceAngle;
                    }

                    var delayTime = 0.2f;
                    StateMachineData._leanDelay = delayTime;
                    StateMachineData._headLookDelay = delayTime;
                    StateMachineData._bodyLookDelay = delayTime;

                    StateMachineData._locomotionStartTimer = delayTime;
                }
            }
            else
            {
                isStartingCheck = true;
            }

            StateMachineData._isStarting = isStartingCheck;
            StateMachineData.InvokeStartMovement(isStartingCheck);
        }
        
        private float VariableOverrideDelayTimer(float timeVariable)
        {
            if (timeVariable > 0.0f)
            {
                timeVariable -= Time.deltaTime;
                timeVariable = Mathf.Clamp(timeVariable, 0.0f, 1.0f);
            }
            else
            {
                timeVariable = 0.0f;
            }

            return timeVariable;
        }

        private void ResetMovementInputState()
        {
            StateMachineData._movementInputDuration = 0;
            StateMachineData._movementInputTapped = false;
            StateMachineData._movementInputPressed = false;
            StateMachineData._movementInputHeld = false;
        }

        public virtual void OnFixedUpdateBehaviour()
        {
            StateMachineData.CheckForGround();
            StateMachineData._inclineAngle = StateMachineData.GroundInclineCheck();
            HandleMomentum();
            CalculateVelocity();
            
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

        protected virtual void AddActionsCallbacks()
        {
            Player.PlayerInputReader.OpenInventory += OpenInventory;
            Player.PlayerInputReader.TakeItem += PickUpItem;
        }

        protected virtual void RemoveActionCallbacks()
        {
            Player.PlayerInputReader.OpenInventory -= OpenInventory;
            Player.PlayerInputReader.TakeItem -= PickUpItem;
        }
        
        protected virtual void CalculateFriction() { }

        protected void CheckIsUseLocalMomentum()
        {
            if (StateMachineData.PhysicsConfig.UseLocalMomentum)
                StateMachineData._momentum =
                    StateMachineData._transform.localToWorldMatrix * StateMachineData._momentum;
        }

        protected Vector3 CalculateMovementVelocity()
        {
            var velocity = CalculateMovementDirection() * StateMachineData.MovementConfig.MovementSpeed;
            
            velocity.x = Mathf.Round(velocity.x * 1000f) / 1000f;
            velocity.y = Mathf.Round(velocity.y * 1000f) / 1000f;
            velocity.z = Mathf.Round(velocity.z * 1000f) / 1000f;
            
            var playerForwardVector = StateMachineData._transform.forward;
            var direction = StateMachineData.GetDirection();
            
            StateMachineData._newDirectionDifferenceAngle = playerForwardVector != direction
                ? Vector3.SignedAngle(playerForwardVector, direction, Vector3.up)
                : 0f;

            return velocity;
        }
        

        protected void OnGroundContactRegained()
        {
            Vector3 collisionVelocity = StateMachineData.PhysicsConfig.UseLocalMomentum
                ? StateMachineData._transform.localToWorldMatrix * StateMachineData._momentum
                : StateMachineData._momentum;
            
            StateMachineData.InvokeLand(collisionVelocity);
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
        
        protected bool TryGetHitInfo(out RaycastHit hit)
        {
            var ray = StateMachineData._camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f));
            var config = StateMachineData.FindItemConfig;

            return Physics.Raycast(ray, out hit, config.Distance, config.Layer);
        }
        
        private void PickUpItem()
        {
            if (!TryGetHitInfo(out RaycastHit hit)) return;
            if (!hit.collider.gameObject.TryGetComponent(out ItemType type)) return;
            
            StateMachineData._isPickUpItem = true;
            StateMachineData.Data.Mediator.TakeItem(type);
        }
        
        private void CheckIfStopped()
        {
            var speed = StateMachineData.GetVelocity();
            StateMachineData._isStopped = StateMachineData.GetDirection().magnitude == 0 && speed.magnitude < .5;
        }

        private void FindItemView()
        {
            var width = Screen.width / 2;
            var heigth = Screen.height / 2;
            
            if (TryGetHitInfo(out RaycastHit hit))
            {
                var render = hit.collider.gameObject.GetComponent<Renderer>();
                Debug.DrawLine(StateMachineData._camera.ScreenPointToRay(new Vector3(width, heigth, 0f)).origin, hit.point, Color.red);

                if (render != null)
                {
                    render.material.color = Color.cyan;
                }
            }
            else
            {
                Debug.DrawLine(StateMachineData._camera.ScreenPointToRay(new Vector3(width, heigth, 0f)).origin, 
                    StateMachineData._camera.ScreenPointToRay(new Vector3(width, heigth, 0f)).origin + 
                    StateMachineData._camera.ScreenPointToRay(new Vector3(width, heigth, 0f)).direction * 5f, Color.red);
            }
        }
        
        private void OpenInventory()
        {
            StateMachineData.IsOpenInventory.Value = !StateMachineData.IsOpenInventory.Value;
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
                StateMachineData._friction * Time.deltaTime);

            StateMachineData._momentum = StateMachineData.horizontalMovement + StateMachineData.verticalMovement;
            
            CheckIsUseLocalMomentum();
        }
        
        private void CalculateVelocity()
        {
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
        
        private Vector3 CalculateMovementDirection()
        {
            var input = StateMachineData.PlayerInputReader.Direction;
            var cameraTransform = StateMachineData._cameraTransform;
            var playerTransform = StateMachineData._transform;
            
            var direction = Vector3.ProjectOnPlane(cameraTransform.right, playerTransform.up).normalized * input.x + 
                            Vector3.ProjectOnPlane(cameraTransform.forward, playerTransform.up).normalized * input.z;

            StateMachineData._direction = direction;
            
            return direction.magnitude > 1f ? direction.normalized : direction;
        }
    }
}