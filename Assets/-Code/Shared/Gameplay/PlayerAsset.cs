using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Assertions;
using NaughtyAttributes;

namespace Game.Shared
{
    /// <summary>
    /// Holds session data of a player.
    /// </summary>
    [CreateAssetMenu( menuName="Game/Player" , fileName="player (0)" , order=1 )]
    public class PlayerAsset : ScriptableObject
    {
        public static PlayerAsset Player { get; private set; }
        public static PlayerAsset Opponent { get; private set; }

        [SerializeField] DeckAsset _userAccountCards;
        public DeckAsset UserAccountCards => _userAccountCards;

        // [SerializeField] DeckAsset _cardsInHand;
        // public DeckAsset CardsInHand => _cardsInHand;

        [SerializeField] DeckAsset _cardsInDeck;
        public DeckAsset CardsInDeck => _cardsInDeck;

        [SerializeField] DeckAsset _cardsInGraveyard;
        public DeckAsset CardsInGraveyard => _cardsInGraveyard;

        [ShowNativeProperty] public float Health { get; private set; }
        [ShowNativeProperty] public float HealthMax { get; private set; }

        public event System.Action<PlayerAsset> OnResetToDefault = (player) => Debug.Log($"{nameof(PlayerAsset)}.{nameof(OnResetToDefault)}( {nameof(player)}:{player.name} )",player);
        public event System.Action<PlayerAsset,float> OnDamaged = (player,value) => Debug.Log($"{nameof(PlayerAsset)}.{nameof(OnDamaged)}( {nameof(player)}:{player.name} , {nameof(value)}:{value} )",player);
        public event System.Action<PlayerAsset,float> OnHealed = (player,value) => Debug.Log($"{nameof(PlayerAsset)}.{nameof(OnHealed)}( {nameof(player)}:{player.name} , {nameof(value)}:{value} )",player);

        public void Damage ( float value )
        {
            Health = Mathf.Max( Health - value , 0 );
            OnDamaged( this , value );
        }

        public void Heal ( float value )
        {
            Health = Mathf.Min( Health + value , HealthMax );
            OnHealed( this , value );
        }

        public void ResetToDefaultValues ( bool clearDeck )
        {
            Health = 500;
            HealthMax = 500;

            if( clearDeck )
            {
                _userAccountCards.RemoveAllCards();
                // _cardsInHand.RemoveAllCards();
                _cardsInDeck.RemoveAllCards();
                _cardsInGraveyard.RemoveAllCards();
            }

            OnResetToDefault(this);
        }

        void Reset () => ResetToDefaultValues( clearDeck:false );

        [RuntimeInitializeOnLoadMethod( RuntimeInitializeLoadType.BeforeSceneLoad )]
        static void InitializeType ()
        {
            Addressables.InitializeAsync().WaitForCompletion();
            Player = Addressables.LoadAssetAsync<PlayerAsset>( "player.asset" ).WaitForCompletion();
            Opponent = Addressables.LoadAssetAsync<PlayerAsset>( "opponent.asset" ).WaitForCompletion();
        }

    }
}
