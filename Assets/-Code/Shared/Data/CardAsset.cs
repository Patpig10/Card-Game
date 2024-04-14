using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Assertions;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using NaughtyAttributes;

namespace Game.Shared
{
    /// <summary>
    /// Holds all the information that defines a card.
    /// </summary>
    [CreateAssetMenu( menuName="Game/Card" , fileName="card (0)" , order=1 )]
    public class CardAsset : ScriptableObject
    {

        [SerializeField][FormerlySerializedAs("guid")] string cardUniqueID;
        [SerializeField][System.Obsolete("Get rid of me")] int id;
        [SerializeField] string cardName;
        [SerializeField] int cost;
        [SerializeField] int power;
        [SerializeField] string cardDescription;
        [SerializeField] int drawXcards;
        [SerializeField] int addXmaxGil;
        [SerializeField] Sprite thisImage;
        [SerializeField] string color;
        [SerializeField] int returnXcards;
        [SerializeField] int healXpower;
        [SerializeField] int boostXpower;
        [SerializeField] bool spell;
        [SerializeField] int damageDealtBySpell;
        [SerializeField] bool ward;
        [SerializeField] int resurrectXcards;

        public string CardUniqueID => cardUniqueID;
        public string CardName => cardName;
        public int Cost => cost;
        public int Power => power;
        public string CardDescription => cardDescription;
        public int DrawXcards => drawXcards;
        public int AddXmaxGil => addXmaxGil;
        public Sprite Image => thisImage;
        public string Color => color;
        public int ReturnXcards => returnXcards;
        public int HealXpower => healXpower;
        public int BoostXpower => boostXpower;
        public bool IsSpell => spell;
        public int DamageDealtBySpell => damageDealtBySpell;
        public bool IsWard => ward;
        public int ResurrectXcards => resurrectXcards;

#if UNITY_EDITOR
        void OnValidate ()
        {
            if( cardUniqueID==null ) SetBrandNewGUID();
        }

        [Button( "Assign different GUID (think twice)" , EButtonEnableMode.Editor )]
        void SetBrandNewGUID ()
        {
            UnityEditor.Undo.RecordObject( this , nameof(SetBrandNewGUID) );
            cardUniqueID = System.Guid.NewGuid().ToString();
        }
#endif
        
        static Dictionary< AssetLabelReference , Dictionary<string,CardAsset> > Lookup = new ();

        static Dictionary<AssetLabelReference,AsyncOperationHandle> operationsInProgress = new ();
        static Dictionary<AssetLabelReference,AsyncOperationHandle> operationsCompleted = new ();

        public static bool Get ( string cardUniqueID , out CardAsset cardAsset )
        {
            ForceSynchronousComplete();
            
            foreach( var label_map in Lookup )
            {
                var map = label_map.Value;
                if( map.TryGetValue(cardUniqueID,out cardAsset) )
                {
                    return true;
                }
            }

            // entry not found
            Debug.LogWarning($"Failed to find a card asset with ID '{cardUniqueID}' in pool of currently loaded ones");
            cardAsset = null;
            return false;
        }
        public static CardAsset Get ( string cardUniqueID )
        {
            Get( cardUniqueID , out var cardAsset );
            return cardAsset;
        }
        //public static CardAsset Get ( int index )
        //{
        //    Get( cardUniqueID , out var cardAsset );
        //    return cardAsset;
        //}
        public static bool GetRandomCard ( out CardAsset cardAsset , uint randomSeed = default )
        {
            ForceSynchronousComplete();

            var rnd = new Unity.Mathematics.Random( randomSeed!=default ? randomSeed : (uint) Random.Range(0,uint.MaxValue) );
            
            Assert.AreNotEqual( 0 , Lookup.Count , "something gone wrong, no cards has been loaded yet" );
            var label = Lookup.Keys.ElementAt( rnd.NextInt(Lookup.Count) );
            
            var map = Lookup[label];

            if( map.Count==0 )
            {
                cardAsset = null;
                return false;
            }

            cardAsset = map.ElementAt( rnd.NextInt(map.Count) ).Value;
            return true;
        }
        public static bool GetRandomCard ( out CardAsset cardAsset , AssetLabelReference label , uint randomSeed = default )
        {
            ForceSynchronousComplete();

            var rnd = new Unity.Mathematics.Random( randomSeed!=default ? randomSeed : (uint) Random.Range(0,uint.MaxValue) );
            
            if( !Lookup.ContainsKey(label) )
            {
                Debug.LogError("");
                cardAsset = null;
                return false;
            }
            
            var map = Lookup[label];

            if( map.Count==0 )
            {
                cardAsset = null;
                return false;
            }

            cardAsset = map.ElementAt( rnd.NextInt(map.Count) ).Value;
            return true;
        }

        public static void RequestLoad ( AssetLabelReference label )
        {
            Debug.Log($"CardAsset.RequestLoad( {nameof(label)}:(`{label.labelString}`,`{label.RuntimeKey}`) )");

            if( !Lookup.ContainsKey(label) )
            {
                Lookup.Add( label , new () );
            }

            var op = Addressables.LoadAssetsAsync<CardAsset>( label , (cardAsset) =>
            {
                Assert.IsNotNull( cardAsset );
                Lookup[label].Add( cardAsset.CardUniqueID , cardAsset );
                Debug.Log($"CardAsset.RequestLoad(): card `{cardAsset.CardUniqueID}` added under `{label.labelString}` label (len:{Lookup[label].Count})");
            } );

            operationsInProgress.Add( label , op );
            op.Completed += (arg) => {
                operationsInProgress.Remove( label );
                operationsCompleted.Add( label , op );
                Debug.Log($"CardAsset.RequestLoad(): load op of `{label.labelString}` completed");
            };
        }

        public static void RequestUnload ( AssetLabelReference label )
        {
            // @TODO: not tested, make sure this works
            {
                if( operationsCompleted.TryGetValue(label,out var op) )
                {
                    Addressables.Release( op );
                }
            }
            {
                if( operationsInProgress.TryGetValue(label,out var op) )
                {
                    Addressables.Release( op );
                }
            }
        }

        public static void ForceSynchronousComplete ()
        {
            while( operationsInProgress.Count!=0 )
            {
                var kv = operationsInProgress.First();
                object obj = kv.Value.WaitForCompletion();
                if( obj is CardAsset )
                {
                    CardAsset cardAsset = (CardAsset) obj;
                    Debug.Log($"CardAsset.ForceSynchronousComplete(): `{cardAsset.cardUniqueID}` card loaded");
                }
            }
        }

        [RuntimeInitializeOnLoadMethod( RuntimeInitializeLoadType.BeforeSceneLoad )]
        public static void InitializeType ()
        {
            RequestLoad( new AssetLabelReference(){ labelString="card" } );
        }

    }
}
