using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] Dialogue CurrentDialogue;
    [SerializeField] GameObject DialoguePanel;
    [SerializeField] TextMeshProUGUI DialogueLabel;
    [SerializeField] TextMeshProUGUI DialogueText;
    int PhraseID;

    void Start(){
        StartDialogue(CurrentDialogue);
    }  

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Return)){
            NextPhrase();
        }
    }

    public void NextPhrase(){
        PhraseID ++;
        if(PhraseID == CurrentDialogue.Phrases.Count){
            EndDialogue();
            return;
        }
        DialogueLabel.text = CurrentDialogue.OtherName;
        DialogueText.text = CurrentDialogue.Phrases[PhraseID];
    }

    public void EndDialogue(){
        DialoguePanel.SetActive(false);
    }
    public void StartDialogue(Dialogue dialogue){
        PhraseID = 0;
        DialogueLabel.text = CurrentDialogue.OtherName;
        DialogueText.text = CurrentDialogue.Phrases[PhraseID];
        CurrentDialogue = dialogue;
        DialoguePanel.SetActive(true);
    }
}
