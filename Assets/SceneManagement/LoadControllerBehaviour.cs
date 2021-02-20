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

    private AsyncOperationHandle<SceneInstance> _loadOperation;
    public float Progress
    {
        get
        {
            if (!_loadOperation.IsValid())
                return 1f;
            return _loadOperation.PercentComplete;
        }
    }

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

        StartCoroutine(nameof(LoadRoutine), scene);
    }


    private IEnumerator LoadRoutine(AssetReference targetScene)
    {
        if(_screenTransition == null)
        {

        }
        yield return SceneChange(LoadingSystem.LoadingScene);
        yield return SceneChange(targetScene);
        Loading = false;
    }

    private IEnumerator SceneChange(AssetReference scene)
    {
        bool transitionLoaded = _screenTransition != null;

        if (transitionLoaded)
        {
            _screenTransition.FadeOut();
        }

        _loadOperation = Addressables.LoadSceneAsync(scene, LoadSceneMode.Single, activateOnLoad: false);
        
        yield return _loadOperation;
        if (transitionLoaded)
        {
            yield return new WaitUntil(() => _screenTransition.Faded);
        }

        yield return _loadOperation.Result.ActivateAsync();
        if (transitionLoaded)
        {
            _screenTransition.FadeIn();
            yield return new WaitUntil(() => !_screenTransition.Faded);
        }


    }


}
