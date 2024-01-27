using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class Card
{
    public int id;
    public string cardName;
    public int cost;
    public int power;
    public string cardDescription;

    public int drawXcards;
    public int addXmaxGil;
    public Sprite thisImage;

    public string color;

    public int returnXcards;

    public int healXpower;
    public int boostXpower;
    // Start is called before the first frame update
    public Card()
    {

    }

    // Update is called once per frame
    public Card(int ID, string CardName, int Cost, int Power, string CardDescription, Sprite ThisImage, string Color, int DrawXCards, int AddXmaxGil, int ReturnXcards, int HealXpower,int BoostXpower)
    {
        id = ID;
        cardName = CardName;
        cost = Cost;
        power = Power;
        cardDescription = CardDescription;
        
        thisImage = ThisImage;
        color = Color;
        drawXcards = DrawXCards;
        addXmaxGil = AddXmaxGil;
        returnXcards = ReturnXcards;
        healXpower = HealXpower;
        boostXpower = BoostXpower;
    }
}
