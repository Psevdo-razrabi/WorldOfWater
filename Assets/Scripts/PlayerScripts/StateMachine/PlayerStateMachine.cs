using System;
using System.Collections.Generic;
using System.Linq;
using State;
using Zenject;

namespace StateMachine
{
    public class PlayerStateMachine : ITickable, IInitializable, IDisposable
    {
        private List<IState> _states;
        private StateMachine _stateMachine;
        private readonly List<IDisposable> _disposables = new();

        public void AddDispose(IDisposable disposable)
        {
            _disposables.Add(disposable);
        }

        public void TrySwapState<T>() where T : IState
        {
            if (_stateMachine.currentStates is T == false)
            {
                var state = _stateMachine.States.Values.FirstOrDefault(typeState => typeState is T);

                if (state == null) throw new ArgumentNullException("Dont find state");

                if (state.TrySwapState())
                {
                    _stateMachine.SwitchStates<T>();
                }
            }
        }

        public void Tick()
        {
            _stateMachine.currentStates.OnUpdateBehaviour();
        }
        
        public void Initialize()
        {
            _states = new()
            {
                new PlayerIdle(this),
                new PlayerMovement(this),
            };
            
            _stateMachine = new StateMachine(_states);
        }

        public void Dispose()
        {
            foreach (var dispose in _disposables)
            {
                dispose.Dispose();
            }
        }
    }
}