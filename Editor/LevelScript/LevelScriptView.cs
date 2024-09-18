using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace IvoriesStudios.LevelScripting.Editor
{
    public class LevelScriptView : GraphView
    {
        #region Variables
        private LevelScript _levelScript;
        private SerializedObject _serializedObject;
        private LevelScriptEditorWindow _window;
        private LevelScriptWindowSearchProvider _searchProvider;
        #endregion

        #region Properties
        public LevelScriptEditorWindow Window => _window;
        public List<EditorScriptingNode> ScriptingNodes { get; private set; }
        public Dictionary<string, EditorScriptingNode> NodeDictionary { get; private set; }
        public Dictionary<Edge, ScriptingNodeConnection> ConnectionDictionary { get; private set; }
        #endregion

        #region Lifecycle
        public LevelScriptView(SerializedObject serializedObject, LevelScriptEditorWindow window)
        {
            _serializedObject = serializedObject;
            _levelScript = serializedObject.targetObject as LevelScript;
            _window = window;

            ScriptingNodes = new List<EditorScriptingNode>();
            NodeDictionary = new Dictionary<string, EditorScriptingNode>();
            ConnectionDictionary = new Dictionary<Edge, ScriptingNodeConnection>();

            _searchProvider = ScriptableObject.CreateInstance<LevelScriptWindowSearchProvider>();
            _searchProvider.Sequence = this;
            nodeCreationRequest += ShowSearchWindow;

            StyleSheet style = AssetDatabase.LoadAssetAtPath<StyleSheet>("Packages/com.ivories-studios.level-scripting/Editor/USS/LevelScriptEditor.uss");
            styleSheets.Add(style);

            GridBackground gridBackground = new GridBackground();
            gridBackground.name = "Grid";
            Add(gridBackground);
            gridBackground.SendToBack();

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ClickSelector());
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new EdgeManipulator());

            DrawNodes();
            DrawConnections();

            graphViewChanged += OnGraphViewChanged;
        }
        #endregion

        #region Events
        private void ShowSearchWindow(NodeCreationContext obj)
        {
            _searchProvider.Target = (VisualElement)focusController.focusedElement;
            SearchWindow.Open(new SearchWindowContext(obj.screenMousePosition), _searchProvider);
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            if (graphViewChange.movedElements != null)
            {
                Undo.RecordObject(_serializedObject.targetObject, "Moved Scripting Node");
                foreach (EditorScriptingNode node in graphViewChange.movedElements.OfType<EditorScriptingNode>())
                {
                    node.SavePosition();
                }
            }
            if (graphViewChange.elementsToRemove != null)
            {
                Undo.RecordObject(_serializedObject.targetObject, "Removed Scripting Node");
                List<EditorScriptingNode> nodesToRemove = graphViewChange.elementsToRemove.OfType<EditorScriptingNode>().ToList();
                for (int i = nodesToRemove.Count - 1; i >= 0; i--)
                {
                    RemoveNode(nodesToRemove[i]);
                }

                foreach (Edge edge in graphViewChange.elementsToRemove.OfType<Edge>())
                {
                    RemoveConnection(edge);
                }
            }
            if (graphViewChange.edgesToCreate != null)
            {
                Undo.RecordObject(_serializedObject.targetObject, "Added Scripting Connections");
                foreach (Edge edge in graphViewChange.edgesToCreate)
                {
                    CreateEdge(edge);
                }
            }

            return graphViewChange;
        }
        #endregion

        #region Public Methods
        public void Add(ScriptingNode node)
        {
            Undo.RecordObject(_serializedObject.targetObject, "Added Scripting Node");
            _levelScript.Nodes.Add(node);
            _serializedObject.Update();

            AddNodeToGraph(node);
            Bind();
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> allPorts = new List<Port>();
            List<Port> compatiblePorts = new List<Port>();
            foreach (EditorScriptingNode node in ScriptingNodes)
            {
                allPorts.AddRange(node.Ports);
            }

            foreach (Port port in allPorts)
            {
                if (startPort != port && startPort.node != port.node && startPort.direction != port.direction && startPort.portType == port.portType)
                {
                    compatiblePorts.Add(port);
                }
            }

            return compatiblePorts;
        }
        #endregion

        #region Private Methods
        private void AddNodeToGraph(ScriptingNode node)
        {
            node.TypeName = node.GetType().AssemblyQualifiedName;
            EditorScriptingNode editorNode = new EditorScriptingNode(node, _serializedObject);
            editorNode.SetPosition(node.Position);
            ScriptingNodes.Add(editorNode);
            NodeDictionary.Add(node.Id, editorNode);

            AddElement(editorNode);
        }

        private void DrawNodes()
        {
            foreach (ScriptingNode node in _levelScript.Nodes)
            {
                AddNodeToGraph(node);
            }
            Bind();
        }

        private void RemoveNode(EditorScriptingNode editorScriptingNode)
        {
            _levelScript.Nodes.Remove(editorScriptingNode.Node);
            NodeDictionary.Remove(editorScriptingNode.Node.Id);
            ScriptingNodes.Remove(editorScriptingNode);
            _serializedObject.Update();
        }

        private void DrawConnections()
        {
            if (_levelScript.Connections == null) return;
            foreach (ScriptingNodeConnection connection in _levelScript.Connections)
            {
                DrawConnection(connection);
            }
        }

        private void DrawConnection(ScriptingNodeConnection connection)
        {
            EditorScriptingNode inputNode = GetNode(connection.InputPort.NodeId);
            EditorScriptingNode outputNode = GetNode(connection.OutputPort.NodeId);

            if (inputNode == null || outputNode == null) return;

            Port inputPort = inputNode.Ports[connection.InputPort.PortIndex];
            Port outputPort = outputNode.Ports[connection.OutputPort.PortIndex];
            Edge edge = inputPort.ConnectTo(outputPort);
            AddElement(edge);
            ConnectionDictionary.Add(edge, connection);
        }

        private void RemoveConnection(Edge edge)
        {
            if (ConnectionDictionary.TryGetValue(edge, out ScriptingNodeConnection connection))
            {
                _levelScript.Connections.Remove(connection);
                ConnectionDictionary.Remove(edge);
            }
        }

        private EditorScriptingNode GetNode(string guid)
        {
            NodeDictionary.TryGetValue(guid, out EditorScriptingNode node);
            return node;
        }

        private void CreateEdge(Edge edge)
        {
            EditorScriptingNode inputNode = edge.input.node as EditorScriptingNode;
            int inputIndex = inputNode.Ports.IndexOf(edge.input);

            EditorScriptingNode outputNode = edge.output.node as EditorScriptingNode;
            int outputIndex = outputNode.Ports.IndexOf(edge.output);

            ScriptingNodeConnection connection = new ScriptingNodeConnection(inputNode.Node.Id, inputIndex, outputNode.Node.Id, outputIndex);
            _levelScript.Connections.Add(connection);
        }

        private void Bind()
        {
            _serializedObject.Update();
            this.Bind(_serializedObject);
        }
        #endregion
    }
}
