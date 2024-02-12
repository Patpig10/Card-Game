using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDataBase : MonoBehaviour
{
    public static List<Card> cardList = new List<Card>();

    void Awake()
    {
        //monster
        cardList.Add(new Card (0, "None", 0, 0, "None", Resources.Load <Sprite>("1"),"None", 0,0,0,0,0,false, 0, false));
        cardList.Add(new Card(1, "Ezeh", 2, 30, "Warrior of light", Resources.Load < Sprite>("2"), "White",0,0,0,0,0, false, 0, false));
        cardList.Add(new Card(2, "Luci", 1, 50, "Cute dummy", Resources.Load < Sprite>("3"),"Blue", 0,0,0,0,0, false, 0, false));
        cardList.Add(new Card(3, "Liz", 3, 70, "power boosrt 500, Draw 1", Resources.Load < Sprite>("4"), "Green", 1, 0,0,0,500,false,0, true));
        cardList.Add(new Card(4, "Kris", 2, 400, "From Grave To hand, 2 Gil", Resources.Load < Sprite>("5"), "Black", 0, 2,1,500000,0, false, 0, false));
        //spells
        cardList.Add(new Card(5, "Holy Light", 1, 0, "Spell Card damage 3000", Resources.Load<Sprite>("Heal"), "Blue", 0, 0, 0, 0, 0, true, 3000, false));
      //  cardList.Add(new Card(6, "Luci2", 1, 50, "Cute dummy", Resources.Load<Sprite>("3"), "Blue", 0, 0, 0, 0, 0, false, 0));
      //  cardList.Add(new Card(7, "Liz2", 3, 70, "power boosrt 500, Draw 1", Resources.Load<Sprite>("4"), "Green", 1, 0, 0, 0, 500, false, 0));
     //   cardList.Add(new Card(8, "Kris2", 2, 400, "From Grave To hand, 2 Gil", Resources.Load<Sprite>("5"), "Black", 0, 2, 1, 500000, 0, false, 0));
    }
}
