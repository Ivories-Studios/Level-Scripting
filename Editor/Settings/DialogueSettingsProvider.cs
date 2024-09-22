using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace IvoriesStudios.LevelScripting.Dialogue
{
    public static class DialogueSettingsProvider
    {
        private const string _preferencesPath = "Project/Level Scripting/Dialogue";
        private static Vector2 _scrollPos;
        private static bool _showCharacters = true;

        [SettingsProvider]
        private static SettingsProvider CreateSettingsProvider()
        {
            return new SettingsProvider(_preferencesPath, SettingsScope.Project)
            {
                label = "Dialogue Settings",
                guiHandler = (searchContext) =>
                {
                    SerializedObject settings = DialogueSettings.GetSerializedSettings();
                    SerializedProperty characterListProperty = settings.FindProperty("CharacterList");

                    _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

                    _showCharacters = EditorGUILayout.Foldout(_showCharacters, "Characters");
                    if (_showCharacters)
                    {
                        DrawCharacters(characterListProperty);
                    }

                    EditorGUILayout.EndScrollView();
                    if (settings.ApplyModifiedProperties())
                    {
                        EditorUtility.SetDirty(settings.targetObject);
                        AssetDatabase.SaveAssetIfDirty(settings.targetObject);
                    }
                },
                keywords = new HashSet<string>(new[] { "Dialogue", "Character", "Portrait" })
            };
        }

        private static void DrawCharacters(SerializedProperty characterListProperty)
        {
            for (int i = 0; i < characterListProperty.arraySize; i++)
            {
                SerializedProperty characterProperty = characterListProperty.GetArrayElementAtIndex(i);
                SerializedProperty nameProperty = characterProperty.FindPropertyRelative("<Name>k__BackingField");
                SerializedProperty portraitProperty = characterProperty.FindPropertyRelative("<Portrait>k__BackingField");

                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(15);
                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);

                GUILayout.Label("Name", GUILayout.Width(70));
                nameProperty.stringValue = EditorGUILayout.TextField(nameProperty.stringValue, GUILayout.Width(200));

                GUILayout.Space(10);
                GUILayout.Label("Portrait", GUILayout.Width(70));
                portraitProperty.objectReferenceValue = (Sprite)EditorGUILayout.ObjectField(portraitProperty.objectReferenceValue, typeof(Sprite), allowSceneObjects: false, GUILayout.Width(50), GUILayout.Height(50));

                EditorGUILayout.Space(1, true);
                if (GUILayout.Button("Remove", GUILayout.Width(100)))
                {
                    characterListProperty.DeleteArrayElementAtIndex(i);
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
                characterListProperty.arraySize++;
            }
        }
    }
}
