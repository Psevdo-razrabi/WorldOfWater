using System;
using UnityEngine;

namespace Sources.Scripts.Tools.Hook.Spear
{
    public class Spear : Tool
    {
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent(out IHealth iHealth))
            {
                //логика кароч дамага
            }
        }
    }
}