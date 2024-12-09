using System;
using UnityEngine;

namespace Helpers
{
    public class RaycastHelper
    {
        private float _castLength;
        private LayerMask _layerMask;
        
        private Vector3 _origin = Vector3.zero;
        private Transform _playerTransform;
        private CastDirection _castDirection;
        private RaycastHit hitInfo;

        public RaycastHelper(Transform playerTransform, RaycastHelpersSO parameters)
        {
            _playerTransform = playerTransform;
            _castLength = parameters.CastLength;
            _layerMask = parameters.LayerMask;
        }

        public bool HasDetectedHit() => hitInfo.collider != null;
        public float GetDistance() => hitInfo.distance;
        public Vector3 GetNormal() => hitInfo.normal;
        public Vector3 GetPosition() => hitInfo.point;
        public Collider GetCollider() => hitInfo.collider;

        public void SetLayerMask(int layer) => _layerMask = layer;
        public void SetCastDirection(CastDirection castDirection) => _castDirection = castDirection;
        public void SetCastOrigin(Vector3 pos) => _origin = _playerTransform.InverseTransformPoint(pos);
        public void SetCastLength(float castLength) => _castLength = castLength;
        

        public void Cast()
        {
            var worldOrigin = _playerTransform.TransformPoint(_origin);
            var worldDirection = GetCastDirection();

            Physics.Raycast(worldOrigin, worldDirection, out hitInfo, _castLength, _layerMask,
                QueryTriggerInteraction.Ignore);
        }
        
        public void Cast(Transform transform, Vector3 origin, float castLength, out RaycastHit hit)
        {
            var worldOrigin = transform.TransformPoint(origin);
            
            Physics.Raycast(transform.position, worldOrigin, out hit, castLength, _layerMask,
                QueryTriggerInteraction.Ignore);
        }

        public void Draw()
        {
            if (!HasDetectedHit()) return;

            Debug.DrawRay(hitInfo.point, hitInfo.normal, Color.red, Time.deltaTime);
            float markerSize = 0.2f;
            Debug.DrawLine(hitInfo.point + Vector3.up * markerSize, hitInfo.point - Vector3.up * markerSize, Color.green, Time.deltaTime);
            Debug.DrawLine(hitInfo.point + Vector3.right * markerSize, hitInfo.point - Vector3.right * markerSize, Color.green, Time.deltaTime);
            Debug.DrawLine(hitInfo.point + Vector3.forward * markerSize, hitInfo.point - Vector3.forward * markerSize, Color.green, Time.deltaTime);
        }

        private Vector3 GetCastDirection()
        {
            return _castDirection switch
            {
                CastDirection.Backward => -_playerTransform.forward,
                CastDirection.Forward => _playerTransform.forward,
                CastDirection.Down => -_playerTransform.up,
                CastDirection.Up => _playerTransform.up,
                CastDirection.Left => -_playerTransform.right,
                CastDirection.Right => _playerTransform.right,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    public enum CastDirection
    {
        Forward,
        Right,
        Left,
        Backward,
        Down,
        Up
    }
}