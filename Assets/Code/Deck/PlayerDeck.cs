using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
public class PlayerDeck : MonoBehaviour
{
    public List<Card> deck = new List<Card>();
    public List<Card> container = new List<Card>();
    public static List<Card> staticDeck = new List<Card>();
    private CustomDeckData deckData;
    public int x;
    public static int deckSize;

    public GameObject cardInDeck1;
    public GameObject cardInDeck2;
    public GameObject cardInDeck3;
    public GameObject cardInDeck4;

    public GameObject CardBack;
    public GameObject CardToHand;
    public GameObject Deck;

    public GameObject[] Clones;

    public GameObject Hand;

    // Reference to ThisCard script
    public ThisCard thisCardScript;

    public TextMeshProUGUI LoseText;

    public GameObject LoseTextGameObject;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Persistent Data Path: " + Application.persistentDataPath);

        LoadDeckFromJSON(); // Load the deck from JSON

        thisCardScript = GetComponentInChildren<ThisCard>();
        StartCoroutine(StartGame());
    }

    // Update is called once per frame
    void Update()
    {
        if (deckSize <= 0)
        {
            LoseTextGameObject.SetActive(true);
            LoseText.text = "Deck Out, You Lose";

        }


        staticDeck = deck;
        if (deckSize < 30)
        {
            cardInDeck1.SetActive(false);
        }
        if (deckSize < 20)
        {
            cardInDeck2.SetActive(false);
        }
        if (deckSize < 2)
        {
            cardInDeck3.SetActive(false);
        }
        if (deckSize < 1)
        {
            cardInDeck4.SetActive(false);
        }

        if (ThisCard.drawX > 0)
        {
            StartCoroutine(Draw(ThisCard.drawX));
            ThisCard.drawX = 0;
        }

        if (TurnSystem.startTurn == true)
        {
            StartCoroutine(Draw(1));
            TurnSystem.startTurn = false;
        }
    }
    void LoadDeckFromJSON()
    {
        // Load the JSON file into a string
        string jsonString = File.ReadAllText(Application.persistentDataPath + "/deck.json");

        // Parse the JSON string into a CustomDeckData object
        deckData = JsonUtility.FromJson<CustomDeckData>(jsonString);

        
            // Clear the existing deck
            deck.Clear();

            // Populate the deck based on the loaded data
            for (int i = 0; i < deckData.cardsWithThisId.Length; i++)
            {
                int cardId = deckData.cardsWithThisId[i];

                // Find the corresponding card in the database and add it to the deck
                for (int j = 0; j < cardId; j++)
                {
                    // Create a new instance of the card based on its ID
                    Card foundCard = CardDataBase.cardList.Find(card => card.id == i);

                    if (foundCard != null)
                    {
                        deck.Add(foundCard);
                    }
                }
            }

            deckSize = deck.Count; // Set the deck size based on the loaded deck
        
    }


    IEnumerator Example()
    {
        yield return new WaitForSeconds(1);
        Clones = GameObject.FindGameObjectsWithTag("Deck");

        foreach (GameObject Deck in Clones)
        {
            Destroy(Deck);
        }
    }

    IEnumerator StartGame()
    {
        // Draw 4 initial cards
        for (int i = 0; i <= 4; i++)
        {
            DrawCardToHand();
            yield return new WaitForSeconds(1);
        }
    }

    public void DrawCardToHand()
{
    if (deck.Count > 0)
    {
        // List to hold the indices of non-zero quantities
        List<int> validIndices = new List<int>();

        // Find indices of non-zero quantities
        for (int i = 0; i < deckData.cardsWithThisId.Length; i++)
        {
            if (deckData.cardsWithThisId[i] > 0)
            {
                validIndices.Add(i);
            }
        }

        // If there are valid indices, select a random one
        if (validIndices.Count > 0)
        {
            int randomIndex = Random.Range(0, validIndices.Count);
            int selectedId = validIndices[randomIndex];

            // Decrement the quantity in cardsWithThisId for the selected ID
            deckData.cardsWithThisId[selectedId]--;

            // Get the drawn card
            Card drawnCard = deck.Find(card => card.id == selectedId);

            // Set the ID on the ThisCard script before instantiating
            if (thisCardScript != null && drawnCard != null)
            {
                thisCardScript.id = selectedId; // Assign the selected ID
            }
            else
            {
                Debug.LogError("thisCardScript or drawnCard is null");
                return;
            }

            // Instantiate the CardToHand object
            Instantiate(CardToHand, transform.position, transform.rotation);

            // Remove the drawn card from the deck
            deck.Remove(drawnCard);
            deckSize--;

            // Check if the quantity for the selected ID becomes zero and update the deck data structure
            if (deckData.cardsWithThisId[selectedId] == 0)
            {
                deck.RemoveAll(card => card.id == selectedId);
            }
        }
        else
        {
            Debug.LogWarning("No more cards to draw from the loaded deck.");
        }
    }
    else
    {
        Debug.LogWarning("No more cards to draw from the loaded deck.");
    }
}


    public void Shuffle()
    {
        for (int i = 0; i < deckSize; i++)
        {
            container[0] = deck[i];
            int randomIndex = Random.Range(i, deckSize);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = container[0];
        }

        Instantiate(CardBack, transform.position, transform.rotation);

        StartCoroutine(Example());
    }

    IEnumerator Draw(int x)
    {
        for (int i = 0; i < x; i++)
        {
            yield return new WaitForSeconds(1);

            DrawCardToHand();
        }
    }
}
[System.Serializable]
public class CustomDeckData
{
    public int[] cardsWithThisId;
}