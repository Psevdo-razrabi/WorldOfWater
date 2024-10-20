using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Player
{
    public class PlayerStateMachine
    {
        private List<State> _states;
        private State _currentState;

        public PlayerStateMachine(List<State> states)
        {
            _states = states;
            _currentState = _states[0];
            _currentState.Enter();
        }

        public void Tick()
        {
            _currentState.Tick();
        }

        public PlayerStateMachine TrySwitchState<T>() where T : State
        {
            if (!(_currentState is T))
            {
                var state = _states.FirstOrDefault(s => s is T);

                if (state.CanHandle())
                {
                    _currentState.Exit();
                    _currentState = state;
                    _currentState.Enter();

                    Debug.Log("Switch state: " + _currentState.GetType().Name);
                }
            }

            return this;
        }
    }
}
