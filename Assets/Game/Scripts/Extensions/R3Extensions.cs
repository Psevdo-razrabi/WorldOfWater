using R3;
using System;
using ObservableCollections;

public static class R3Extensions
{
    public static IDisposable Subscribe(this ReactiveCommand command, Action action)
    {
        return command.Subscribe(_ => action());
    }

    public static void Execute(this ReactiveCommand command)
    {
        command.Execute(Unit.Default);
    }

    public static void OnNext(this Subject<Unit> subject)
    {
        subject.OnNext(Unit.Default);
    }

    public static IDisposable Subscribe(this Observable<Unit> observable, Action action)
    {
        return observable.Subscribe(_ => action());
    }
}