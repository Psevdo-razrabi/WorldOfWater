using System;
using UniRx;

namespace Sync
{
    public static class ProjectActions
    {
        public static Subject<TypeSync> OnTypeLoad = new();
    }
}