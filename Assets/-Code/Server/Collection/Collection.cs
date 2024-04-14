using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using TMPro;
using Game.Shared;

namespace Game.Server
{
    /// <summary>
    /// What is the sole responsibility of this class?
    /// </summary>
    public class Collection : MonoBehaviour
    {
        [SerializeField][System.Obsolete("Replace with "+nameof(_cardOne))] GameObject CardOne;// remove when replaced
        [SerializeField][System.Obsolete("Replace with "+nameof(_cardTwo))] GameObject CardTwo;// remove when replaced
        [SerializeField][System.Obsolete("Replace with "+nameof(_cardThree))] GameObject CardThree;// remove when replaced
        [SerializeField][System.Obsolete("Replace with "+nameof(_cardFour))] GameObject CardFour;// remove when replaced
        [SerializeField] CardInCollection _cardOne;
        [SerializeField] CardInCollection _cardTwo;
        [SerializeField] CardInCollection _cardThree;
        [SerializeField] CardInCollection _cardFour;
        [SerializeField][FormerlySerializedAs("CardOneText")] TextMeshProUGUI _cardOneText;
        [SerializeField][FormerlySerializedAs("CardTwoText")] TextMeshProUGUI _cardTwoText;
        [SerializeField][FormerlySerializedAs("CardThreeText")] TextMeshProUGUI _cardThreeText;
        [SerializeField][FormerlySerializedAs("CardFourText")] TextMeshProUGUI _cardFourText;

        [System.Obsolete("Get rid of me")] public static int DECK_POSITION;
        [System.Obsolete("Get rid of me")] public int[] HowManyCards;
        DeckAsset _deck;

        const string k_PlayerPrefs_deck_key = "@TODO: come up with a sensible keyword here";

#if UNITY_EDITOR
        void OnValidate ()// delete when references get reassigned and saved in every scene
        {
            _cardOne = CardOne.GetComponent<CardInCollection>();
            _cardTwo = CardTwo.GetComponent<CardInCollection>();
            _cardThree = CardThree.GetComponent<CardInCollection>();
            _cardFour = CardFour.GetComponent<CardInCollection>();
        }
#endif

        void Awake ()
        {
            // @TODO: source this deck from elsewhere
            string json = PlayerPrefs.GetString( k_PlayerPrefs_deck_key , DeckAsset.GetDefaultJson() );
            bool loadedSuccessfully = DeckAsset.FromJson(json,out _deck);
            if( !loadedSuccessfully )
            {
                // idk
                // @TODO: handle the situation somehow
            }

            OnDeckPositionChanged();
        }

        [System.Obsolete("Get rid of me. Refactor this into events or methods that are not called every second(!) but immediately after something related happens.")]
        void FixedUpdate ()
        {
            // @TODO: saving a deck every frame is insane, remove
            PlayerPrefs.SetString( k_PlayerPrefs_deck_key , DeckAsset.ToJson(_deck) );
        }

        void OnDeckPositionChanged ()
        {
            int numCards = _deck.Length;
            if( DECK_POSITION<numCards ) _cardOne.AssignCard( _deck.PeekAtIndex(DECK_POSITION) );
            if( DECK_POSITION+1<numCards ) _cardTwo.AssignCard( _deck.PeekAtIndex(DECK_POSITION+1) );
            if( DECK_POSITION+2<numCards ) _cardThree.AssignCard( _deck.PeekAtIndex(DECK_POSITION+2) );
            if( DECK_POSITION+3<numCards ) _cardFour.AssignCard( _deck.PeekAtIndex(DECK_POSITION+3) );

            UpdateUI();
        }

        void UpdateUI ()
        {
            _cardOneText.text = $"x{HowManyCards[DECK_POSITION]}";
            _cardTwoText.text = $"x{HowManyCards[DECK_POSITION+1]}";
            _cardThreeText.text = $"x{HowManyCards[DECK_POSITION+2]}";
            _cardFourText.text = $"x{HowManyCards[DECK_POSITION+3]}";

            _cardOne.beGrey = HowManyCards[DECK_POSITION]==0;
            _cardTwo.beGrey = HowManyCards[DECK_POSITION+1]==0;
            _cardThree.beGrey = HowManyCards[DECK_POSITION+2]==0;
            _cardFour.beGrey = HowManyCards[DECK_POSITION+3]==0;
        }

        public void Left ()
        {
            DECK_POSITION -= 4;
            OnDeckPositionChanged();
        }
        public void Right ()
        {
            DECK_POSITION += 4;
            OnDeckPositionChanged();
        }
        public void Card1Minus () => HowManyCards[DECK_POSITION]--;
        public void Card1Plus () => HowManyCards[DECK_POSITION]++;
        public void Card2Minus () => HowManyCards[DECK_POSITION+1]--;
        public void Card2Plus () => HowManyCards[DECK_POSITION+1]++;
        public void Card3Minus () => HowManyCards[DECK_POSITION+2]--;
        public void Card3Plus () => HowManyCards[DECK_POSITION+2]++;
        public void Card4Minus () => HowManyCards[DECK_POSITION+3]--;
        public void Card4Plus () => HowManyCards[DECK_POSITION+3]++;
        public void ResetData () => PlayerPrefs.DeleteAll();

    }
}
