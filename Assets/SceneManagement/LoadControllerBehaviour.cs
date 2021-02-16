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

    private IScreenTransition _screenTransition;

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
            _screenTransition = obj.Result.GetComponent<IScreenTransition>();

        }
    }


    public void LoadScene(AssetReference scene)
    {
        if(Loading == true)
        {
            Debug.LogWarning($"Cannot load scene {scene.SubObjectName} while another is currently loading.");
            return;
        }
        Loading = true;
        _targetScene = scene;

        if(_screenTransition != null)
        {
            _screenTransition.FadeInComplete += ScreenTransitionFadeInComplete;
            _screenTransition.FadeIn();
            return;
        }

        Addressables.LoadSceneAsync(LoadingSystem.LoadingScene, LoadSceneMode.Single, activateOnLoad: false)
            .Completed += OnLoadingScreenLoaded;
    }

    private void ScreenTransitionFadeInComplete()
    {
        _screenTransition.FadeInComplete -= ScreenTransitionFadeInComplete;

    }

    private void OnLoadingScreenLoaded(AsyncOperationHandle<SceneInstance> handle)
    {
        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError("Failed to load loading screen");
            Loading = false;
            return;
        }
        handle.Result.ActivateAsync().completed += OnLoadingScreenActive;
    }

    private void OnLoadingScreenActive(AsyncOperation obj)
    {
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
