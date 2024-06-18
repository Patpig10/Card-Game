using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{
    public string play;
    //public string options;
    public string collection;
    public string deck;
    public string shop;
    public string menu;
    public string Book;

    public GameObject options;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void loadPlay()
    {
        SceneManager.LoadScene(play);
    }
   
    public void loadOptions()
    {
        options.SetActive(true);
    }
    public void closeOptions()
    {
        options.SetActive(false);
    }
    public void loadCollection()
    {
        SceneManager.LoadScene(collection);
    }
    public void loadDeck()
    {
        SceneManager.LoadScene(deck);
    }
    public void loadShop()
    {
        SceneManager.LoadScene(shop);
    }
    public void loadMenu()
    {
        SceneManager.LoadScene(menu);
    }

    public void loadBook()
    {
        SceneManager.LoadScene(Book);
    }
    public void Quit()
    {
        // If we are running in a standalone build of the game

        Application.Quit();



        // UnityEditor.EditorApplication.isPlaying = false;
       // UnityEditor.EditorApplication.isPlaying = false;

    }

}
