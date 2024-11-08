using UnityEngine;

namespace State
{
    public interface IPlayerParameters
    {
        public Rigidbody Rigidbody { get; }
        public CapsuleCollider CapsuleCollider { get; }
        public GameObject PlayerGameObject { get; }
        public Transform PlayerTransform { get; }
    }
}