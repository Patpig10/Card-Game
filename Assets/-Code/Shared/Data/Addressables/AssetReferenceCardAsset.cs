using UnityEngine.AddressableAssets;

namespace Game.Shared
{
    [System.Serializable]
    public class AssetReferenceCardAsset : AssetReferenceT<CardAsset>
    {
        public AssetReferenceCardAsset ( string guid ) : base( guid ) {}
    }
}
