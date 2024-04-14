using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
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
        AsyncOperationHandle<Sprite> _mainImageAsyncHandle;

        void OnDestroy ()
        {
            if( _mainImageAsyncHandle.IsValid() ) Addressables.Release( _mainImageAsyncHandle );
        }

        void UpdateUI ()
        {
            nameText.text = _cardAsset.CardName;
            costText.text = _cardAsset.Cost.ToString();
            powerText.text = _cardAsset.Power.ToString();
            descriptionText.text = _cardAsset.CardDescription;

            // load card sprite asynchronically (delayed but better performance)
            if( _cardAsset.MainImage.IsDone )
            {
                thatImage.sprite = _cardAsset.MainImage.LoadAssetAsync().WaitForCompletion();
            }
            else
            {
                _cardAsset.MainImage.LoadAssetAsync().Completed += (op) => {
                    thatImage.sprite = op.Result;
                };
            }

            if( beGrey==true ) frame.color = new Color32(255,0,0,255);
            else
            {
                if( _cardAsset.Tint=="White" ) frame.color = new Color32(255,255,255,255);// Set the color to white
                else if( _cardAsset.Tint=="Blue" ) frame.color = new Color32(26,109,236,255);// Set the color to white
                else if( _cardAsset.Tint=="Green" ) frame.color = new Color32(122,236,26,255);// Set the color to white
                else if( _cardAsset.Tint=="Black" ) frame.color = new Color32(51,32,32,255);// Set the color to white
            }
        }

        public void AssignCard ( CardAsset cardAsset )
        {
            _cardAsset = cardAsset;

            // load card sprite asynchronically (delayed but better performance)
            if( _mainImageAsyncHandle.IsValid() ) Addressables.Release( _mainImageAsyncHandle );
            _mainImageAsyncHandle = Addressables.LoadAssetAsync<Sprite>( _cardAsset.MainImage );
            _mainImageAsyncHandle.Completed += (op) => {
                thatImage.sprite = op.Result;
            };

            UpdateUI();
        }

    }
}
