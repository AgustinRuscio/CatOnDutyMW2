using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour
{
    private Slider _slider;

    private void Awake()
    { 
        _slider = GetComponent<Slider>();
    }

    public void UpdateLifeBar(float life, float maxLife)
    {
        _slider.value = life / maxLife;
    }
}
