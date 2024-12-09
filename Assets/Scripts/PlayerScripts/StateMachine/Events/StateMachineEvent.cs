using System;
using UnityEngine;

namespace StateMachine.Events
{
    public class StateMachineEvent
    {
        public event Action<Vector3> OnLand = delegate { };
        public event Action<Vector3> OnJump = delegate { };
        public event Action<int> OnGateState = delegate { };
        public event Action<bool> OnWalk = delegate { };
        public event Action<bool> OnStartMove = delegate { }; 
        
        public void InvokeLand(Vector3 vector)
        {
            OnLand?.Invoke(vector);
        }

        public void InvokeJump(Vector3 vector)
        {
            OnJump?.Invoke(vector);
        }

        public void InvokeGateState(int state)
        {
            OnGateState?.Invoke(state);
        }

        public void InvokeWalk(bool enable)
        {
            OnWalk?.Invoke(enable);
        }

        public void InvokeStartMove(bool obj)
        {
            OnStartMove?.Invoke(obj);
        }
    }
}