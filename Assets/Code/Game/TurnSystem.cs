using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TurnSystem : MonoBehaviour
{
    public static bool isYourTurn;
    public int yourTurn;
    public int yourOpponentTurn;

    public TextMeshProUGUI turnText; // Change the type to TextMeshProUGUI

    public static int maxGil;
    public static int currentGil;
    public TextMeshProUGUI gilText; // Change the type to TextMeshProUGUI

    public static bool startTurn;
    public int random;

    public bool turnEnd;
    public TextMeshProUGUI timerText;
    public int seconds;
    public bool timerStart;
    public Image timerImage; // Reference to the Image component for the timer fill.
    public  static int maxEnemyGil;
    public static int currentEnemyGil;
    public TextMeshProUGUI enemyGilText;
    // Start is called before the first frame update
    void Start()
    {

        StartGame();
        seconds = 60;
        timerStart = true;
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

        if(isYourTurn == true && seconds > 0 && timerStart ==true) 
        {

            StartCoroutine(Timer());
            timerStart = false;
           // seconds = 60;


        }
        if (seconds == 0 && isYourTurn == true) 
        {
            timerStart = true;
            seconds = 60;
            EndYourTurn();
           // timerStart = true;
            //seconds = 60;
        }
        if (isYourTurn == false && seconds > 0 && timerStart == true)
        {
            StartCoroutine(EnemyTimer());

            timerStart=false;

        }

        if(seconds == 0 && isYourTurn == false)
        {
            seconds = 60;
            timerStart = true;
            EndYourOpponentTurn();
           // timerStart = true;
           // seconds = 60;
        }
        timerText.text = seconds + "";

        gilText.text = currentGil + "/" + maxGil;
        float fillAmount = (float)seconds / 60f; // Assuming 60 seconds
        timerImage.fillAmount = fillAmount;

        enemyGilText.text = currentEnemyGil + "/" + maxEnemyGil;

        if(AI.AiEndPhase == true)
        {
            EndYourOpponentTurn();
            AI.AiEndPhase = false;
        }




    }

    public void EndYourTurn()
    {

        isYourTurn = false;
        yourOpponentTurn++;
        yourTurn--;
        seconds = 60;
        // timerStart = true;
        maxEnemyGil += 1;
        currentEnemyGil = maxEnemyGil;
        StartCoroutine(EnemyTimer());


        AI.draw = false;
    }

    public void EndYourOpponentTurn()
    {
        isYourTurn = true;
        yourTurn += 1;
        yourOpponentTurn--;

        maxGil += 1;
        currentGil = maxGil;
        startTurn = true;
        seconds = 60;
       // timerStart = true;
        StartCoroutine(Timer());

    }
    public void StartGame()
    {
        random = Random.Range(0,2); 
        if(random == 0)
        {
            isYourTurn = true;
            yourTurn = 1;
            yourOpponentTurn = 0;

            maxGil = 1;
            currentGil = 1;
            maxEnemyGil = 0;
            currentEnemyGil = 0;
            startTurn = false;
        }
        if(random == 1)
        {
            isYourTurn = false;
            yourTurn = 0;
            yourOpponentTurn = 1;
            maxEnemyGil = 1;
            currentEnemyGil = 1;
            maxGil = 0;
            currentGil = 0;
            //startTurn = true;
        }
    }

    IEnumerator Timer()
    {
        if(isYourTurn == true && seconds > 0) 
        {

            yield return new WaitForSeconds(1);
            seconds--;
            StartCoroutine(Timer());


        }
    }

    IEnumerator EnemyTimer()
    {
        if (isYourTurn == false && seconds > 0)
        {

            yield return new WaitForSeconds(1);
            seconds--;
            StartCoroutine(EnemyTimer());


        }


    }

}
