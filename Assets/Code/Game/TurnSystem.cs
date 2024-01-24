using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnSystem : MonoBehaviour
{
    public bool isYourTurn;
    public int yourTurn;
    public int yourOpponentTurn;

    public TextMeshProUGUI turnText; // Change the type to TextMeshProUGUI

    public static int maxGil;
    public static int currentGil;
    public TextMeshProUGUI gilText; // Change the type to TextMeshProUGUI

    public static bool startTurn;

    // Start is called before the first frame update
    void Start()
    {
        isYourTurn = true;
        yourTurn = 1;
        yourOpponentTurn = 0;

        maxGil = 1;
        currentGil = 1;
        startTurn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isYourTurn == true)
        {
            turnText.text = "Your Turn";
        }
        else
        {
            turnText.text = "Opponent Turn";
        }

        gilText.text = currentGil + "/" + maxGil;
    }

    public void EndYourTurn()
    {

        isYourTurn = false;
        yourOpponentTurn++;
        if (isYourTurn == false)
        {
            
        }
    }

    public void EndYourOpponentTurn()
    {
        isYourTurn = true;
        yourTurn += 1;

        maxGil += 1;
        currentGil = maxGil;
        startTurn = true;
    }
}
