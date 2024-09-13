using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LegendaryTools.Systems.AssetProvider
{
    public class AssetLoaderConfig : ScriptableObject, IAssetLoaderConfig
    {
        [SerializeField] protected bool preload;
        [SerializeField] protected bool dontUnloadAfterLoad;
        [SerializeField] protected AssetProvider loadingStrategy;
        [SerializeField] protected string assetPath;
        
        public virtual bool PreLoad
        {
            get => preload;
            set => preload = value;
        }
        public virtual bool DontUnloadAfterLoad 
        {
            get => dontUnloadAfterLoad;
            set => dontUnloadAfterLoad = value;
        }

        public AssetProvider LoadingStrategy
        {
            get => loadingStrategy;
            set => loadingStrategy = value;
        }
        
        public virtual string AssetReference => assetPath;
        public virtual object LoadedAsset => loadedAsset;

        public virtual bool IsInScene { private set; get; } //Flag used to identify that this asset does not need load/unload because it is serialized in the scene

        public virtual bool IsLoaded => loadedAsset != null;

        public virtual bool IsLoading { private set; get; }

        private object loadedAsset;
        private ILoadOperation handle;
        
        public T Load<T>() where T : UnityEngine.Object
        {
            if (IsInScene || IsLoaded)
            {
                return loadedAsset as T;
            }

            if (LoadingStrategy != null)
            {
                IsLoading = true;
                return LoadingStrategy.Load<T>(AssetReference);
            }
            else
            {
                Debug.LogError("[AssetLoaderConfig:Load] -> LoadingStrategy is null");
                return null;
            }
        }

        public async Task<ILoadOperation> LoadAsync<T>(Action<object> onComplete = null)
            where T : UnityEngine.Object
        {
            void DualCallback(object arg)
            {
                OnLoadAssetAsync(arg);
                onComplete?.Invoke(arg);
            }
            
            if (IsInScene || IsLoaded)
            {
                DualCallback(loadedAsset);
            }

            if (LoadingStrategy != null)
            {
                IsLoading = true;
                Task<ILoadOperation> handleTask = LoadingStrategy.LoadAsync<T>(AssetReference, DualCallback);
                handle = handleTask.Result;
                return await handleTask;
            }

            Debug.LogError("[AssetLoaderConfig:Load] -> LoadingStrategy is null");
            return null;
        }

        public ILoadOperation PrepareLoadRoutine<T>(Action<object> onComplete = null)
            where T : UnityEngine.Object
        {
            void DualCallback(object arg)
            {
                OnLoadAssetAsync(arg);
                onComplete?.Invoke(arg);
            }
            
            if (IsInScene || IsLoaded)
            {
                DualCallback(loadedAsset);
            }

            if (LoadingStrategy != null)
            {
                IsLoading = true;
                handle = LoadingStrategy.PrepareLoadRoutine<T>(AssetReference, DualCallback);
                return handle;
            }

            Debug.LogError("[AssetLoaderConfig:Load] -> LoadingStrategy is null");
            return null;
        }

        public IEnumerator WaitLoadRoutine()
        {
            if (LoadingStrategy != null)
            {
                yield return LoadingStrategy.WaitLoadRoutine(handle);
            }
        }

        public ILoadOperation LoadWithCoroutines<T>(Action<object> onComplete) where T : UnityEngine.Object
        {
            void DualCallback(object arg)
            {
                OnLoadAssetAsync(arg);
                onComplete?.Invoke(arg);
            }
            
            if (IsInScene || IsLoaded)
            {
                DualCallback(loadedAsset);
            }

            if (LoadingStrategy != null)
            {
                IsLoading = true;
                handle = LoadingStrategy.LoadWithCoroutines<T>(AssetReference, DualCallback);
                return handle;
            }

            Debug.LogError("[AssetLoaderConfig:Load] -> LoadingStrategy is null");
            return null;
        }

        public virtual void Unload()
        {
            if (!IsInScene)
            {
                if (loadedAsset != null && LoadingStrategy != null)
                {
                    LoadingStrategy.Unload(handle);
                    handle = null;
                }
            }
        }

        public void SetAsSceneAsset(Object sceneInstanceInScene)
        {
            loadedAsset = sceneInstanceInScene;
        }

        public void ClearLoadedAssetRef()
        {
            loadedAsset = null;
        }
        
        private void OnLoadAssetAsync(object asset)
        {
            loadedAsset = asset;
            IsLoading = false;
        }
    }
}