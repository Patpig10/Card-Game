using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using TMPro;
using NaughtyAttributes;
using Game.Shared;

using Draggable = Game.Client.Draggable;// get rid of this dependency (Server shall never access Client's code)

namespace Game.Server
{
    /// <summary>
    /// What is the sole responsibility of this class?
    /// </summary>
    [RequireComponent( typeof(CardBackComponent) )]
    public class CardComponent : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI nameText;
        [SerializeField] TextMeshProUGUI costText;
        [SerializeField] TextMeshProUGUI powerText;
        [SerializeField] TextMeshProUGUI descriptionText;
        [SerializeField] Image thatImage;
        [SerializeField] Image frame;
        [SerializeField] GameObject attackBorder;
        [SerializeField] GameObject wardguard;
        [SerializeField] GameObject summonBorder;
        [SerializeField] TurnSystem _turnSystem;
        [SerializeField] PlayerDeckComponent _parentDeck;

        [ShowNonSerializedField] bool _cardBack;
        [ShowNonSerializedField] bool _canBeSummon;
        [ShowNonSerializedField] bool _isSummoned;
        [ShowNonSerializedField] bool _summoningSickness;
        [ShowNonSerializedField] bool _cantAttack;
        [ShowNonSerializedField] bool _canAttack;
        [ShowNonSerializedField] bool _isTargeting;
        [ShowNonSerializedField] bool _isTargetingEnemy;
        [ShowNonSerializedField] bool _onlyThisCardAttack;
        [ShowNonSerializedField] bool _canBeDestroyed;
        [ShowNonSerializedField] bool _isInGraveyard;
        [ShowNonSerializedField] int _damaged;
        [ShowNonSerializedField] bool _canBoost;
        [ShowNonSerializedField] bool _dealDamage;
        [ShowNonSerializedField] bool _stopDealDamage;
        [ShowNonSerializedField] bool _directattack;
        [ShowNonSerializedField] bool _useReturn;
        [ShowNonSerializedField] bool _canHeal;
        [ShowNonSerializedField] bool _uCanReturn;
        [ShowNonSerializedField] bool _useRevive;

        int actualpower => _cardAsset.Power - _damaged;
        
        CardAsset _cardAsset;
        Transform _enemyZone;
        Transform _graveyard;
        GameObject _targetGO;
        GameObject _enemyHP;
        GameObject _hand;
        GameObject _battleZone;
        CardBackComponent _cardBackComp;

        void Awake ()
        {
            _cardBackComp = GetComponent<CardBackComponent>();

            _isTargeting = false;
            _isTargetingEnemy = false;
            _isInGraveyard = false;
            _canHeal = true;
            _canBoost = true;
            _directattack = true;
            _canBeSummon = false;
            _isSummoned = false;
            _canAttack = false;
            _summoningSickness = true;
        }

        void Start ()
        {
            _enemyHP = GameObject.Find("Enemy HP");
            _enemyZone = GameObject.Find("EnemyZone").transform;
            _graveyard = GameObject.Find("MyGraveyard").transform;
        }

