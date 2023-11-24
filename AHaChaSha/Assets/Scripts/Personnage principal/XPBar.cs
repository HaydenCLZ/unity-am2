using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XPBar : MonoBehaviour
{

    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void SetRequiredXP(int XP)
    {
        slider.maxValue = XP;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetXP(int XP)
    {
        slider.value = XP;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}