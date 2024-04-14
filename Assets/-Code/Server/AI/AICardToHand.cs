using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TMPro;
using UnityEngine.UI;
using NaughtyAttributes;

using Game.Shared;

namespace Game.Server
{
    /// <summary>
    /// What is the sole responsibility of this class?
    /// </summary>
    public class AICardToHand : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI nameText;
        [SerializeField] TextMeshProUGUI costText;
        [SerializeField] TextMeshProUGUI powerText;
        [SerializeField] TextMeshProUGUI descriptionText;
        [SerializeField] GameObject cardBack;
        [SerializeField] GameObject wardguard;
        [SerializeField] Transform It;
        //[SerializeField] Sprite thisSprite;
        [SerializeField] Image thatImage;
        [SerializeField] Image frame;

        [ShowNativeProperty] public int damaged { get; private set; }
        [ShowNativeProperty] public bool canHeal { get; private set; }
        [ShowNativeProperty] public bool isTarget { get; private set; }
        [ShowNativeProperty] public bool thisCardCanBeDestroyed { get; private set; }
        [ShowNativeProperty] public bool summoned { get; private set; }
        [ShowNativeProperty] public bool canAttack { get; private set; }
        [ShowNativeProperty] public bool summoningSickness { get; private set; }
        public CardAsset assignedCardAsset => _assignedCardAsset;

        TurnSystem _turnSystem;
        CardAsset _assignedCardAsset;
        public int actualpower => _assignedCardAsset.Power - damaged;
        Transform _hand;
        Transform _aiZone;
        Transform _graveyard;
        Transform _playerZone;
        
        void Awake ()
        {
            canHeal = true;
            summoned = false;
            summoningSickness = true;
        }
        
        IEnumerator Start ()
        {
            _hand = GameObject.Find("EnemyHand").transform;
            _graveyard = GameObject.Find("EnemyGraveyard").transform;
            _aiZone = GameObject.Find("EnemyZone").transform;
            _playerZone = GameObject.Find("Zone").transform;

            It.SetParent( _hand );
            It.localScale = Vector3.one;
            It.position = new Vector3( transform.position.x , transform.position.y , -48 );
            It.eulerAngles = new Vector3(25,0,0);

            const float refreshTime = 0.2f;
            InvokeRepeating( nameof(UpdateUI) , refreshTime , refreshTime );

            thisCardCanBeDestroyed = false;
            yield return new WaitForSeconds(1);
            thisCardCanBeDestroyed = true;
        }

        [System.Obsolete("Get rid of me. Refactor this into events or methods that are not called every second(!) but immediately after something related happens.")]
        void FixedUpdate ()
        {
            if( canHeal==true && summoned==true )
            {
                PlayerAsset.Opponent.Heal( _assignedCardAsset.HealXpower );
                canHeal = false;
            }

            if( transform.parent==_aiZone.transform )
            {
                summoned = true;
            }

            if( damaged>=_assignedCardAsset.Power && thisCardCanBeDestroyed==true )
            {
                transform.SetParent( _graveyard.transform );
                damaged = 0;
            }
       
            if( transform.parent==_hand )
            {
               cardBack.SetActive(true);
            }

            if( transform.parent==_aiZone.transform )
            {
                cardBack.SetActive(false);
            }

            canAttack = _turnSystem.IsPlayerTurn==false && summoningSickness==false;

            if( _turnSystem.IsPlayerTurn==true && transform.parent==_aiZone.transform )
            {
                summoningSickness = false;
            }

            wardguard.SetActive( _assignedCardAsset.IsWard==true && summoned==true );

            /*foreach( var childThisCard in PlayerZone.GetComponentsInChildren<ThisCard>() )
            {
                if( childThisCard.ward==true )
                {
                    directAttack = false;
                }
                else
                {
                    directAttack = true;
                }
            }*/
        }

        public void InitializeInstance ( TurnSystem turnSystem , CardAsset cardAsset )
        {
            _turnSystem = turnSystem;
            _assignedCardAsset = cardAsset;
            this.tag = "Clone";

            UpdateUI();
        }

        public void BeingTarget () => isTarget = true;
        public void DontBeingTarget () => isTarget = false;
        public void ApplyDamage ( int value ) => damaged += Mathf.Max(value,0);

        void UpdateUI ()
        {
            nameText.text = _assignedCardAsset.CardName;
            costText.text = _assignedCardAsset.Cost.ToString();
            powerText.text = actualpower.ToString();
            descriptionText.text = _assignedCardAsset.CardDescription;
            thatImage.sprite = _assignedCardAsset.Image;

            if( _assignedCardAsset.Tint=="White" ) frame.color = new Color32(255,255,255,255);// Set the color to white
            else if( _assignedCardAsset.Tint=="Blue" ) frame.color = new Color32(26,109,236,255);// Set the color to white
            else if( _assignedCardAsset.Tint=="Green" ) frame.color = new Color32(122,236,26,255);// Set the color to white
            else if( _assignedCardAsset.Tint=="Black" ) frame.color = new Color32(51,32,32,255);// Set the color to white
        }

    }
}
