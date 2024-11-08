using UnityEngine;

namespace Input
{
    public interface IInputReader
    {
        Vector3 Direction { get; }
        void EnablePlayerAction();
    }
}