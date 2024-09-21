using UnityEditor;

namespace IvoriesStudios.LevelScripting
{
    public static class LevelScriptingSettingsManager
    {
        internal const string PackageName = "com.ivories-studios.level-scripting";

        private static UnityEditor.SettingsManagement.Settings _instance;

        public static UnityEditor.SettingsManagement.Settings Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UnityEditor.SettingsManagement.Settings(PackageName);

                return _instance;
            }
        }

        public static void Save()
        {
            Instance.Save();
        }

        public static T Get<T>(string key, SettingsScope scope = SettingsScope.Project, T fallback = default(T))
        {
            return Instance.Get<T>(key, scope, fallback);
        }

        public static void Set<T>(string key, T value, SettingsScope scope = SettingsScope.Project)
        {
            Instance.Set<T>(key, value, scope);
        }

        public static bool ContainsKey<T>(string key, SettingsScope scope = SettingsScope.Project)
        {
            return Instance.ContainsKey<T>(key, scope);
        }
    }
}
