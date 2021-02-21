using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;

public class SceneLoadRequester : MonoBehaviour
{
    [SerializeField] private SceneLoadingSystem _loadSystem;
    [SerializeField] private AssetReference _sceneToLoad;


    public void Request()
    {
        _loadSystem.LoadScene(_sceneToLoad);
    }
}
