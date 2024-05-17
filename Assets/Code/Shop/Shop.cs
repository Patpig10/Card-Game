using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class Shop : MonoBehaviour
{
   public TMPro.TextMeshProUGUI soulText;
    public int soul;

    // Start is called before the first frame update
    void Start()
    {
        soul = PlayerPrefs.GetInt("soul");
        soulText.text = "Soul: " + soul;

    }

    // Update is called once per frame
    void Update()
    {
        soulText.text = "Soul: " + soul;
    }

    public void BuyCard(int cost)
    {
        if (soul >= cost)
        {
            soul -= cost;
            PlayerPrefs.SetInt("soul", soul);
            SceneManager.LoadScene("OpenPack");
        }
    }
}
