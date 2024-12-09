using System;
using StateMachine;
using UnityEngine;

namespace State
{
    public class PlayerRising : PlayerBehaviour, IDisposable
    {
        public PlayerRising(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
            playerStateMachine.AddDispose(this);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log("Вход в рисинг");
            OnGroundContactLost();
        }
        
        public override void OnExit()
        {
            base.OnExit();
        }

        public override bool TrySwapState()
        {
            if (StateMachine.GetStateMachine().currentStates is PlayerJumping)
            {
                return StateMachineData._jumpIsFinished;
            }

            return StateMachineData.IsRising() && StateMachineData.IsInventoryOpen() == false;
        }

        public override void OnFixedUpdateBehaviour()
        {
            CalculateFriction();
            base.OnFixedUpdateBehaviour();
        }
        
        public override void OnUpdateBehaviour()
        {
            StateMachine.TrySwapState<PlayerMovement>();
            StateMachine.TrySwapState<PlayerIdle>();
            StateMachine.TrySwapState<PlayerFalling>();
            StateMachine.TrySwapState<PlayerSliding>();
        }

        public void Dispose()
        {
            
        }
        
        protected override void CalculateFriction()
        {
            StateMachineData._friction = StateMachineData.PhysicsConfig.AirFriction;
        }
    }
}