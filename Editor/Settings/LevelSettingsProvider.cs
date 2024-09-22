using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace IvoriesStudios.LevelScripting
{
    public static class LevelSettingsProvider
    {
        private const string _preferencesPath = "Project/Level Scripting/Level";
        private static Vector2 _scrollPos;
        private static bool _showTriggers = true;

        [SettingsProvider]
        private static SettingsProvider CreateSettingsProvider()
        {
            return new SettingsProvider(_preferencesPath, SettingsScope.Project)
            {
                label = "Level Settings",
                guiHandler = (searchContext) =>
                {
                    SerializedObject settings = LevelSettings.GetSerializedSettings();
                    SerializedProperty showObjectivesProperty = settings.FindProperty("ShowObjectives");
                    SerializedProperty triggersProperty = settings.FindProperty("Triggers");

                    _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

                    EditorGUILayout.PropertyField(showObjectivesProperty, new GUIContent("Show Objectives"));
                    _showTriggers = EditorGUILayout.Foldout(_showTriggers, "Triggers");
                    if (_showTriggers)
                    {
                        DrawTriggers(triggersProperty);
                    }

                    EditorGUILayout.EndScrollView();
                    if (settings.ApplyModifiedProperties())
                    {
                        EditorUtility.SetDirty(settings.targetObject);
                        AssetDatabase.SaveAssetIfDirty(settings.targetObject);
                    }
                },
                keywords = new HashSet<string>(new[] { "Objectives", "Triggers" })
            };
        }

        private static void DrawTriggers(SerializedProperty triggersProperty)
        {
            for (int i = 0; i < triggersProperty.arraySize; i++)
            {
                SerializedProperty triggerProperty = triggersProperty.GetArrayElementAtIndex(i);

                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(15);
                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);

                triggerProperty.stringValue = EditorGUILayout.TextField(triggerProperty.stringValue, GUILayout.Width(300));

                EditorGUILayout.Space(1, true);
                if (GUILayout.Button("Remove", GUILayout.Width(100)))
                {
                    triggersProperty.DeleteArrayElementAtIndex(i);
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
                triggersProperty.arraySize++;
            }
        }
    }
}
