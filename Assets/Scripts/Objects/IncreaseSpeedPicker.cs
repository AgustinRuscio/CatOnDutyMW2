using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IncreaseSpeedPicker : Pickables
{
    protected override void Interact()
    {
        switch (_type)
        {
            case PickableType.Common:
                _p.IncreaseSpeed(1.1f, .5f);
                break;
            case PickableType.NotCommon:
                _p.IncreaseSpeed(1.15f, .7f);
                break;
            case PickableType.NotTooCommonToBeCommonButTooRareToBeRare:
                _p.IncreaseSpeed(1.20f, .5f);
                break;
            case PickableType.Rare:
                _p.IncreaseSpeed(1.25f, .7f);
                break;
            case PickableType.UltraRare:
                _p.IncreaseSpeed(1.5f, .7f);
                break;
            case PickableType.Legendary:
                _p.IncreaseSpeed(2, .75f);
                break;
        }
        
        
        Destroy(gameObject);
    }
}
