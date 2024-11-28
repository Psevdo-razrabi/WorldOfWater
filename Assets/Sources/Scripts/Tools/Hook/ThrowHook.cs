using System;
using Cysharp.Threading.Tasks;
using Sources.Scripts.Configs;
using UniRx;
using UnityEngine;
using Zenject;

public class ThrowHook : MonoBehaviour
{
    private const float MIN_DISTANCE_FOR_HOOK = 3f;
    
    [SerializeField] private HookConfig _hookConfig;
    [SerializeField] private Hook _hook;
    [SerializeField] private Transform _hookPoint;
    
    private CompositeDisposable _compositeDisposable = new();
    private Rigidbody _rigidbodyHook;
    private HookModel _hookModel;
    private bool _isThrow;
    private bool _isPulling;
    
    [Inject]
    private void Construct(HookModel hookModel)
    {
        _hookModel = hookModel;
        _hookModel.LoadConfig(_hookConfig);
        SubscribeOnThrow();
    }

    private void Start()
    {
        SettingHook();
    }

    private void SettingHook()
    {
        _hook.transform.position = _hookPoint.transform.position;
        _rigidbodyHook = _hook.GetComponent<Rigidbody>();
        _rigidbodyHook.isKinematic = true;
    }
    
    private void SubscribeOnThrow()
    {
        _hookModel.CircleFillValue
            .Skip(1)
            .Subscribe(Hook)
            .AddTo(_compositeDisposable);
    }

    private void Hook()
    {
        SwitchThrowHookState();
        Vector3 forwardForce = _hookPoint.transform.forward * _hookConfig.ThrowForce;
        Vector3 arcForce = forwardForce * (_hookModel.CircleFillValue.Value * 10) + Vector3.up * 3;
        _rigidbodyHook.AddForce(arcForce, ForceMode.VelocityChange);
    }
        
    public void HookBackPerformed()
    {
        _hookModel.SwitchToolThrowState(false);
        SwitchTakedHookState();
        _hook.transform.position = _hookPoint.transform.position;
    }

    public void StartPullingHook()
    {
        _isPulling = true;
        PullingHookAsync().Forget();
    }

    public void StopPullingHook()
    {
        _isPulling = false;
    }

    private async UniTask PullingHookAsync()
    {
        _rigidbodyHook.isKinematic = true;
        while (_isPulling == true)
        {
            Vector3 targetPosition = _hookPoint.position;
            Vector3 currentPosition = _hook.transform.position;

            Vector3 nextPosition = Vector3.Lerp(currentPosition, targetPosition, _hookConfig.ReturnOnWaterSpeed * Time.deltaTime);

            _hook.transform.position = nextPosition;

            if (_hook.transform.position.y != _hook.PointOfHit.position.y)
            {
                _hook.transform.position = new Vector3(nextPosition.x, _hook.PointOfHit.position.y, nextPosition.z);
            }
            
            Debug.Log(Vector3.Distance(_hook.transform.position, _hookPoint.position));
            
            if (Vector3.Distance(_hook.transform.position, _hookPoint.position) < MIN_DISTANCE_FOR_HOOK)
            {
                HookBackPerformed();
            }
            
            await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
        }

        if (Vector3.Distance(_hook.transform.position, _hookPoint.position) < MIN_DISTANCE_FOR_HOOK)
        {
            HookBackPerformed();
            _hook.ReturnHook();
        }
    }
    private void SwitchThrowHookState()
    {
        _rigidbodyHook.isKinematic = false;
        _isThrow = true;
    }
    private void SwitchTakedHookState()
    {
        _rigidbodyHook.isKinematic = true;
        _isThrow = false;
    }
}
