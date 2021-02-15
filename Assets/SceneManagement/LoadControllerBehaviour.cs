using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class LoadControllerBehaviour : MonoBehaviour
{
    public bool Loading { get; private set; }
    public SceneLoadingSystem LoadingSystem { get; private set; }

    private AssetReference _targetScene;
    private AsyncOperationHandle<SceneInstance> _targetSceneLoadOperation;
    public float Progress => _targetSceneLoadOperation.PercentComplete;

    private GameObject _transitionGameObject;

    public static LoadControllerBehaviour InstantiateNew(SceneLoadingSystem loadingSystem, AssetReference transitionPrefab = null)
    {
        LoadControllerBehaviour instance = new GameObject()
            .AddComponent<LoadControllerBehaviour>();
        instance.LoadingSystem = loadingSystem;
        DontDestroyOnLoad(instance.gameObject);
        instance.gameObject.name = $"{typeof(LoadControllerBehaviour)}";

        if(transitionPrefab != null)
        {
            transitionPrefab.InstantiateAsync(instance.transform).Completed += instance.OnTransitionInstantiated; ;
        }

        return instance;
    }

    private void OnTransitionInstantiated(AsyncOperationHandle<GameObject> obj)
    {
        if(obj.Status == AsyncOperationStatus.Succeeded)
        {
            _transitionGameObject = obj.Result;
        }
    }

    public void LoadScene(AssetReference scene)
    {
        if(Loading == true)
        {
            Debug.LogWarning($"Cannot load scene {scene.SubObjectName} while another is currently loading.");
            return;
        }

        _targetScene = scene;
        Addressables.LoadSceneAsync(LoadingSystem.LoadingScene, LoadSceneMode.Single)
            .Completed += OnLoadingScreenLoaded;
    }

    private void OnLoadingScreenLoaded(AsyncOperationHandle<SceneInstance> handle)
    {
        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError("Failed to load loading screen");
            Loading = false;
            return;
        }

        _targetSceneLoadOperation = Addressables.LoadSceneAsync(_targetScene, LoadSceneMode.Single);
        _targetSceneLoadOperation.Completed += OnTargetSceneLoad;
    }

    private void OnTargetSceneLoad(AsyncOperationHandle<SceneInstance> handle)
    {
        Loading = false;
        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogWarning($"Failed to load scene {_targetScene.SubObjectName}");
            return;
        }

    }
}
