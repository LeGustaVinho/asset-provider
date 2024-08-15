using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace LegendaryTools.Systems.AssetProvider
{
    [CreateAssetMenu(menuName = "Tools/AssetProvider/ResourcesAssetProvider", order = 1)]
    public class ResourcesAssetProvider : AssetProvider
    {
        public override T Load<T>(object arg)
        {
            string path = (string)arg;
            if (path.Length > 0)
            {
                return Resources.Load<T>(path);
            }

            return null;
        }

        public override async Task<ILoadOperation> LoadAsync<T>(string path, Action<object> onComplete = null)
        {
            if (path.Length > 0)
            {
                ResourceRequest resourcesRequest = Resources.LoadAsync<T>(path);
                ILoadOperation loadOperation = new LoadOperation(resourcesRequest);
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
            ResourceRequest resourcesRequest = Resources.LoadAsync<T>(path);
            ILoadOperation loadOperation = new LoadOperation(resourcesRequest);
            if(onComplete != null)
                loadOperation.OnCompleted += onComplete;
            
            return loadOperation;
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