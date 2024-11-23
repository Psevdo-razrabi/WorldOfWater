using Netcode.Extensions;
using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;

public class PlayerExample : NetworkBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private CinemachineVirtualCameraBase camera;
    [SerializeField] private GameObject _prefab;
    public NetworkVariable<int> IsTestPressed = new();
    private Vector3 _input;

    public override void OnNetworkSpawn()
    {
        transform.position = new Vector3(0, 2, 0);
        
        if (IsOwner)
            camera.Priority = 1;
        else
            camera.Priority = 0;
        
        NetworkObjectPool.Singleton.InitializePool();

        IsTestPressed.OnValueChanged += OnTested;
    }

    private void OnTested(int prevValue, int newValue)
    {
        Debug.Log("TestValue " + newValue);
    }
    
    private void Update()
    {
        if (IsLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                if (IsHost)
                {
                    var obj = NetworkObjectPool.Singleton.GetNetworkObject(_prefab).gameObject;
                    var netObj = obj.GetComponent<NetworkObject>();
                    netObj.Spawn();

                    var obj2 = Instantiate(_prefab, Vector3.zero, Quaternion.identity);
                    obj2.GetComponent<NetworkObject>().Spawn();
                    
                    NetworkObjectPool.Singleton.ReturnNetworkObject(netObj, _prefab);
                    obj.GetComponent<NetworkObject>().Despawn();
                    Destroy(obj);
                }
                else
                {
                    SpawnServerRpc();
                }
            }
            
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (IsHost)
                {
                    // NetworkObjectPool.Singleton.ReturnNetworkObject(netObj, _prefab);
                    // obj.GetComponent<NetworkObject>().Despawn();
                    // Destroy(obj);
                }
                else
                {
                    // DespawnServerRpc();
                }
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                if (IsHost)
                {
                    IsTestPressed.Value++;
                }
                else
                {
                    TestServerRpc();
                }
            }
        }
    }

    [ServerRpc]
    private void SpawnServerRpc()
    {
        var obj = NetworkObjectPool.Singleton.GetNetworkObject(_prefab).gameObject;
        obj.GetComponent<NetworkObject>().Spawn();
    }
    
    // [ServerRpc]
    // private void DespawnServerRpc()
    // {
    //     NetworkObjectPool.Singleton.ReturnNetworkObject(obj, _prefab);
    //     obj.GetComponent<NetworkObject>().Despawn();
    //     Destroy(obj);
    // }

    [ServerRpc]
    private void TestServerRpc()
    {
        IsTestPressed.Value++;
    }
}
