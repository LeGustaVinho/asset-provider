using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace LegendaryTools.Systems.AssetProvider
{
    public abstract class AssetProvider : ScriptableObject
    {
        public abstract T Load<T>(object arg) where T : UnityEngine.Object;
        
        public abstract Task<ILoadOperation> LoadAsync<T>(string key, Action<object> onComplete = null) where T : UnityEngine.Object;

        public abstract ILoadOperation PrepareLoadRoutine<T>(string path, Action<object> onComplete = null) where T : UnityEngine.Object;

        public abstract IEnumerator WaitLoadRoutine(ILoadOperation loadOperation);

        public abstract ILoadOperation LoadWithCoroutines<T>(string path, Action<object> onComplete) where T : UnityEngine.Object;

        public abstract void Unload(ILoadOperation handle);
    }
}