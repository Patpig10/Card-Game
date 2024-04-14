using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Assertions;
using TMPro;
using NaughtyAttributes;
using Game.Shared;

using IO = System.IO;

namespace Game.Server
{
    /// <summary>
    /// What is the sole responsibility of this class?
    /// </summary>
    public class PlayerDeckComponent : MonoBehaviour
    {
        [SerializeField][Required] GameObject cardInDeck1;
        [SerializeField][Required] GameObject cardInDeck2;
        [SerializeField][Required] GameObject cardInDeck3;
        [SerializeField][Required] GameObject cardInDeck4;
        [SerializeField][Required] GameObject CardBack;
        [SerializeField][Required] GameObject CardToHand;
        [SerializeField][Required] GameObject Deck;
        [SerializeField][Required] GameObject Hand;
        [SerializeField][Required] TextMeshProUGUI _loseText;
        [SerializeField] TurnSystem _turnSystem;

        void Awake ()
        {
            _turnSystem.OnGameStarted += TurnSystem_GameStarted;
            _turnSystem.OnOpponentTurnEnded += TurnSystem_PlayerTurnStarted;
            _turnSystem.OnPlayerTurnEnded += TurnSystem_PlayerTurnEnded;
        }

        void Start ()
        {
            OnNumberOfCardsInHandChanged();
        }

        void OnDestroy ()
        {
            _turnSystem.OnGameStarted -= TurnSystem_GameStarted;
            _turnSystem.OnOpponentTurnEnded -= TurnSystem_PlayerTurnStarted;
            _turnSystem.OnPlayerTurnEnded -= TurnSystem_PlayerTurnEnded;
        }

        IEnumerator DestroyEvertDeckGameobjectAfterASecondOfDelayRoutine ()
        {
            yield return new WaitForSeconds(1);
            foreach( GameObject go in GameObject.FindGameObjectsWithTag("Deck") )
            {
                Destroy( go );
            }
        }

        IEnumerator StartGameRoutine ()
        {
            // Draw 4 initial cards
            var delay = new WaitForSeconds(1);
            for( int i=0 ; i<=4 ; i++ )
            {
                DrawCardToHand( shuffleBeforeDrawing:false );
                yield return delay;
            }
        }

        public bool DrawCardFromTheTop ( out CardAsset drawnCard ) => PlayerAsset.Player.CardsInDeck.DrawCardFromTheTop(out drawnCard);
        public void AddCard ( CardAsset cardAsset ) => PlayerAsset.Player.CardsInDeck.AddCardAtTheTop( cardAsset );
        public void Shuffle ( uint randomnessSeed ) => PlayerAsset.Player.CardsInDeck.Shuffle(randomnessSeed);
        public void Shuffle () => Shuffle((uint)Random.Range(1,int.MaxValue));

        public void DrawCardToHand ( bool shuffleBeforeDrawing )
        {
            if( shuffleBeforeDrawing )
            {
                Shuffle();

                // @TODO: WHY IS SHUFFLE INSTANTIATING ANYTHING?
                Instantiate( CardBack , transform.position , transform.rotation );

                StartCoroutine( DestroyEvertDeckGameobjectAfterASecondOfDelayRoutine() );
            }

            // If there are valid cards, select a random one
            if( DrawCardFromTheTop(out var drawnCard) )
            {
                // Instantiate the CardToHand object
                var go = Instantiate( CardToHand , transform.position , transform.rotation );
                var cardComp = go.GetComponent<CardComponent>();
                cardComp.InitializeInstance( drawnCard , _turnSystem , this );

                OnNumberOfCardsInHandChanged();
            }
            else Debug.LogWarning("No more cards to draw from the loaded deck.",this);
        }

        void TurnSystem_GameStarted ( TurnSystem turnSystem )
        {
            StartCoroutine( StartGameRoutine() );
        }
        void TurnSystem_PlayerTurnStarted ( TurnSystem turnSystem )
        {
            StartCoroutine( DrawRoutine(1) );
        }
        void TurnSystem_PlayerTurnEnded ( TurnSystem turnSystem )
        {
            
        }

        public void Draw ( int x )
        {
            StartCoroutine( DrawRoutine(x) );
        }
        IEnumerator DrawRoutine ( int x )
        {
            var delay = new WaitForSeconds(1);
            for( int i=0 ; i<x ; i++ )
            {
                yield return delay;
                DrawCardToHand( shuffleBeforeDrawing:false );
            }
        }

        void OnNumberOfCardsInHandChanged ()
        {
            int deckSize = PlayerAsset.Player.CardsInHand.Length;
            cardInDeck1.SetActive(deckSize>=30);
            cardInDeck2.SetActive(deckSize>=20);
            cardInDeck3.SetActive(deckSize>=2);
            cardInDeck4.SetActive(deckSize>=1);

            if( deckSize==0 )
            {
                _loseText.text = "Deck Out, You Lose";
                _loseText.gameObject.SetActive( true );
            }
        }

    }
}