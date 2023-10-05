using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveAmmoPicker : Pickables
{
    protected override void Interact()
    {
        switch (_type)
        {
            case PickableType.Common:
                _p.GetAmmo(1);
                break;
            case PickableType.NotCommon:
                _p.GetAmmo(2);
                break;
            case PickableType.NotTooCommonToBeCommonButTooRareToBeRare:
                _p.GetAmmo(3);
                break;
            case PickableType.Rare:
            case PickableType.UltraRare:
                _p.GetAmmo(4);
                break;
            case PickableType.Legendary:
                _p.GetAmmo(10);
                break;
        }
        
        Destroy(gameObject);
    }
}
