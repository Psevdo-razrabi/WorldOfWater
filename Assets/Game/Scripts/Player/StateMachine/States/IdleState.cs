namespace Game.Player
{
    public class IdleState : State
    {
        public IdleState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {

        }

        public override void Tick()
        {
            SwitchState();
        }

        public override void Exit()
        {

        }

        public override bool CanHandle()
        {
            return true;
        }

        private void SwitchState()
        {
            StateMachine.TrySwitchState<WalkState>();
            StateMachine.TrySwitchState<JumpState>();
        }
    }
}
