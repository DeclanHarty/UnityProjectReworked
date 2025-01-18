using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public abstract class SplineObserver : MonoBehaviour
{
    public abstract void NotifyUpdate();
}
