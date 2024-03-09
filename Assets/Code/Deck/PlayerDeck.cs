using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerDeck : MonoBehaviour
{
    public List<Card> deck = new List<Card>();
    public List<Card> container = new List<Card>();
    public static List<Card> staticDeck = new List<Card>();

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
        x = 0;
        deckSize = 40;

        // Clear the deck before initializing
        deck.Clear();

        // Initialize the deck with cards from PlayerPrefs
        for (int i = 1; i <= 8; i++)
        {
            int cardId = PlayerPrefs.GetInt("deck" + i, 0);
            Debug.Log("CardID " + i + ": " + cardId);

            if (cardId > 0)
            {
                // Add the card from CardDataBase using the retrieved cardId
                deck.Add(CardDataBase.cardList[cardId]);
            }
            // If no cardId in PlayerPrefs, do not add any random cards
        }

        Shuffle();

        // Get reference to ThisCard script
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
        staticDeck = new List<Card>(deck);
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
            // Draw a card from the deck
            Card drawnCard = deck[deck.Count - 1];
            deck.RemoveAt(deck.Count - 1);

            // Set the ID on the ThisCard script before instantiating
            if (thisCardScript != null) // Add this check
            {
                thisCardScript.thisID = drawnCard.id;
            }
            else
            {
                Debug.LogError("thisCardScript is null");
            }

            // Instantiate the CardToHand object
            Instantiate(CardToHand, transform.position, transform.rotation);
            deckSize--;
        }
    }
    public void Shuffle()
    {
      //  for (int i = 0; i < deckSize; i++)
            for (int i = 0; i < deck.Count; i++)
            {
                container[0] = deck[i];
               // int randomIndex = Random.Range(i, deckSize);
                int randomIndex = Random.Range(i, deck.Count); // Use deck.Count instead of deckSize
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
