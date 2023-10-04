using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthPicker : Pickables
{
    private Player _p;
    private void OnTriggerEnter(Collider other)
    {
         if(other.gameObject.TryGetComponent(out  Player _p))
         {
             Interact();
         }
        
    }

    protected override void Interact()
    {
        switch (_type)
        {
            case PickableType.Common:
                _p.GetHealth(5);
                break;
            case PickableType.NotCommon:
                _p.GetHealth(15);
                break;
            case PickableType.NotTooCommonToBeCommonButTooRareToBeRare:
                _p.GetHealth(25);
                break;
            case PickableType.Rare:
                _p.GetHealth(30);
                break;
            case PickableType.UltraRare:
                _p.GetHealth(40);
                break;
            case PickableType.Legendary:
                _p.GetHealth(75);
                break;
        }
    }
}
