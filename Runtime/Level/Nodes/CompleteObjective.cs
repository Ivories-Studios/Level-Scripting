using IvoriesStudios.LevelScripting.Attributes;

namespace IvoriesStudios.LevelScripting.Level
{
    [NodeInfo("Complete Objective", "Level/Complete Objective")]
    public class CompleteObjective : ScriptingNode
    {
        [ExposedProperty] public int ObjectiveId;

        public override string[] OnProcess(LevelScript currentSequence)
        {
            LevelObject.CompleteObjective(ObjectiveId);
            return base.OnProcess(currentSequence);
        }
    }
}
