using Helpers;
using State;
using UnityEngine;

public class PlayerGroundHelper
{
    #region Fields
    private float _colliderHeight;
    private float _colliderThickness;
    private Vector3 _colliderOffset;
    private bool _isGrounded;
    private float _stepHeightRatio;
    private float _baseSensorRange;
    private Vector3 _currentGroundAdjustmentVelocity;
    private int _currentLayer;
    private bool _isUsingExtendedSensorRange = true;
    private PlayerGroundHelperConfig _playerGroundHelper;
    private float _inclineAngle;
    private const int MAX_COUNT_LAYER = 32;
    
    private Rigidbody _rigidbody;
    private CapsuleCollider _capsuleCollider;
    private Transform _playerTransform;
    private Transform _rearRayPos;
    private Transform _frontRayPos;
    private GameObject _playerObject;
    private RaycastHelper _raycastHelper;

    #endregion
    
    public PlayerGroundHelper(IPlayerParameters playerParameters, PlayerGroundHelperConfig playerGroundHelperConfig, RaycastHelper raycastHelper)
    {
        _rigidbody = playerParameters.Rigidbody;
        _capsuleCollider = playerParameters.CapsuleCollider;
        _playerTransform = playerParameters.PlayerTransform;
        _playerObject = playerParameters.PlayerGameObject;
        _raycastHelper = raycastHelper;

        _colliderHeight = playerGroundHelperConfig.ColliderHeight;
        _colliderThickness = playerGroundHelperConfig.ColliderThickness;
        _colliderOffset = playerGroundHelperConfig.ColliderOffset;
        _stepHeightRatio = playerGroundHelperConfig.StepHeightRatio;
        _playerGroundHelper = playerGroundHelperConfig;

        _rearRayPos = playerParameters.RearRayPosition;
        _frontRayPos = playerParameters.FrontRayPosition;

        Init();
    }
    
    #region PublicMethod

    public void LateUpdate()
    {
        if(_playerGroundHelper.IsInDebugMode == false) return;
        _raycastHelper.Draw();
    }
    
    public bool IsGrounded() => _isGrounded;
    public Vector3 GetGroundNormal() => _raycastHelper.GetNormal();
    public void SetExtendSensorRange(bool isExtended) => _isUsingExtendedSensorRange = isExtended;
    public void SetVelocity(Vector3 velocity) =>
        _rigidbody.linearVelocity = velocity + _currentGroundAdjustmentVelocity;

    public float GroundInclineCheck()
    {
        var rayDistance = Mathf.Infinity;
        _rearRayPos.rotation = Quaternion.Euler(_playerTransform.rotation.x, 0, 0);
        _frontRayPos.rotation = Quaternion.Euler(_playerTransform.rotation.x, 0, 0);

        _raycastHelper.Cast(_rearRayPos, -Vector3.up, rayDistance, out RaycastHit rearHit);
        _raycastHelper.Cast(_frontRayPos, -Vector3.up, rayDistance, out RaycastHit frontHit);
    
        var hitDifference = frontHit.point - rearHit.point;
        var xPlaneLength = new Vector2(hitDifference.x, hitDifference.z).magnitude;
    
        return Mathf.Lerp(_inclineAngle, Mathf.Atan2(hitDifference.y, xPlaneLength) * Mathf.Rad2Deg, 20f * Time.deltaTime);
    }

    public void CheckForGround()
    {
        if (_currentLayer != _playerObject.layer)
            RecalculateLayerMask();

        _currentGroundAdjustmentVelocity = Vector3.zero;

        var length = _isUsingExtendedSensorRange
            ? _baseSensorRange + _colliderHeight * _playerTransform.localScale.x * _stepHeightRatio
            : _baseSensorRange;
        _raycastHelper.SetCastLength(length);
        _raycastHelper.Cast();

        _isGrounded = _raycastHelper.HasDetectedHit();
        
        if(_isGrounded == false) return;
        var distance = _raycastHelper.GetDistance();
        var upperLimit = _colliderHeight * _playerTransform.localScale.x * (1f - _stepHeightRatio) * 0.5f;
        var middle = upperLimit + _colliderHeight * _playerTransform.localScale.x * _stepHeightRatio;
        var distanceToGo = middle - distance;

        _currentGroundAdjustmentVelocity = _playerTransform.up * (distanceToGo / Time.fixedDeltaTime);
    }

    #endregion

    private void Init()
    {
        Setup();
        RecalculateColliderDimensions();
    }

    private void Setup()
    {
        _rigidbody.freezeRotation = true;
        _rigidbody.useGravity = false;
    }

    private void RecalculateColliderDimensions()
    {
        _capsuleCollider.height = _colliderHeight * (1f - _stepHeightRatio);
        _capsuleCollider.radius = _colliderThickness / 2f;
        _capsuleCollider.center = _colliderOffset * _colliderHeight +
                                  new Vector3(0f, _stepHeightRatio * _capsuleCollider.height / 2f, 0f);

        if (_capsuleCollider.height / 2f < _capsuleCollider.radius)
        {
            _capsuleCollider.radius = _capsuleCollider.height / 2f;
        }
        
        RecalibrateRaycastHelpers();
    }

    private void RecalibrateRaycastHelpers()
    {
        _raycastHelper.SetCastOrigin(_capsuleCollider.bounds.center);
        _raycastHelper.SetCastDirection(CastDirection.Down);
        RecalculateLayerMask();
        var safetyDistanceFactor = 0.001f;

        var length = _colliderHeight * (1f - _stepHeightRatio) * 0.5f + _colliderHeight * _stepHeightRatio;
        _baseSensorRange = length * (1f + safetyDistanceFactor) * _playerTransform.localScale.x;
        _raycastHelper.SetCastLength(length * _playerTransform.localScale.x);
    }

    private void RecalculateLayerMask()
    {
        var objectLayer = _playerObject.layer;
        var layerMask = Physics.AllLayers;

        for (int i = 0; i < MAX_COUNT_LAYER; i++)
        {
            if (Physics.GetIgnoreLayerCollision(objectLayer, i))
            {
                layerMask &= ~(1 << 1);
            }
        }

        var ignoreRaycastLayer = LayerMask.GetMask("Ignore Raycast");
        layerMask &= ~(1 << ignoreRaycastLayer);

        _currentLayer = objectLayer;
        _raycastHelper.SetLayerMask(layerMask);
    }
}