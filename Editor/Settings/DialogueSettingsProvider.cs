using UnityEditor;

namespace IvoriesStudios.LevelScripting.Dialogue
{
    public static class DialogueSettingsProvider
    {
        private const string _preferencesPath = "Project/Level Scripting/Dialogue";

        [SettingsProvider]
        private static SettingsProvider CreateSettingsProvider()
        {
            return new SettingsProvider(_preferencesPath,
                SettingsScope.Project)
            { 
                guiHandler = (searchContext) => DialogueSettings.GUI(searchContext)
            };
        }
    }
}
