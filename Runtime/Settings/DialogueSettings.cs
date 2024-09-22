using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace IvoriesStudios.LevelScripting.Dialogue
{
    public class DialogueSettings : ScriptableObject
    {
        private const string _dialogueSettingsPath = "Assets/LevelScripting/Resources/Settings/DialogueSettings.asset";

        public List<Character> CharacterList;

        private static DialogueSettings _instance = null;
        public static DialogueSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<DialogueSettings>("Settings/DialogueSettings");
                }
#if UNITY_EDITOR
                if (_instance == null)
                {
                    _instance = ScriptableObject.CreateInstance<DialogueSettings>();
                    _instance.CharacterList = new List<Character>();
                    Directory.CreateDirectory("Assets/LevelScripting/Resources/Settings");
                    AssetDatabase.CreateAsset(_instance, _dialogueSettingsPath);
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

        public static Character GetCharacter(Characters character)
        {
            return Instance.CharacterList[(int)character];
        }
    }
}
