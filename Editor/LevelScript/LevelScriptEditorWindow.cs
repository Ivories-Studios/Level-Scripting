using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace IvoriesStudios.LevelScripting.Editor
{
    public class LevelScriptEditorWindow : EditorWindow
    {
        #region Variables
        [SerializeField] private LevelScript _currentSequence;
        [SerializeField] private SerializedObject _serializedObject;
        [SerializeField] private LevelScriptView _currentView;
        #endregion

        #region Properties
        public LevelScript CurrentSequence => _currentSequence;
        #endregion

        #region Lifecycle
        private void OnEnable()
        {
            if (_currentSequence != null)
            {
                DrawGraph();
            }
        }

        private void OnGUI()
        {
            if (_currentSequence != null)
            {
                if (EditorUtility.IsDirty(_currentSequence))
                {
                    hasUnsavedChanges = true;
                }
                else
                {
                    hasUnsavedChanges = false;
                }
            }
        }
        #endregion

        #region Events
        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            EditorUtility.SetDirty(_currentSequence);
            return graphViewChange;
        }
        #endregion

        #region Static Methods
        public static void Open(LevelScript sequence)
        {
            LevelScriptEditorWindow[] windows = Resources.FindObjectsOfTypeAll<LevelScriptEditorWindow>();
            foreach (LevelScriptEditorWindow window in windows)
            {
                if (window._currentSequence == sequence)
                {
                    window.Focus();
                    return;
                }
            }

            LevelScriptEditorWindow editorWindow = CreateWindow<LevelScriptEditorWindow>(typeof(LevelScriptEditorWindow), typeof(SceneView));
            editorWindow.titleContent = new GUIContent($"{sequence.name}", EditorGUIUtility.ObjectContent(null, typeof(LevelScript)).image);
            editorWindow.Load(sequence);
        }
        #endregion

        #region Public Methods
        public void Load(LevelScript sequence)
        {
            _currentSequence = sequence;
            DrawGraph();
        }
        #endregion

        #region Private Methods
        private void DrawGraph()
        {
            _serializedObject = new SerializedObject(_currentSequence);
            _currentView = new LevelScriptView(_serializedObject, this);
            _currentView.graphViewChanged += OnGraphViewChanged;
            rootVisualElement.Add(_currentView);
        }
        #endregion
    }
}
