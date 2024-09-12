namespace IvoriesStudios.LevelScripting
{
    [System.Serializable]
    public struct ScriptingNodeConnection
    {
        public ScriptingNodeConnectionPort InputPort;
        public ScriptingNodeConnectionPort OutputPort;

        public ScriptingNodeConnection(ScriptingNodeConnectionPort inputPort, ScriptingNodeConnectionPort outputPort)
        {
            InputPort = inputPort;
            OutputPort = outputPort;
        }

        public ScriptingNodeConnection(string inputNodeId, int inputPortIndex, string outputNodeId, int outputPortIndex)
        {
            InputPort = new ScriptingNodeConnectionPort(inputNodeId, inputPortIndex);
            OutputPort = new ScriptingNodeConnectionPort(outputNodeId, outputPortIndex);
        }
    }

    [System.Serializable]
    public struct ScriptingNodeConnectionPort
    {
        public string NodeId;
        public int PortIndex;

        public ScriptingNodeConnectionPort(string nodeId, int portIndex)
        {
            NodeId = nodeId;
            PortIndex = portIndex;
        }
    }
}
