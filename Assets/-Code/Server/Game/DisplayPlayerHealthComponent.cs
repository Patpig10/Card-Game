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
            var player = PlayerAsset.Player;
            UpdateUI( player );
            player.OnResetToDefault += OnResetToDefault;
            player.OnDamaged += OnDamaged;
            player.OnHealed += OnHealed;
        }

        void OnDestroy ()
        {
            var player = PlayerAsset.Player;
            player.OnResetToDefault -= OnResetToDefault;
            player.OnDamaged -= OnDamaged;
            player.OnHealed -= OnHealed;
        }

        void OnResetToDefault ( PlayerAsset player ) => UpdateUI(player);
        void OnDamaged ( PlayerAsset player , float value ) => UpdateUI(player);
        void OnHealed ( PlayerAsset player , float value ) => UpdateUI(player);

        void UpdateUI ( PlayerAsset player )
        {
            float hp = Mathf.Clamp( player.Health , 0 , player.HealthMax );
            Health.fillAmount = hp / player.HealthMax;
            hpText.text = $"{hp}HP";
        }
    }
}
