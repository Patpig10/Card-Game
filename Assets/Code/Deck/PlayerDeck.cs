using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerDeck : MonoBehaviour
{

    public List<Card> deck = new List<Card>();
    public List<Card> container = new List<Card>();
    public static List<Card> staticDeck = new List<Card>();


    public int x;
    public static int deckSize;

    public GameObject cardInDeck1;
    public GameObject cardInDeck2;
    public GameObject cardInDeck3;
    public GameObject cardInDeck4;

    public GameObject CardToHand;

    public GameObject CardBack;
    public GameObject Deck;

    public GameObject[] Clones;

    public GameObject Hand;


    public TMP_Text LoseText;
    public GameObject LoseTextGameObject;

    public GameObject concedeWindow;
   // public string menu = "Menu";

    //NEW
  //  public AudioSource audioSource;
   // public AudioClip shuffle, draw;
    //NEW END

    void Awake()
    {
        //Shuffle(); //comment
    }

    // Use this for initialization
    void Start()
    {

        x = 0;
        deckSize = 40;

        for (int i = 1; i <= 8; i++)
        {
            if (PlayerPrefs.GetInt("deck" + i, 0) > 0)
            {
                for (int j = 1; j <= PlayerPrefs.GetInt("deck" + i, 0); j++)
                {
                    deck[x] = CardDataBase.cardList[i];
                    x++;
                }
            }
        }

        Shuffle();


        StartCoroutine(StartGame());
    }

    // Update is called once per frame
    void Update()
    {


        if (deckSize <= 0)
        {
            LoseTextGameObject.SetActive(true);
            LoseText.text = "You Lose";
        }


        staticDeck = deck;

        if (deckSize < 30)
        {
            cardInDeck1.SetActive(false);
        }
        if (deckSize < 20)
        {
            cardInDeck2.SetActive(false);
        }
        if (deckSize < 2)
        {
            cardInDeck3.SetActive(false);
        }
        if (deckSize < 1)
        {
            cardInDeck4.SetActive(false);
        }

        if (ThisCard.drawX > 0)
        {
            StartCoroutine(Draw(ThisCard.drawX));
            ThisCard.drawX = 0;
        }
        if (TurnSystem.startTurn == true)
        {

         
                StartCoroutine(Draw(1));
          
            TurnSystem.startTurn = false;
        }


    }

    IEnumerator Example()
    {
        //COMMENT
        // yield return new WaitForSeconds(1);
        // Clones = GameObject.FindGameObjectsWithTag("Clone");

        // foreach(GameObject Clone in Clones)
        // {
        // 	Destroy(Clone);
        // }
        //COMMENT END

        //NEW
        GameObject prefb = Instantiate(CardBack, transform.position, transform.rotation);
        yield return new WaitForSeconds(1.5f);
        Destroy(prefb);
        //NEW END
    }

    IEnumerator StartGame()
    {
        for (int i = 0; i <= 4; i++)
        {
            yield return new WaitForSeconds(1);
            //NEW
          //  audioSource.PlayOneShot(draw, 1f);
            //NEW
            Instantiate(CardToHand, transform.position, transform.rotation);
        }
    }




    public void Shuffle()
    {


        for (int i = 0; i < deckSize; i++)
        {
            container[0] = deck[i];
            int randomIndex = Random.Range(i, deckSize);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = container[0];

        }

        //NEW 
        //audioSource.PlayOneShot(shuffle, 1f);

        StartCoroutine(Example());
        //NEW END


        //Instantiate(CardBack, transform.position, transform.rotation);// comment

        //StartCoroutine(Example());// comment


    }


    IEnumerator Draw(int x)
    {
        for (int i = 0; i < x; i++)
        {
            yield return new WaitForSeconds(1);
            //NEW
        //    audioSource.PlayOneShot(draw, 1f);
            //NEW END
            Instantiate(CardToHand, transform.position, transform.rotation);
        }
    }

   /* public void OpenWindow()
    {
        concedeWindow.SetActive(true);
    }

    public void CloseWindow()
    {
        concedeWindow.SetActive(false);
    }

    public void ConcedeDefeat()
    {
        StartCoroutine(EndGame());
    }*/

    IEnumerator EndGame()
    {
        LoseTextGameObject.SetActive(true);
        LoseText.text = "You Lose";
       // concedeWindow.SetActive(false);

        yield return new WaitForSeconds(2.5f);

      //  SceneManager.LoadScene(menu);
    }


}

