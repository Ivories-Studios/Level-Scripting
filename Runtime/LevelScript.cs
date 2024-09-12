using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using IvoriesStudios.LevelScripting.Process;
using IvoriesStudios.LevelScripting.Level;
using System;

namespace IvoriesStudios.LevelScripting
{
    [CreateAssetMenu(fileName = "NewLevelScript", menuName = "Level Scripting/Level Script")]
    public class LevelScript : ScriptableObject
    {
        #region Variables
        [SerializeReference] private List<ScriptingNode> _nodes;
        [SerializeField] private List<ScriptingNodeConnection> _connections;
        private Dictionary<string, ScriptingNode> _nodeDictionary;
        #endregion

        #region Properties
        public List<ScriptingNode> Nodes => _nodes;
        public List<ScriptingNodeConnection> Connections => _connections;
        #endregion

        #region Lifecycle
        public LevelScript()
        {
            _nodes = new List<ScriptingNode>();
            _connections = new List<ScriptingNodeConnection>();
        }
        #endregion

        #region Public Methods
        public void Init()
        {
            _nodeDictionary = new Dictionary<string, ScriptingNode>();
            foreach (ScriptingNode node in Nodes)
            {
                _nodeDictionary.Add(node.Id, node);
            }
        }

        public ScriptingNode GetStartNode()
        {
            Start startNode = Nodes.OfType<Start>().First();
            if (startNode == null)
            {
                Debug.LogError("No start node found in the level script.");
                return null;
            }
            return startNode;
        }

        public ScriptingNode Trigger(Triggers tag)
        {
            Trigger[] triggerNodes = Nodes.OfType<Trigger>().ToArray();
            foreach (Trigger trigger in triggerNodes)
            {
                if (trigger.Name == tag)
                {
                    return trigger;
                }
            }
            Debug.LogWarning($"No trigger with the tag {tag} founde.");
            return null;
        }

        public ScriptingNode GetNode(string nodeId)
        {
            if (_nodeDictionary.TryGetValue(nodeId, out ScriptingNode node))
            {
                return node;
            }
            return null;
        }

        public List<ScriptingNode> GetNodeFromOutput(string outputNodeId, int index)
        {
            List<ScriptingNode> nodes = new List<ScriptingNode>();
            foreach (ScriptingNodeConnection connection in _connections)
            {
                if (connection.OutputPort.NodeId == outputNodeId && connection.OutputPort.PortIndex == index)
                {
                    string nodeId = connection.InputPort.NodeId;
                    ScriptingNode inputNode = _nodeDictionary[nodeId];
                    nodes.Add(inputNode);
                }
            }

            return nodes;
        }
        #endregion
    }
}
