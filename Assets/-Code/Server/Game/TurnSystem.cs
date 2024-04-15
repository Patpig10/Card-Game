using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Assertions;
using TMPro;
using NaughtyAttributes;
using Game.Shared;

// FSM pattern taken from https://gist.github.com/andrew-raphael-lukasik/e340d8d8b8ef926cbf2b6d15380aca17

namespace Game.Server
{
    /// <summary>
    /// High-level control over turn-based gameplay
    /// </summary>
    public class TurnSystem : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI turnText;
        [SerializeField] TextMeshProUGUI gilText;
        [SerializeField] TextMeshProUGUI timerText;
        [SerializeField] Image timerImage;// reference to the Image component for the timer fill.
        [SerializeField] TextMeshProUGUI enemyGilText;

        [ShowNonSerializedField] int _playerTurnCount;
        [ShowNonSerializedField] int _opponentTurnCount;
        [ShowNonSerializedField] uint _seconds;
        OpponentDeckComponent _aiComponent;

        public event System.Action<TurnSystem> OnGameStarted = (system) => Debug.Log($"{nameof(TurnSystem)}.{nameof(OnGameStarted)}",system);
        public event System.Action<TurnSystem> OnPlayerTurnStarted = (system) => Debug.Log($"{nameof(TurnSystem)}.{nameof(OnPlayerTurnStarted)}",system);
        public event System.Action<TurnSystem> OnPlayerTurnEnded = (system) => Debug.Log($"{nameof(TurnSystem)}.{nameof(OnPlayerTurnEnded)}",system);
        public event System.Action<TurnSystem> OnOpponentTurnStarted = (system) => Debug.Log($"{nameof(TurnSystem)}.{nameof(OnOpponentTurnStarted)}",system);
        public event System.Action<TurnSystem> OnOpponentTurnEnded = (system) => Debug.Log($"{nameof(TurnSystem)}.{nameof(OnOpponentTurnEnded)}",system);
        public event System.Action<TurnSystem> OnPlayerWin = (system) => Debug.Log($"{nameof(TurnSystem)}.{nameof(OnPlayerWin)}",system);
        public event System.Action<TurnSystem> OnPlayerDefeat = (system) => Debug.Log($"{nameof(TurnSystem)}.{nameof(OnPlayerDefeat)}",system);


        public bool IsPlayerTurn => _state==EState.PLAYER_TURN;
        public bool IsOpponentTurn => _state==EState.OPPONENT_TURN;
        [System.Obsolete("Make this private")] public int MaxGil { get; set; }
        [System.Obsolete("Make this private")] public int CurrentGil { get; set; }
        [System.Obsolete("Make this private")] public int CurrentEnemyGil { get; set; }
        int _maxEnemyGil;
        Coroutine _tickRoutine;
        EState _state = EState.NOT_STARED_YET;
        float _stateChangeTime;

        IEnumerator Start ()
        {
            _aiComponent = FindObjectOfType<OpponentDeckComponent>();

            // lets wait a second so every component has time to initialize
            // and also human player will have a moment to "read" the screen
            yield return new WaitForSecondsRealtime(1f);

            ChangeState( EState.GAME_START );
        }

        void OnDestroy ()
        {
            #if UNITY_EDITOR
            // clear runtime-only sets of player cards:
            var player = PlayerAsset.Player;
            player.CardsInDeck.RemoveAllCards();
            // player.CardsInHand.RemoveAllCards();
            player.CardsInGraveyard.RemoveAllCards();

            // clear runtime-only sets of opponent cards:
            var opponent = PlayerAsset.Opponent;
            opponent.CardsInDeck.RemoveAllCards();
            // opponent.CardsInHand.RemoveAllCards();
            opponent.CardsInGraveyard.RemoveAllCards();
            #endif
        }

