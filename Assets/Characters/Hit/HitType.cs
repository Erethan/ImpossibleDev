using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[Serializable]
public class HitTypeUnityEvent : UnityEvent<HitType> { }

public abstract class HitType : ScriptableObject
{
    
}

