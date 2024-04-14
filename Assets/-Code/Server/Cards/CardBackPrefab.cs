using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Shared;

namespace Game.Server
{
    /// <summary>
    /// What is the sole responsibility of this class?
    /// </summary>
    public class CardBackPrefab : MonoBehaviour
    {
        [SerializeField] GameObject It;

        void Start ()
        {
            var playerDeckComponent = FindObjectOfType<PlayerDeckComponent>();
            It.transform.SetParent( playerDeckComponent.transform );
            It.transform.localScale = Vector3.one;
            It.transform.position = new Vector3( transform.position.x , transform.position.y , -48 );
            It.transform.eulerAngles = new Vector3(25,0,0);
        }
    }
}
