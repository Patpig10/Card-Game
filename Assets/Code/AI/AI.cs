using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public List<Card> deck = new List<Card>();
    public List<Card> container = new List<Card>();
    public static List<Card> staticEnemyDeck = new List<Card>();

    public List<Card> cardsInHand = new List<Card>();

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

    public int currentGil;
    public bool[] AiCanSummon;
    public bool drawPhase;
    public bool summonPhase;
    public bool attackPhase;
    public bool endPhase;

    public int[] cardsID;
    public int summonThisCard;
    public AICardToHand aiCardToHand;
    public int summonID;
    public int summonThisId;

    public int howManyCards;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitFiveSeconds());
        StartCoroutine(StartGame());

        Hand = GameObject.Find("EnemyHand");
        Zone = GameObject.Find("EnemyZone");
        Graveyard = GameObject.Find("EnemyGraveyard");

        x = 0;
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

        if (TurnSystem.startTurn == false && draw == false)
        {
            StartCoroutine(Draw(1));
            draw = true;
        }

        currentGil = TurnSystem.currentEnemyGil;

        int j = 0;
        howManyCards = 0;
        cardsInHand.Clear();

        foreach (Transform child in Hand.transform)
        {
            AICardToHand aiCardToHand = child.GetComponent<AICardToHand>();

            if (aiCardToHand != null && aiCardToHand.thisCardList.Count > 0)
            {
                cardsInHand.Add(aiCardToHand.thisCardList[0]);
                j++;
                howManyCards++;
            }
        }

        if (TurnSystem.isYourTurn == false)
        {
            AiCanSummon = new bool[howManyCards]; // Initialize the array size

            for (int i = 0; i < AiCanSummon.Length; i++)
            {
                if (i < cardsInHand.Count && cardsInHand[i].id != 0)
                {
                    if (currentGil >= cardsInHand[i].cost)
                    {
                        AiCanSummon[i] = true;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < AiCanSummon.Length; i++)
            {
                if (i < howManyCards)
                {
                    AiCanSummon[i] = false;
                }
                else
                {
                    // If the index is beyond the valid range, break out of the loop
                    break;
                }
            }
        }

        if (TurnSystem.isYourTurn == false)
        {
            drawPhase = true;
        }
        if (drawPhase == true && summonPhase == false && attackPhase == false)
        {
            StartCoroutine(WaitForSummonPhase());
        }

        if (TurnSystem.isYourTurn == true)
        {
            drawPhase = false;
            summonPhase = false;
            attackPhase = false;
            endPhase = false;
        }

        if (summonPhase == true)
        {
            summonID = 0;
            summonThisId = 0;

            int index = 0;
            for (int i = 0; i < howManyCards; i++)
            {
                if (AiCanSummon[i])
                {
                    cardsID[index] = cardsInHand[i].id;
                    index++;
                }
            }

            if (index > 0)
            {
                summonID = cardsID[Random.Range(0, index)]; // Pick a random ID from the available ones
                summonThisId = summonID;

                foreach (Transform child in Hand.transform)
                {
                    if (child.GetComponent<AICardToHand>().id == summonThisId && CardDataBase.cardList[summonThisId].cost <= currentGil)
                    {
                        child.transform.SetParent(Zone.transform);
                        TurnSystem.currentEnemyGil -= CardDataBase.cardList[summonThisId].cost;
                        break;
                    }
                }
            }

            summonPhase = false;
            attackPhase = true;
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
        for (int i = 0; i < 4; i++)
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
        // AIcanPlay = true;
    }

    IEnumerator WaitForSummonPhase()
    {
        yield return new WaitForSeconds(5);
        summonPhase = true;
       // drawPhase = false;
    }
}
