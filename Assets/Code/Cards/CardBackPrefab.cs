using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBackPrefab : MonoBehaviour
{
    public GameObject Deck;
    public GameObject It;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Deck = GameObject.Find("Deck Panel");
            It.transform.SetParent(Deck.transform);
        It.transform.localScale =Vector3.one;
        It.transform.position = new Vector3(transform.position.x, transform.position.y, -48);
        It.transform.eulerAngles = new Vector3(25,0, 0);
    }
}
