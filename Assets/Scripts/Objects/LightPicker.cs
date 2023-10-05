using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LightPicker : Pickables
{
    protected override void Interact()
    {
        switch (_type)
        {
            case PickableType.Common:
                _p.TurnLightsOn(.3f);
                break;
            case PickableType.NotCommon:
                _p.TurnLightsOn(.45f);
                break;
            case PickableType.NotTooCommonToBeCommonButTooRareToBeRare:
            case PickableType.Rare:
                _p.TurnLightsOn(.5f);
                break;
            case PickableType.UltraRare:
                _p.TurnLightsOn(.75f);
                break;
            case PickableType.Legendary:
                _p.TurnLightsOn(1f);
                break;
        }
        
        Destroy(gameObject);
    }
}
