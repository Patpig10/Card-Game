using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Puppet : MonoBehaviour
{
    public int mood; // This will be set to determine the mood (1 for Normal, 2 for Mad)
    public Image imageComponent;
    private int currentMood; // To keep track of the current mood
    //public mood moodG;
    // Start is called before the first frame update
    void Start()
    {
        // Get the Image component attached to this GameObject

        // Initialize currentMood with the starting mood
        currentMood = mood;

        // Call the method to update the sprite based on the current mood
       // UpdateSprite();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the mood has changed
        if (mood !=currentMood)
        {
            // Update the sprite if the mood has changed
           currentMood = mood;
            UpdateSprite();
        }
    }

    // Update the sprite based on the mood
    // Update the sprite based on the mood
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
    // Public method to set the mood, can be called from other scripts
    public void SetMood(int newMood)
    {
        mood = newMood;
    }
}
