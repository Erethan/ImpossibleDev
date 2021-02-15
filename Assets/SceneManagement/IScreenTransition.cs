using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IScreenTransition
{
    event Action FadeInComplete;
    event Action FadeOutComplete;

    void FadeIn();
    void FadeOut();
}
