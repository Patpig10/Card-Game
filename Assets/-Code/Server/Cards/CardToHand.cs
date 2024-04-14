using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.Shared;

namespace Game.Server
{
    /// <summary>
    /// What is the sole responsibility of this class?
    /// </summary>
    public class CardToHand : MonoBehaviour
    {
        public GameObject It;
        GameObject _hand;
    
        void Start ()
        {
            _hand = GameObject.Find("Hand");
            It.transform.SetParent( _hand.transform );
            It.transform.localScale = Vector3.one;
            It.transform.position = new Vector3( transform.position.x , transform.position.y , -48 );
            It.transform.eulerAngles = new Vector3(25,0,0);
        }

    }
}
