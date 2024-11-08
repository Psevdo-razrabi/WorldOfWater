using System;
using Data;
using Helpers;
using StateMachine;
using StateMachine.Data;

namespace State
{
    public abstract class PlayerBehaviour : IState
    {
        public PlayerStateMachine StateMachine { get; private set; }
        public PlayerData Player { get; private set; }
        public StateMachineData StateMachineData { get; private set; }

        protected PlayerBehaviour(PlayerStateMachine playerStateMachine)
        {
            Preconditions.CheckNotNull(playerStateMachine);
            StateMachine = playerStateMachine;
            Player = StateMachine.GetData();
            StateMachineData = StateMachine.GetStateMachineData();
            Player.PlayerInputReader.EnablePlayerAction();
        }
        
        public virtual void OnEnter() {}

        public virtual void OnExit() {}

        public virtual void OnUpdateBehaviour() {}

        public virtual void OnFixedUpdateBehaviour() {}

        public virtual bool TrySwapState() => default;
        
        protected virtual void AddActionsCallbacks() {}

        protected virtual void RemoveActionCallbacks() => ClearDelegates();

        public void ClearDelegates(params Action[] clearActions)
        {
            foreach (var action in clearActions)
            {
                action?.Invoke();
            }

            clearActions = null;
        }
    }
}