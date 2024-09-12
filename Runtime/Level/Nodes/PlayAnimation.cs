using IvoriesStudios.LevelScripting.Attributes;
using UnityEngine;

namespace IvoriesStudios.LevelScripting.Level
{
    [NodeInfo("Play Animation", "Level/Play Animation")]
    public class PlayAnimation : ScriptingNode
    {
        [ExposedProperty] public string ReferenceTag;
        [ExposedProperty] public string TriggerName;

        public override string[] OnProcess(LevelScript currentSequence)
        {
            LevelObject.GetSceneReference<Animator>(ReferenceTag).SetTrigger(TriggerName);
            return base.OnProcess(currentSequence);
        }
    }
}
