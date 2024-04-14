using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;

using Game.Shared;

namespace Game.Server
{
    /// <summary>
    /// Displays player health
    /// </summary>
    public class DisplayPlayerHealthComponent : MonoBehaviour
    {
        [SerializeField] Image Health;
        [SerializeField] TextMeshProUGUI hpText;

        void Awake ()
        {
            const float refreshTime = 0.2f;
            InvokeRepeating( nameof(UpdateUI) , refreshTime , refreshTime );
        }

        void UpdateUI ()
        {
            float hp = Mathf.Clamp( PlayerAsset.Player.Health , 0 , PlayerAsset.Player.HealthMax );
            Health.fillAmount = hp / PlayerAsset.Player.HealthMax;
            hpText.text = $"{hp}HP";
        }
    }
}