        void FixedUpdate ()
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
            if( state==EState.NOT_STARED_YET )
            {

            }
            else if( state==EState.GAME_START )
            {
                // reset both player's and opponent's assets:
                PlayerAsset.Player.ResetToDefaultValues(clearDeck:true);
                PlayerAsset.Opponent.ResetToDefaultValues(clearDeck:true);

                // load player's user account cards:
                string jsonFilePath = $"{Application.persistentDataPath}/deck.json";
                if( DeckAsset.ReadJsonFile(jsonFilePath,out var playerDeckFromJson) )
                {
                    DeckAsset.CopyCards( src:playerDeckFromJson , dst:PlayerAsset.Player.UserAccountCards , raiseEvents:false );
                    Debug.Log($"Loaded deck from JSON file '{jsonFilePath}'",this);
                }
                else
                {
                    Debug.Log($"JSON file not found '{jsonFilePath}'",this);

                    Debug.Log("Assigning a starting deck from reference",this);
                    var defaultPlayerDeck = Addressables.LoadAssetAsync<DeckAsset>( "default-player-deck.asset" ).WaitForCompletion();
                    DeckAsset.CopyCards( src:defaultPlayerDeck , dst:PlayerAsset.Player.UserAccountCards , raiseEvents:false );
                }
                // create deck of cards for player
                {
                    uint timeHash = (uint)Mathf.Abs( System.DateTime.Now.GetHashCode() );
                    var userAccountCards = PlayerAsset.Player.UserAccountCards;
                    var cardsInDeck = PlayerAsset.Player.CardsInDeck;
                    for( uint i=0 ; i<16; i++ )
                    {
                        uint seed = timeHash;
                        unchecked
                        {
                            seed *= i;
                        }
                        userAccountCards.GetRandomCard( out var cardAsset , seed );
                        cardsInDeck.AddCardAtTheTop( cardAsset );
                    }
                }

                // load opponent deck of cards:
                DeckAsset.CopyCards(
                    src:            Addressables.LoadAssetAsync<DeckAsset>( "default-opponent-deck.asset" ).WaitForCompletion() ,
                    dst:            PlayerAsset.Opponent.UserAccountCards ,
                    raiseEvents:    false
                );
                // create deck of cards for opponent
                {
                    uint timeHash = (uint)Mathf.Abs( System.DateTime.Now.GetHashCode() );
                    var userAccountCards = PlayerAsset.Opponent.UserAccountCards;
                    var cardsInDeck = PlayerAsset.Opponent.CardsInDeck;
                    for( uint i=0 ; i<16; i++ )
                    {
                        uint seed = timeHash;
                        unchecked
                        {
                            seed *= i;
                        }
                        userAccountCards.GetRandomCard( out var cardAsset , seed );
                        cardsInDeck.AddCardAtTheTop( cardAsset );
                    }
                }
                
                _playerTurnCount = 0;
                _opponentTurnCount = 0;
                _seconds = 60;
                UpdateUI();

                // initialize coroutine field so no nullcheck will be needed later on
                _tickRoutine = StartCoroutine( TimerTickRoutine() );
                StopCoroutine( _tickRoutine );

                OnGameStarted(this);
            }
            else if( state==EState.PLAYER_TURN )
            {
                _playerTurnCount++;
                MaxGil++;
                CurrentGil = MaxGil;
                
                _seconds = 60;
                _tickRoutine = StartCoroutine( TimerTickRoutine() );

                UpdateUI();
                OnPlayerTurnStarted(this);
            }
            else if( state==EState.OPPONENT_TURN )
            {
                _opponentTurnCount++;
                _maxEnemyGil++;
                CurrentEnemyGil = _maxEnemyGil;
                _seconds = 60;
                // if(_tickRoutine!=null) 
                _tickRoutine = StartCoroutine( TimerTickRoutine() );

                UpdateUI();
                OnOpponentTurnStarted(this);
            }
            else if( state==EState.PLAYER_WIN )
            {
                OnPlayerWin(this);
            }
            else if( state==EState.PLAYER_DEFEAT )
            {
                OnPlayerDefeat(this);
            }
            else if( state==EState.GAME_END )
            {
                // @TODO: this could handle game state when player quits. This could happen normally and also prematurely (before win/defeat state happened)
            }
            else if( state==EState.GAME_PAUSE )
            {
                // @TODO: this could handle game paused state
            }
            else throw new System.NotImplementedException($"{state} not implemented");
        }

        /// <summary>Good place to reset values</summary>
        void OnStateExit ( EState state )
        {
            // state ends:
            if( state==EState.NOT_STARED_YET )
            {

            }
            else if( state==EState.GAME_START )
            {
                
            }
            else if( state==EState.PLAYER_TURN )
            {
                StopCoroutine(_tickRoutine);
                OnPlayerTurnEnded(this);
            }
            else if( state==EState.OPPONENT_TURN )
            {
                StopCoroutine(_tickRoutine);
                OnOpponentTurnEnded(this);
            }
            else if( state==EState.PLAYER_WIN )
            {

            }
            else if( state==EState.PLAYER_DEFEAT )
            {

            }
            else if( state==EState.GAME_END )
            {

            }
            else if( state==EState.GAME_PAUSE )
            {

            }
            else throw new System.NotImplementedException($"{state} not implemented");
        }

        /// <summary>Good place to change states - always using ChangeState(_)!.</summary>
        void OnStateTick ( EState state , float duration )
        {
            // state update:
            if( state==EState.NOT_STARED_YET )
            {

            }
            else if( state==EState.GAME_START )
            {
                if( Random.Range(0,2)==0 )
                {
                    MaxGil = 1;
                    CurrentGil = 1;
                    _maxEnemyGil = 0;
                    CurrentEnemyGil = 0;

                    ChangeState( EState.PLAYER_TURN );
                }
                else
                {
                    _maxEnemyGil = 1;
                    CurrentEnemyGil = 1;
                    MaxGil = 0;
                    CurrentGil = 0;

                    ChangeState( EState.OPPONENT_TURN );
                }
            }
            else if( state==EState.PLAYER_TURN )
            {
                // test win/lose conditions
                if( PlayerAsset.Player.Health<=0 )
                {
                    ChangeState( EState.PLAYER_DEFEAT );
                }
                else if( PlayerAsset.Opponent.Health<=0 )
                {
                    ChangeState( EState.PLAYER_WIN );
                }
            }
            else if( state==EState.OPPONENT_TURN )
            {
                // test win/lose conditions
                if( PlayerAsset.Player.Health<=0 )
                {
                    ChangeState( EState.PLAYER_DEFEAT );
                }
                else if( PlayerAsset.Opponent.Health<=0 )
                {
                    ChangeState( EState.PLAYER_WIN );
                }
                
                // test for opponent turn's end
                if( _aiComponent.phase==OpponentDeckComponent.EState.END )
                {
                    ChangeState( EState.PLAYER_TURN );
                }
            }
            else if( state==EState.PLAYER_WIN )
            {

            }
            else if( state==EState.PLAYER_DEFEAT )
            {

            }
            else if( state==EState.GAME_END )
            {

            }
            else if( state==EState.GAME_PAUSE )
            {

            }
            else throw new System.NotImplementedException($"{state} not implemented");
        }

        public void StartGame () => ChangeState( EState.GAME_START );

        public void EndPlayerTurn ()
        {
            if( IsOpponentTurn )
            {
                Debug.Log($"{nameof(EndPlayerTurn)}() denied. It's opponent's turn");
                return;
            }

            ChangeState( EState.OPPONENT_TURN );
        }

        public void EndOpponentTurn ()
        {
            if( IsPlayerTurn )
            {
                Debug.Log($"{nameof(EndOpponentTurn)}() denied. It's player's turn");
                return;
            }

            ChangeState( EState.PLAYER_TURN );
        }

        IEnumerator TimerTickRoutine ()
        {
            var delay = new WaitForSeconds(1);
            while(_seconds>0)
            {
                yield return delay;

                _seconds--;
                UpdateUI();
            }

            // time is up!

            if( IsPlayerTurn ) ChangeState( EState.OPPONENT_TURN );
            else ChangeState( EState.PLAYER_TURN );
        }

        void UpdateUI ()
        {
            if( IsPlayerTurn ) UpdateUI_PlayerTimer();
            else UpdateUI_OpponentTimer();
        }
        void UpdateUI_PlayerTimer ()
        {
            turnText.text = "Your Turn";

            timerText.text = _seconds.ToString();
            float fillAmount = (float)_seconds / 60f;// assuming 60 seconds
            timerImage.fillAmount = fillAmount;

            gilText.text = $"{CurrentGil}/{MaxGil}";
        }
        void UpdateUI_OpponentTimer ()
        {
            turnText.text = "Opponent Turn";

            timerText.text = _seconds.ToString();
            float fillAmount = (float)_seconds / 60f;// assuming 60 seconds
            timerImage.fillAmount = fillAmount;

            enemyGilText.text = $"{CurrentEnemyGil}/{_maxEnemyGil}";
        }

        public enum EState : byte
        {
            NOT_STARED_YET = 0,
            GAME_START,
            PLAYER_TURN,
            OPPONENT_TURN,
            PLAYER_WIN,
            PLAYER_DEFEAT,
            GAME_END,
            GAME_PAUSE,
        }

    }

}
