using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.Shared;

namespace Game.Server
{
    public class CardBackComponent : MonoBehaviour
    {

        public GameObject cardBack;

        public void UpdateCard ( bool updown )
        {
            cardBack.SetActive( updown );
        }

    }
}
