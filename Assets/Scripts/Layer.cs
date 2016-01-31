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
    private float startTime = float.NaN;

    [System.NonSerialized]
    List<KeyCode> invalidKeyPressed = new List<KeyCode>();

    [System.NonSerialized]
    List<KeyCode> validKeyPressed = new List<KeyCode>();
    
    public float Score
    {
        get { return this.score; }
        set
        {
            this.score = Mathf.Clamp(value, 0, this.ScoreToReach*2f);
            Debug.Log("Layer: " + this.Name + " Score: " + this.Score);
        }
    }

    public bool IsValid
    {
        get { return this.ScoreToReach <= 0f || this.Progress >= 1f; }
    }

    public bool IsPlaying
    {
        get { return !float.IsNaN(startTime); }
    }

    public bool NeedInteraction
    {
        get
        {
            for (int index = 0; index < this.Notes.Count; index++)
            {
                if (this.Notes[index].InputKey != KeyCode.None)
                {
                    return true;
                }
            }

            return false;
        }
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
        this.Score = this.Score - errorCount;
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
                
                if (relativeTime >= 0f)
                {
                    this.Score += note.Accuracy;
                }
                else
                {
                    this.Score += note.NextAccuracy;
                }
            }
            else
            {
                if (relativeTime >= 0f)
                {
                    if (float.IsNaN(note.Accuracy) && float.IsNaN(note.NextAccuracy) && !note.AlreadyFailed &&
                        timeSinceCurrentLoopStart > note.StartTime + Note.Tolerance)
                    {
                        Debug.Log(string.Format("Fail: acc: {0} next acc: {1} loop: {2}", note.Accuracy,
                                note.NextAccuracy, this.loopCount));

                        this.Score--;
                        note.AlreadyFailed = true;
                    }
                }
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