using IvoriesStudios.LevelScripting.Dialogue;

public interface IDialoguePanel
{
    public static IDialoguePanel Instance { get; protected set; }
    public void StartDialogue();
    public void SetDialogueLine(Character speaker, string line);
    public void StopDialogue();
}
