using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AICardToHand : MonoBehaviour
{

    public List<Card> thisCardList = new List<Card>();

   

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

   public static int drawX;
   public int drawXcards;
    public int addXmaxGil;
    public bool canHeal;

    public int hurted;
    public int actualpower;
    public int returnXcards;
    public int healXpower;
    public GameObject Hand;
    public int z = 0;
    public GameObject It;

    public int numberOfCardsInDeck;

    public bool isTarget;
    public GameObject Graveyard;

    public bool thisCardCanBeDestroyed;
    public bool summoned;

    public GameObject cardBack;
    public GameObject AiZone;
    public bool canAttack;
    public bool summoningSickness;
    // Start is called before the first frame update
    void Start()
    {



        thisCardList.Add(CardDataBase.cardList[Random.Range(1, CardDataBase.cardList.Count)]);
        thatImage.sprite = thisCardList[0].thisImage;

        thisCardList[0] = CardDataBase.cardList[thisID];
        Hand = GameObject.Find("EnemyHand");

        z = 0;
        numberOfCardsInDeck = AI.deckSize;

        Graveyard = GameObject.Find("EnemyGraveyard");
        StartCoroutine(AfterVoidStart());
        AiZone = GameObject.Find("EnemyZone");
        canHeal = true;
        summoned = false;
        summoningSickness = true;

    }

    // Update is called once per frame
    void Update()
    {
        if( z== 0)
        {
            Hand = GameObject.Find("EnemyHand");
            It.transform.SetParent(Hand.transform);
            It.transform.localScale = Vector3.one;
            It.transform.position = new Vector3(transform.position.x, transform.position.y, -48);
            It.transform.eulerAngles = new Vector3(25, 0, 0);
            z = 1;
        }

        id = thisCardList[0].id;
        cardName = thisCardList[0].cardName;
        cost = thisCardList[0].cost;
        power = thisCardList[0].power;
        actualpower = power - hurted;
        cardDescription = thisCardList[0].cardDescription;
        thisSprite = thisCardList[0].thisImage;

        drawXcards = thisCardList[0].drawXcards;
        addXmaxGil = thisCardList[0].addXmaxGil;
        returnXcards = thisCardList[0].returnXcards;

        healXpower = thisCardList[0].healXpower;
        //boostXpower = thisCardList[0].boostXpower;

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
        if (canHeal == true && summoned == true)
        {
            Heal();
            canHeal = false;
        }
        if(this.transform.parent == AiZone.transform)
        {
            summoned = true;
        }
        if (this.tag == "EDeck")
        {
           


            thisCardList[0] = AI.staticEnemyDeck[numberOfCardsInDeck - 1];
            numberOfCardsInDeck--;
            AI.deckSize--;
            this.tag = "Clone";
        }
        UpdateUI();
      if(hurted >= power && thisCardCanBeDestroyed == true)
        {
           this.transform.SetParent(Graveyard.transform);
            hurted = 0;
        }
       
      if(this.transform.parent == Hand.transform)
        {
           cardBack.SetActive(true);
        }
        if (this.transform.parent == AiZone.transform)
        {
            cardBack.SetActive(false);
        }

        if(TurnSystem.isYourTurn == false && summoningSickness == false)
        {
           canAttack = true;
        }
        else
        {
            canAttack = false;
        }

        if(TurnSystem.isYourTurn == true && this.transform.parent == AiZone.transform)
        {
            summoningSickness = false;
        }
    }
    public void Heal()
    {
        EnemyHp.staticHp += healXpower;
    }
    public void BeingTarget()
    {
        isTarget = true;
    }
    public void DontBeingTarget()
    {
        isTarget = false;
    }


    IEnumerator AfterVoidStart()
    {
        yield return new WaitForSeconds(1);
       thisCardCanBeDestroyed = true;
            }
    void UpdateUI()
    {

        nameText.text = cardName;
        costText.text = cost.ToString();
        powerText.text = actualpower.ToString();
        descriptionText.text = cardDescription;
        thatImage.sprite = thisSprite;
    }
}
