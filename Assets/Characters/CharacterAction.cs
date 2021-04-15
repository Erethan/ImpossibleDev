using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct CharacterAction 
{
    public GameObject Prefab;
    public float StaminaCost;

    /// <summary>
    /// Used to bind with state machine behaviours in animators
    /// </summary>
    public string BindingTag; 
}
