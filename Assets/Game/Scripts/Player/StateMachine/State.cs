namespace Game.Player
{
    public class State
    {
        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void Tick() { }
        public virtual bool CanHandle() { return default; }
    }
}
