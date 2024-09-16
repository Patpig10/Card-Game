using DS.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodePuppet : MonoBehaviour
{
    public int mood;
    public Image imageComponent;
    [SerializeField] public DSDialogueSO startingDialogue;
    private DSDialogueSO currentDialogue;


    private void Awake()
    {
        currentDialogue = startingDialogue;
    }
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        currentDialogue.DialogueName = "Dialogue Name";
        if (currentDialogue.DialogueName == "Act0A")
        {
            mood = 1;
            UpdateSprite();
        }
    }

    void UpdateSprite()
    {
        // Define the sprite path based on the mood
        string spritePath = "Charater/Ann/";

        switch (mood)
        {
            case 1:
                spritePath += "Ann Normal";
                break;
            case 2:
                spritePath += "Ann Mad";
                break;
            default:
                Debug.LogWarning("Mood not recognized, defaulting to Ann_Normal");
                spritePath += "Ann Normal";
                break;
        }

        // Load the sprite from the Resources folder
        Sprite newSprite = Resources.Load<Sprite>(spritePath);

        // Check if the sprite was successfully loaded
        if (newSprite != null)
        {
            // Set the sprite of the Image component to the new sprite
            imageComponent.sprite = newSprite;
        }
        else
        {
            Debug.LogError("Sprite not found at path: " + spritePath);
        }
    }
}
