using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;

public class PlayerDeck : MonoBehaviour
{

    public List<Card> deck = new List<Card>();
    public List<Card> container = new List<Card>();
    public static List<Card> staticDeck = new List<Card>();


    public int x;
    public static int deckSize = 40;

    public GameObject cardInDeck1;
    public GameObject cardInDeck2;
    public GameObject cardInDeck3;
    public GameObject cardInDeck4;

    public GameObject CardToHand;
    private int currentDeckIndex = 0;

    public GameObject CardBack;
    public GameObject Deck;

    public GameObject[] Clones;

    public GameObject Hand;


    public TMP_Text LoseText;
    public GameObject LoseTextGameObject;

    public GameObject concedeWindow;
   // public string menu = "Menu";

    //NEW
  //  public AudioSource audioSource;
   // public AudioClip shuffle, draw;
    //NEW END

    void Awake()
    {
        //Shuffle(); //comment
    }

    // Use this for initialization
    void Start()
    {

        InitializeDeck();
        ShuffleDeck();
        StartCoroutine(StartGame());
    }

    // Update is called once per frame
    void Update()
    {


        if (deckSize <= 0)
        {
            LoseTextGameObject.SetActive(true);
            LoseText.text = "You Lose";
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

    void InitializeDeck()
    {
        // Build the deck based on player preferences (example)
        for (int i = 1; i <= 8; i++)
        {
            int cardCount = PlayerPrefs.GetInt("deck" + i, 0);
            if (cardCount > 0)
            {
                for (int j = 0; j < cardCount; j++)
                {
                    deck.Add(CardDataBase.cardList[i]);
                }
            }
        }
    }

    void ShuffleDeck()
    {
        // Shuffle the deck using Fisher-Yates algorithm
        for (int i = 0; i < deck.Count; i++)
        {
            Card temp = deck[i];
            int randomIndex = Random.Range(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }
    IEnumerator Example()
    {
        //COMMENT
        // yield return new WaitForSeconds(1);
        // Clones = GameObject.FindGameObjectsWithTag("Clone");

        // foreach(GameObject Clone in Clones)
        // {
        // 	Destroy(Clone);
        // }
        //COMMENT END

        //NEW
        GameObject prefb = Instantiate(CardBack, transform.position, transform.rotation);
        yield return new WaitForSeconds(1.5f);
        Destroy(prefb);
        //NEW END
    }

    IEnumerator StartGame()
    {
        // Draw initial cards at the start of the game
        const int initialCardsToDraw = 4;
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < initialCardsToDraw; i++)
        {
            DrawCardToHand();
            yield return new WaitForSeconds(0.5f);
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

        //NEW 
        //audioSource.PlayOneShot(shuffle, 1f);

        StartCoroutine(Example());
        //NEW END


        //Instantiate(CardBack, transform.position, transform.rotation);// comment

        //StartCoroutine(Example());// comment


    }
    void DrawCardToHand()
    {
        if (deck.Count > 0)
        {
            // Get the top card from the deck
            Card drawnCard = deck[0];
            deck.RemoveAt(0); // Remove the drawn card from the deck

            // Instantiate the CardToHand prefab and set its thisId
            GameObject cardInHand = Instantiate(CardToHand, transform.position, Quaternion.identity);
            ThisCard thisCardComponent = cardInHand.GetComponent<ThisCard>();
            if (thisCardComponent != null)
            {
                thisCardComponent.thisId = drawnCard.id;
                thisCardComponent.UpdateUI(); // Update UI with card information
            }
        }
    }

    IEnumerator Draw(int x)
    {
        for (int i = 0; i < x; i++)
        {
            yield return new WaitForSeconds(1);

            if (deck.Count > 0)
            {
                Card drawnCard = deck[0]; // Get the top card from the deck
                deck.RemoveAt(0); // Remove the drawn card from the deck

                GameObject cardInHand = Instantiate(CardToHand, transform.position, transform.rotation);
                ThisCard thisCardComponent = cardInHand.GetComponent<ThisCard>();
                if (thisCardComponent != null)
                {
                    thisCardComponent.thisId = drawnCard.id; // Assign the thisId based on the drawn card's id
                    thisCardComponent.UpdateUI(); // Update UI with card information
                }
            }
        }
    }

    /* public void OpenWindow()
     {
         concedeWindow.SetActive(true);
     }

     public void CloseWindow()
     {
         concedeWindow.SetActive(false);
     }

     public void ConcedeDefeat()
     {
         StartCoroutine(EndGame());
     }*/

    IEnumerator EndGame()
    {
        LoseTextGameObject.SetActive(true);
        LoseText.text = "You Lose";
       // concedeWindow.SetActive(false);

        yield return new WaitForSeconds(2.5f);

      //  SceneManager.LoadScene(menu);
    }

   

}

