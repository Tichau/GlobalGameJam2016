using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Note
{
    public const float Tolerance = 0.2f;

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

    [System.NonSerialized]
    public float NextAccuracy = float.NaN;

    [System.NonSerialized]
    public bool AlreadyFailed;

    /// <summary>
    /// Update the note state.
    /// </summary>
    /// <param name="relativeTime">The time relative to the note start (the note should be played when its value is 0).</param>
    /// <param name="layerUI"></param>
    /// <param name="index"></param>
    public bool UpdateNote(float relativeTime, LayerUI layerUI, int index, float layerProgress, out KeyCode invalidKeyPressed, GameObject fx, GameObject fxError, float fillAmount)
    {
        invalidKeyPressed = KeyCode.None;
        bool validKeyPressed = false;
        if (Input.GetKeyDown(this.InputKey))
        {
            float fxPlacement = relativeTime / Tolerance;
            if (relativeTime >= -Tolerance && relativeTime <= Tolerance)
            {
                if (float.IsNaN(this.Accuracy) && relativeTime >= 0)
                {
                    this.Accuracy = Mathf.Clamp01(Tolerance - Mathf.Abs(relativeTime)) / Tolerance;
                    validKeyPressed = true;
                    if (layerUI != null)
                    {
                        layerUI.DisplayFx(index, fxPlacement, fx);
                    }
                }
                else if (float.IsNaN(this.NextAccuracy) && relativeTime < 0)
                {
                    this.NextAccuracy = Mathf.Clamp01(Tolerance - Mathf.Abs(relativeTime)) / Tolerance;
                    validKeyPressed = true;
                    if (layerUI != null)
                    {
                        layerUI.DisplayFx(index, fxPlacement, fx);
                    }
                }
                else
                {
                    invalidKeyPressed = this.InputKey;

                    if (layerUI != null && invalidKeyPressed != KeyCode.None)
                    {
                        layerUI.DisplayFx(index, fxPlacement, fxError);
                    }
                }
            }
            else
            {
                invalidKeyPressed = this.InputKey;

                if (layerUI != null && invalidKeyPressed != KeyCode.None)
                {
                    layerUI.DisplayFx(index, fxPlacement, fxError);
                }
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

                case Action.PlaySmokeLeft:
                    GameManager.Instance.PlaySmokeLeft();
                    break;

                case Action.PlaySmokeRight:
                    GameManager.Instance.PlaySmokeRight();
                    break;

                case Action.EmitABassParticule:
                    GameManager.Instance.EmitBassParticle();
                    break;

                case Action.EndGame:
                    GameManager.Instance.EndGame();
                    break;
            }
        }
        else if (relativeTime >= -this.AnimAdvance && !this.animAlreadyPlayed)
        {
            if (layerUI != null)
            {
                layerUI.DisplayInputKey(index);
            }

            this.animAlreadyPlayed = true;
        }

        if (layerUI != null)
        {
            layerUI.UpdateNote(index, fillAmount);
        }

        return validKeyPressed;
    }

    public void Reset()
    {
        this.alreadyPlayed = false;
        this.animAlreadyPlayed = false;
        this.Accuracy = this.NextAccuracy;
        if (!float.IsNaN(this.NextAccuracy))
        {
            Debug.Log(string.Format("Transfert accuracy: {0}", this.Accuracy));
        }
        this.NextAccuracy = float.NaN;
        this.AlreadyFailed = false;
    }
}