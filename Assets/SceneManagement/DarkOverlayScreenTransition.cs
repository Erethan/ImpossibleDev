using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkOverlayScreenTransition : MonoBehaviour, IScreenTransition
{
    public event Action FadeInComplete;
    public event Action FadeOutComplete;

    public void FadeIn()
    {
        throw new NotImplementedException();
    }

    public void FadeOut()
    {
        throw new NotImplementedException();
    }

}
