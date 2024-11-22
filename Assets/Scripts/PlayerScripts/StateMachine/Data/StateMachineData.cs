using System;
using Cysharp.Threading.Tasks;
using Data;
using Helpers;
using Helpers.Extensions;
using NewInput;
using PlayerScripts.SO;
using R3;
using State;
using UnityEngine;
using Zenject;

namespace StateMachine.Data
{
    public class StateMachineData
    {
        #region Fields
        public PlayerData Data { get; private set; }
        public PlayerGroundHelper _groundHelper;
        public CeilingDetector ceilingDetector;
        public Transform _transform;
        public Transform _cameraTransform;
        public Camera _camera;
        public StateMachine stateMachine;
        public PlayerInputReader PlayerInputReader { get; private set; }
        public Vector3 _momentum;
        public Vector3 _savedVelocity;
        public Vector3 _savedMovementVelocity;
        public Vector3 verticalMovement;
        public Vector3 horizontalMovement;
        public float friction;
        public bool _jumpKeyIsPressed; 
        public bool _jumpKeyWasPressed;
        public bool _jumpInputIsLocked;
        public bool _jumpIsFinished;
        public bool _isPickUpItem;
        public ReactiveProperty<bool> IsOpenInventory { get; set; } = new();
        private CompositeDisposable _compositeDisposable = new();
        #endregion

        #region Configs

        public PlayerFindItemConfig FindItemConfig { get; private set; }
        public PlayerMovementConfig MovementConfig { get; private set; }
        public PlayerPhysicsConfig PhysicsConfig { get; private set; }

        #endregion

        public void CheckForGround() => _groundHelper.CheckForGround();

        public Vector3 GetMomentum() => Data.PlayerControllerConfig.PhysicsConfig.UseLocalMomentum
                ? _transform.localToWorldMatrix * _momentum
                : _momentum;

        public Transform GetTransform() => _transform;
        public Vector3 GetVelocity() => _savedVelocity;
        public Vector3 GetMovementVelocity() => _savedMovementVelocity;
        public void SetPlayerStateMachine(StateMachine stateMachine) => this.stateMachine = stateMachine;

        public bool IsGroundForState() => stateMachine.currentStates is PlayerIdle or PlayerMovement or PlayerSliding;
        public bool IsGroundForGround() => _groundHelper.IsGrounded();
        public bool IsInputZero() => Data.PlayerInputReader.Direction == Vector3.zero;
        public bool IsInventoryOpen() => IsOpenInventory.Value;
        public bool IsRising() => VectorMath.GetDotProduct(GetMomentum(), _transform.up) > 0f;

        public bool IsFalling() => VectorMath.GetDotProduct(GetMomentum(), _transform.up) < 0f;

        public bool IsPickUpItem() => _isPickUpItem;

        public bool IsGroundTooSteep() => IsGroundForGround() == false ||
                                          Vector3.Angle(_groundHelper.GetGroundNormal(), _transform.up) >
                                          Data.PlayerControllerConfig.PhysicsConfig.SlopeLimit;
        
        public void Dispose()
        {
            PlayerInputReader?.Dispose();
            IsOpenInventory?.Dispose();
        }
        
        [Inject]
        private async void Construct(PlayerData playerData)
        {
            Data = playerData;
            _transform = Data.Player.transform;
            _groundHelper = Data.PlayerGroundHelper;
            MovementConfig = Data.PlayerControllerConfig.MovementConfig;
            PhysicsConfig = Data.PlayerControllerConfig.PhysicsConfig;
            FindItemConfig = Data.PlayerControllerConfig.FindItemConfig;
            PlayerInputReader = Data.PlayerInputReader;
            ceilingDetector = Data.CeilingDetector;
            _cameraTransform = Data.Player.playerCamera.transform;
            _camera = Data.Player.playerCamera;
            
            await Subscribe();
        }
        
        private async UniTask Subscribe()
        {
            await UniTask.WaitUntil(() => Data.IsInit);

            IsOpenInventory
                .Subscribe(Data.InventoryModel.OnOpenInventory.OnNext)
                .AddTo(_compositeDisposable);
        }
    }
}