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

    public GameObject attackBorder;
    public GameObject Target;
    public GameObject Enemy;
    public bool summoningSickness;
    public bool cantAttack;
    public bool canAttack;
    public static bool staticTargeting;
    public static bool staticTargetingEnemy;

    public bool targeting;
    public bool targetingEnemy;

    public bool onlyThisCardAttack;

    public GameObject summonBorder;

    public bool canBeDestroyed;
    public GameObject Graveyard;
    public bool beInGraveyard;
    public int hurted;
    public int actualpower;
    public int returnXcards;
    public bool useReturn;
    public int resurrectXcards;
    public static bool UcanReturn;
    public int healXpower;
    public bool canHeal;
    public static bool useRevive;
    public int boostXpower;
    public bool canBoost;

    public GameObject EnemyZone;
    public AICardToHand aiCardToHand;
    public bool spell;
    public int damageDealtBySpell;
    public bool dealDamage;
    public bool stopDealDamage;
    public bool ward;
    public bool directattack;
    public GameObject wardguard;
    // Start is called before the first frame update
    void Start()
    {
        CardBackScript = GetComponent<CardBack>();
        thisCardList.Add(CardDataBase.cardList[Random.Range(1, CardDataBase.cardList.Count)]);
        thatImage.sprite = thisCardList[0].thisImage;

        //UpdateUI();
        canBeSummon = false;
        summoned = false;

        drawX = 0;

        canAttack = false;
        summoningSickness = true;
        Enemy = GameObject.Find("Enemy HP");
        targeting = false;
        targetingEnemy = false;
        beInGraveyard = false;
        canHeal = true;
        canBoost = true;
        directattack = true;
        EnemyZone = GameObject.Find("EnemyZone");
        Graveyard = GameObject.Find("MyGraveyard");


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
        actualpower = power - hurted;
        cardDescription = thisCardList[0].cardDescription;
        thisSprite = thisCardList[0].thisImage;

        drawXcards = thisCardList[0].drawXcards;
        addXmaxGil = thisCardList[0].addXmaxGil;
        returnXcards = thisCardList[0].returnXcards;
        healXpower = thisCardList[0].healXpower;
        boostXpower = thisCardList[0].boostXpower;
        spell = thisCardList[0].spell;
        damageDealtBySpell = thisCardList[0].damageDealtBySpell;
        ward = thisCardList[0].ward;
        resurrectXcards = thisCardList[0].resurrectXcards;
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
            if (TurnSystem.currentGil >= cost && summoned == false && beInGraveyard == false && TurnSystem.isYourTurn ==true)
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
            if(ward == true && summoned == true)
            {
                wardguard.SetActive(true);
            }
            else
            {
                wardguard.SetActive(false);
            }
        }

        if(canAttack == true && beInGraveyard == false )
        {
            Debug.Log("Can attack1.");
            attackBorder.SetActive(true);
        }
        else
        {
            Debug.Log("Can't attack1.");
            attackBorder.SetActive(false);
        }

        if(TurnSystem.isYourTurn == false && summoned == true)
        {
            summoningSickness = false;
            cantAttack = false;
        }

        if(TurnSystem.isYourTurn == true && summoningSickness == false && cantAttack == false)
        {
            canAttack = true;
        }
        else
        {
            canAttack = false;
        }
        targeting = staticTargeting;
        targetingEnemy = staticTargetingEnemy;

        if(targetingEnemy == true)
        {
            Target = Enemy;
        }
        else
        {
            Target = null;
        }

        if(targeting == true && onlyThisCardAttack == true)
        {
            Attack();
        }

        if(canBeSummon == true || UcanReturn == true && beInGraveyard == true || useRevive == true && beInGraveyard == true) 
        { 
            summonBorder.SetActive(true);
        
        
        }
        else
        { 
            summonBorder.SetActive(false);
        }
       /* if (canBeSummon == true || useRevive == true && beInGraveyard == true)
        {
            summonBorder.SetActive(true);
            Debug.Log("revive border test.");


        }
        else
        {
            summonBorder.SetActive(false);
        }*/

        if (actualpower <= 0 && spell == false)
        {
            Destroy();
            canBeDestroyed = true;
        }
        if(returnXcards > 0 && summoned == true && useReturn == false && TurnSystem.isYourTurn == true)
        {
            Return(returnXcards);
            useReturn = true;
        }

        if (resurrectXcards > 0 && summoned == true && useRevive == false && TurnSystem.isYourTurn == true)
        {
            Revive(resurrectXcards);
            useRevive = true;
        }
        if (TurnSystem.isYourTurn == false)
        {
            useRevive = false;
            useReturn = false;
        }
        
        if(canHeal == true && summoned == true)
        {
            Heal();
            canHeal = false;
        }

        if(canBoost == true && summoned == true)
        {
           // AttackBoost();
           // canBoost = false;
        }
        if(damageDealtBySpell >0)
        {
            dealDamage = true;
        }
        if(dealDamage == true && this.transform.parent == battleZone.transform && spell ==true)
        {
            Debug.Log("spell test1.");
            attackBorder.SetActive(true);
        }
        else if(dealDamage == false && this.transform.parent == battleZone.transform && spell == true)
        {
            Debug.Log("spell test2.");  
            attackBorder.SetActive(false);
        }
        if(dealDamage == true && this.transform.parent == battleZone.transform)
        {
            Debug.Log("Attempting to speelll test.");
            dealxDamage(damageDealtBySpell);
        }
        if(stopDealDamage == true)
        {
            Debug.Log("Stop deal damage test.");
            attackBorder.SetActive(false);
            dealDamage = false;
        }
        if(this.transform.parent == battleZone.transform && spell == true && dealDamage == false)
        {
            Debug.Log("Spell gone");
            canBeDestroyed = true;
            Destroy();
        }
        foreach (Transform child in EnemyZone.transform)
        {
            AICardToHand childAICard = child.GetComponent<AICardToHand>();
            if (childAICard.ward == true)
            {
                directattack = false;
            }
            else
            {
                directattack = true;
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
        powerText.text = actualpower.ToString();
        descriptionText.text = cardDescription;
        thatImage.sprite = thisSprite;
    }
    public void Attack()
    {
        if (canAttack && summoned && Target != null)
        {
            Debug.Log("Attempting to attack.");

            if (Target == Enemy && directattack == true)
            {
                Debug.Log("Attacking Enemy");
                EnemyHp.staticHp -= power;
                targeting = false;
                cantAttack = true;
            }
        }
        else
        {
            // Debug.Log("Attempting to attack AI.");

            foreach (Transform child in EnemyZone.transform)
            {
                AICardToHand childAICard = child.GetComponent<AICardToHand>();

                if ( childAICard.isTarget == true && cantAttack == false)
                {
                    Debug.Log("Target found in EnemyZone.");
                    childAICard.hurted += actualpower;  // Adjusting hurted value by the power of the attacking card
                    hurted = childAICard.actualpower;
                    cantAttack = true;
                }
               /* else
                {
                    Debug.LogError("AICardToHand component not found or conditions not met on the target AI card.");
                }*/
            }
        }
    }



    public void UntargetEnemy()
    {
        staticTargetingEnemy = false;
    }
    public void TargetEnemy()
    {
        staticTargetingEnemy = true;
    }

    public void StartAttack()
    {
        staticTargeting = true;
    }

    public void StopAttack()
    {
        staticTargeting = false;

    }
    public void OneCardAttack()
    {
        onlyThisCardAttack = true;
    }
    public void OneCardAttackStop()
    {
        onlyThisCardAttack = false;

    }

    public void Destroy()
    {
        Graveyard = GameObject.Find("MyGraveyard");

        if(canBeDestroyed == true)
        {
            this.transform.SetParent(Graveyard.transform);
            canBeDestroyed = false;
            summoned = false;
            beInGraveyard = true;

            hurted = 0;
        }
    }

    public void Revive(int x)
    {
        for (int i = 0; i <= x; i++)
        {
            ReviveCard();
        }
    }

    public void ReviveCard()
    {
        Debug.Log("Revive card test.");
        useRevive = true;
        actualpower += power;

    }
    public void Return(int x)
    {
        for(int i = 0; i <= x; i++)
        {
            ReturnCard();
        }
    }
    
    public void ReturnCard()
    {
        UcanReturn = true;
    }

    public void ReturnThis()
    {
        if (beInGraveyard == true && UcanReturn == true && Graveyard.transform.childCount > 0)
        {
            this.transform.SetParent(Hand.transform);
            UcanReturn = false;
            beInGraveyard = false;
            summoningSickness = true;
        }
        else if (beInGraveyard == true && useRevive == true && Graveyard.transform.childCount > 0)
        {

            this.transform.SetParent(battleZone.transform);
            useRevive = false;
            beInGraveyard = false;
            summoningSickness = true;

        }
        /* else if (beInGraveyard == true && useRevive == true && Graveyard.transform.childCount == 0)
         {
             this.transform.SetParent(Hand.transform);
             UcanReturn = false;
             beInGraveyard = false;
             summoningSickness = true;
         }*/

    }

   /* public void ReviveThis()
    {
        if (beInGraveyard == true && useRevive == true && Graveyard.transform.childCount > 0)
        {
            Debug.Log("Reviving card");
            this.transform.SetParent(Hand.transform);
            useRevive = false;
            beInGraveyard = false;
            summoningSickness = true;
        }
        else
        {
            Debug.Log("Revive conditions not met");
            Debug.Log("beInGraveyard: " + beInGraveyard);
            Debug.Log("useRevive: " + useRevive);
            Debug.Log("Graveyard child count: " + Graveyard.transform.childCount);
        }
    }*/
    public void Heal()
    {
        PlayerHp.staticHp += healXpower;
    }

    /*public void AttackBoost()
    {
        power += boostXpower; // Update power first
        actualpower = power - hurted; // Update actualpower after modifying power
        UpdateUI();
    }*/
    public void dealxDamage(int x)
    {
        if (Target != null)
        {
            if (staticTargetingEnemy = true && stopDealDamage == false && Input.GetMouseButton(0))
            {
                EnemyHp.staticHp -= damageDealtBySpell;
                stopDealDamage = true;

            }

        }

        else
        {
            foreach (Transform child in EnemyZone.transform)
            {
               Debug.Log("Attempting to deal damage to AI.");
                AICardToHand childAICard = child.GetComponent<AICardToHand>();

                if (childAICard.isTarget == true && stopDealDamage == false)
                {
                    
                    childAICard.hurted += damageDealtBySpell;  // Adjusting hurted value by the power of the attacking card
                    stopDealDamage = true;
                   Debug.Log("Test)");
                }
            }
        }
        }
    
}
