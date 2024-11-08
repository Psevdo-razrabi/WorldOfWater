using Data;
using UnityEngine;
using Zenject;

namespace StateMachine.Data
{
    public class StateMachineData
    {
        public PlayerData Data { get; private set; }
        private Vector3 _momentum;
        private Vector3 _savedVelocity;
        private Vector3 _savedMovementVelocity;


        public void SetMomentum(Vector3 momentum) => _momentum = momentum;
        public void SetVelocity(Vector3 velocity) => _savedVelocity = velocity;
        public void SavedMovementVelocity(Vector3 movement) => _savedMovementVelocity = movement;

        public bool IsGround() => Data.PlayerGroundHelper.IsGrounded();
        
        
        
        [Inject]
        private void Construct(PlayerData playerData)
        {
            Data = playerData;
        }
    }
}