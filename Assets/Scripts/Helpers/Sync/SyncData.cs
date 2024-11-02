namespace Helpers.Sync
{
    public class SyncData
    {
        public bool StartLoad;
        public bool ConfigLoad;
        public bool PrefabLoad;

        public bool IsAllLoaded => ConfigLoad && PrefabLoad;
    }
}