        void FixedUpdate ()
        {
            OnStateTick(_state, Time.time - _stateChangeTime);

            //
            // @TODO: move ALL the code from below to a FSM pattern (OnStateEnter, OnStateExit, OnStateTick)
            //

            _hand = GameObject.Find("Hand");

            if( transform.parent==_hand.transform )
            {
                _cardBack = false;
            }

            _cardBackComp.UpdateCard( _cardBack );

            if( this.CompareTag("Clone") )
            {
                if( PlayerAsset.Player.CardsInDeck.DrawCardFromTheTop(out var drawnCard) )
                {
                    _cardAsset = drawnCard;
                    _cardBack = false;
                    this.tag = "Untagged";
                }
                else throw new System.NotImplementedException("Implement me. React to attempted card pull when there are no cards anymore.");
            }
            else
            {
                _canBeSummon = _turnSystem.CurrentGil>=_cardAsset.Cost && !_isSummoned && _isInGraveyard==false && _turnSystem.IsPlayerTurn;
                
                if( TryGetComponent<Draggable>(out var draggable) )
                {
                    draggable.enabled = _canBeSummon;
                }
                
                _battleZone = GameObject.Find("Zone");
                
                if( !_isSummoned && transform.parent==_battleZone.transform )
                {
                    Summon();
                }

                if( wardguard!=null )
                {
                    wardguard.SetActive( _cardAsset.IsWard && _isSummoned );
                }
            }

            if( !_turnSystem.IsPlayerTurn && _isSummoned )
            {
                _summoningSickness = false;
                _cantAttack = false;
            }

            _canAttack = _turnSystem.IsPlayerTurn && _summoningSickness==false && _cantAttack==false;
            _targetGO = _isTargetingEnemy ? _enemyHP : null;

            if( attackBorder!=null )
            {
                attackBorder.SetActive( _canAttack && !_isInGraveyard );
            }

            if( _isTargeting && _onlyThisCardAttack )
            {
                Attack();
            }

            if( summonBorder!=null )
            {
                summonBorder.SetActive( _canBeSummon || _uCanReturn && _isInGraveyard || _useRevive && _isInGraveyard );
            }

            /* if (canBeSummon == true || useRevive == true && beInGraveyard == true)
            {
                summonBorder.SetActive(true);
                Debug.Log("revive border test.",this);
            }
            else
            {
                summonBorder.SetActive(false);
            }*/

            if( actualpower<=0 && _cardAsset.IsSpell==false )
            {
                Destroy();
                _canBeDestroyed = true;
            }
            if( _cardAsset.ReturnXcards>0 && _isSummoned && _useReturn==false && _turnSystem.IsPlayerTurn )
            {
                Return( _cardAsset.ReturnXcards );
                _useReturn = true;
            }

            if( _cardAsset.ResurrectXcards>0 && _isSummoned && _useRevive==false && _turnSystem.IsPlayerTurn )
            {
                Revive( _cardAsset.ResurrectXcards );
                _useRevive = true;
            }

            if( !_turnSystem.IsPlayerTurn )
            {
                _useRevive = false;
                _useReturn = false;
            }
        
            if( _canHeal && _isSummoned )
            {
                PlayerAsset.Player.Heal( _cardAsset.HealXpower );
                _canHeal = false;
            }

            if( _canBoost && _isSummoned )
            {
               // AttackBoost();
               // canBoost = false;
            }

            if( _cardAsset.DamageDealtBySpell>0 )
            {
                _dealDamage = true;
            }

            if( _dealDamage && transform.parent==_battleZone.transform && _cardAsset.IsSpell )
            {
                Debug.Log("spell test1.",this);
                attackBorder.SetActive(true);
            }
            else if( _dealDamage==false && transform.parent==_battleZone.transform && _cardAsset.IsSpell )
            {
                Debug.Log("spell test2.",this);  
                attackBorder.SetActive(false);
            }

            if( _dealDamage && transform.parent==_battleZone.transform )
            {
                Debug.Log("Attempting to speelll test.",this);
                DealxDamage( _cardAsset.DamageDealtBySpell );
            }
            
            if( _stopDealDamage )
            {
                Debug.Log("Stop deal damage test.",this);
                attackBorder.SetActive(false);
                _dealDamage = false;
            }

            if( transform.parent==_battleZone.transform && _cardAsset.IsSpell && _dealDamage==false )
            {
                Debug.Log("Spell gone",this);
                _canBeDestroyed = true;
                Destroy();
            }

            bool wardCardPresent = false;
            foreach( var childAICard in _enemyZone.GetComponentsInChildren<AICardToHand>() )
            if( childAICard.assignedCardAsset.IsWard )
            {
                wardCardPresent = true;
                break;
            }
            _directattack = !wardCardPresent;
        }

        public enum EState : byte
        {
            BEFORE_START = 0,
            START,
            SUMMONED,
            A,
            B,
            C,
        }

