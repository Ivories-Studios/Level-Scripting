using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using IvoriesStudios.LevelScripting.Dialogue;

namespace IvoriesStudios.LevelScripting
{
    public class GeneralSettings : ScriptableObject
    {
        private const string _generalSettingsPath = "Assets/LevelScripting/Resources/Settings/GeneralSettings.asset";

        private static GeneralSettings _instance = null;
        public static GeneralSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<GeneralSettings>("Settings/GeneralSettings");
                }
#if UNITY_EDITOR
                if (_instance == null)
                {
                    _instance = ScriptableObject.CreateInstance<GeneralSettings>();
                    Directory.CreateDirectory("Assets/LevelScripting/Resources/Settings");
                    AssetDatabase.CreateAsset(_instance, _generalSettingsPath);
                    AssetDatabase.SaveAssets();
                }
#endif
                return _instance;
            }
        }

        public static SerializedObject GetSerializedSettings()
        {
            return new SerializedObject(Instance);
        }

        public static void GenerateEnums()
        {
            Directory.CreateDirectory("Assets/LevelScripting/Generated");
            List<string> characterNames = new List<string>();
            foreach (Character character in DialogueSettings.Instance.CharacterList)
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
            File.WriteAllText($"Assets/LevelScripting/Generated/{enumName}.cs", enumContent);

            enumName = "Triggers";
            enumContent = "namespace IvoriesStudios.LevelScripting.Process\n{\n    public enum " + enumName + "\n    {\n";
            foreach (string name in LevelSettings.Instance.Triggers)
            {
                enumContent += "        " + name.Replace(" ", "_") + ",\n";
            }
            enumContent += "    }\n}";
            File.WriteAllText($"Assets/LevelScripting/Generated/{enumName}.cs", enumContent);

            AssetDatabase.Refresh();
        }
    }
}
