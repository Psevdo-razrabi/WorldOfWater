using System;
using UniRx;

public static class ObservableExtension
{
    public static IDisposable Subscribe<T>(
        this IObservable<T> source,
        Action onNext)
    {
        return source.Subscribe(_ => onNext.Invoke());
    }
}
