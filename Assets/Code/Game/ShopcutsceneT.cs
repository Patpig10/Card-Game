using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopcutsceneT : MonoBehaviour
{
    public GameData gameData;
    public TypingEffect typingEffect;
    public GameObject shopcutscene;
    public GameObject Shop;
    // Start is called before the first frame update
    void Start()
    {
        gameData = FindObjectOfType<GameData>();

        // Optionally, handle the case where GameData is not found
        if (gameData == null)
        {
            Debug.LogError("GameData object not found in the scene!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (typingEffect.currentLineIndex == 6)
        {
            gameData.hasEnteredShop = true;
            gameData.isTutorialCompleted = true;
            SaveSystem.SaveGame(gameData);
            shopcutscene.SetActive(false);
        }
        else
        {
           // gameData.hasEnteredShop = false;
            shopcutscene.SetActive(true);
        }

        if(gameData.hasEnteredShop)
        {
            shopcutscene.SetActive(false);
            Shop.SetActive(true);
        }
    }
}
