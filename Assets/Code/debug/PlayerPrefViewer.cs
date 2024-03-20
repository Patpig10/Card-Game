using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefViewer : MonoBehaviour
{
    void Start()
    {
        // Display player preferences on start
        DisplayPlayerPrefs();
    }

    void Update()
    {
        // You can update the displayed preferences in real-time if needed
    }

    public void DisplayPlayerPrefs()
    {
        Debug.Log("Player Preferences:");

        // Display deck size
        int deckSize = PlayerPrefs.GetInt("deckSize", 0);
        Debug.Log("Deck Size: " + deckSize);

        // Display deck data
        for (int i = 0; i < 9; i++)
        {
            int cardId = PlayerPrefs.GetInt("deck" + i, 0);
            Debug.Log("CardID " + i + ": " + cardId);
        }

        // Add more PlayerPrefs keys as needed

        // You can also use PlayerPrefs.HasKey to check if a key exists
        if (PlayerPrefs.HasKey("someKey"))
        {
            string someValue = PlayerPrefs.GetString("someKey", "");
            Debug.Log("Some Key: " + someValue);
        }
        else
        {
            Debug.Log("Some Key does not exist.");
        }

        // Add more PlayerPrefs keys and their types as needed
    }
}
