using Sources.Scripts.Configs;
using Sources.Scripts.Configs.Interfaces;
using Sources.Scripts.UI.ThrowToolsUI.Hook;
using Sources.Scripts.UI.ThrowToolsUI.SpearModel;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Sources.Scripts.Tools.Hook.Spear
{
    public class ThrowSpear : MonoBehaviour
    {
        [SerializeField] private SpearConfig _spearConfig;
        [SerializeField] private Spear _spear;
        [SerializeField] private Transform _spearPoint;
        
        private CompositeDisposable _compositeDisposable = new();
        private Rigidbody _rigidbodySpear;
        private SpearModel _spearModel;
        private bool _isThrow;
        private bool _isPulling;
    
        [Inject]
        private void Construct(SpearModel spearModel)
        {
            _spearModel = spearModel;
            _spearModel.LoadConfig(_spearConfig);
            SubscribeOnThrow();
        }

        private void Start()
        {
            SettingSpear();
        }

        private void SettingSpear()
        {
            _spear.transform.position = _spearPoint.transform.position;
            _rigidbodySpear = _spear.GetComponent<Rigidbody>();
            _rigidbodySpear.isKinematic = true;
        }
    
        private void SubscribeOnThrow()
        {
            _spearModel.CircleFillValue
                .Skip(1)
                .Subscribe(ThrowingSpear)
                .AddTo(_compositeDisposable);
        }

        private void ThrowingSpear()
        {
            if (_isThrow) return;

            _isThrow = true;
            _rigidbodySpear.isKinematic = false;
            
            Vector3 throwDirection = _rigidbodySpear.transform.forward;
            
            _rigidbodySpear.AddForce(throwDirection * _spearConfig.ThrowForce * _spearModel.CircleFillValue.Value, ForceMode.Impulse);
            
            Vector3 torque = new Vector3(0f, 0f, 10f);
            _rigidbodySpear.AddTorque(torque, ForceMode.Impulse);
        }
    }
}