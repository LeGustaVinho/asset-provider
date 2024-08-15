using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LegendaryTools.Systems.AssetProvider
{
    [CreateAssetMenu(menuName = "Tools/AssetProvider/AddressablesAssetLoaderConfig", fileName = "AddressablesAssetLoaderConfig", order = 0)]
    public class AddressablesAssetLoaderConfig : AssetLoaderConfig
    {
        [SerializeField] protected AssetReference assetReference;
        
        public override string AssetReference => assetReference.RuntimeKey as string;
    }
    
    public class AddressablesAssetLoaderConfig<T> : AssetLoaderConfig
        where T : UnityEngine.Object
    {
        [SerializeField] protected AssetReferenceT<T> assetReference;
        
        public override string AssetReference => assetReference.RuntimeKey as string;
    }
}