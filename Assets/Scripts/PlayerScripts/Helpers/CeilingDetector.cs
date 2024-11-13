using System;
using UnityEngine;

namespace Helpers
{
    public class CeilingDetector : MonoBehaviour
    {
        [SerializeField] private CeilingDetectorSO _ceilingDetector;
        private bool _ceilingWasHit;

        public bool HitCeiling() => _ceilingWasHit;
        public void Reset() => _ceilingWasHit = false;

        private void OnCollisionEnter(Collision collision) => CheckForContact(collision);

        private void OnCollisionStay(Collision collision) => CheckForContact(collision);

        private void CheckForContact(Collision collision)
        {
            if(collision.contacts.Length == 0) return;

            var angle = Vector3.Angle(-transform.up, collision.contacts[0].normal);

            if (angle < _ceilingDetector.CeilingAngleLimit)
            {
                _ceilingWasHit = true;
            }

            if (_ceilingDetector.IsInDebugMode)
            {
                Debug.DrawRay(collision.contacts[0].point, collision.contacts[0].normal, Color.red, _ceilingDetector.DebugDrawDuration);
            }
        }
    }
}