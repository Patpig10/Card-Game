using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardInCollection : MonoBehaviour
{
    private List<Card> thisCardList = new List<Card>();
    public int thisID;

    public int id;
    public string cardName;
    public int cost;
    public int power;
    public string cardDescription;
    public int battlepower;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI powerText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI Battlepowertext;

    public Sprite thisSprite;
    public Image thatImage;

    public Image frame;

    public bool beGrey;
    //public GameObject frame;
    // Start is called before the first frame update
    void Start()
    {
        if (thisID >= 0 && thisID < CardDataBase.cardList.Count)
        {
            thisCardList.Add(CardDataBase.cardList[thisID]);
            thatImage.sprite = thisCardList[0].thisImage;
            StartCoroutine(DelayedUpdate());
        }
        else
        {
            Debug.LogError("Invalid thisID: " + thisID);
        }
        // id = thisID;
    }

    // Update is called once per frame
    void Update()
    {
        
        thisCardList[0] = CardDataBase.cardList[thisID];

        id = thisCardList[0].id;
        cardName = thisCardList[0].cardName;
        cost = thisCardList[0].cost;
        power = thisCardList[0].power;
       // actualpower = power - hurted;
        cardDescription = thisCardList[0].cardDescription;
        thisSprite = thisCardList[0].thisImage;
        battlepower = thisCardList[0].attackPower;
        if (beGrey == true)
        {
            frame.color = new Color(255, 0, 0, 255);
        }
        else
        {
            if (thisCardList[0].color == "White")
            {
                frame.color = new Color32(255, 255, 255, 255);  // Set the color to white
            }
            else if (thisCardList[0].color == "Blue")
            {
                frame.color = new Color32(26, 109, 236, 255);  // Set the color to white
            }
            else if (thisCardList[0].color == "Green")
            {
                frame.color = new Color32(122, 236, 26, 255);  // Set the color to white
            }
            else if (thisCardList[0].color == "Black")
            {
                frame.color = new Color32(51, 32, 32, 255);  // Set the color to white
            }
        }
        UpdateUI();

    }
    void UpdateUI()
    {

        nameText.text = cardName;
        costText.text = cost.ToString();
        powerText.text = power.ToString();
        descriptionText.text = cardDescription;
        thatImage.sprite = thisSprite;
        Battlepowertext.text = "Attack: " + battlepower.ToString();
    }
    IEnumerator DelayedUpdate()
    {
        yield return null; // Wait for one frame
        Update();
    }
}
