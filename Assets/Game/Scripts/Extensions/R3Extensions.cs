using R3;
using System;

public static class R3Extensions
{
    public static IDisposable Subscribe(this ReactiveCommand command, Action action)
    {
        Action<Unit> actionWithUnit = _ => action();
        return command.Subscribe(actionWithUnit);
    }

    public static void Execute(this ReactiveCommand command)
    {
        command.Execute(Unit.Default);
    }
}
