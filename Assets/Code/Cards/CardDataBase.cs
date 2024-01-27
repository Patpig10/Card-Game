using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDataBase : MonoBehaviour
{
    public static List<Card> cardList = new List<Card>();

    void Awake()
    {
        cardList.Add(new Card (0, "None", 0, 0, "None", Resources.Load <Sprite>("1"),"None", 0,0,0,0,0));
        cardList.Add(new Card(1, "Ezeh", 2, 3000, "Warrior of light", Resources.Load < Sprite>("2"), "White",0,0,0,0,0));
        cardList.Add(new Card(2, "Luci", 1, 5000, "Cute dummy", Resources.Load < Sprite>("3"),"Blue", 0,0,0,0,0));
        cardList.Add(new Card(3, "Liz", 3, 7000, "power boosrt, Draw 1", Resources.Load < Sprite>("4"), "Green", 1, 0,0,0,200000));
        cardList.Add(new Card(4, "Kris", 2, 40000, "From Grave To hand, 2 Gil", Resources.Load < Sprite>("5"), "Black", 0, 2,1,500000,0));

    }
}
