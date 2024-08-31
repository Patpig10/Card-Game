using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NamesAct0 : MonoBehaviour
{

    public test Test;

    public GameObject Name1;
    public GameObject Name2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(Test.clicks == 0)
        {
            Name1.SetActive(true);
        }

        if(Test.clicks == 1)
        {
            Name1.SetActive(false);
            Name2.SetActive(true);
        }


        
    }
}
