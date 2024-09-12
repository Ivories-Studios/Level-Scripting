using IvoriesStudios.LevelScripting.Attributes;

namespace IvoriesStudios.LevelScripting.Dialogue
{
    [NodeInfo("End Dialogue", "Dialogue/End Dialogue")]
    public class EndDialogue : ScriptingNode
    {
        public override string[] OnProcess(LevelScript currentSequence)
        {
            IDialoguePanel.Instance.StopDialogue();
            return base.OnProcess(currentSequence);
        }
    }
}
