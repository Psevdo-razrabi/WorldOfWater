using StateMachine;
using StateMachine.Data;
using StateMachine.Events;

public class StateMachineInject : BaseBindings
{
    public override void InstallBindings()
    {
        BindStateMachine();
    }

    private void BindStateMachine()
    {
        BindNewInstance<PlayerStateMachine>();
        BindNewInstance<StateMachineEvent>();
        BindNewInstance<StateMachineData>();
    }
}
