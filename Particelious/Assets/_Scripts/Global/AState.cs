using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AState : MonoBehaviour {

    [HideInInspector]
    public GameManager manager;

    public abstract void Enter(AState from);
    public abstract void Exit(AState to);

    public abstract string GetName();
}
