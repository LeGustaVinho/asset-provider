using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace LegendaryTools.Systems.AssetProvider
{
    public class AddressableProvider
    {
        public static async Task<ILoadOperation> LoadAsync<T>(string key, Action<object> onComplete = null)
        {
            if (key.Length > 0)
            {
                AsyncOperationHandle<T> request = Addressables.LoadAssetAsync<T>(key);
                ILoadOperation loadOperation = new LoadOperation(request);
                if(onComplete != null)
                    loadOperation.OnCompleted += onComplete;
                
                while (!loadOperation.IsDone)
                {
                    await Task.Delay(25);
                }
                
                return loadOperation;
            }

            return null;
        }
        
        public static ILoadOperation PrepareLoadRoutine<T>(string path, Action<object> onComplete = null)
        {
            if (path.Length > 0)
            {
                AsyncOperationHandle<T> request = Addressables.LoadAssetAsync<T>(path);
                ILoadOperation loadOperation = new LoadOperation(request);
                if(onComplete != null)
                    loadOperation.OnCompleted += onComplete;
                return loadOperation;
            }

            return null;
        }

        public static IEnumerator WaitLoadRoutine(ILoadOperation loadOperation)
        {
            while (!loadOperation.IsDone)
            {
                yield return null;
            }
        }

        public static ILoadOperation LoadWithCoroutines<T>(string path, Action<object> onComplete)
        {
            ILoadOperation loadOperation = PrepareLoadRoutine<T>(path, onComplete);
            MonoBehaviourFacade.Instance.StartRoutine(WaitLoadRoutine(loadOperation));
            return loadOperation;
        }

        public static void Unload(ILoadOperation handle)
        {
            handle?.Release();
        }
    }
}