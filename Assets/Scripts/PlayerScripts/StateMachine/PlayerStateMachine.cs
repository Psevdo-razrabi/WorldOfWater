using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using State;
using StateMachine.Data;
using Zenject;

namespace StateMachine
{
    public class PlayerStateMachine : ITickable, IInitializable, IDisposable, IFixedTickable
    {
        private List<IState> _states;
        private StateMachine _stateMachine;
        private StateMachineData _stateMachineData;
        private readonly List<IDisposable> _disposables = new();

        public void AddDispose(IDisposable disposable)
        {
            _disposables.Add(disposable);
        }

        public PlayerData GetData() => _stateMachineData.Data;
        public StateMachineData GetStateMachineData() => _stateMachineData;
        public StateMachine GetStateMachine() => _stateMachine;

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
        
        public void FixedTick()
        {
            _stateMachine.currentStates.OnFixedUpdateBehaviour();
        }
        
        public void Initialize()
        {
            _states = new()
            {
                new PlayerIdle(this),
                new PlayerMovement(this),
                new PlayerRising(this),
                new PlayerSliding(this),
                new PlayerFalling(this),
                new PlayerJumping(this)
            };
            
            _stateMachine = new StateMachine(_states);
            _stateMachine.SwitchStates<PlayerIdle>();
            _stateMachineData.SetPlayerStateMachine(_stateMachine);
        }

        public void Dispose()
        {
            foreach (var dispose in _disposables)
            {
                dispose.Dispose();
            }
        }

        [Inject]
        private void Construct(StateMachineData stateMachineData)
        {
            _stateMachineData = stateMachineData;
        }
    }
}