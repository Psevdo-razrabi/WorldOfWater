using Unity.Netcode;
using UnityEngine;
using VContainer;

namespace Game.Player
{
    public class Player : NetworkBehaviour
    {
        [Header("Components")]
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Collider _collider;
    }

    public class PlayerFactory
    {
        [Inject]
        private void Construct()
        {

        }

        public void CreatePlayer()
        {
            //var player = Object.Instantiate()
        }
    }
}
