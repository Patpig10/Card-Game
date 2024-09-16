using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DS.ScriptableObjects; 

public class test : MonoBehaviour
{
    [SerializeField] public DSDialogueSO startingDialogue;
    [SerializeField] public TextMeshProUGUI textUI;
    [SerializeField] public TextMeshProUGUI nameUI;


    private DSDialogueSO currentDialogue; 

    private void Awake()
    {
        currentDialogue = startingDialogue;
        ShowText(); 
    }

    private void ShowText()
    {
        textUI.text = currentDialogue.Text;
        nameUI.text = currentDialogue.Speaker;
    }

    public void OnOptionChosen(int choiceIndex) 
    {
        if (choiceIndex < 0 || choiceIndex >= currentDialogue.Choices.Count)
        {
            Debug.LogError("Invalid choice index");
            return;
        }

        DSDialogueSO nextDialogue = currentDialogue.Choices[choiceIndex].NextDialogue;

        if (nextDialogue == null)
        {
            Debug.Log("End of dialogue");
            return; // No more dialogues to show, you can reset or end the dialogue flow here
        }

        currentDialogue = nextDialogue;
        ShowText();
    }



}
