using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Note
{
    public const float Tolerance = 0.4f;

    [Tooltip("Musical Time")]
    public float StartTime;
    public AudioClip AudioClipLite;
    public AudioClip AudioClipFat;
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
    
    /// <summary>
    /// Update the note state.
    /// </summary>
    /// <param name="relativeTime">The time relative to the note start (the note should be played when its value is 0).</param>
    /// <param name="layerUI"></param>
    /// <param name="index"></param>
    public void UpdateNote(float relativeTime, LayerUI layerUI, int index, float layerProgress)
    {
        if (Input.GetKeyDown(this.InputKey) && float.IsNaN(this.Accuracy))
        {
            if (relativeTime >= -Tolerance && relativeTime <= Tolerance)
            {
                this.Accuracy = Mathf.Clamp01(Tolerance - Mathf.Abs(relativeTime)) / Tolerance;
            }
        }

        if (relativeTime >= 0 && !this.alreadyPlayed)
        {
            AudioManager.Instance.Play(this.AudioClipLite);
            AudioManager.Instance.Play(this.AudioClipFat, layerProgress);
            this.alreadyPlayed = true;

            switch (this.Action)
            {
                case Action.ChangeBackgroundColor:
                    GameManager.Instance.ChangeBackgroundColor();
                    break;
            }
        }
        else if (relativeTime >= -this.AnimAdvance && !this.animAlreadyPlayed)
        {
            layerUI.DisplayInputKey(index);
            this.animAlreadyPlayed = true;
        }   
    }

    public void Reset()
    {
        this.alreadyPlayed = false;
        this.animAlreadyPlayed = false;
        this.Accuracy = float.NaN;
    }
}