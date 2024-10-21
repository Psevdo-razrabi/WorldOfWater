using R3;
using UniRx;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerCameraSettings : MonoBehaviour
{
    [field: SerializeField] public FloatReactiveProperty MouseX { get; private set; }
    [field: SerializeField] public FloatReactiveProperty MouseY { get; private set; }
    [field: SerializeField] public FloatReactiveProperty FOV { get; private set; }
    [field: SerializeField] public CinemachineCamera Camera { get; private set; }
    [field: SerializeField] public CinemachineInputAxisController InputAxisController { get; private set; }

    private void OnEnable()
    {
        SubscribeProperty();
    }

    private void SubscribeProperty()
    {
        MouseX.SkipLatestValueOnSubscribe().Subscribe(SetMouseX).AddTo(this);
        MouseY.SkipLatestValueOnSubscribe().Subscribe(SetMouseY).AddTo(this);
        FOV.SkipLatestValueOnSubscribe().Subscribe(SetFOV).AddTo(this);
    }

    private void SetMouseX(float mouseX)
    {
        InputAxisController.Controllers[0].Input.LegacyGain = mouseX;
    }
    
    private void SetMouseY(float mouseX)
    {
        InputAxisController.Controllers[1].Input.LegacyGain = mouseX;
    }

    private void SetFOV(float fov)
    {
        Camera.Lens.FieldOfView = fov;
    }
}