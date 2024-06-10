using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public List<Card> deck = new List<Card>();
    public List<Card> container = new List<Card>();
    public static List<Card> staticEnemyDeck = new List<Card>();

    public List<Card> cardsInHand = new List<Card>();

    public List<Card> cardsInZone = new List<Card>();

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

    public GameObject AICardBack;

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
    public bool[] canAttack;
    public static bool AiEndPhase;
    public GameObject PlayerZone;

    public Deck enemyDeck; // Add a Deck object for the enemy

    void Start()
    {

        enemyDeck = new Deck
        {
            cardIDs = new List<int> { 3, 1, 8, 8, 8, 3, 1, 8, 2, 8, 3, 1, 2, 2, 2, 3, 1, 8, 2, 8, 3, 1, 2, 8, 2, 3, 1, 8, 2, 2, 3, 1, 8, 8, 8, 3, 1, 2, 8, 8 }
        };

        StartCoroutine(WaitFiveSeconds());
        StartCoroutine(StartGame());

        Hand = GameObject.Find("EnemyHand");
        Zone = GameObject.Find("EnemyZone");
        Graveyard = GameObject.Find("EnemyGraveyard");
        PlayerZone = GameObject.Find("Zone");

        x = 0;
        deckSize = 40;

        draw = true;

        // Initialize the deck with predefined cards from enemyDeck
        foreach (int cardID in enemyDeck.cardIDs)
        {
            if (deck.Count < deckSize)
            {
                deck.Add(CardDataBase.GetCardById(cardID));
            }
        }
    }

    void Update()
    {
        staticEnemyDeck = deck;

        if (deckSize < 30) cardInDeck1.SetActive(false);
        if (deckSize < 20) cardInDeck2.SetActive(false);
        if (deckSize < 2) cardInDeck3.SetActive(false);
        if (deckSize < 1) cardInDeck4.SetActive(false);

        if (AICardToHand.drawX > 0)
        {
            StartCoroutine(Draw(AICardToHand.drawX));
            AICardToHand.drawX = 0;
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
            AiCanSummon = new bool[howManyCards];

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
                summonID = cardsID[Random.Range(0, index)];
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

        if (0 == 0)
        {
            int k = 0;
            int howManyCards2 = 0;
            cardsInHand.Clear();

            foreach (Transform child in Zone.transform)
            {
                howManyCards2++;
            }
            foreach (Transform child in Zone.transform)
            {
                canAttack[k] = child.GetComponent<AICardToHand>().canAttack;
                k++;
            }
            for (int i = 0; i < 40; i++)
            {
                if (i >= howManyCards2)
                {
                    canAttack[i] = false;
                }
            }
            k = 0;
        }

        if (0 == 0)
        {
            int l = 0;
            int howManyCards3 = 0;
            cardsInZone.Clear();

            foreach (Transform child in Zone.transform)
            {
                howManyCards3++;
            }
            foreach (Transform child in Zone.transform)
            {
                cardsInZone.Add(child.GetComponent<AICardToHand>().thisCardList[0]);
                l++;
            }
            for (int i = 0; i < 40; i++)
            {
                if (i >= howManyCards3)
                {
                    cardsInZone.Add(CardDataBase.cardList[0]);
                }
            }
            l = 0;
        }

        if (attackPhase == true && endPhase == false)
        {
            for (int i = 0; i < 40; i++)
            {
                if (canAttack[i] == true)
                {
                    //AttackPlayerCards();
                   PlayerHp.staticHp -= cardsInZone[i].power;
                }
            }

            endPhase = true;
        }

        if (endPhase == true)
        {
            AiEndPhase = true;
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

        Instantiate(AICardBack, transform.position, transform.rotation);
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
    /* void Attack()
     {
         // Iterate over AI's cards in the zone
         foreach (var aiCardToHand in cardsInZone)
         {
             // Flag to track if the AI has attacked any player card
             bool attackedPlayerCard = false;

             // Iterate over the player's cards in the PlayerZone
             foreach (Transform playerCardTransform in PlayerZone.transform)
             {
                 // Get the ThisCard component of the player's card
                 var playerCard = playerCardTransform.GetComponent<ThisCard>();

                 // Check if the player's card can be attacked
                 if (playerCard.summoned)
                 {
                     // Reduce the player's card power by AI card's power
                     playerCard.actualpower -= aiCardToHand.power;

                     // If the player's card power drops to or below 0, remove it from the zone
                     if (playerCard.actualpower <= 0)
                     {
                         // Destroy the player's card
                         playerCard.Destroy();
                     }

                     // Mark that the AI has attacked a player card
                     attackedPlayerCard = true;

                     // Break out of the loop since the AI can attack only one player card per turn
                     break;
                 }
             }

             // If the AI didn't attack any player card, attack the player's health directly
             if (!attackedPlayerCard)
             {
                 // Reduce the player's health by AI card's power
                 PlayerHp.staticHp -= aiCardToHand.power;
             }
         }
     }*/

    IEnumerator Draw(int x)
    {
        for (int i = 0; i < x; i++)
        {
            // Ensure there are enough cards in the deck to draw
            if (deck.Count > 0)
            {
                // Wait for a second before drawing each card
                yield return new WaitForSeconds(1);

                // Get the top card from the deck
                Card drawnCard = deck[0];

                // Remove the card from the deck
                deck.RemoveAt(0);

                // Instantiate the card in the hand (replace CardToHand with the drawnCard if needed)
                GameObject cardObject = Instantiate(CardToHand, transform.position, transform.rotation);

                // Add the drawn card to the hand
                cardsInHand.Add(drawnCard);


            }
        }
    }

    IEnumerator WaitFiveSeconds()
    {
        yield return new WaitForSeconds(5);
    }

    IEnumerator WaitForSummonPhase()
    {
        yield return new WaitForSeconds(5);
        summonPhase = true;
    }



    void AttackPlayerCards()
    {
        foreach (Transform enemyCardTransform in Zone.transform)
        {
            AICardToHand aiCard = enemyCardTransform.GetComponent<AICardToHand>();

            if (aiCard != null && aiCard.canAttack)
            {
                if (PlayerZone.transform.childCount > 0)
                {
                    Transform targetCardTransform = PlayerZone.transform.GetChild(0); // Simplified for example, better target selection logic can be added
                    ThisCard targetCard = targetCardTransform.GetComponent<ThisCard>();

                    if (targetCard != null)
                    {
                        aiCard.AttackCard(targetCard);
                    }
                }
                else
                {
                    aiCard.AttackPlayerDirectly();
                }
            }
        }
    }

}

