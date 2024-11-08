using State;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class Player : MonoBehaviour, IPlayerParameters
{
    public Rigidbody Rigidbody { get; private set; }
    public CapsuleCollider CapsuleCollider { get; private set; }
    public GameObject PlayerGameObject => this.gameObject;
    public Transform PlayerTransform => this.transform;

    public void Init()
    {
        Rigidbody = GetComponent<Rigidbody>();
        CapsuleCollider = GetComponent<CapsuleCollider>();
    }
}
