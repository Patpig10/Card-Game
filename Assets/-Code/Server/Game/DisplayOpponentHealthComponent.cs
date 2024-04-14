using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Game.Shared;

namespace Game.Server
{
    /// <summary>
    /// Displays enemy health
    /// </summary>
    public class DisplayOpponentHealthComponent : MonoBehaviour
    {
        [SerializeField] Image Health;
        [SerializeField] TextMeshProUGUI hpText;

        void Awake ()
        {
            var opponent = PlayerAsset.Opponent;
            UpdateUI( opponent );
            opponent.OnResetToDefault += OnResetToDefault;
            opponent.OnDamaged += OnDamaged;
            opponent.OnHealed += OnHealed;
        }

        void OnDestroy ()
        {
            var opponent = PlayerAsset.Opponent;
            opponent.OnResetToDefault -= OnResetToDefault;
            opponent.OnDamaged -= OnDamaged;
            opponent.OnHealed -= OnHealed;
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
