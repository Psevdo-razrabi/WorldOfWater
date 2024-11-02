using StateMachine;

namespace State
{
    public interface IState
    {
        PlayerStateMachine StateMachine { get; }
        void OnEnter();
        void OnExit();
        void OnUpdateBehaviour();
        void OnFixedUpdateBehaviour();
        bool TrySwapState();
    }
}