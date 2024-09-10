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
    public TextMeshProUGUI AttackPowerText;

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
    public  bool ward;
    public bool directAttack;
    public GameObject PlayerZone;
    public GameObject wardguard;

    public bool isSummoned;
    public GameObject battlefield;
    public bool spell;
    public int damageDealtBySpell;
    public bool beInGraveyard;

    public int lightStatus;
    public int darkStatus;
    public bool givelight;
    public bool givedark;

    public GameObject lightstatus;
    public GameObject darkstatus;
    public TextMeshProUGUI darkText;
    public TextMeshProUGUI lightText;

    public bool canbestolen;


    public  bool staticTargetingEnemy;
    public  bool staticTargeting;
    public  bool onlyThisCardAttack;

    public bool targeting;
    public bool targetingEnemy;
    public GameObject Target;
    public GameObject Enemy;
    public bool cantAttack;
    public bool cantTargeting;
    public bool directattack;
    public AICardToHand aiCardToHand;

    public bool stolen;
    public bool rush;
    public int AttackPower; 
    // Start is called before the first frame update
    void Start()
    {



        thisCardList.Add(CardDataBase.cardList[Random.Range(1, CardDataBase.cardList.Count)]);
        thatImage.sprite = thisCardList[0].thisImage;

        thisCardList[0] = CardDataBase.cardList[thisID];
        Hand = GameObject.Find("EnemyHand");

        z = 0;
        numberOfCardsInDeck = AI.deckSize;



        Enemy = GameObject.Find("Enemy HP");


        Graveyard = GameObject.Find("EnemyGraveyard");
        StartCoroutine(AfterVoidStart());
        AiZone = GameObject.Find("EnemyZone");
        PlayerZone = GameObject.Find("Zone");
        canHeal = true;
        summoned = false;
        summoningSickness = true;
        beInGraveyard = false;
        directattack = true;
        targeting = false;
        targetingEnemy = false;

        battlefield = GameObject.Find("EnemyZone");

        lightstatus.SetActive(false);
        darkstatus.SetActive(false);

        givelight = thisCardList[0].givelight;
        givedark = thisCardList[0].givedark;
        rush = thisCardList[0].rush;
        canAttack = false;
        stolen = false;
        AttackPower = thisCardList[0].attackPower;
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
        spell = thisCardList[0].spell;
        damageDealtBySpell = thisCardList[0].damageDealtBySpell;
        drawXcards = thisCardList[0].drawXcards;
        addXmaxGil = thisCardList[0].addXmaxGil;
        returnXcards = thisCardList[0].returnXcards;

        healXpower = thisCardList[0].healXpower;
        ward = thisCardList[0].ward;


      
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
        if (actualpower <= 0 && spell == false)
        {
            Destroy();
            thisCardCanBeDestroyed = true;
        }
       
      if(this.transform.parent == Hand.transform)
        {
           cardBack.SetActive(true);
        }
        if (this.transform.parent == AiZone.transform)
        {
            cardBack.SetActive(false);
        }

        if(TurnSystem.isYourTurn == true)
        {
           canAttack = true;
        }
        else
        {
            canAttack = false;
        }

        if(TurnSystem.isYourTurn == true)
        {
            summoningSickness = false;
        }
        if(ward == true && summoned == true)
        {
            wardguard.SetActive(true);
        }
        else
        {
            wardguard.SetActive(false);
        }


        if (lightStatus >= 1 && darkStatus == 0)
        {
            lightstatus.SetActive(true);
        }

        if (darkStatus >= 1 && lightStatus == 0)
        {
            darkstatus.SetActive(true);
        }



        if (this.transform.parent == battlefield.transform && isSummoned == false)
        {

            if(drawXcards > 0)
            {
                drawX = drawXcards;
                isSummoned = true;
            }

        }

        if (this.transform.parent == battlefield.transform && spell == true)
        {
            Debug.Log("Spell gone");
            thisCardCanBeDestroyed = true;
            Destroy();
        }
        /*foreach (Transform child in PlayerZone.transform)
        {
            ThisCard childAICard = child.GetComponent<ThisCard>();
            if (childAICard.ward == true)
            {
                directAttack = false;
            }
            else
            {
                directAttack = true;
            }
        }*/



        if (summoned == true && this.transform.parent == battlefield.transform && givelight == true)
        {
            givelight = false;
            GiveLight();
        }


        if ((lightStatus >= 3))
        {
            canbestolen = true;
        }



        if (summoned == true && rush == true)
        {
            Rush();

        }




        targeting = staticTargeting;
        targetingEnemy = staticTargetingEnemy;

        if (targetingEnemy == true)
        {
            Target = Enemy;
        }
        else
        {
            Target = null;
        }

        if (targeting == true && onlyThisCardAttack == true)
        {
            Attack();
        }


        foreach (Transform child in AiZone.transform)
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
        if (PlayerZone != null && PlayerZone.transform.childCount > 0)
        {
            int randomIndex = Random.Range(0, PlayerZone.transform.childCount);
            Transform randomCardTransform = PlayerZone.transform.GetChild(randomIndex);
            ThisCard randomCard = randomCardTransform.GetComponent<ThisCard>();  // Assuming AICardToHand has an int property 'light'
            if (randomCard != null)
            {
                randomCard.lightStatus = 1;  // Or any other int value to indicate "light status"
                givelight = false;
            }
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
        darkText.text = darkStatus.ToString();
        lightText.text = lightStatus.ToString();
        AttackPowerText.text = "Attack: " + AttackPower.ToString();
    }

    public void Destroy()
    {
        Graveyard = GameObject.Find("EnemyGraveyard");

        if (thisCardCanBeDestroyed == true)
        {
            this.transform.SetParent(Graveyard.transform);
            thisCardCanBeDestroyed = false;
            summoned = false;
            beInGraveyard = true;
            lightstatus.SetActive(false);
            darkstatus.SetActive(false);
            lightStatus = 0;
            darkStatus = 0;
            hurted = 0;
        }
    }


    public void AttackCard(ThisCard targetCard)
    {
        targetCard.hurted += power;
        hurted += targetCard.actualpower;
        canAttack = false;
    }

    public void AttackPlayerDirectly()
    {
        EnemyHp.staticHp -= power;
        canAttack = false;
    }





    //Stolen

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
            if (canAttack == true)
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






    public void Attack()
    {
        if (canAttack && Target != null)
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

            foreach (Transform child in AiZone.transform)
            {
                AICardToHand childAICard = child.GetComponent<AICardToHand>();

                if (childAICard.isTarget == true && cantAttack == false)
                {


                    if (actualpower <= 0 || power <= 0)
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
    public void Rush()
    {
        summoningSickness = false;
    }

}
