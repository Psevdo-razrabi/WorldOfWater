using StateMachine;
using StateMachine.Data;

public class StateMachineInject : BaseBindings
{
    public override void InstallBindings()
    {
        BindStateMachine();
    }

    private void BindStateMachine()
    {
        BindNewInstance<PlayerStateMachine>();
        BindNewInstance<StateMachineData>();
    }
}
