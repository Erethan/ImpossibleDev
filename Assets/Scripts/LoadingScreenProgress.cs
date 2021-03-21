using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Erethan.ScreneTransition;

public class LoadingScreenProgress : MonoBehaviour
{
    [SerializeField] private SceneLoadService _loadingSystem;
    [SerializeField] private Transform _start;
    [SerializeField] private Transform _end;

    private void Update()
    {
        float progress = _loadingSystem.Progress;
        transform.position = _start.position * (1 - progress) + _end.position * progress;
    }

}
