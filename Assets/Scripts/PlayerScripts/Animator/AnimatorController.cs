using StateMachine.Data;
using StateMachine.Events;
using UnityEngine;
using Zenject;

namespace Animator
{
    [RequireComponent(typeof(UnityEngine.Animator))]
    public class AnimatorController : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Animator _animator;
        private StateMachineData _stateMachineData;
        private StateMachineEvent _stateMachineEvent;

        [Inject]
        private void Construct(StateMachineData stateMachineData, StateMachineEvent stateMachineEvent)
        {
            _stateMachineEvent = stateMachineEvent;
            _stateMachineData = stateMachineData;
        }

        private void OnEnable()
        {
            _stateMachineEvent.OnJump += JumpAnimation;
            _stateMachineEvent.OnLand += LandAnimation;
            _stateMachineEvent.OnGateState += GateStateAnimator;
            _stateMachineEvent.OnWalk += WalkAnimation;
            _stateMachineEvent.OnStartMove += StartAnimation;
        }

        private void StartAnimation(bool obj)
        {
            _animator.SetBool(AnimatorHash.IsStartingHash, _stateMachineData._isStarting);
        }

        private void WalkAnimation(bool enable)
        {
            _animator.SetBool(AnimatorHash.IsWalkingHash, enable);
        }

        private void GateStateAnimator(int state)
        {
            _animator.SetInteger(AnimatorHash.CurrentGaitHash, state);
        }

        private void JumpAnimation(Vector3 obj)
        {
            _animator.SetBool(AnimatorHash.IsJumpingAnimHash, true);
        }

        private void LandAnimation(Vector3 obj)
        {
            _animator.SetBool(AnimatorHash.IsJumpingAnimHash, false);
        }

        private void Update()
        {
            _animator.SetFloat(AnimatorHash.LocomotionStartDirectionHash, _stateMachineData._locomotionStartDirection);
            _animator.SetBool(AnimatorHash.IsStoppedHash, _stateMachineData._isStopped);
            _animator.SetBool(AnimatorHash.IsGroundedHash, _stateMachineData.IsGroundForGround());
            _animator.SetFloat(AnimatorHash.FallingDurationHash, _stateMachineData._fallingDuration);
            _animator.SetFloat(AnimatorHash.MoveSpeedHash, _stateMachineData.GetVelocity().magnitude);
            _animator.SetBool(AnimatorHash.MovementInputTappedHash, _stateMachineData._movementInputTapped);
            _animator.SetBool(AnimatorHash.MovementInputPressedHash, _stateMachineData._movementInputPressed);
            _animator.SetBool(AnimatorHash.MovementInputHeldHash, _stateMachineData._movementInputHeld);
            _animator.SetFloat(AnimatorHash.InclineAngleHash, _stateMachineData._inclineAngle);
        }
    }
}