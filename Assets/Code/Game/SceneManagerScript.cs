using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public void LoadGameScene()
    {
        SceneManager.LoadScene("Card");
    }

    public void LoadDeckCreatorScene()
    {
        SceneManager.LoadScene("CreateDeck");
    }

    // Add more methods for other scenes as needed
}
