using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using IvoriesStudios.LevelScripting.Attributes;
using UnityEngine;

namespace IvoriesStudios.LevelScripting.Editor
{
    public class EditorScriptingNode : Node
    {
        #region Variables
        private ScriptingNode _node;
        private Port _outputPort;
        private List<Port> _ports;
        private SerializedProperty _serializedProperty;
        private SerializedObject _serializedObject;
        #endregion

        #region Properties
        public ScriptingNode Node => _node;
        public List<Port> Ports => _ports;
        #endregion

        #region Lifecycle
        public EditorScriptingNode(ScriptingNode node, SerializedObject levelScript)
        {
            AddToClassList("scripting-node");

            _serializedObject = levelScript;
            _node = node;

            Type typeInfo = node.GetType();
            NodeInfoAttribute att = typeInfo.GetCustomAttribute<NodeInfoAttribute>();

            title = att.Title;

            _ports = new List<Port>();

            string[] depths = att.MenuItem.Split('/');
            foreach (string depth in depths)
            {
                AddToClassList(depth.ToLower().Replace(' ', '-'));
            }
            name = typeInfo.Name;

            CreateFlowOutput(att.OutputType);
            CreateFlowInput(att.InputType);

            foreach (FieldInfo property in typeInfo.GetFields())
            {
                if (property.GetCustomAttribute<ExposedPropertyAttribute>() is ExposedPropertyAttribute exposedProperty)
                {
                    PropertyField field = DrawProperty(property.Name);
                    //field.RegisterValueChangeCallback(OnFieldChangeCallback);
                }
            }

            RefreshExpandedState();
        }
        #endregion

        #region Public Methods
        public void SavePosition()
        {
            _node.SetPosition(GetPosition());
        }
        #endregion

        #region Private Methods
        private void CreateFlowInput(IOTypes type)
        {
            if (type == IOTypes.None) return;
            Port inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, type == IOTypes.Multiple ? Port.Capacity.Multi : Port.Capacity.Single, typeof(PortTypes.FlowPort));
            inputPort.portName = "Input";
            inputPort.tooltip = "Flow Input";
            _ports.Add(inputPort);
            inputContainer.Add(inputPort);
        }

        private void CreateFlowOutput(IOTypes type)
        {
            if (type == IOTypes.None) return;
            _outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, type == IOTypes.Multiple ? Port.Capacity.Multi : Port.Capacity.Single, typeof(PortTypes.FlowPort));
            _outputPort.portName = "Out";
            _outputPort.tooltip = "Flow Output";
            _ports.Add(_outputPort);
            outputContainer.Add(_outputPort);
        }

        private PropertyField DrawProperty(string propertyName)
        {
            if (_serializedProperty == null)
            {
                FetchSerializedProperty();
            }
            SerializedProperty prop = _serializedProperty.FindPropertyRelative(propertyName);
            PropertyField field = new PropertyField(prop);
            field.bindingPath = prop.propertyPath;
            extensionContainer.Add(field);
            return field;
        }

        private void FetchSerializedProperty()
        {
            SerializedProperty nodes = _serializedObject.FindProperty("_nodes");
            if (nodes.isArray)
            {
                int size = nodes.arraySize;
                for (int i = 0; i < size; i++)
                {
                    SerializedProperty node = nodes.GetArrayElementAtIndex(i);
                    if (node.FindPropertyRelative("_guid").stringValue == _node.Id)
                    {
                        _serializedProperty = node;
                        break;
                    }
                }
            }
        }
        #endregion
    }
}
