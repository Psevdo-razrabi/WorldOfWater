using R3;
using System;

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
}
