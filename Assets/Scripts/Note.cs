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
    public KeyCode InputKey;
    public float AnimAdvance = 0.2f;

    [System.NonSerialized]
    private bool alreadyPlayed;
    [System.NonSerialized]
    private bool animAlreadyPlayed;
    
    [System.NonSerialized]
    public float Accuracy;
    
    public void Update(float time, LayerUI layerUI, int index)
    {
        if (Input.GetKeyDown(this.InputKey))
        {
            float accuracy = Mathf.Clamp01(0.01f / Mathf.Max(Mathf.Abs(time - this.StartTime), 0.001f));
            this.Accuracy = Mathf.Max(this.Accuracy, accuracy);
            Debug.Log("Accuracy: " + accuracy + " Max: " + this.Accuracy + " time: " + time + " start time: " + this.StartTime);
        }

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
        else if (time >= this.StartTime - this.AnimAdvance && !this.animAlreadyPlayed)
        {
            layerUI.DisplayInputKey(Enum.GetName(typeof(KeyCode), this.InputKey), index);
            this.animAlreadyPlayed = true;
        }   
    }

    public void Reset()
    {
        this.alreadyPlayed = false;
        this.animAlreadyPlayed = false;
        this.Accuracy = 0f;
    }
}