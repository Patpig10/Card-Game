using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using Unity.VisualScripting;

public class ThisCard : MonoBehaviour
{
    public List<Card> thisCard = new List<Card>();

    public int thisId;

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
    public static bool staticCardBack;
    public bool beGrey;
    public int lightStatus;
    public int darkStatus;
    public bool givelight;
    public bool givedark;

    public GameObject lightstatus;
    public GameObject darkstatus;
    public TextMeshProUGUI darkText;
    public TextMeshProUGUI lightText;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Initial thisId: " + thisId); // Check initial thisId value
       

        thisCard[0] = CardDataBase.cardList[thisId];
        numberOfCardsInDeck = PlayerDeck.deckSize;

        UpdateUI();
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
        Hand = GameObject.Find("Hand");

        lightstatus.SetActive(false);
        darkstatus.SetActive(false);

        returnXcards = thisCard[0].returnXcards;

      //  power = thisCard[0].power;

        resurrectXcards = thisCard[0].resurrectXcards;
       // power = thisCard[0].power;

    }

    // Update is called once per frame
    void Update()
    {
        Hand = GameObject.Find("Hand");
        if (this.transform.parent == Hand.transform)
        {
            cardBack = false;
        }


        id = thisId;

        cardName = thisCard[0].cardName;
        cost = thisCard[0].cost;
        power = thisCard[0].power;
        cardDescription = thisCard[0].cardDescription;
        thisSprite = thisCard[0].thisImage;
        actualpower = power - hurted;


        drawXcards = thisCard[0].drawXcards;
        addXmaxGil = thisCard[0].addXmaxGil;
        healXpower = thisCard[0].healXpower;
        boostXpower = thisCard[0].boostXpower;
        spell = thisCard[0].spell;
        damageDealtBySpell = thisCard[0].damageDealtBySpell;
        ward = thisCard[0].ward;

        lightStatus = thisCard[0].lightStatus;
        darkStatus = thisCard[0].darkStatus;
        givelight = thisCard[0].givelight;
        givedark = thisCard[0].givedark;


        if (beGrey == true)
        {
            frame.color = new Color(255, 0, 0, 255);
        }
        else
        {

            if (thisCard[0].color == "White")
            {
                frame.color = new Color32(255, 255, 255, 255);  // Set the color to white
            }
            else if (thisCard[0].color == "Blue")
            {
                frame.color = new Color32(26, 109, 236, 255);  // Set the color to white
            }
            else if (thisCard[0].color == "Green")
            {
                frame.color = new Color32(122, 236, 26, 255);  // Set the color to white
            }
            else if (thisCard[0].color == "Black")
            {
                frame.color = new Color32(51, 32, 32, 255);  // Set the color to white
            }
        }
        staticCardBack = cardBack;

        UpdateUI();

        if (this.tag == "Clone")
        {
          
            thisCard[0] = PlayerDeck.staticDeck[numberOfCardsInDeck - 1];
            numberOfCardsInDeck -= 1;
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

            if(summoned == true && this.transform.parent == battleZone.transform && givelight == true)
            {
            
                GiveLight(); 
                givelight = false;
                canAttack = false;

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

        if(lightStatus >= 1 && darkStatus == 0)
        {
            lightstatus.SetActive(true);
        }

        if(darkStatus >= 1 && lightStatus == 0)
        {
            darkstatus.SetActive(true);
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

        if(canBeSummon == true) 
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
            Arrow._Show = true;
            Arrow.startPoint = transform.position;
        }
        else if(dealDamage == false && this.transform.parent == battleZone.transform && spell == true)
        {
            Debug.Log("spell test2.");  
            attackBorder.SetActive(false);
            Arrow._Hide = true;
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

    public void GiveLight()
    {
        // Logic to give light status to a random card in the enemy's zone
        if (EnemyZone != null && EnemyZone.transform.childCount > 0)
        {
            int randomIndex = Random.Range(0, EnemyZone.transform.childCount);
            Transform randomCardTransform = EnemyZone.transform.GetChild(randomIndex);
            AICardToHand randomCard = randomCardTransform.GetComponent<AICardToHand>();  // Assuming AICardToHand has an int property 'light'
            if (randomCard != null)
            {
                randomCard.lightStatus = 1;  // Or any other int value to indicate "light status"
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
   public void UpdateUI()
    {
        nameText.text = cardName;
        costText.text = cost.ToString();
        powerText.text = actualpower.ToString();
        actualpower =power - hurted;
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
                EnemyHp.staticHp -= actualpower;
                targeting = false;
                cantAttack = true;


                Arrow._Hide = true;
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


                    if (actualpower <= 0 || power <=0)
                    {
                        actualpower = 0;
                        Destroy();
                    }
                    Debug.Log("Target found in EnemyZone.");
                    childAICard.hurted += actualpower;  // Adjusting hurted value by the power of the attacking card
                    hurted += childAICard.actualpower;
                    cantAttack = true;

                    Arrow._Hide = true;
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
        Arrow._Hide = true;

    }
    public void TargetEnemy()
    {
        staticTargetingEnemy = true;
    }

    public void StartAttack()
    {
        staticTargeting = true;
        if(canAttack == true)
        {
            Arrow._Show = true;
            Arrow.startPoint = transform.position;
        }
    }

    public void StopAttack()
    {
        staticTargeting = false;
        Arrow._Hide = true;
      //  Arrow.startPoint = transform.position;

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
            /* this.transform.SetParent(Graveyard.transform);
             canBeDestroyed = false;
             summoned = false;
             beInGraveyard = true;

             hurted = 0;*/

            for (int i = 0; i < 40; i++)
            {
                if (Graveyard.GetComponent<Graveyard>().graveyard[i].id == 0)
                {
                    Graveyard.GetComponent<Graveyard>().graveyard[i] = CardDataBase.cardList[id];

                    Graveyard.GetComponent<Graveyard>().objectsInGraveyard[i] = this.gameObject;

                    canBeDestroyed = false;
                    summoned = false;
                    beInGraveyard = true;

                    hurted = 0;

                    transform.SetParent(Graveyard.transform);

                    transform.position = new Vector3(transform.position.x + 4000, transform.position.y, transform.position.z); // Hidden outside of canvas but not disabled

                    break;
                }
            }
        }
    }


    public void Revive(int x)
    {
        for (int i = 0; i <= x; i++)
        {
            Graveyard.GetComponent<Graveyard>().ReviveCard = x;
            useRevive = false;
            resurrectXcards = 0;
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
            Graveyard.GetComponent<Graveyard>().returnCard = x;
            useReturn = false;
            returnXcards = 0;

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
        else if (beInGraveyard == true && useRevive == true && Graveyard.transform.childCount > 0 && spell == false)
        {
      

            this.transform.SetParent(battleZone.transform);
            useRevive = false;
            beInGraveyard = false;
            summoningSickness = true;

        }
        

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
