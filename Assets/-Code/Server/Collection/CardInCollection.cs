using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Game.Shared;

namespace Game.Server
{
    /// <summary>
    /// What is the sole responsibility of this class?
    /// </summary>
    public class CardInCollection : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI nameText;
        [SerializeField] TextMeshProUGUI costText;
        [SerializeField] TextMeshProUGUI powerText;
        [SerializeField] TextMeshProUGUI descriptionText;
        [SerializeField] Image thatImage;
        [SerializeField] Image frame;
        public bool beGrey;
        //[SerializeField] GameObject frame;

        CardAsset _cardAsset;

        void UpdateUI ()
        {
            nameText.text = _cardAsset.CardName;
            costText.text = _cardAsset.Cost.ToString();
            powerText.text = _cardAsset.Power.ToString();
            descriptionText.text = _cardAsset.CardDescription;
            thatImage.sprite = _cardAsset.Image;

            if( beGrey==true ) frame.color = new Color32(255,0,0,255);
            else
            {
                if( _cardAsset.Color=="White" ) frame.color = new Color32(255,255,255,255);// Set the color to white
                else if( _cardAsset.Color=="Blue" ) frame.color = new Color32(26,109,236,255);// Set the color to white
                else if( _cardAsset.Color=="Green" ) frame.color = new Color32(122,236,26,255);// Set the color to white
                else if( _cardAsset.Color=="Black" ) frame.color = new Color32(51,32,32,255);// Set the color to white
            }
        }

        public void AssignCard ( CardAsset cardAsset )
        {
            _cardAsset = cardAsset;
            UpdateUI();
        }

    }
}
