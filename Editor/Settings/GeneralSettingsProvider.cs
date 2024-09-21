using UnityEditor;

namespace IvoriesStudios.LevelScripting
{
    public static class GeneralSettingsProvider
    {
        private const string _preferencesPath = "Project/Level Scripting";

        [SettingsProvider]
        private static SettingsProvider CreateSettingsProvider()
        {
            return new SettingsProvider(_preferencesPath,
                SettingsScope.Project)
            { 
                guiHandler = (searchContext) => GeneralSettings.GUI(searchContext)
            };
        }
    }
}
