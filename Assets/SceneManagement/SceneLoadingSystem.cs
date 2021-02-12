using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;



[CreateAssetMenu(fileName = "Scene Loading", menuName = "Pulse/Scene Management/Scene Loading System")]
public class SceneLoadingSystem : ScriptableObject
{
    private LoadControllerBehaviour _controllerBehaviour;
    private LoadControllerBehaviour ControllerBehaviour
    {
        get
        {
            if(_controllerBehaviour == null)
            {
                return LoadControllerBehaviour.InstantiateNew(this);
            }
            return _controllerBehaviour;
        }
    }
    public float Progress => ControllerBehaviour.Progress;
    public void LoadScene(AssetReference scene) => ControllerBehaviour.LoadScene(scene);


    [SerializeField] private AssetReference _loadingScene;
    public AssetReference LoadingScene => _loadingScene;



#if UNITY_EDITOR
    private void OnEnable()
    {
        UnityEditor.EditorApplication.playModeStateChanged += playmodeChange;
    }

    private void OnDisable()
    {
        UnityEditor.EditorApplication.playModeStateChanged -= playmodeChange;
    }

    private void playmodeChange(UnityEditor.PlayModeStateChange obj)
    {
        if (obj != UnityEditor.PlayModeStateChange.ExitingPlayMode)
            return;
        if (_controllerBehaviour == null)
            return;

        DestroyImmediate(_controllerBehaviour.gameObject);
        _controllerBehaviour = null;
    }
#endif
}
