﻿using System;
using Helpers.Extensions;
using StateMachine;
using UnityEngine;

namespace State
{
    public class PlayerFalling : PlayerBehaviour, IDisposable
    {
        public PlayerFalling(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
            playerStateMachine.AddDispose(this);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log("Вход в падение");
            FallStart();
        }
        
        public override void OnExit()
        {
            base.OnExit();
        }

        public override void OnFixedUpdateBehaviour()
        {
            CalculateFriction();
            base.OnFixedUpdateBehaviour();
        }

        public override bool TrySwapState()
        {
            if (StateMachine.GetStateMachine().currentStates is PlayerJumping)
            {
                return StateMachineData.ceilingDetector != null && StateMachineData.ceilingDetector.HitCeiling();
            }

            if (StateMachine.GetStateMachine().currentStates is PlayerRising)
            {
                return StateMachineData.IsFalling() || (StateMachineData.ceilingDetector != null &&
                    StateMachineData.ceilingDetector.HitCeiling());
            }

            return StateMachineData.IsGroundForGround() == false && StateMachineData.IsInventoryOpen() == false;
        }
        
        public override void OnUpdateBehaviour()
        {   
            StateMachine.TrySwapState<PlayerMovement>();
            StateMachine.TrySwapState<PlayerIdle>();
            StateMachine.TrySwapState<PlayerRising>();
            StateMachine.TrySwapState<PlayerSliding>();
        }

        public void Dispose()
        {
            
        }
        
        protected override void CalculateFriction()
        {
            StateMachineData.friction = StateMachineData.PhysicsConfig.AirFriction;
        }

        private void FallStart()
        {
            var momentum = StateMachineData.GetMomentum();
            var transform = StateMachineData.GetTransform();
            var currentUpMomentum =
                VectorMath.ExtractDotVector(momentum, transform.up);
            momentum = VectorMath.RemoveDotVector(momentum, transform.up);
            momentum -= transform.up * currentUpMomentum.magnitude;
            StateMachineData._momentum = momentum;
        }
    }
}