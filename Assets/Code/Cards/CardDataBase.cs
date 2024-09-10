using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDataBase : MonoBehaviour
{
    public static List<Card> cardList = new List<Card>();

    void Awake()
    {
        //int ID, string CardName, int Cost, int Power, string CardDescription, Sprite ThisImage, string Color, int DrawXCards, int AddXmaxGil, int ReturnXcards, int HealXpower,int BoostXpower,bool Spell,int DamageDealtBySpell, bool Ward,int ResurrectXcards, int lightStatus, int darkStatus,bool givelight,bool givedark, int aoe ,bool steal, bool rush)

        //monster
        cardList.Add(new Card (0, "None", 0, 0, "None", Resources.Load<Sprite>("1"), "None", 0, 0, 0, 0, 0, false, 0, false, 0, 0, 0, false, false, false, 0, false, false,0));
        cardList.Add(new Card(1, "Morbol Jr", 1, 30, "A beast that has a strong bite", Resources.Load < Sprite>("morbol jr"), "White",0,0,0,0,0, false, 0, false,0, 0, 0, false, false, false,0,false,false,20));
        cardList.Add(new Card(2, "Morbol King", 5, 50, "Devour those who are unlucky to meet it", Resources.Load < Sprite>("Morbol King"), "White", 0,0,0,0,0, false, 0, false,0, 0, 0, false, false, false,0,false,false, 20));
        cardList.Add(new Card(3, "Liz", 3, 20, "Draw 1", Resources.Load < Sprite>("4"), "White", 1, 0,0,0,0,false,0, false,0, 0, 0, false, false, false,0, false, false, 20));
        cardList.Add(new Card(4, "Kris", 4, 30, "From Grave To hand", Resources.Load < Sprite>("5"), "White", 0, 2,1,0,0, false, 0, false,0, 0, 0, false, false, false, 0, false,false, 20));

      

        //spells
        cardList.Add(new Card(5, "Holy Light", 6, 0, "Spell Card damage 300", Resources.Load<Sprite>("Heal"), "Black", 0, 0, 0, 0, 0, true, 300, false,0, 0, 0, false, false, false,0,false,false, 0));
      cardList.Add(new Card(6, "Luci2", 3, 0, "revive", Resources.Load<Sprite>("3"), "Blue", 0, 0, 0, 0, 0, true, 0, false, 1, 0, 0, false, false, false, 0, false, false, 0));
       cardList.Add(new Card(7, "Liz2", 3, 0, "Heal spell", Resources.Load<Sprite>("Heal"), "Green", 0, 0, 0, 500, 0, true, 0, false, 0, 0, 0, false, false, false,0, false,false, 0));
     cardList.Add(new Card(8, "Kris2", 2, 0, "Draw 2", Resources.Load<Sprite>("5"), "Blue", 2, 0, 0, 0, 0, true, 0, false,0, 0, 0, false, false, false,0, false, false, 0));



        //Monsters light
       cardList.Add(new Card(9, "Alane", 1, 40, "Give 1 light", Resources.Load<Sprite>("2"), "White", 0, 0, 0, 0, 0, false, 0, false, 0, 1, 0, true, false, true,0,true, false, 20));
        cardList.Add(new Card(10, "Alice", 1, 40, "Give 1 light", Resources.Load<Sprite>("2"), "White", 0, 0, 0, 0, 0, false, 0, false, 0, 1, 0, true, false, true, 0, false, false, 20));
        cardList.Add(new Card(11, "Sora", 1, 40, "Give 1 light", Resources.Load<Sprite>("2"), "White", 0, 0, 0, 0, 0, false, 0, false, 0, 1, 0, true, false, true, 0, false,false, 20));
        cardList.Add(new Card(12, "Beta", 1, 40, "Give 1 light", Resources.Load<Sprite>("2"), "White", 0, 0, 0, 0, 0, false, 0, false, 0, 1, 0, true, false, true, 0, false, false, 20));

        //Monsters dark
        cardList.Add(new Card(13, "Lust", 1, 40, "Give 1 dark", Resources.Load<Sprite>("2"), "White", 0, 0, 0, 0, 0, false, 0, false, 0, 0, 1, false, true, false, 0, false, false, 20));
        cardList.Add(new Card(14, "Greed", 1, 40, "Give 1 dark", Resources.Load<Sprite>("2"), "White", 0, 0, 0, 0, 0, false, 0, false, 0, 0, 1, false, true, false, 0, false, false, 20));
        cardList.Add(new Card(15, "Sloth", 1, 40, "Give 1 dark", Resources.Load<Sprite>("2"), "White", 0, 0, 0, 0, 0, false, 0, false, 0, 0, 1, false, true, false, 0, false, false, 20));
        cardList.Add(new Card(16, "Envy", 1, 40, "Give 1 dark", Resources.Load<Sprite>("2"), "White", 0, 0, 0, 0, 0, false, 0, false, 0, 0, 1, false, true, false, 0, false, false, 20));

        //ninja
        cardList.Add(new Card(17, "red", 1, 40, "rush", Resources.Load<Sprite>("2"), "White", 0, 0, 0, 0, 0, false, 0, false, 0, 0, 1, false, true, false, 0, false, false, 20));
        cardList.Add(new Card(18, "green", 1, 40, "rush", Resources.Load<Sprite>("2"), "White", 0, 0, 0, 0, 0, false, 0, false, 0, 0, 1, false, true, false, 0, false, false, 20));
        cardList.Add(new Card(19, "blue", 1, 40, "rush", Resources.Load<Sprite>("2"), "White", 0, 0, 0, 0, 0, false, 0, false, 0, 0, 1, false, true, false, 0, false, false, 20));
        cardList.Add(new Card(20, "yellow", 1, 40, "rush", Resources.Load<Sprite>("2"), "White", 0, 0, 0, 0, 0, false, 0, false, 0, 0, 1, false, true, false, 0, false, false, 20));


    }

    public static Card GetCardById(int id)
    {
        return cardList.Find(card => card.id == id);
    }
}
