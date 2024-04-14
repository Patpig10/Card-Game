using System.Collections.Generic;
using System.Linq;
using IO = System.IO;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game.Shared
{
    /// <summary>
    /// Asset that defines a deck of card assets
    /// </summary>
    [CreateAssetMenu( menuName="Game/Deck" , fileName="deck (0)" , order=1 )]
    public class DeckAsset : ScriptableObject
    {

        [SerializeField][InspectorName("Read-Only")] bool IsReadOnly = false;
        
        [SerializeField] List<CardAsset> Cards;

        public event System.Action<CardAsset,DeckAsset> onCardTaken = (card,deck) => Debug.Log($"{nameof(DeckAsset)}.{nameof(onCardTaken)}( {nameof(card)}:{card.name} , {nameof(deck)}:{deck.name} )",deck);
        public event System.Action<CardAsset,DeckAsset> onCardPlaced = (card,deck) => Debug.Log($"{nameof(DeckAsset)}.{nameof(onCardPlaced)}( {nameof(card)}:{card.name} , {nameof(deck)}:{deck.name} )",deck);
        
        public void Shuffle ( uint randomSeed = default )
        {
            if( IsReadOnly )
            {
                Debug.LogError($"This {nameof(DeckAsset)} is Read-Only. Modifying it is forbidden.",this);
                return;
            }
            
            var rnd = new Unity.Mathematics.Random( randomSeed!=default ? randomSeed : (uint) Random.Range(0,uint.MaxValue) );
            Cards.Sort( (x,y) => rnd.NextInt(-1,2) );
        }

        /// <remarks>Does NOT remove the card from the deck - only provides a reference.</remarks>
        public bool GetRandomCard ( out CardAsset cardAsset , uint randomSeed = default )
        {
            var rnd = new Unity.Mathematics.Random( randomSeed!=default ? randomSeed : (uint) Random.Range(0,uint.MaxValue) );
            
            cardAsset = Cards[ rnd.NextInt(Cards.Count) ];
            return true;
        }
        
        /// <summary>Empties the deck. Cards disappear without raising events.</summary>
        public void RemoveAllCards ()
        {
            if( IsReadOnly )
            {
                Debug.LogError($"This {nameof(DeckAsset)} is Read-Only. Modifying it is forbidden.",this);
                return;
            }

            Cards.Clear();
        }

        /// <summary>Adds a card on top of the deck.</summary>
        public void AddCardAtTheTop ( CardAsset cardAsset )
        {
            if( IsReadOnly )
            {
                Debug.LogError($"This {nameof(DeckAsset)} is Read-Only. Modifying it is forbidden.",this);
                return;
            }

            Cards.Add( cardAsset );
        }
        public void AddCardsAtTheTop ( IEnumerable<CardAsset> collectionOfCardAssets , bool raiseEvents )
        {
            if( IsReadOnly )
            {
                Debug.LogError($"This {nameof(DeckAsset)} is Read-Only. Modifying it is forbidden.",this);
                return;
            }

            if( raiseEvents )
            {
                foreach( var cardAsset in collectionOfCardAssets )
                {
                    Cards.Add( cardAsset );

                    // raise event:
                    onCardPlaced( cardAsset , this );
                }
            }
            else
            {
                Cards.AddRange( collectionOfCardAssets );
            }
        }
        
        public bool Contains ( CardAsset cardAsset ) => Cards.Contains( cardAsset );

        /// <returns>Number of cards.</returns>
        public int Length => Cards.Count;

        public int Count ( CardAsset cardAsset )
        {
            string id = cardAsset.CardUniqueID;
            int count = Cards.Count( (next) => next.CardUniqueID==id );
            return count;
        }

        /// <summary> Peek at the top card without removing it from the deck. </summary>
        /// <returns>Card at the top of the stack or <see langword="null"/> when deck is empty.</returns>
        public CardAsset Peek () => Cards.LastOrDefault();
        public CardAsset PeekAtIndex ( int i ) => Cards[i];

        /// <summary> Enables `foreach( var cardAsset in deckAsset )` iterator </summary>
        public IEnumerator<CardAsset> GetEnumerator () => Cards.GetEnumerator();
        
        /// <summary> Removes the top card from the deck </summary>
        public bool DrawCardFromTheTop ( out CardAsset cardAsset )
        {
            if( IsReadOnly )
            {
                Debug.LogError($"This {nameof(DeckAsset)} is Read-Only. Modifying it is forbidden.",this);
                cardAsset = null;
                return false;
            }

            int numCards = Cards.Count;
            int lastIndex = numCards - 1;
            if( lastIndex<numCards && lastIndex!=-1 )
            {
                cardAsset = Cards[ lastIndex ];
                Cards.RemoveAt( lastIndex );
                
                // raise event:
                onCardTaken( cardAsset , this );
                
                return true;
            }
            else
            {
                cardAsset = null;
                return false;
            }
        }

        /// <summary>Copies cards from one deck to the other without removing anything. Use this to initialize a specific decks from a template deck(s) when a new game starts.</summary>
        /// <param name="src">Source deck. IMPORTANT: it's content will **not** be emptied out after this call.</param>
        /// <param name="dst">Destination deck. IMPORTANT: it's content will **not** be emptied out before this call.</param>
        
        public static void CopyCards ( DeckAsset src , DeckAsset dst , bool raiseEvents )
        {
            if( raiseEvents )
            {
                foreach( var cardAsset in src.Cards )
                {
                    dst.Cards.Add( cardAsset );

                    // raise event:
                    dst.onCardPlaced( cardAsset , dst );
                }
            }
            else
            {
                dst.Cards.AddRange( src.Cards );
            }
        }

        public static string ToJson ( DeckAsset obj )
        {
            var cards = obj.Cards;
            int numCards = cards.Count;
            string[] array = new string[ numCards ];
            for( int i=0 ; i<numCards ; i++ )
            {
                array[i] = cards[i].CardUniqueID;
            }
            var serializableData = new SerializableDeckData001{
                Version = 1 ,
                UniqueCardIDs = array ,
                IsReadOnly = obj.IsReadOnly ,
            };
            return JsonUtility.ToJson( serializableData );
        }

        /// <summary>
        /// Writes obj as JSON file
        /// </summary>
        /// <returns>True when no error was detected</returns>
        public static bool ReadJsonFile ( string jsonFilePath , out DeckAsset deckAsset )
        {
            if( IO.File.Exists(jsonFilePath) )
            {
                string json = IO.File.ReadAllText( jsonFilePath );
                return FromJson( json , out deckAsset );
            }
            else
            {
                deckAsset = null;
                return false;
            }
        }

        /// <summary>
        /// Writes obj as JSON file
        /// </summary>
        /// <returns>True when no error was detected</returns>

        public static bool WriteJsonFile ( DeckAsset deckAsset , string jsonFilePath )
        {
            if( deckAsset==null )
            {
                Debug.LogError($"{nameof(deckAsset)} argument is null");
                return false;
            }
            if( jsonFilePath==null || jsonFilePath.Length==0 )
            {
                Debug.LogError($"{nameof(jsonFilePath)} argument is empty");
                return false;
            }
            
            string json = ToJson( deckAsset );

            try
            {
                IO.File.WriteAllText( jsonFilePath , json );
            }
            catch( System.Exception ex)
            {
                Debug.LogException( ex );
                return false;
            }

            return true;
        }

        public static bool FromJson ( string json , out DeckAsset deckAsset )
        {
            int version = 0;
            try
            {
                version = JsonUtility.FromJson<SerializableDeckDataBase>(json).Version;
            }
            catch( System.Exception ex )
            {
                Debug.LogException(ex);
                Debug.LogError($"Error while reading a JSON: '{json}'");
                deckAsset = null;
                return false;
            }
            if( version<001 ) throw new System.Exception($"Serialized data version '{version}' is incorrect. Input json is probably either corrupted or empty. JSON: '{json}'");
            switch( version )
            {
                case 001:
                {
                    deckAsset = ScriptableObject.CreateInstance<DeckAsset>();
                    var dataV001 = JsonUtility.FromJson<SerializableDeckData001>(json);
                    foreach( var id in dataV001.UniqueCardIDs )
                    if( CardAsset.Get(id,out var cardAsset) )
                    {
                        deckAsset.AddCardAtTheTop( cardAsset );
                    }
                    deckAsset.IsReadOnly = dataV001.IsReadOnly;
                    return true;
                }
                //case 002: return ...
                // etc.
                default: throw new System.NotImplementedException($"file version:{version} not implemented yet. Implement it here.");
            }
        }

        static string _defaultJson;
        public static string GetDefaultJson ()
        {
            if( _defaultJson==null )
            {
                _defaultJson = JsonUtility.ToJson(
                    new SerializableDeckData001{
                        Version = 1 ,
                        UniqueCardIDs = new string[0] ,
                        IsReadOnly = false ,
                    }
                );
            }
            return _defaultJson;
        }

    }

    public abstract class SerializableDeckDataBase
    {
        public int Version;
    }
    
    // This is the current save file
    // Version field helps differentiate between old and new save files and makes it possible that all can be read with no data lost
    [System.Serializable]
    public class SerializableDeckData001 : SerializableDeckDataBase
    {
        public string[] UniqueCardIDs;
        public bool IsReadOnly;
    }

    // This is the future save file.
    [System.Serializable]
    [System.Obsolete("this is an example class",true)]
    public class SerializableDeckData002 : SerializableDeckDataBase
    {
        public string[] UniqueCardIDs;
        public int ExampleOfNewDataYouCouldNeedInTheFuture;
    }

}
