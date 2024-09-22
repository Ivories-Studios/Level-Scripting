using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace IvoriesStudios.LevelScripting
{
    public class LevelSettings : ScriptableObject
    {
        private const string _levelSettingsPath = "Assets/LevelScripting/Resources/Settings/LevelSettings.asset";

        public bool ShowObjectives = false;
        public List<string> Triggers;

        private static LevelSettings _instance = null;
        public static LevelSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<LevelSettings>("Settings/LevelSettings");
                }
#if UNITY_EDITOR
                if (_instance == null)
                {
                    _instance = ScriptableObject.CreateInstance<LevelSettings>();
                    _instance.Triggers = new List<string>();
                    Directory.CreateDirectory("Assets/LevelScripting/Resources/Settings");
                    AssetDatabase.CreateAsset(_instance, _levelSettingsPath);
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
    }
}
