using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName ="Lighting Preset", menuName ="Scriptable/Lighting Preset", order = 1)]
public class TimeController : ScriptableObject
{
    public Gradient AmbientColor;
    public Gradient DirectionalColor;
    public Gradient FogColor;
}