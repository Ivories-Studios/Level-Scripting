using UnityEngine;
using UnityEditor;
using IvoriesStudios.LevelScripting.Blackboard;

[CustomEditor(typeof(Blackboard))]
public class BlackboardEditor : Editor
{
    private Blackboard _blackboard;
    private SerializedProperty _variables;

    private void OnEnable()
    {
        _blackboard = (Blackboard)target;
        _variables = serializedObject.FindProperty("_variables");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Variables", EditorStyles.boldLabel);
        for (int i = 0; i < _variables.arraySize; i++)
        {
            SerializedProperty variable = _variables.GetArrayElementAtIndex(i);
            SerializedProperty nameProp = variable.FindPropertyRelative("<Name>k__BackingField");
            SerializedProperty typeProp = variable.FindPropertyRelative("<Type>k__BackingField");

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(nameProp, GUIContent.none);
            VariableType newType = (VariableType)EditorGUILayout.EnumPopup((VariableType)typeProp.enumValueIndex);
            EditorGUILayout.EndHorizontal();

            Variable variableRef = _blackboard.GetVariable(i);
            VariableType type = (VariableType)typeProp.enumValueIndex;
            if (type != newType)
            {
                typeProp.enumValueIndex = (int)newType;
                variableRef.Value = Variable.GetDefault(newType);
                serializedObject.ApplyModifiedProperties();
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Value", GUILayout.Width(50));

            try
            {
                switch (type)
                {
                    case VariableType.Integer:
                        variableRef.Value = EditorGUILayout.IntField((int)variableRef.Value);
                        break;
                    case VariableType.Float:
                        variableRef.Value = EditorGUILayout.FloatField((float)variableRef.Value);
                        break;
                    case VariableType.String:
                        variableRef.Value = EditorGUILayout.TextField((string)variableRef.Value);
                        break;
                    case VariableType.Boolean:
                        variableRef.Value = EditorGUILayout.Toggle((bool)variableRef.Value);
                        break;
                    default:
                        EditorGUILayout.LabelField("Unsupported type");
                        break;
                }
            }
            catch { }

            if (GUILayout.Button("Remove", GUILayout.Width(60)))
            {
                _variables.DeleteArrayElementAtIndex(i);
                break;
            }
            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add Variable"))
        {
            _variables.arraySize++;
            SerializedProperty newVariable = _variables.GetArrayElementAtIndex(_variables.arraySize - 1);
            newVariable.FindPropertyRelative("<Name>k__BackingField").stringValue = "New Variable";
            newVariable.FindPropertyRelative("<Type>k__BackingField").enumValueIndex = 0;
            serializedObject.ApplyModifiedProperties();
            (_blackboard.GetVariable(_variables.arraySize - 1)).Value = Variable.GetDefault(VariableType.Integer);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
