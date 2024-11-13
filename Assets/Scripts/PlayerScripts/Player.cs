using Helpers;
using State;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class Player : MonoBehaviour, IPlayerParameters
{
    [field: SerializeField] public Transform playerCamera;
    public Rigidbody Rigidbody { get; private set; }
    public CapsuleCollider CapsuleCollider { get; private set; }
    public GameObject PlayerGameObject => this.gameObject;
    public Transform PlayerTransform => this.transform;
    public CeilingDetector CeilingDetector { get; private set; }

    public void Init()
    {
        Rigidbody = GetComponent<Rigidbody>();
        CapsuleCollider = GetComponent<CapsuleCollider>();
        CeilingDetector = GetComponent<CeilingDetector>();
    }
}
