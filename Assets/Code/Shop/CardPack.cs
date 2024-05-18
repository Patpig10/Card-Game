using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardPack
{
    public string packName;
    public List<int> cardIds;

    public CardPack(string name, List<int> ids)
    {
        packName = name;
        cardIds = ids;
    }
}