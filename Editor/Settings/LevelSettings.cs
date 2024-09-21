using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.SettingsManagement;

namespace IvoriesStudios.LevelScripting
{
    public class LevelSettings : EditorWindow
    {
        private static UserSetting<bool> _showObjectives = new(LevelScriptingSettingsManager.Instance, "level.showObjectives", false, SettingsScope.Project);
        private static UserSetting<List<string>> _triggers = new(LevelScriptingSettingsManager.Instance, "level.triggers", new List<string>(), SettingsScope.Project);

        private static bool _showTriggers = true;
        private static Vector2 _scrollPos;

        private void OnEnable()
        {
            _triggers.value = LevelScriptingSettingsManager.Get<List<string>>("level.triggers") ?? new List<string>();
            _showObjectives.value = LevelScriptingSettingsManager.Get<bool>("level.showObjectives");
        }

        public static void GUI(string searchContext)
        {
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

            _showObjectives.value = EditorGUILayout.Toggle("Show Objectives", _showObjectives.value);
            _showTriggers = EditorGUILayout.Foldout(_showTriggers, "Triggers");
            if (_showTriggers)
            {
                DrawTriggers();
            }

            EditorGUILayout.EndScrollView();

            _triggers.SetValue(_triggers.value, true);
            _showObjectives.SetValue(_showObjectives.value, true);
        }

        private static void DrawTriggers()
        {
            for (int i = 0; i < _triggers.value.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(15);
                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);

                _triggers.value[i] = EditorGUILayout.TextField(_triggers.value[i], GUILayout.Width(300));

                EditorGUILayout.Space(1, true);
                if (GUILayout.Button("Remove", GUILayout.Width(100)))
                {
                    _triggers.value.RemoveAt(i);
                    i--;
                }

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
            }

            GUIStyle buttonStyle = new GUIStyle(EditorStyles.miniButton);
            buttonStyle.margin = new RectOffset(15, 0, 0, 0);
            if (GUILayout.Button("Add Trigger", buttonStyle))
            {
                _triggers.value.Add("");
            }
        }
    }
}
