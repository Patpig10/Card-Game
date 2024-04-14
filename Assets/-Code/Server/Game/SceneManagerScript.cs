using UnityEngine;
using UnityEngine.SceneManagement;

using Game.Shared;

namespace Game.Server
{
    public class SceneManagerScript : MonoBehaviour
    {
        public void LoadGameScene ()
        {
            SceneManager.LoadScene("Card");
        }

        public void LoadDeckCreatorScene ()
        {
            SceneManager.LoadScene("CreateDeck");
        }

        // Add more methods for other scenes as needed
    }
}
