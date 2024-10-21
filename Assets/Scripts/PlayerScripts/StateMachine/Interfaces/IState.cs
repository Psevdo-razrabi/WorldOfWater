namespace StateMachine
{
    public interface IState
    {
        void OnEnter();
        void OnExit();
        void OnUpdateBehaviour();
        void OnFixedUpdateBehaviour();
    }
}