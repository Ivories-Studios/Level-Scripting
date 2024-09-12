using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

namespace IvoriesStudios.LevelScripting.Editor
{
    [CustomEditor(typeof(LevelScript))]
    public class LevelScriptEditor : UnityEditor.Editor
    {
        [OnOpenAsset()]
        public static bool OnOpenAsset(int instanceId, int index)
        {
            Object asset = EditorUtility.InstanceIDToObject(instanceId);
            if (asset.GetType() == typeof(LevelScript))
            {
                LevelScriptEditorWindow.Open((LevelScript)asset);
                return true;
            }
            return false;
        }

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open"))
            {
                LevelScriptEditorWindow.Open((LevelScript)target);
            }
        }
    }
}
