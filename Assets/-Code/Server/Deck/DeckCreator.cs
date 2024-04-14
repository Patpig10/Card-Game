using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using System.IO;
using NaughtyAttributes;
using Game.Shared;

namespace Game.Server
{
    /// <summary>
    /// What is the sole responsibility of this class?
    /// </summary>
    public class DeckCreator : MonoBehaviour
    {
        [SerializeField][System.Obsolete("Replace with "+nameof(_collectionComponent))] GameObject coll;// remove when replaced
        [SerializeField] Collection _collectionComponent;
        [SerializeField] GameObject prefab;
        
        [ShowNonSerializedField] bool _mouseOverDeck;
        [ShowNonSerializedField] CardAsset _dragged;
        [ShowNonSerializedField] int _numberOfCardsInDatabase;
        [ShowNonSerializedField] int _cardsDroppedCount = 0;
        //[System.Obsolete("Get rid of me")] int[] _cardsWithThisId;
        HashSet<CardAsset> _alreadyCreated;
        [System.Obsolete("Get rid of me")] public static CardAsset lastAdded;
        //[System.Obsolete("Get rid of me")] public int[] quantity;
        
        DeckAsset _deck;
        public DeckAsset deck => _deck;

        string _jsonFilePath;

#if UNITY_EDITOR
        void OnValidate ()
        {
            _collectionComponent = coll.GetComponent<Collection>();// delete line when replaced
        }
#endif

        void Awake ()
        {
            _jsonFilePath = $"{Application.persistentDataPath}/deck.json";

            // make sure default json exists:
            if( !File.Exists(_jsonFilePath) )
            {
                File.WriteAllText( path:_jsonFilePath , contents:DeckAsset.GetDefaultJson() );
            }
        }

        void Start ()
        {
            LoadDeck();
        }

        public void CreateDeck ()
        {
            SaveDeck();
            LoadDeck();
        }

        bool SaveDeck ()
        {
            bool success = DeckAsset.WriteJsonFile( _deck , _jsonFilePath );
            
            Debug.Log("Deck saved as JSON.");
            
            return success;
        }

        /// <returns>True when succeeded.</returns>
        bool LoadDeck ()
        {
            bool success = DeckAsset.ReadJsonFile( _jsonFilePath , out _deck );
            
            if( success ) Debug.Log("Deck loaded from JSON.");
            else Debug.LogError("Deck NOT loaded from JSON.");

            return success;
        }

        public void EnterDeck () => _mouseOverDeck = true;
        public void ExitDeck () => _mouseOverDeck = false;
        public void Card1 () => _dragged = _deck.PeekAtIndex( Collection.DECK_POSITION );
        public void Card2 () => _dragged = _deck.PeekAtIndex( Collection.DECK_POSITION + 1 );
        public void Card3 () => _dragged = _deck.PeekAtIndex( Collection.DECK_POSITION + 2 );
        public void Card4 () => _dragged = _deck.PeekAtIndex( Collection.DECK_POSITION + 3 );

        public void Drop ()
        {
            //if( _mouseOverDeck==true && _collectionComponent.HowManyCards[_dragged]>0 )
            //{
            //    _cardsWithThisId[_dragged]++;

            //    if( _cardsWithThisId[_dragged]<0 )
            //    {
            //        _cardsWithThisId[_dragged] = 0;
            //    }
            //    _collectionComponent.HowManyCards[_dragged]--;

            //    CalculateDrop();

            //    _cardsDroppedCount++;

            //    // Check if this is the last card to be dropped
            //    if( AllCardsDropped() )
            //    {
            //        CreateDeck();
            //        ClearDeck();// Optionally clear the deck after creating it
            //    }
            //}
        }

        bool AllCardsDropped () => _deck.Length==0;

        public void ClearDeck ()
        {
            // Reset necessary variables and arrays
            _deck.RemoveAllCards();
            _cardsDroppedCount = 0;

            // Implement additional cleanup if needed
        }

        public void CalculateDrop ()
        {
            // @TODO: this code likely doesn't make sense

            if( _deck.Contains(_dragged) )
            {
                if( _alreadyCreated.Contains(_dragged) )
                {
                    _deck.AddCardAtTheTop( _dragged );
                    lastAdded = null;
                }
                else
                {
                    Instantiate( prefab , Vector3.zero , Quaternion.identity );
                    lastAdded = _dragged;
                    _alreadyCreated.Add( _dragged );
                }
            }
        }
    }
}
