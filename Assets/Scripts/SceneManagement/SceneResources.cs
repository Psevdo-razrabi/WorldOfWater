using System;
using System.Collections.Generic;

namespace SceneManagment
{
    public class SceneResources
    {
        private List<Action> _objectsToRelease = new();
        public IReadOnlyList<Action> ObjectToRelease => _objectsToRelease;

        public void AddToListDelegate(Action action) => _objectsToRelease.Add(action);

        public void ClearObject() => _objectsToRelease.Clear();
    }
}