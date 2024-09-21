using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.SettingsManagement;

namespace IvoriesStudios.LevelScripting.Dialogue
{
    public class DialogueSettings : EditorWindow
    {
        private static UserSetting<List<Character>> _characterList = new(LevelScriptingSettingsManager.Instance, "dialogue.characterList", new List<Character>(), SettingsScope.Project);

        private static bool _showCharacters = true;
        private static Vector2 _scrollPos;

        private void OnEnable()
        {
            _characterList.value = LevelScriptingSettingsManager.Get<List<Character>>("dialogue.characterList") ?? new List<Character>();
        }

        public static void GUI(string searchContext)
        {
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

            _showCharacters = EditorGUILayout.Foldout(_showCharacters, "Characters");
            if (_showCharacters)
            {
                DrawCharacters();
            }

            EditorGUILayout.EndScrollView();

            _characterList.SetValue(_characterList.value, true);
        }

        private static void DrawCharacters()
        {
            for (int i = 0; i < _characterList.value.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(15);
                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);

                GUILayout.Label("Name", GUILayout.Width(70));
                _characterList.value[i].Name = EditorGUILayout.TextField(_characterList.value[i].Name, GUILayout.Width(200));

                GUILayout.Space(10);
                GUILayout.Label("Portrait", GUILayout.Width(70));
                _characterList.value[i].Portrait = (Sprite)EditorGUILayout.ObjectField(_characterList.value[i].Portrait, typeof(Sprite), allowSceneObjects: false, GUILayout.Width(50), GUILayout.Height(50));

                EditorGUILayout.Space(1, true);
                if (GUILayout.Button("Remove", GUILayout.Width(100)))
                {
                    _characterList.value.RemoveAt(i);
                    i--;
                }

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
            }

            GUIStyle buttonStyle = new GUIStyle(EditorStyles.miniButton);
            buttonStyle.margin = new RectOffset(15, 0, 0, 0);
            if (GUILayout.Button("Add Character", buttonStyle))
            {
                _characterList.value.Add(new Character());
            }
        }
    }
}
