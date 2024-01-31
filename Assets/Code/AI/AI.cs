using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public List<Card> deck = new List<Card>();
    public List<Card> container = new List<Card>();
    public static List<Card> staticEnemyDeck = new List<Card>();

    public List<Card> cardsInHand = new List<Card>();
    public bool AIcanPlay;

    public GameObject Hand;
    public GameObject Zone;
    public GameObject Graveyard;

    public int x;
    public static int deckSize;

    public GameObject cardInDeck1;
    public GameObject cardInDeck2;
    public GameObject cardInDeck3;
    public GameObject cardInDeck4;

    public GameObject CardToHand;
    public GameObject[] Clones;
    public static bool draw;

    public GameObject CardBack;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitFiveSeconds());

        StartCoroutine(StartGame());

        Hand = GameObject.Find("EnemyHand");
        Zone = GameObject.Find("EnemyZone");
        Graveyard = GameObject.Find("EnemyGraveyard");

        x= 0;
        deckSize = 40;

        draw = true;

        for (int i = 0; i < deckSize; i++)
        {
            x = Random.Range(1, 4);
            deck.Add(CardDataBase.cardList[x]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        staticEnemyDeck = deck;
        if(deckSize < 30)
        {
           cardInDeck1.SetActive(false);
        }
        if(deckSize < 20)
        {
            cardInDeck2.SetActive(false);
        }
        if(deckSize < 2)
        {
            cardInDeck3.SetActive(false);
        }
        if(deckSize < 1)
        {
            cardInDeck4.SetActive(false);
        }

        if(ThisCard.drawX > 0)
        {
            StartCoroutine(Draw(ThisCard.drawX));
            ThisCard.drawX = 0;
        }

        if(TurnSystem.startTurn == false && draw == false)
        {
            StartCoroutine(Draw(1));
            draw = true;
           // TurnSystem.startTurn = false;
        }

        if (AIcanPlay == true)
        {
            // Clear the cardsInHand list before adding cards
            cardsInHand.Clear();

            for (int i = 0; i < 40; i++)
            {
                if (AICardToHand.cardsInHandStatic[i].id != 0)
                {
                    cardsInHand.Add(AICardToHand.cardsInHandStatic[i]);
                }
            }
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
        StartCoroutine(ShuffleNow());
    }

    IEnumerator StartGame()
    {
      for(int i = 0; i < 4; i++)
        {
            yield return new WaitForSeconds(1);
            Instantiate(CardToHand, transform.position, transform.rotation);
        }
    }



    IEnumerator ShuffleNow()
    {
        yield return new WaitForSeconds(1);
        Clones = GameObject.FindGameObjectsWithTag("Deck");

        foreach (GameObject Deck in Clones)
        {
            Destroy(Deck);
        }
    }


    IEnumerator Draw(int x)
    {
        for (int i = 0; i < x; i++)
        {
            yield return new WaitForSeconds(1);
            Instantiate(CardToHand, transform.position, transform.rotation);
        }
    }
    IEnumerator WaitFiveSeconds()
    {
        yield return new WaitForSeconds(5);
        AIcanPlay = true;
    }
}
