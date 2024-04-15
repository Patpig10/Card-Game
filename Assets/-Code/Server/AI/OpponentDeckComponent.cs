using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using NaughtyAttributes;
using Game.Shared;

// FSM pattern taken from https://gist.github.com/andrew-raphael-lukasik/e340d8d8b8ef926cbf2b6d15380aca17

namespace Game.Server
{
    /// <summary>
    /// What is the sole responsibility of this class?
    /// </summary>
    public class OpponentDeckComponent : MonoBehaviour
    {
        [SerializeField] GameObject cardInDeck1;
        [SerializeField] GameObject cardInDeck2;
        [SerializeField] GameObject cardInDeck3;
        [SerializeField] GameObject cardInDeck4;
        [SerializeField][FormerlySerializedAs("CardToHand")] GameObject _cardToHandPrefab;
        [SerializeField][FormerlySerializedAs("AICardBack")] GameObject _aiCardBackPrefab;
        [SerializeField] TurnSystem _turnSystem;

        [ShowNativeProperty] public EState phase => _state;
        EState _state = EState.IDLE;
        float _stateChangeTime;
        Transform _hand;
        Transform _zone;
        Transform _graveyard;

        void Awake ()
        {
            _turnSystem.OnGameStarted += TurnSystem_GameStarted;
            _turnSystem.OnPlayerTurnEnded += TurnSystem_OpponentTurnStarted;
            _turnSystem.OnOpponentTurnEnded += TurnSystem_OpponentTurnEnded;
        }

        void Start ()
        {
            _hand = GameObject.Find("EnemyHand").transform;
            _zone = GameObject.Find("EnemyZone").transform;
            _graveyard = GameObject.Find("EnemyGraveyard").transform;
            OnNumberOfCardsInDeckChanged();
        }

        void OnDestroy ()
        {
            _turnSystem.OnGameStarted -= TurnSystem_GameStarted;
            _turnSystem.OnPlayerTurnEnded -= TurnSystem_OpponentTurnStarted;
            _turnSystem.OnOpponentTurnEnded -= TurnSystem_OpponentTurnEnded;
        }

        void FixedUpdate()
        {
            OnStateTick(_state, Time.time - _stateChangeTime);
        }

        /// <summary>Call this to change state.</summary>
        /// <remarks>Do not modify value _state outside this.</summaremarksry>
        void ChangeState ( EState nextState )
        {
            if( nextState==_state)
            {
                // do NOT comment this out, rethink the call logic to fix the damn warning
                Debug.LogError($"{nameof(ChangeState)} fails as {nextState} is a current state already.");
                return;
            }

            OnStateExit(_state);
            _state = nextState;
            _stateChangeTime = Time.time;
            OnStateEnter(_state);
        }

        /// <summary>Good place to change animations, moving speed or play sounds etc.</summary>
        void OnStateEnter ( EState state )
        {
            // state starts:
            if( state==EState.IDLE )
            {
                
            }
            else if( state==EState.DRAW )
            {

            }
            else if( state==EState.SUMMON )
            {
                List<AICardToHand> list = new ();
                foreach( var next in _hand.GetComponentsInChildren<AICardToHand>() )
                if( _turnSystem.CurrentEnemyGil>=next.assignedCardAsset.Cost )
                {
                    list.Add( next );
                }

                if( list.Count!=0 )
                {
                    var summonThis = list[ Random.Range(0,list.Count) ];
                    summonThis.transform.SetParent( _zone );
                    _turnSystem.CurrentEnemyGil -= summonThis.assignedCardAsset.Cost;
                }
            }
            else if( state==EState.ATTACK )
            {
                foreach( var childAICardToHand in _zone.GetComponentsInChildren<AICardToHand>() )
                if( childAICardToHand.canAttack )
                {
                    PlayerAsset.Player.Damage( childAICardToHand.assignedCardAsset.Power );
                }
            }
            else if( state==EState.END )
            {

            }
            else throw new System.NotImplementedException($"{state} not implemented");
        }

        /// <summary>Good place to reset values</summary>
        void OnStateExit ( EState state )
        {
            // state ends:
            if( state==EState.IDLE )
            {
                
            }
            else if( state==EState.DRAW )
            {

            }
            else if( state==EState.SUMMON )
            {

            }
            else if( state==EState.ATTACK )
            {

            }
            else if( state==EState.END )
            {

            }
            else throw new System.NotImplementedException($"{state} not implemented");
        }

