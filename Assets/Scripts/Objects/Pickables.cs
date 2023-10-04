using System;using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickables : MonoBehaviour
{
    [SerializeField]
    protected PickableType _type;

    protected abstract void Interact();
}

public enum PickableType
{
    Common,
    NotCommon,
    NotTooCommonToBeCommonButTooRareToBeRare,
    Rare,
    UltraRare,
    Legendary,
}