using System;
using Helpers;
using StateMachine;

namespace State
{
    public abstract class PlayerBehaviour : IState
    {
        public PlayerStateMachine StateMachine { get; private set; }

        protected PlayerBehaviour(PlayerStateMachine playerStateMachine)
        {
            Preconditions.CheckNotNull(playerStateMachine);
            StateMachine = playerStateMachine;
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