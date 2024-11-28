using UniRx;

public interface ICircleFillState
{
    IReactiveProperty<float> CircleFillValue { get; }
}