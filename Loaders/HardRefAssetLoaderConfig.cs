using UnityEngine;

namespace LegendaryTools.Systems.AssetProvider
{ 
    [CreateAssetMenu(menuName = "Tools/AssetProvider/HardRefAssetLoaderConfig", fileName = "HardRefAssetLoaderConfig", order = 0)]
    public class HardRefAssetLoaderConfig : AssetLoaderConfig
    {
        [SerializeField] protected Object HardReference;
        
        public override bool PreLoad => false; //Already preload by serialization
        public override bool IsLoaded => true;
        public override bool IsLoading => false;
        public override bool DontUnloadAfterLoad => true;
        public override bool IsInScene => false;
        public override object LoadedAsset => HardReference;

        public override void Unload()
        {
            Debug.LogWarning("Hard Reference assets cannot be released ", this);
        }
    }
    
    public class HardRefAssetLoaderConfigT<T> : AssetLoaderConfig
        where T : UnityEngine.Object
    {
        [SerializeField] protected T HardReference;
        
        public override bool PreLoad => false; //Already preload by serialization
        public override bool IsLoaded => true;
        public override bool IsLoading => false;
        public override bool DontUnloadAfterLoad => true;
        public override bool IsInScene => false;
        public override object LoadedAsset => HardReference;

        public override void Unload()
        {
            Debug.LogWarning("Hard Reference assets cannot be released ", this);
        }
    }
}