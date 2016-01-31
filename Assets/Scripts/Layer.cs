using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class Layer
{
    [UnityEngine.Range(0f, 1f)]
    public float Difficulty = 0.5f;
    
    [UnityEngine.Range(0f, 10f)]
    public float ScoreToReach = 3f;

    public string Name;
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

    public bool IsPlaying
    {
        get { return !float.IsNaN(startTime); }
    }

    public void StartLayer(float time)
    {
        int measureSinceBegining = (int) time/4;
        this.startTime = (measureSinceBegining + 1)*4;
        this.loopCount = 0;
        this.IsValid = false;
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

    public void UpdateLayer(float time, LayerUI layerUI)
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
                    this.score += note.Accuracy - this.Difficulty;
                }
            }

            this.score = Mathf.Max(0f, this.score);

            this.IsValid = this.score >= this.ScoreToReach;

            Debug.Log("Layer: " + this.Name + " Score: " + this.score + " IsValid: " + this.IsValid);

            this.loopCount++;
            for (int index = 0; index < this.Notes.Count; index++)
            {
                this.Notes[index].Reset();
            }
        }

        for (int index = 0; index < this.Notes.Count; index++)
        {
            Note note = this.Notes[index];

            float relativeTime = timeSinceCurrentLoopStart - note.StartTime;

            if (Mathf.Abs(timeSinceCurrentLoopStart - this.TimesByLoop) < relativeTime)
            {
                relativeTime = timeSinceCurrentLoopStart - this.TimesByLoop;
            }

            note.UpdateNote(relativeTime, layerUI, index);
        }
    }
}