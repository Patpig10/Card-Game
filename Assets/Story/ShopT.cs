using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopT : MonoBehaviour
{

    public int diaCount = 0;

    public GameObject Options;
    public GameObject Shop;

    public bool fin;
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
        if(diaCount == 7)
        {
          fin = true;
        }
      


    }
}
