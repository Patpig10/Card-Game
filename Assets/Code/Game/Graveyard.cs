using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graveyard : MonoBehaviour
{    public List<Card> graveyard = new List<Card>();

    public int howManyCards;

    public GameObject cardBack;


    public GameObject graveWindow;
    public GameObject card1, card2, card3, card4;

    public int controller;


    //NEW 2
    public GameObject[] objectsInGraveyard;
    public GameObject hand;
    public GameObject Battlefield;

    public int returnCard;
    public int ReviveCard;


    public bool UcanReturn;
    public bool useRevive;
    //NEW 2 END

    // Start is called before the first frame update
    void Start()
    {
        controller = 4;
    }

    // Update is called once per frame
    void Update()
    {
        card1.GetComponent<CardInCollection>().thisID = graveyard[controller - 4].id;
        card2.GetComponent<CardInCollection>().thisID = graveyard[controller - 3].id;
        card3.GetComponent<CardInCollection>().thisID = graveyard[controller - 2].id;
        card4.GetComponent<CardInCollection>().thisID = graveyard[controller - 1].id;

        if(card1.GetComponent<CardInCollection>().thisID == 0)
        {
            card1.SetActive(false);
        }else
        {
            card1.SetActive(true);
        }

        if(card2.GetComponent<CardInCollection>().thisID == 0)
        {
            card2.SetActive(false);
        }else
        {
            card2.SetActive(true);
        }

        if(card3.GetComponent<CardInCollection>().thisID == 0)
        {
            card3.SetActive(false);
        }else
        {
            card3.SetActive(true);
        }

        if(card4.GetComponent<CardInCollection>().thisID == 0)
        {
            card4.SetActive(false);
        }else
        {
            card4.SetActive(true);
        }

        CalculateGraveyard();

        if(howManyCards > 0)
        {
            cardBack.SetActive(true);
        }
        else
        {
            cardBack.SetActive(false);
        }

        //NEW 2
        if(returnCard >0 && UcanReturn == false)
        {
            Open();
            UcanReturn = true;
        }

        if (ReviveCard > 0 && useRevive == false) // Change this condition based on your game logic
        {
            Open();

            useRevive = true;
        }
        //NEW 2 END
    }


    public void CalculateGraveyard()
    {
        int x =0;

        for(int i=0; i<40; i++)
        {
            if(graveyard[i].id !=0)
            {
                x++;
            }
        }

        howManyCards = x;
    }

    public void Open()
    {
        graveWindow.SetActive(true);
    }

    public void Close()
    {
        graveWindow.SetActive(false);
    }

    public void Left()
    {
        print("left");
        if(controller > 4)
        {
            controller --;
        }
    }

    public void Right()
    {
        print("right");
        if(controller < howManyCards)
        {
            controller ++;
        }
    }

    //NEW 2
    public void ReturnCard1()
    {
        if(UcanReturn == true)
        {
            objectsInGraveyard[controller -4].GetComponent<ThisCard>().summoned = false;
            objectsInGraveyard[controller -4].GetComponent<ThisCard>().useReturn = false;

            objectsInGraveyard[controller -4].GetComponent<ThisCard>().beInGraveyard = false;
            objectsInGraveyard[controller -4].transform.parent = hand.transform;

            card1.GetComponent<CardInCollection>().thisID =0;

            graveyard[controller -4] = CardDataBase.cardList[0];
            objectsInGraveyard[controller -4] = null;

            Close();

            UcanReturn = false;
            returnCard --;
        }
      else  if(useRevive == true)
        {
            objectsInGraveyard[controller -4].GetComponent<ThisCard>().summoned = false;
            objectsInGraveyard[controller -4].GetComponent<ThisCard>().useReturn = false;

            objectsInGraveyard[controller -4].GetComponent<ThisCard>().beInGraveyard = false;
            objectsInGraveyard[controller -4].transform.parent = Battlefield.transform;

            card1.GetComponent<CardInCollection>().thisID =0;

            graveyard[controller -4] = CardDataBase.cardList[0];
            objectsInGraveyard[controller -4] = null;

            Close();

            useRevive = false;
            ReviveCard--;
        }
    }

    public void ReturnCard2()
    {
        if(UcanReturn == true)
        {
            objectsInGraveyard[controller -3].GetComponent<ThisCard>().summoned = false;
            objectsInGraveyard[controller -3].GetComponent<ThisCard>().useReturn = false;

            objectsInGraveyard[controller -3].GetComponent<ThisCard>().beInGraveyard = false;
            objectsInGraveyard[controller -3].transform.parent = hand.transform;

            card1.GetComponent<CardInCollection>().thisID =0;

            graveyard[controller -3] = CardDataBase.cardList[0];
            objectsInGraveyard[controller -3] = null;

            Close();

            UcanReturn = false;
            returnCard --;
        }
        else if (useRevive == true)
        {
            objectsInGraveyard[controller -3].GetComponent<ThisCard>().summoned = false;
            objectsInGraveyard[controller -3].GetComponent<ThisCard>().useReturn = false;

            objectsInGraveyard[controller -3].GetComponent<ThisCard>().beInGraveyard = false;
            objectsInGraveyard[controller -3].transform.parent = Battlefield.transform;

            card1.GetComponent<CardInCollection>().thisID =0;

            graveyard[controller -3] = CardDataBase.cardList[0];
            objectsInGraveyard[controller -3] = null;

            Close();

            useRevive = false;
            ReviveCard--;
        }
    }

    public void ReturnCard3()
    {
        if(UcanReturn == true)
        {
            objectsInGraveyard[controller -2].GetComponent<ThisCard>().summoned = false;
            objectsInGraveyard[controller -2].GetComponent<ThisCard>().useReturn = false;
    
            objectsInGraveyard[controller -2].GetComponent<ThisCard>().beInGraveyard = false;
            objectsInGraveyard[controller -2].transform.parent = hand.transform;

            card1.GetComponent<CardInCollection>().thisID =0;

            graveyard[controller -2] = CardDataBase.cardList[0];
            objectsInGraveyard[controller -2] = null;

            Close();

            UcanReturn = false;
            returnCard --;
        }

        else if ( UcanReturn == true )
        {
            objectsInGraveyard[controller - 2].GetComponent<ThisCard>().summoned = false;
            objectsInGraveyard[controller - 2].GetComponent<ThisCard>().useReturn = false;

            objectsInGraveyard[controller - 2].GetComponent<ThisCard>().beInGraveyard = false;
            objectsInGraveyard[controller - 2].transform.parent = Battlefield.transform;

            card1.GetComponent<CardInCollection>().thisID = 0;

            graveyard[controller - 2] = CardDataBase.cardList[0];
            objectsInGraveyard[controller - 2] = null;

            Close();

            useRevive = false;
            ReviveCard--;
        }
    }

    public void ReturnCard4()
    {
        if(UcanReturn == true)
        {
            objectsInGraveyard[controller -1].GetComponent<ThisCard>().summoned = false;
            objectsInGraveyard[controller -1].GetComponent<ThisCard>().useReturn = false;

            objectsInGraveyard[controller -1].GetComponent<ThisCard>().beInGraveyard = false;
            objectsInGraveyard[controller -1].transform.parent = hand.transform;

            card1.GetComponent<CardInCollection>().thisID =0;

            graveyard[controller -1] = CardDataBase.cardList[0];
            objectsInGraveyard[controller -1] = null;

            Close();

            UcanReturn = false;
            returnCard --;
        }

        else if (UcanReturn == true)
        {
            objectsInGraveyard[controller - 1].GetComponent<ThisCard>().summoned = false;
            objectsInGraveyard[controller - 1].GetComponent<ThisCard>().useReturn = false;

            objectsInGraveyard[controller - 1].GetComponent<ThisCard>().beInGraveyard = false;
            objectsInGraveyard[controller - 1].transform.parent = Battlefield.transform;

            card1.GetComponent<CardInCollection>().thisID = 0;

            graveyard[controller - 1] = CardDataBase.cardList[0];
            objectsInGraveyard[controller - 1] = null;

            Close();

            useRevive = false;
            ReviveCard--;
        }
    }
}
