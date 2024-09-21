using UnityEditor;

namespace IvoriesStudios.LevelScripting
{
    public static class LevelSettingsProvider
    {
        private const string _preferencesPath = "Project/Level Scripting/Level";

        [SettingsProvider]
        private static SettingsProvider CreateSettingsProvider()
        {
            return new SettingsProvider(_preferencesPath,
                SettingsScope.Project)
            {
                guiHandler = (searchContext) => LevelSettings.GUI(searchContext)
            };
        }
    }
}
