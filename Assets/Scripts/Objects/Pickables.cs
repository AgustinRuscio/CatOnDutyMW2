using System;using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickables : MonoBehaviour
{
    [SerializeField]
    protected PickableType _type;

    protected Player _p;
    protected abstract void Interact();
    
    
    private void OnTriggerEnter(Collider other)
    {
        _p= other.gameObject.GetComponent<Player>();

        if (_p == null)
            return;

        Interact();
    }

    public void SetType(PickableType type)
    {
        _type = type;
    }

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