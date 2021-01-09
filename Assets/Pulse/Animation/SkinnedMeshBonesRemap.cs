using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pulse.Experimental
{
    public class SkinnedMeshBonesRemap : MonoBehaviour
    {
#pragma warning disable CS0649
        [SerializeField] private SkinnedMeshRenderer skinnedRenderer;
        [SerializeField] private Transform[] targetBones;
#pragma warning restore CS0649


        void OnEnable()
        {
            skinnedRenderer.bones = targetBones;
        }
    }
}
