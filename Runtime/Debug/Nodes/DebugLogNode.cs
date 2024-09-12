using UnityEngine;
using IvoriesStudios.LevelScripting.Attributes;

namespace IvoriesStudios.LevelScripting.Process
{
    [NodeInfo("Debug Log", "Debug/Debug Log Console")]
    public class DebugLogNode : ScriptingNode
    {
        [ExposedProperty]
        public string LogMessage;

        public override string[] OnProcess(LevelScript currentSequence)
        {
            Debug.Log(LogMessage);
            return base.OnProcess(currentSequence);
        }
    }
}
