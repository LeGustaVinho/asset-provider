using UnityEngine;

namespace LegendaryTools.Systems.AssetProvider
{
    [CreateAssetMenu(menuName = "Tools/AssetProvider/ResourcesAssetLoadableConfig", fileName = "ResourcesAssetLoadableConfig", order = 0)]
    public class ResourcesAssetLoaderConfig : AssetLoaderConfig
    {
        [SerializeField] protected ResourcePathReference ResourcePathReference;

        public override string AssetReference => ResourcePathReference.resourcePath;
    }
}