using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Import the TMPro namespace

public class PlayerHp : MonoBehaviour
{
    public static float maxHp;
    public static float staticHp;
    public float hp;
    public Image Health;
    public TextMeshProUGUI hpText; // Use TextMeshProUGUI for TMPro support

    // Start is called before the first frame update
    void Start()
    {
        maxHp = 800;
        staticHp = 800;
    }

    // Update is called once per frame
    void Update()
    {
        hp = staticHp;
        Health.fillAmount = hp / maxHp;

        if (hp > maxHp)
        {
            hp = maxHp;
        }

        // Use text property to update the text in TextMeshProUGUI
        hpText.text = hp + "HP";
    }
}
