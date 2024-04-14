using UnityEngine.AddressableAssets;

namespace Game.Shared
{
    [System.Serializable]
    public class AssetReferenceDeckAsset : AssetReferenceT<DeckAsset>
    {
        public AssetReferenceDeckAsset ( string guid ) : base( guid ) {}
    }
}
