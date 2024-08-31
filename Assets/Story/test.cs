using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DS.ScriptableObjects; // Add this to include the correct namespace

public class test : MonoBehaviour
{
    [SerializeField] public DSDialogueSO startingDialogue;
    [SerializeField] public TextMeshProUGUI textUI;

    public int clicks;

    private DSDialogueSO currentDialogue; // Update the type to DSDialogueSO

    private void Awake()
    {
        currentDialogue = startingDialogue;
        ShowText(); // Call this method to display the starting dialogue immediately if needed
    }

    private void ShowText()
    {
        textUI.text = currentDialogue.Text;
    }

    public void OnOptionChosen(int choiceIndex) // Make this method public if you want to call it from outside
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

        clicks++;

        currentDialogue = nextDialogue;
        ShowText();
    }
}
