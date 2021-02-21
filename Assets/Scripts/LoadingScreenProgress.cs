using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pulse;

public class LoadingScreenProgress : MonoBehaviour
{
    [SerializeField] private SceneLoadingSystem _loadingSystem;
    [SerializeField] private Transform _start;
    [SerializeField] private Transform _end;

    private void Update()
    {
        float progress = _loadingSystem.Progress;
        transform.position = _start.position * (1 - progress) + _end.position * progress;
    }

}
