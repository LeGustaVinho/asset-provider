using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

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
        public override string AssetReference { get; }
        public override bool IsInScene => false;
        public override object LoadedAsset => HardReference;

        public override T Load<T>(object arg)
        {
            return HardReference as T;
        }

        public override Task<ILoadOperation> LoadAsync<T>(string key, Action<object> onComplete = null)
        {
            onComplete?.Invoke(HardReference as T);
            return null;
        }

        public override ILoadOperation PrepareLoadRoutine<T>(string path, Action<object> onComplete = null)
        {
            onComplete?.Invoke(HardReference as T);
            return null;
        }

        public override IEnumerator WaitLoadRoutine(ILoadOperation loadOperation)
        {
            yield return null;
        }

        public override ILoadOperation LoadWithCoroutines<T>(string path, Action<object> onComplete)
        {
            onComplete?.Invoke(HardReference as T);
            return null;
        }

        public override void Unload()
        {
            Debug.LogWarning("Hard Reference assets cannot be released ", this);
        }
        
        public override void Unload(ILoadOperation handle)
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
        public override string AssetReference { get; }
        public override bool IsInScene => false;
        public override object LoadedAsset => HardReference;
#pragma warning disable 0693
        public override T Load<T>(object arg)
        {
            return HardReference as T;
        }

        public override Task<ILoadOperation> LoadAsync<T>(string key, Action<object> onComplete = null)
        {
            onComplete?.Invoke(HardReference as T);
            return null;
        }

        public override ILoadOperation PrepareLoadRoutine<T>(string path, Action<object> onComplete = null)
        {
            onComplete?.Invoke(HardReference as T);
            return null;
        }

        public override IEnumerator WaitLoadRoutine(ILoadOperation loadOperation)
        {
            yield return null;
        }

        public override ILoadOperation LoadWithCoroutines<T>(string path, Action<object> onComplete)
        {
            onComplete?.Invoke(HardReference as T);
            return null;
        }
#pragma warning restore 0693
        
        public override void Unload()
        {
            Debug.LogWarning("Hard Reference assets cannot be released ", this);
        }
        
        public override void Unload(ILoadOperation handle)
        {
            Debug.LogWarning("Hard Reference assets cannot be released ", this);
        }
    }
}