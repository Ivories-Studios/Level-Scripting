using UnityEngine;
using IvoriesStudios.LevelScripting.Process;
using IvoriesStudios.Utils.Patterns;
using System.Collections.Generic;
using IvoriesStudios.LevelScripting.Level;
using System;

namespace IvoriesStudios.LevelScripting
{
    public class LevelObject : Singleton<LevelObject>
    {
        #region Editor Variables
        [SerializeField] private LevelScript _levelScript;
        [SerializeField] private List<GameObject> _sceneReferences = new List<GameObject>();
        #endregion

        #region Variables
        private static LevelScript _levelScriptInstance;
        private static ScriptingNode _currentNode;
        private static Dictionary<string, GameObject> _sceneReferencesDict = new Dictionary<string, GameObject>();
        private static List<LevelObjective> _levelObjectives = new List<LevelObjective>();
        #endregion

        #region Properties
        public static Action<LevelObjective[]> ObjectivesChanged { get; set; }
        #endregion

        #region Lifecycle
        public override void Awake()
        {
            base.Awake();
            foreach (GameObject gameObject in _sceneReferences)
            {
                _sceneReferencesDict.Add(gameObject.name, gameObject);
            }
            _levelObjectives.Clear();
        }

        private void Start()
        {
            _levelScriptInstance = Instantiate(_levelScript);
            _levelScriptInstance.Init();
            _currentNode = _levelScriptInstance.GetStartNode();
            ProcessNodes();
        }
        #endregion

        #region Public Methods
        public static T GetSceneReference<T>(string tag) where T : Component
        {
            return _sceneReferencesDict[tag].GetComponent<T>();
        }

        public static void Trigger(Triggers tag)
        {
            ScriptingNode node = _levelScriptInstance.Trigger(tag);
            if (node != null)
            {
                _currentNode = node;
                ProcessNodes();
            }
        }

        public static void ProcessNodes()
        {
            string[] nextNodesId = _currentNode.OnProcess(_levelScriptInstance);
            if (nextNodesId.Length == 0)
            {
                _currentNode = null;
                return;
            }
            ScriptingNode nextNode = null;
            do
            {
                nextNode = _levelScriptInstance.GetNode(nextNodesId[0]);
                nextNodesId = nextNode.OnProcess(_levelScriptInstance);
                if (nextNodesId.Length == 0)
                {
                    _currentNode = null;
                    return;
                }
            } while (!nextNode.IsStopping);
            _currentNode = nextNode;
        }

        public static void AddObjective(LevelObjective objective)
        {
            _levelObjectives.Add(objective);
            ObjectivesChanged.Invoke(_levelObjectives.ToArray());
        }

        public static void CompleteObjective(int objectiveId)
        {
            _levelObjectives.RemoveAll((obj) => obj.Id == objectiveId);
            ObjectivesChanged.Invoke(_levelObjectives.ToArray());
        }
        #endregion
    }
}
