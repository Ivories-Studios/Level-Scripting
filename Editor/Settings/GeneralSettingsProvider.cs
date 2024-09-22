using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace IvoriesStudios.LevelScripting
{
    public static class GeneralSettingsProvider
    {
        private const string _preferencesPath = "Project/Level Scripting";

        [SettingsProvider]
        private static SettingsProvider CreateSettingsProvider()
        {
            return new SettingsProvider(_preferencesPath, SettingsScope.Project)
            {
                label = "General Settings",
                guiHandler = (searchContext) =>
                {
                    if (GUILayout.Button("Generate Enums"))
                    {
                        GeneralSettings.GenerateEnums();
                    }
                },
                keywords = new HashSet<string>(new[] { "Enums", "Generate" })
            };
        }
    }
}