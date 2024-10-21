namespace Game.Player
{
    public class State
    {
        protected PlayerStateMachine StateMachine { get; private set; }

        public State(PlayerStateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }

        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void Tick() { }
        public virtual bool CanHandle() { return default; }
    }
}
