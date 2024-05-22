using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameData gameData;

    private void Start()
    {
        LoadGame();
    }

    public void CompleteTutorial()
    {
        gameData.isTutorialCompleted = true;
        SaveGame();
    }

    public void EnterShop()
    {
        if (!gameData.hasEnteredShop)
        {
            // Start cutscene
            StartShopCutscene();
            gameData.hasEnteredShop = true;
            SaveGame();
        }
        else
        {
            // Enter shop without cutscene
            EnterShopDirectly();
        }
    }

    public void SaveGame()
    {
        SaveSystem.SaveGame(gameData);
    }

    private void LoadGame()
    {
        gameData = SaveSystem.LoadGame();
        if (gameData == null)
        {
            gameData = new GameData();
        }
    }

    private void StartShopCutscene()
    {
        // Implementation for starting the cutscene
        Debug.Log("Starting shop cutscene...");
    }

    private void EnterShopDirectly()
    {
        // Implementation for entering the shop without cutscene
        Debug.Log("Entering shop directly...");
    }
}
