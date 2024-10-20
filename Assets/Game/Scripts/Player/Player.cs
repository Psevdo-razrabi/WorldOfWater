using UnityEngine;
using VContainer;

namespace Game.Player
{
    public class Player : MonoBehaviour
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
