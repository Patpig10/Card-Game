using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ThisCard : MonoBehaviour
{
    private List<Card> thisCardList = new List<Card>();
    public int thisID;

    public int id;
    public string cardName;
    public int cost;
    public int power;
    public string cardDescription;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI powerText;
    public TextMeshProUGUI descriptionText;

    public Sprite thisSprite;
    public Image thatImage;

    public Image frame;

    public bool cardBack;
    CardBack CardBackScript;

    public GameObject Hand;

    public int numberOfCardsInDeck;

    public bool canBeSummon;
    public bool summoned;
    public GameObject battleZone;


    public static int drawX;
    public int drawXcards;
    public int addXmaxGil;
    // Start is called before the first frame update
    void Start()
    {
        CardBackScript = GetComponent<CardBack>();
        thisCardList.Add(CardDataBase.cardList[Random.Range(1, CardDataBase.cardList.Count)]);
        UpdateUI();
        canBeSummon = false;
        summoned = false;

        drawX = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Hand = GameObject.Find("Hand");
        if (this.transform.parent == Hand.transform)
        {
            cardBack = false;
        }

        id = thisCardList[0].id;
        cardName = thisCardList[0].cardName;
        cost = thisCardList[0].cost;
        power = thisCardList[0].power;
        cardDescription = thisCardList[0].cardDescription;
        thisSprite = thisCardList[0].thisImage;

        drawXcards = thisCardList[0].drawXcards;
        addXmaxGil = thisCardList[0].addXmaxGil;

        // Check for color condition using the color property of the Card class
        if (thisCardList[0].color == "White")
        {
            frame.color = new Color32(255, 255, 255, 255);  // Set the color to white
        }
        else if (thisCardList[0].color == "Blue")
        {
            frame.color = new Color32(26, 109, 236, 255);  // Set the color to white
        }
        else if (thisCardList[0].color == "Green")
        {
            frame.color = new Color32(122, 236, 26, 255);  // Set the color to white
        }
        else if (thisCardList[0].color == "Black")
        {
            frame.color = new Color32(51, 32, 32, 255);  // Set the color to white
        }
        CardBackScript.UpdateCard(cardBack);
        UpdateUI();

        if (this.tag == "Clone")
        {
          
            thisCardList[0] = PlayerDeck.staticDeck[PlayerDeck.deckSize - 1];

            PlayerDeck.deckSize--;
            cardBack = false;
            this.tag = "Untagged";
        }
        if (this.tag != "Clone")
        {
            if (TurnSystem.currentGil >= cost && !summoned)
            {
                canBeSummon = true;
            }
            else
            {
                canBeSummon = false;
            }

            if (canBeSummon == true)
            {
                gameObject.GetComponent<Draggable>().enabled = true;
            }
            else
            {
                gameObject.GetComponent<Draggable>().enabled = false;
            }

            battleZone = GameObject.Find("Zone");

            if (!summoned && this.transform.parent == battleZone.transform)
            {
                Summon();
            }
        }


        
    }
    public void Summon()
    {
        TurnSystem.currentGil -= cost;
        summoned = true;
        MaxGil(addXmaxGil);
        drawX = drawXcards;
    }

    public void MaxGil(int x) 
    {
        TurnSystem.maxGil += x;
    }
    void UpdateUI()
    {
        nameText.text = cardName;
        costText.text = cost.ToString();
        powerText.text = power.ToString();
        descriptionText.text = cardDescription;
        thatImage.sprite = thisSprite;
    }
}
