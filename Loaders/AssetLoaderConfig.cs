using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LegendaryTools.Systems.AssetProvider
{
    public abstract class AssetLoaderConfig : ScriptableObject, IAssetLoaderConfig, IAssetProvider
    {
        [SerializeField] protected bool preload;
        [SerializeField] protected bool dontUnloadAfterLoad;
        
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
        
        public abstract string AssetReference { get; }
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

            IsLoading = true;
            return Load<T>(AssetReference);
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


            IsLoading = true;
            Task<ILoadOperation> handleTask = LoadAsync<T>(AssetReference, DualCallback);
            handle = handleTask.Result;
            return await handleTask;
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
                return null;
            }

            IsLoading = true;
            handle = PrepareLoadRoutine<T>(AssetReference, DualCallback);
            return handle;
        }

        public IEnumerator WaitLoadRoutine()
        {
            yield return WaitLoadRoutine(handle);
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
                return null;
            }
            
            IsLoading = true;
            handle = LoadWithCoroutines<T>(AssetReference, DualCallback);
            return handle;
        }

        public virtual void Unload()
        {
            if (!IsInScene)
            {
                if (loadedAsset != null )
                {
                    Unload(handle);
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

        public abstract T Load<T>(object arg) where T : Object;
        public abstract Task<ILoadOperation> LoadAsync<T>(string key, Action<object> onComplete = null) where T : Object;

        public abstract ILoadOperation PrepareLoadRoutine<T>(string path, Action<object> onComplete = null)
            where T : Object;
        public abstract IEnumerator WaitLoadRoutine(ILoadOperation loadOperation);
        public abstract ILoadOperation LoadWithCoroutines<T>(string path, Action<object> onComplete) where T : Object;
        public abstract void Unload(ILoadOperation handle);
    }
}