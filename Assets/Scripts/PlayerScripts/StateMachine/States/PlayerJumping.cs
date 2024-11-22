using System;
using Helpers.Extensions;
using Helpers.Timer;
using StateMachine;
using UnityEngine;

namespace State
{
    public class PlayerJumping : PlayerBehaviour, IDisposable
    {
        private Timer _timerJump;
        private Action _jumpFinish = delegate { };
        
        public PlayerJumping(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
            playerStateMachine.AddDispose(this);
            playerStateMachine.GetStateMachineData().PlayerInputReader.Jump += HandleJumpingInput;
        }
        
        public override void OnEnter()
        {
            base.OnEnter();
            OnGroundContactLost();
            JumpStart();
            Debug.Log("Вход в прыжок");
        }

        public override void OnExit()
        {
            base.OnExit();
            StateMachineData._jumpIsFinished = false;
        }

        protected override void AddActionsCallbacks()
        {
            base.AddActionsCallbacks();
            _jumpFinish = () => StateMachineData._jumpIsFinished = true;
        }

        protected override void RemoveActionCallbacks()
        {
            base.RemoveActionCallbacks();
            _jumpFinish = null;
        }

        public override bool TrySwapState()
        {
            return (StateMachineData._jumpKeyIsPressed || StateMachineData._jumpKeyWasPressed) &&
                    StateMachineData._jumpInputIsLocked == false && StateMachineData.IsInventoryOpen() == false;
        }
        
        public override void OnFixedUpdateBehaviour()
        {
            CalculateFriction();
            base.OnFixedUpdateBehaviour();
            ResetJumpKeys();
            HandleJumping();
        }
        
        public override void OnUpdateBehaviour()
        {
            StateMachine.TrySwapState<PlayerFalling>();
            StateMachine.TrySwapState<PlayerRising>();
        }

        public void Dispose()
        {
            StateMachineData.PlayerInputReader.Jump -= HandleJumpingInput;
        }

        private void HandleJumping()
        {
            var momentum = StateMachineData._momentum;
            var transform = StateMachineData._transform;
            momentum = VectorMath.RemoveDotVector(momentum, transform.up);
            momentum += transform.up * StateMachineData.MovementConfig.JumpSpeed;
            StateMachineData._momentum = momentum;
        }
        
        private void HandleJumpingInput(bool isButtonPressed)
        {
            if (StateMachineData._jumpKeyIsPressed == false && isButtonPressed)
                StateMachineData._jumpKeyWasPressed = true;

            if (StateMachineData._jumpKeyIsPressed && isButtonPressed == false)
            {
                StateMachineData._jumpInputIsLocked = false;
            }

            StateMachineData._jumpKeyIsPressed = isButtonPressed;
        }

        private void JumpStart()
        {
            CheckIsUseLocalMomentum();

            var momentum = StateMachineData._momentum;
            var transform = StateMachineData._transform;

            momentum += transform.up * StateMachineData.MovementConfig.JumpSpeed;
            _timerJump = new Timer(StateMachineData.MovementConfig.JumpDuration, _jumpFinish);
            _timerJump.Start();
            StateMachineData._jumpInputIsLocked = true;
            InvokeJump(momentum);
            StateMachineData._momentum = momentum;
            
            CheckIsUseLocalMomentum();
        }
        
        private void ResetJumpKeys()
        {
            StateMachineData._jumpKeyWasPressed = false;
        }
        
        protected override void CalculateFriction()
        {
            StateMachineData.friction = StateMachineData.PhysicsConfig.AirFriction;
        }
    }
}