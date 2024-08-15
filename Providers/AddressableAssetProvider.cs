using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace LegendaryTools.Systems.AssetProvider
{
    [CreateAssetMenu(menuName = "Tools/AssetProvider/AdddressableAssetProvider", order = 1)]
    public class AdddressableAssetProvider : AssetProvider
    {
        public override T Load<T>(object arg)
        {
            return null; //Sync loading is not supported by addressables
        }
        
        public override async Task<ILoadOperation> LoadAsync<T>(string key, Action<object> onComplete = null)
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
        
        public override ILoadOperation PrepareLoadRoutine<T>(string path, Action<object> onComplete = null)
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

        public override IEnumerator WaitLoadRoutine(ILoadOperation loadOperation)
        {
            while (!loadOperation.IsDone)
            {
                yield return null;
            }
        }

        public override ILoadOperation LoadWithCoroutines<T>(string path, Action<object> onComplete)
        {
            ILoadOperation loadOperation = PrepareLoadRoutine<T>(path, onComplete);
            MonoBehaviourFacade.Instance.StartRoutine(WaitLoadRoutine(loadOperation));
            return loadOperation;
        }

        public override void Unload(ILoadOperation handle)
        {
            handle?.Release();
        }
    }
}