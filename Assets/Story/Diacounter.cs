using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diacounter : MonoBehaviour
{
    public int diaCount = 0;

    public GameObject Options;
  
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void AddDia()
    {
        diaCount++;
    }


    // Update is called once per frame
    void Update()
    {
        if (diaCount == 1)
        {
            Options.SetActive(true);
        }
        else
        {
            Options.SetActive(false);
        }


    }
}
