using System;
using System.Collections;
using System.Threading.Tasks;

namespace LegendaryTools.Systems.AssetProvider
{
    public interface  IAssetProvider
    {
        public T Load<T>(object arg) where T : UnityEngine.Object;
        
        public Task<ILoadOperation> LoadAsync<T>(string key, Action<object> onComplete = null) where T : UnityEngine.Object;

        public ILoadOperation PrepareLoadRoutine<T>(string path, Action<object> onComplete = null) where T : UnityEngine.Object;

        public IEnumerator WaitLoadRoutine(ILoadOperation loadOperation);

        public ILoadOperation LoadWithCoroutines<T>(string path, Action<object> onComplete) where T : UnityEngine.Object;

        public void Unload(ILoadOperation handle);
    }
}