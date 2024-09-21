using UnityEditor;
using IvoriesStudios.LevelScripting.Dialogue;
using UnityEditor.SettingsManagement;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace IvoriesStudios.LevelScripting
{
    public class GeneralSettings : EditorWindow
    {
        private static UserSetting<string> _path = new(LevelScriptingSettingsManager.Instance, "general.path", "", SettingsScope.Project);

        public static void GUI(string searchContext)
        {
            string path = LevelScriptingSettingsManager.Get<string>("general.path");
            path = EditorGUILayout.TextField("Path:", path);
            _path.SetValue(path, true);

            EditorGUILayout.Space();
            if (GUILayout.Button("Generate"))
            {
                LevelScriptingSettingsManager.Save();
                GenerateEnums();
            }
        }

        public static void GenerateEnums()
        {
            List<Character> characters = LevelScriptingSettingsManager.Get<List<Character>>("dialogue.characterList");
            List<string> characterNames = new List<string>();
            foreach (Character character in characters)
            {
                characterNames.Add(character.Name);
            }

            string enumName = "Characters";
            string enumContent = "namespace IvoriesStudios.LevelScripting.Dialogue\n{\n    public enum " + enumName + "\n    {\n";
            foreach (string name in characterNames)
            {
                enumContent += "        " + name.Replace(" ", "_") + ",\n";
            }
            enumContent += "    }\n}";

            string directory = Path.Combine(_path.value, "Generated");
            string enumFilePath = Path.Combine(directory, enumName + ".cs");

            File.WriteAllText(enumFilePath, enumContent);

            CharacterList characterList = ScriptableObject.CreateInstance<CharacterList>();
            characterList.Initialize(characters);
            AssetDatabase.CreateAsset(characterList, Path.Combine(directory, "CharacterList.asset"));

            List<string> triggers = LevelScriptingSettingsManager.Get<List<string>>("level.triggers");
            enumName = "Triggers";
            enumContent = "namespace IvoriesStudios.LevelScripting.Process\n{\n    public enum " + enumName + "\n    {\n";
            foreach (string name in triggers)
            {
                enumContent += "        " + name.Replace(" ", "_") + ",\n";
            }
            enumContent += "    }\n}";

            enumFilePath = Path.Combine(directory, enumName + ".cs");

            File.WriteAllText(enumFilePath, enumContent);

            AssetDatabase.Refresh();
        }
    }
}
