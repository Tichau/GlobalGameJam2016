using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class Layer
{
    public float StartingTime;
    public float LoopDurationInNumberOfTimes;
    public List<Note> Notes;

    [System.NonSerialized]
    private int loopCount;

    public void Update(float time)
    {
        if (time >= this.StartingTime)
        {
            float timeSinceLayerStart = time - this.StartingTime;
            float timeSinceCurrentLoopStart = timeSinceLayerStart % this.LoopDurationInNumberOfTimes;

            if ((int)timeSinceLayerStart / (int) this.LoopDurationInNumberOfTimes > this.loopCount)
            {
                this.loopCount++;
                for (int index = 0; index < this.Notes.Count; index++)
                {
                    this.Notes[index].Reset();
                }
            }

            for (int index = 0; index < this.Notes.Count; index++)
            {
                this.Notes[index].Update(timeSinceCurrentLoopStart);
            }
        }
    }
}