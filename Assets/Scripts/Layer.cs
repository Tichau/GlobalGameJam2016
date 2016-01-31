using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class Layer
{
    public string Name;
    
    [UnityEngine.Range(0f, 10f)]
    public float ScoreToReach = 3f;

    public AudioClip AudioClipError;

    public float StartingDelay;
    public float TimesByLoop;
    public List<Note> Notes;

    [System.NonSerialized]
    private float score;

    [System.NonSerialized]
    private int loopCount;

    [System.NonSerialized]
    public bool IsValid = false;

    [System.NonSerialized]
    private float startTime = float.NaN;

    [System.NonSerialized]
    List<KeyCode> invalidKeyPressed = new List<KeyCode>();

    [System.NonSerialized]
    List<KeyCode> validKeyPressed = new List<KeyCode>();

    public bool IsPlaying
    {
        get { return !float.IsNaN(startTime); }
    }

    public float Progress
    {
        get { return Mathf.Clamp01(this.score/this.ScoreToReach); }
    }

    public void StartLayer(float time)
    {
        int measureSinceBegining = (int) time/4;
        this.startTime = (measureSinceBegining + 1)*4;
        this.loopCount = 0;
        this.IsValid = this.ScoreToReach <= 0f;
        this.score = 0;
        for (int index = 0; index < this.Notes.Count; index++)
        {
            this.Notes[index].Reset();
        }
    }

    public void StopLayer()
    {
        this.startTime = float.NaN;
    }

    public void OnErrorAppend(int errorCount)
    {
        this.score = Mathf.Clamp(this.score - errorCount, 0f, this.ScoreToReach * 2f);
        AudioManager.Instance.Play(this.AudioClipError);
        // TODO: Some visual or audio feedback ?
    }

    public void UpdateLayer(float time, LayerUI layerUI, GameObject fx, GameObject fxError)
    {
        float startTimeWithDelay = this.startTime + this.StartingDelay;

        float timeSinceLayerStart = time - startTimeWithDelay;
        float timeSinceCurrentLoopStart = timeSinceLayerStart > 0
            ? timeSinceLayerStart%this.TimesByLoop
            : timeSinceLayerStart;

        if ((int) timeSinceLayerStart/(int) this.TimesByLoop > this.loopCount)
        {
            for (int index = 0; index < this.Notes.Count; index++)
            {
                Note note = this.Notes[index];
                if (float.IsNaN(note.Accuracy))
                {
                    this.score -= 1;
                }
                else
                {
                    this.score += note.Accuracy;
                }
            }

            this.score = Mathf.Clamp(this.score, 0f, this.ScoreToReach * 2f);

            this.IsValid = this.Progress >= 1;

            Debug.Log("Layer: " + this.Name + " Score: " + this.score + " IsValid: " + this.IsValid);

            this.loopCount++;
            for (int index = 0; index < this.Notes.Count; index++)
            {
                this.Notes[index].Reset();
            }
        }

        this.validKeyPressed.Clear();
        this.invalidKeyPressed.Clear();
        for (int index = 0; index < this.Notes.Count; index++)
        {
            Note note = this.Notes[index];

            float relativeTime = timeSinceCurrentLoopStart - note.StartTime;

            if (Mathf.Abs(timeSinceCurrentLoopStart - this.TimesByLoop) < relativeTime)
            {
                relativeTime = timeSinceCurrentLoopStart - this.TimesByLoop;
            }

            KeyCode invalidKeyPressed;
            if (note.UpdateNote(relativeTime, layerUI, index, this.Progress, out invalidKeyPressed, fx, fxError, this.Progress))
            {
                this.validKeyPressed.Add(note.InputKey);
            }

            if (invalidKeyPressed != KeyCode.None)
            {
                this.invalidKeyPressed.Add(invalidKeyPressed);
            }
        }

        // If keys have been pressed outside range of any note, decrease the score.
        for (int index = 0; index < this.validKeyPressed.Count; index++)
        {
            this.invalidKeyPressed.RemoveAll(match => match == this.validKeyPressed[index]);
        }

        if (this.invalidKeyPressed.Count > 0)
        {
            this.OnErrorAppend(this.invalidKeyPressed.Count);
        }
    }
}