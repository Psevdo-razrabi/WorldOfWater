using UnityEngine;

namespace NewInput
{
    public interface IInputReader
    {
        Vector3 Direction { get; }
        void EnablePlayerAction();
    }
}