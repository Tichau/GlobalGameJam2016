using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Note
{
    [Tooltip("Musical Time")]
    public float StartTime;
    public AudioClip AudioClip;
    public Action Action;
    public object ActionParameter;

    [System.NonSerialized]
    private bool alreadyPlayed;

    public void Update(float time)
    {
        if (time >= this.StartTime && !this.alreadyPlayed)
        {
            AudioManager.Instance.Play(this.AudioClip);
            this.alreadyPlayed = true;

            switch (this.Action)
            {
                case Action.ChangeBackgroundColor:
                    GameManager.Instance.ChangeBackgroundColor();
                    break;
            }
        }
    }

    public void Reset()
    {
        this.alreadyPlayed = false;
    }
}