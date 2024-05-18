using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class Collection : MonoBehaviour
{
    public List<CardPack> cardPacks = new List<CardPack>();

    public GameObject CardOne;
    public GameObject CardTwo;
    public GameObject CardThree;
    public GameObject CardFour;

    public GameObject CardFive;
    public bool openPack;

    public int[] o;
    public int oo;
    public int rand;
    public string card;


    public static int x;
    public int[] HowManyCards;

    public TextMeshProUGUI CardOneText;
    public TextMeshProUGUI CardTwoText;
    public TextMeshProUGUI CardThreeText;
    public TextMeshProUGUI CardFourText;

    public int cardsInCollection;
    public int numberOfCardsOnPage;
    // Start is called before the first frame update
    void Start()
    {
        x = 1;

        for(int i=1; i<HowManyCards.Length; i++)
        {
            HowManyCards[i] = PlayerPrefs.GetInt("x" + i,0);
        }

        cardPacks.Add(new CardPack("Pack 1", new List<int> { 1, 4, 8, 5 }));
        cardPacks.Add(new CardPack("Pack 2", new List<int> { 2, 4, 1, 6, 7 }));

        if (openPack == true)
        {
            for(int i=0; i<=4;i++)
            {
                getRandomCard();
            }
        }

        cardsInCollection = 8; // 8 Cards in my database | 0 = none card | 1 - 8 = Cards
        numberOfCardsOnPage = 4;

    }

    // Update is called once per frame
    void Update()
    {
        if (openPack == false)
        {
            CardOne.GetComponent<CardInCollection>().thisID = x;
            CardTwo.GetComponent<CardInCollection>().thisID = x + 1;
            CardThree.GetComponent<CardInCollection>().thisID = x + 2;
            CardFour.GetComponent<CardInCollection>().thisID = x + 3;

            CardOneText.text = "x" + HowManyCards[x];
            CardTwoText.text = "x" + HowManyCards[x + 1];
            CardThreeText.text = "x" + HowManyCards[x + 2];
            CardFourText.text = "x" + HowManyCards[x + 3];

            CardOne.GetComponent<CardInCollection>().beGrey = CardOneText.text == "x0";
            CardTwo.GetComponent<CardInCollection>().beGrey = CardTwoText.text == "x0";
            CardThree.GetComponent<CardInCollection>().beGrey = CardThreeText.text == "x0";
            CardFour.GetComponent<CardInCollection>().beGrey = CardFourText.text == "x0";
        }
        else
        {
          //  Debug.LogError("Invalid indices. Ensure x + 3 is within the bounds of HowManyCards array.");
        }
        for(int i = 1; i < HowManyCards.Length; i++)
        {
            PlayerPrefs.SetInt("x" + i, HowManyCards[i]);
        }



        if (openPack == true)
        {

            CardOne.GetComponent<CardInCollection>().thisID = o[0];
            CardTwo.GetComponent<CardInCollection>().thisID = o[1];
            CardThree.GetComponent<CardInCollection>().thisID = o[2];
            CardFour.GetComponent<CardInCollection>().thisID = o[3];
            CardFive.GetComponent<CardInCollection>().thisID = o[4];
        }
    }



    public void Left()
    {
        if (x != 1)
        {
            x -= numberOfCardsOnPage;
        }
    }
    public void Right()
    {
        if (x != (cardsInCollection - numberOfCardsOnPage) + 1)
        {
            x += numberOfCardsOnPage;
        }
    }
    public void Card1Minus()
    {
        
            HowManyCards[x]--;
        
    }
    public void Card1Plus()
    {

        HowManyCards[x]++;
        
    }

    public void Card2Minus()
    {

        HowManyCards[x + 1]--;
        
    }
    public void Card2Plus()
    {

        HowManyCards[x + 1]++;
        
    }
     
    public void Card3Minus()
    {

        HowManyCards[x + 2]--;
        
    }
    public void Card3Plus()
    {

        HowManyCards[x + 2]++;
        
    }
    public void Card4Minus()
    {

        HowManyCards[x + 3]--;
        
    }
    public void Card4Plus()
    {

        HowManyCards[x + 3]++;
        
    }
    public void reset()
    {
        PlayerPrefs.DeleteAll();

    }


    public void getRandomCard()
    {
       
        rand = Random.Range(1, 9);
        PlayerPrefs.SetInt("x" + rand, (int)HowManyCards[rand]++);
        card = CardDataBase.cardList[rand].cardName;
        print("" + card);


        for (int i = 0; i < 8; i++)
        {
           PlayerPrefs.SetInt("x" + i, HowManyCards[i]);
        }

        o[oo] = rand;
        oo++;
        print("card add");
    }



}
