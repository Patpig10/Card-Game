using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDataBase : MonoBehaviour
{
    public static List<Card> cardList = new List<Card>();

    void Awake()
    {
        //int ID, string CardName, int Cost, int Power, string CardDescription, Sprite ThisImage, string Color, int DrawXCards, int AddXmaxGil, int ReturnXcards, int HealXpower,int BoostXpower,bool Spell,int DamageDealtBySpell, bool Ward,int ResurrectXcards

        //monster
        cardList.Add(new Card (0, "None", 0, 0, "None", Resources.Load <Sprite>("1"),"None", 0,0,0,0,0,false, 0, false,0));
        cardList.Add(new Card(1, "Ezeh", 2, 30, "Warrior of light", Resources.Load < Sprite>("2"), "White",0,0,0,0,0, false, 0, false,0));
        cardList.Add(new Card(2, "Luci", 1, 50, "Cute dummy", Resources.Load < Sprite>("3"), "White", 0,0,0,0,0, false, 0, false,0));
        cardList.Add(new Card(3, "Liz", 3, 70, "Rpower boosrt 500, Draw 1", Resources.Load < Sprite>("4"), "White", 1, 0,0,0,500,false,0, false,0));
        cardList.Add(new Card(4, "Kris", 2, 400, "From Grave To hand, 2 Gil", Resources.Load < Sprite>("5"), "White", 0, 2,1,0,0, false, 0, false,0));

        //spells
        cardList.Add(new Card(5, "Holy Light", 1, 0, "Spell Card damage 3000", Resources.Load<Sprite>("Heal"), "Black", 0, 0, 0, 0, 0, true, 3000, false,0));
      cardList.Add(new Card(6, "Luci2", 1, 50, "revive", Resources.Load<Sprite>("3"), "Blue", 0, 0, 0, 0, 0, false, 0, false, 1));
       cardList.Add(new Card(7, "Liz2", 3, 70, "Heal spell", Resources.Load<Sprite>("Heal"), "Green", 0, 0, 0, 500, 0, true, 0, false, 0));
     cardList.Add(new Card(8, "Kris2", 2, 400, "Draw 2", Resources.Load<Sprite>("5"), "Blue", 2, 0, 0, 0, 0, true, 0, false,0));
    }
}
