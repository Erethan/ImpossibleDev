using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;

public class SceneLoadRequester : MonoBehaviour
{
    [SerializeField] private AssetReference _sceneToLoad;
    [SerializeField] private SceneLoadingSystem _loadSystem;

   
    public void Request()
    {
        _loadSystem.LoadScene(_sceneToLoad);
    }
}
