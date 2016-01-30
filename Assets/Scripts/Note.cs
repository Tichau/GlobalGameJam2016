using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Note
{
    [Tooltip("Musical Time")]
    public float StartTime;
    public AudioClip AudioClip;
}
