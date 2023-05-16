using UnityEngine;

public class QuestGiver : Interactable
{
    [SerializeField] DialogueSequencer _dialogueSequencer;
    [SerializeField] Quest _quest;

    QuestReceiver _receiver;
    
    private void Awake()
    {
        _dialogueSequencer.OnDialogueEnded += OpenQuestWindow;
    }

    public void OpenQuestWindow()
    {
        UIManager.Instance.ShowQuestWindow(_quest, AcceptQuest);
    }

    public void AcceptQuest()
    {
        UIManager.Instance.HideQuestWindow();
        _receiver.SetQuest(_quest);
    }

    public override void Interact()
    {
        _dialogueSequencer.StartDialogue();
    }
}