        /// <summary>Good place to change states - always using ChangeState(_)!.</summary>
        void OnStateTick ( EState state , float duration )
        {
            // state update:
            if( state==EState.IDLE )
            {
                if( _turnSystem.IsOpponentTurn )
                {
                    ChangeState( EState.DRAW );
                }
            }
            else if( state==EState.DRAW )
            {
                if( duration>5 )// waits for summon phase
                {
                    ChangeState( EState.SUMMON );
                }
            }
            else if( state==EState.SUMMON )
            {
                ChangeState( EState.ATTACK );
            }
            else if( state==EState.ATTACK )
            {
                ChangeState( EState.END );
            }
            else if( state==EState.END )
            {
                if( _turnSystem.IsPlayerTurn )
                {
                    ChangeState( EState.IDLE );
                }
            }
            else throw new System.NotImplementedException($"{state} not implemented");
        }

        void TurnSystem_GameStarted ( TurnSystem turnSystem )
        {
            StartCoroutine( StartGameRoutine() );
        }
        void TurnSystem_OpponentTurnStarted ( TurnSystem turnSystem )
        {
            StartCoroutine( DrawRoutine(1) );
        }
        void TurnSystem_OpponentTurnEnded ( TurnSystem turnSystem )
        {
            
        }

        IEnumerator StartGameRoutine ()
        {
            var delay = new WaitForSeconds(1);
            for( int i=0 ; i<4 ; i++ )
            {
                yield return delay;

                if( !PlayerAsset.Opponent.CardsInDeck.DrawCardFromTheTop(out var cardAsset) )
                {
                    Debug.LogError($"{nameof(StartGameRoutine)} interrupted because opponent's deck is empty",this);
                    break;
                }

                var go = Instantiate( _cardToHandPrefab , transform.position , transform.rotation );
                var comp = go.GetComponent<AICardToHand>();
                comp.InitializeInstance( _turnSystem , cardAsset );

                OnNumberOfCardsInDeckChanged();
            }

            yield return delay;
            ChangeState( EState.SUMMON );
        }

        IEnumerator DestroyEveryDeckGameobjectAfterASecondOfDelayRoutineRoutine ()
        {
            yield return new WaitForSeconds(1);
            foreach( GameObject deck in GameObject.FindGameObjectsWithTag("Deck") )
            {
                Destroy( deck );
            }
        }

        IEnumerator DrawRoutine ( int x )
        {
            var delay = new WaitForSeconds(1);
            for( int i=0 ; i<x ; i++ )
            {
                yield return delay;

                if( !PlayerAsset.Opponent.CardsInDeck.DrawCardFromTheTop(out var cardAsset) )
                {
                    Debug.LogError($"{nameof(DrawRoutine)} interrupted because opponent's deck is empty",this);
                    break;
                }

                var go = Instantiate( _cardToHandPrefab , transform.position , transform.rotation );
                var comp = go.GetComponent<AICardToHand>();
                comp.InitializeInstance( _turnSystem , cardAsset );

                OnNumberOfCardsInDeckChanged();
            }
        }

        public bool DrawCardFromTheTop ( out CardAsset drawnCard ) => PlayerAsset.Opponent.CardsInDeck.DrawCardFromTheTop(out drawnCard);
        public void AddCard ( CardAsset cardAsset ) => PlayerAsset.Opponent.CardsInDeck.AddCardAtTheTop( cardAsset );
        public void Shuffle ( uint randomnessSeed = default )
        {
            PlayerAsset.Opponent.CardsInDeck.Shuffle(randomnessSeed);

            var go = Instantiate( _aiCardBackPrefab , transform.position , transform.rotation );
            StartCoroutine( DestroyEveryDeckGameobjectAfterASecondOfDelayRoutineRoutine() );
        }
        public void Shuffle () => Shuffle((uint)Random.Range(1,int.MaxValue));

        void OnNumberOfCardsInDeckChanged ()
        {
            int deckSize = PlayerAsset.Opponent.CardsInDeck.Length;
            cardInDeck1.SetActive(deckSize>=30);
            cardInDeck2.SetActive(deckSize>=20);
            cardInDeck3.SetActive(deckSize>=2);
            cardInDeck4.SetActive(deckSize>=1);
        }

        public enum EState : byte
        {
            IDLE = 0 ,
            DRAW ,
            SUMMON ,
            ATTACK ,
            END ,
        }

    }
}
