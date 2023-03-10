using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace LegendaryTools.Systems.AssetProvider
{
    [CreateAssetMenu(menuName = "Tools/ScreenFlow/ResourcesAssetProvider")]
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

        public override async Task<T> LoadAsync<T>(object arg, Action<T> onComplete)
        {
            string path = (string)arg;
            if (path.Length > 0)
            {
                ResourceRequest resourcesRequest = Resources.LoadAsync<T>(path);

                while (!resourcesRequest.isDone)
                {
                    await Task.Delay(25);
                }

                var result = resourcesRequest.asset as T;
                onComplete.Invoke(result);
                return result;
            }

            return null;
        }

        public override void Unload<T>(ref T instance)
        {
            instance = null;
            Resources.UnloadUnusedAssets();
        }
    }
}