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
            opponent.OnResetToDefault += OnOpponentResetToDefault;
            opponent.OnDamaged += OnOpponentDamaged;
            opponent.OnHealed += OnOpponentHealed;
        }

        void OnDestroy ()
        {
            var opponent = PlayerAsset.Opponent;
            opponent.OnResetToDefault -= OnOpponentResetToDefault;
            opponent.OnDamaged -= OnOpponentDamaged;
            opponent.OnHealed -= OnOpponentHealed;
        }

        void OnOpponentResetToDefault ( PlayerAsset player ) => UpdateUI(player);
        void OnOpponentDamaged ( PlayerAsset player , float value ) => UpdateUI(player);
        void OnOpponentHealed ( PlayerAsset player , float value ) => UpdateUI(player);

        void UpdateUI ( PlayerAsset player )
        {
            float hp = Mathf.Clamp( player.Health , 0 , player.HealthMax );
            Health.fillAmount = hp / player.HealthMax;
            hpText.text = $"{hp}HP";
        }

    }
}
