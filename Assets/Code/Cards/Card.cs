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
    public bool spell;
    public int damageDealtBySpell;
    public bool ward;
    public int resurrectXcards;
    public int lightStatus;
    public int darkStatus;
    public bool givelight;
    public bool givedark;

    // Start is called before the first frame update
    public Card()
    {

    }

    // Update is called once per frame
    public Card(int Id, string CardName, int Cost, int Power, string CardDescription, Sprite ThisImage, string Color, int DrawXCards, int AddXmaxGil, int ReturnXcards, int HealXpower,int BoostXpower,bool Spell,int DamageDealtBySpell, bool Ward,int ResurrectXcards, int LightStatus, int DarkStatus, bool Givelight, bool Givedark)
    {
        id = Id;
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
        spell = Spell;
        damageDealtBySpell = DamageDealtBySpell;
        ward = Ward;
        resurrectXcards = ResurrectXcards;
        lightStatus = LightStatus;
        darkStatus = DarkStatus;
        givelight = Givelight;
        givedark = Givedark;

    }
}