        EState _state = EState.BEFORE_START;
        float _stateChangeTime;

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
            if( state==EState.BEFORE_START )
            {

            }
            else if( state==EState.START )
            {

            }
            else if( state==EState.SUMMONED )
            {

            }
            else if( state==EState.A )
            {

            }
            else if( state==EState.B )
            {

            }
            else if( state==EState.C )
            {

            }
            else throw new System.NotImplementedException($"{state} not implemented");
        }

        /// <summary>Good place to reset values</summary>
        void OnStateExit ( EState state )
        {
            // state ends:
            if( state==EState.BEFORE_START )
            {

            }
            else if( state==EState.START )
            {

            }
            else if( state==EState.SUMMONED )
            {

            }
            else if( state==EState.A )
            {

            }
            else if( state==EState.B )
            {

            }
            else if( state==EState.C )
            {

            }
            else throw new System.NotImplementedException($"{state} not implemented");
        }

        /// <summary>Good place to change states - always using ChangeState(_)!.</summary>
        void OnStateTick ( EState state , float duration )
        {
            // state update:
            if( state==EState.BEFORE_START )
            {
                ChangeState(EState.START);
            }
            else if( state==EState.START )
            {

            }
            else if( state==EState.SUMMONED )
            {

            }
            else if( state==EState.A )
            {

            }
            else if( state==EState.B )
            {

            }
            else if( state==EState.C )
            {

            }
            else throw new System.NotImplementedException($"{state} not implemented");
        }

        public void InitializeInstance ( CardAsset cardAsset , TurnSystem turnSystem , PlayerDeckComponent playerDeckComponent )
        {
            _turnSystem = turnSystem;
            _cardAsset = cardAsset;
            _parentDeck = playerDeckComponent;
            UpdateUI();
        }

        public void Summon ()
        {
            _turnSystem.CurrentGil -= _cardAsset.Cost;
            _isSummoned = true;
            MaxGil( _cardAsset.AddXmaxGil );

            if( _parentDeck==null )
            {
                Debug.Log("_parentDeck is null",this);
                Debug.Break();
            }
            _parentDeck.Draw( _cardAsset.DrawXcards );
        }

        public void MaxGil ( int x )
        {
            _turnSystem.MaxGil += x;
        }

        void UpdateUI ()
        {
            nameText.text = _cardAsset.CardName;
            costText.text = _cardAsset.Cost.ToString();
            descriptionText.text = _cardAsset.CardDescription;
            thatImage.sprite = _cardAsset.Image;
            UpdateUI_PowerText();

            // Check for color condition using the color property of the Card class
            if( _cardAsset.Tint=="White" ) frame.color = new Color32(255,255,255,255);// Set the color to white
            else if( _cardAsset.Tint=="Blue" ) frame.color = new Color32(26,109,236,255);// Set the color to white
            else if( _cardAsset.Tint=="Green" ) frame.color = new Color32(122,236,26,255);// Set the color to white
            else if( _cardAsset.Tint=="Black" ) frame.color = new Color32(51,32,32,255);// Set the color to white
        }
        void UpdateUI_PowerText ()
        {
            powerText.text = actualpower.ToString();
        }

        public void Attack ()
        {
            if( _canAttack && _isSummoned && _targetGO!=null )
            {
                Debug.Log("Attempting to attack.",this);

                if( _targetGO==_enemyHP && _directattack )
                {
                    Debug.Log("Attacking Enemy",this);
                    PlayerAsset.Opponent.Damage( _cardAsset.Power );
                    _isTargeting = false;
                    _cantAttack = true;
                }
            }
            else
            {
                // Debug.Log("Attempting to attack AI.",this);

                foreach( var childAICard in _enemyZone.GetComponentsInChildren<AICardToHand>() )
                {
                    if( childAICard.isTarget && _cantAttack==false )
                    {
                        Debug.Log("Target found in EnemyZone.",this);
                        childAICard.ApplyDamage( actualpower );// Adjusting hurted value by the power of the attacking card
                        
                        _damaged = childAICard.actualpower;
                        UpdateUI_PowerText();// because _damaged changed

                        _cantAttack = true;
                    }
                    /* else
                    {
                        Debug.LogError("AICardToHand component not found or conditions not met on the target AI card.",this);
                    }*/
                }
            }
        }

        public void UntargetEnemy () => _isTargetingEnemy = false;
        public void TargetEnemy () => _isTargetingEnemy = true;
        public void StartAttack () => _isTargeting = true;
        public void StopAttack () => _isTargeting = false;
        public void OneCardAttack () => _onlyThisCardAttack = true;
        public void OneCardAttackStop () => _onlyThisCardAttack = false;

        public void Destroy ()
        {
            if( _canBeDestroyed )
            {
                transform.SetParent( _graveyard );
                _canBeDestroyed = false;
                _isSummoned = false;
                _isInGraveyard = true;
                
                _damaged = 0;
                UpdateUI_PowerText();// because _damaged changed
            }
        }

        public void Revive ( int x )
        {
            for( int i=0 ; i<=x ; i++ )
            {
                ReviveCard();
            }
        }

        public void ReviveCard ()
        {
            Debug.Log("Revive card test.",this);
            _useRevive = true;

            _damaged = 0;
            UpdateUI_PowerText();// because _damaged changed
        }

        public void Return ( int x )
        {
            for( int i=0 ; i<=x ; i++ )
            {
                ReturnCard();
            }
        }
    
        public void ReturnCard ()
        {
            _uCanReturn = true;
        }

        public void ReturnThis ()
        {
            if( _isInGraveyard && _uCanReturn && _graveyard.childCount>0 )
            {
                transform.SetParent( _hand.transform );
                _uCanReturn = false;
                _isInGraveyard = false;
                _summoningSickness = true;
            }
            else if( _isInGraveyard && _useRevive && _graveyard.childCount>0 && _cardAsset.IsSpell==false )
            {

                transform.SetParent( _battleZone.transform );
                _useRevive = false;
                _isInGraveyard = false;
                _summoningSickness = true;

            }
            /* else if (beInGraveyard == true && useRevive == true && Graveyard.transform.childCount == 0)
             {
                 transform.SetParent(Hand.transform);
                 UcanReturn = false;
                 beInGraveyard = false;
                 summoningSickness = true;
             }*/

        }

       /* public void ReviveThis ()
        {
            if (beInGraveyard == true && useRevive == true && Graveyard.transform.childCount > 0)
            {
                Debug.Log("Reviving card",this);
                transform.SetParent(Hand.transform);
                useRevive = false;
                beInGraveyard = false;
                summoningSickness = true;
            }
            else
            {
                Debug.Log("Revive conditions not met",this);
                Debug.Log("beInGraveyard: " + beInGraveyard,this);
                Debug.Log("useRevive: " + useRevive,this);
                Debug.Log("Graveyard child count: " + Graveyard.transform.childCount,this);
            }
        }*/

        /*public void AttackBoost ()
        {
            power += boostXpower; // Update power first
            actualpower = power - hurted; // Update actualpower after modifying power
            UpdateUI();
        }*/

        public void DealxDamage ( int x )
        {
            if( _targetGO!=null )
            {
                if( _isTargetingEnemy && _stopDealDamage==false && Input.GetMouseButton(0) )
                {
                    PlayerAsset.Opponent.Damage( _cardAsset.DamageDealtBySpell );
                    _stopDealDamage = true;
                }
            }
            else
            {
                foreach( AICardToHand childAICard in _enemyZone.GetComponentsInChildren<AICardToHand>() )
                {
                    Debug.Log("Attempting to deal damage to AI.",this);
                    if( childAICard.isTarget && _stopDealDamage==false )
                    {
                        childAICard.ApplyDamage( _cardAsset.DamageDealtBySpell );// Adjusting hurted value by the power of the attacking card
                        _stopDealDamage = true;
                        Debug.Log("Test)",this);
                    }
                }
            }
        }
    
    }
}
