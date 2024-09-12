using IvoriesStudios.LevelScripting.Attributes;

namespace IvoriesStudios.LevelScripting.Dialogue
{
    [NodeInfo("Start Dialogue", "Dialogue/Start Dialogue")]
    public class StartDialogue : ScriptingNode
    {
        public override string[] OnProcess(LevelScript currentSequence)
        {
            IDialoguePanel.Instance.StartDialogue();
            return base.OnProcess(currentSequence);
        }
    }
}
