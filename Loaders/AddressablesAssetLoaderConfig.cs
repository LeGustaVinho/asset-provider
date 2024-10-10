using UnityEngine;
using UnityEngine.AddressableAssets;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace LegendaryTools.Systems.AssetProvider
{
    [CreateAssetMenu(menuName = "Tools/AssetProvider/AddressablesAssetLoaderConfig", fileName = "AddressablesAssetLoaderConfig", order = 0)]
    public class AddressablesAssetLoaderConfig : AssetLoaderConfig
    {
        [SerializeField] protected AssetReference assetReference;
        
        public override string AssetReference => assetReference.RuntimeKey as string;
        public override T Load<T>(object arg)
        {
            throw new NotSupportedException();
        }

        public override Task<ILoadOperation> LoadAsync<T>(string key, Action<object> onComplete = null)
        {
            return AddressableProvider.LoadAsync<T>(key, onComplete);
        }

        public override ILoadOperation PrepareLoadRoutine<T>(string path, Action<object> onComplete = null)
        {
            return AddressableProvider.PrepareLoadRoutine<T>(path, onComplete);
        }

        public override IEnumerator WaitLoadRoutine(ILoadOperation loadOperation)
        {
            return AddressableProvider.WaitLoadRoutine(loadOperation);
        }

        public override ILoadOperation LoadWithCoroutines<T>(string path, Action<object> onComplete)
        {
            return AddressableProvider.LoadWithCoroutines<T>(path, onComplete);
        }

        public override void Unload(ILoadOperation handle)
        {
            AddressableProvider.Unload(handle);
        }
    }
    
    public class AddressablesAssetLoaderConfig<T> : AssetLoaderConfig
        where T : UnityEngine.Object
    {
        [SerializeField] protected AssetReferenceT<T> assetReference;
        
        public override string AssetReference => assetReference.RuntimeKey as string;
#pragma warning disable 0693
        public override T Load<T>(object arg)
        {
            throw new NotSupportedException();
        }

        public override Task<ILoadOperation> LoadAsync<T>(string key, Action<object> onComplete = null)
        {
            return AddressableProvider.LoadAsync<T>(key, onComplete);
        }

        public override ILoadOperation PrepareLoadRoutine<T>(string path, Action<object> onComplete = null)
        {
            return AddressableProvider.PrepareLoadRoutine<T>(path, onComplete);
        }

        public override IEnumerator WaitLoadRoutine(ILoadOperation loadOperation)
        {
            return AddressableProvider.WaitLoadRoutine(loadOperation);
        }

        public override ILoadOperation LoadWithCoroutines<T>(string path, Action<object> onComplete)
        {
            return AddressableProvider.LoadWithCoroutines<T>(path, onComplete);
        }
#pragma warning restore 0693

        public override void Unload(ILoadOperation handle)
        {
            AddressableProvider.Unload(handle);
        }
    }
}