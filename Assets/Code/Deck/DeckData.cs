using UnityEngine;

[CreateAssetMenu(fileName = "DeckData", menuName = "ScriptableObjects/DeckData", order = 1)]
public class DeckData : ScriptableObject
{
    public int[] cardsWithThisId;
    public bool[] alreadyCreated;
    public int[] quantity;

    public void Initialize(int numberOfCards)
    {
        cardsWithThisId = new int[numberOfCards];
        alreadyCreated = new bool[numberOfCards];
        quantity = new int[numberOfCards];
    }
}
