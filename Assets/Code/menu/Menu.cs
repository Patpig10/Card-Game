using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{
    public string play;
    public string options;
    public string collection;
    public string deck;
    public string shop;
    public string menu;
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
        SceneManager.LoadScene(options);
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

}
