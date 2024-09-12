using IvoriesStudios.LevelScripting;
using IvoriesStudios.LevelScripting.Attributes;
using IvoriesStudios.LevelScripting.Blackboard;

[NodeInfo("Set Variable", "Process/Set Variable")]
public class SetVariable : ScriptingNode
{
    [ExposedProperty] public string Variable;
    [ExposedProperty] public object Value;
    [ExposedProperty] public Blackboard Blackboard;

    public override string[] OnProcess(LevelScript currentSequence)
    {
        Blackboard.SetVariable(Variable, Value);
        return base.OnProcess(currentSequence);
    }
}
