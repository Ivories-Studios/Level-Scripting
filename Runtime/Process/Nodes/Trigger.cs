using IvoriesStudios.LevelScripting.Attributes;

namespace IvoriesStudios.LevelScripting.Process
{
    [NodeInfo("Trigger", "Process/Trigger")]
    public class Trigger : ScriptingNode
    {
        [ExposedProperty] public Triggers Name = 0;
    }
}
