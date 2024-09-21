using IvoriesStudios.LevelScripting.Attributes;
using UnityEngine;

namespace IvoriesStudios.LevelScripting.Dialogue
{
    [NodeInfo("Basic Dialogue Line", "Dialogue/Basic Dialogue Line", isStopping: true)]
    public class BasicDialogueLine : ScriptingNode
    {
        [ExposedProperty] public Characters Speaker;
        [ExposedProperty, TextArea] public string Text;

        public override string[] OnProcess(LevelScript currentSequence)
        {
            IDialoguePanel.Instance.SetDialogueLine(CharacterList.GetCharacter(Speaker), Text);
            return base.OnProcess(currentSequence);
        }
    }
}
