using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckCreator : MonoBehaviour
{

    public int[] cardsWithThisId;
    public bool mouseOverDeck;
    public int dragged;
    public GameObject coll;
    public int numberOfCardsInDatabase;
    public int sum;
    public int numberOfDifferentCards;
    public int[] savedDeck;
    public GameObject prefab;
    public bool[] alreadyCreated;
    public static int lastAdded;
    public int[] quantity;
    private int cardsDroppedCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        sum = 0;
        numberOfCardsInDatabase = 8;  // Assuming there are 4 cards initially

        // Populate the cardsWithThisId array
        for (int i = 0; i < numberOfCardsInDatabase; i++)
        {
            cardsWithThisId[i] = Collection.x + i;  // Adjust this based on your logic
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreateDeck()
    {
        try
        {
            for (int i = 0; i < numberOfCardsInDatabase; i++)
            {
                sum += cardsWithThisId[i];
            }

            Debug.Log("Current sum: " + sum);

            if (sum == 40) // 10 cards in deck
            {
                for (int i = 0; i < numberOfCardsInDatabase; i++)
                {
                    PlayerPrefs.SetInt("deck" + i, cardsWithThisId[i]);
                }

                PlayerPrefs.SetInt("deckSize", 40); // Store the deck size in PlayerPrefs
            }

            // Reset variables after creating the deck
            sum = 0;
            numberOfDifferentCards = 0;

            // Retrieve the deck from PlayerPrefs after storing it
            for (int i = 0; i < numberOfCardsInDatabase; i++)
            {
                savedDeck[i] = PlayerPrefs.GetInt("deck" + i, 0);

                // Update cardsWithThisId based on savedDeck
                cardsWithThisId[i] = savedDeck[i];
            }

            // Log the values to ensure they are correct
            Debug.Log("Created deck: " + string.Join(", ", cardsWithThisId));

            // Optionally, reset the PlayerPrefs to ensure a clean start (useful for testing)
            // PlayerPrefs.DeleteAll();
        }
        catch (PlayerPrefsException e)
        {
            Debug.LogError("PlayerPrefs error: " + e.Message);
        }
    }


    public void EnterDeck()
    {
        mouseOverDeck = true;
    }
    public void ExitDeck()
    {
        mouseOverDeck = false;
    }

    public void Card1()
    {
        dragged = Collection.x;
    }

    public void Card2()
    {
        dragged = Collection.x + 1;
    }

    public void Card3()
    {
        dragged = Collection.x + 2;
    }

    public void Card4()
    {
        dragged = Collection.x + 3;
    }

    public void Drop()
    {
        if (mouseOverDeck == true && coll.GetComponent<Collection>().HowManyCards[dragged] > 0)
        {
            cardsWithThisId[dragged]++;

            if (cardsWithThisId[dragged] < 0)
            {
                cardsWithThisId[dragged] = 0;
            }
            coll.GetComponent<Collection>().HowManyCards[dragged]--;

            CalculateDrop();

            cardsDroppedCount++;

            // Check if this is the last card to be dropped
            if (AllCardsDropped())
            {
                CreateDeck();
                ClearDeck(); // Optionally clear the deck after creating it
            }
        }
    }


    bool AllCardsDropped()
    {
        // Assuming you have 40 cards to drop, check if all have been dropped
        return cardsDroppedCount >= 40;
    }
    public void ClearDeck()
    {
        // Reset necessary variables and arrays
        for (int i = 0; i < cardsWithThisId.Length; i++)
        {
            cardsWithThisId[i] = 0;
        }

        cardsDroppedCount = 0;

        // Implement additional cleanup if needed
    }
    public void CalculateDrop()
    {
        lastAdded = 0;
        int i = dragged;

        if (cardsWithThisId[i] > 0 && alreadyCreated[i] == false)
        {
            lastAdded = i;
            Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
            alreadyCreated[i] = true;

            quantity[i] = 1;
        }
        else if (cardsWithThisId[i] > 0 && alreadyCreated[i] == true)
        {
            quantity[i]++;
        }
    }
}