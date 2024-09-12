using IvoriesStudios.LevelScripting.Attributes;

namespace IvoriesStudios.LevelScripting.Level
{
    [NodeInfo("Add Objective", "Level/Add Objective")]
    public class AddObjective : ScriptingNode
    {
        [ExposedProperty] public LevelObjective Objective;

        public override string[] OnProcess(LevelScript currentSequence)
        {
            LevelObject.AddObjective(Objective);
            return base.OnProcess(currentSequence);
        }
    }
}
