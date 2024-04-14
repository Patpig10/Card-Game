using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Game.Shared;

namespace Game.Server
{
    /// <summary>
    /// What is the sole responsibility of this class?
    /// </summary>
    public class WindowInDeck : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI nameText;

        DeckCreator _creator;
        GameObject _panel;

        void Start ()
        {
            _creator = GameObject.Find("Collection").GetComponent<DeckCreator>();
            _panel = GameObject.Find("DeckList");

            transform.SetParent( _panel.transform );
            transform.localScale = new Vector3(1,1,1);
            
            const float refreshTime = 0.2f;
            InvokeRepeating( nameof(UpdateUI) , refreshTime , refreshTime );
        }

        void UpdateUI ()
        {
            var lastAdded = DeckCreator.lastAdded;
            int count = _creator.deck.Count( lastAdded );
            nameText.text = $"{lastAdded.CardName} x{count}";
        }

    }
}
