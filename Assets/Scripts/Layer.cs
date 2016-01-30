using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class Layer
{
    public string Name;
    public float StartingTime;
    public float TimesByLoop;
    public List<Note> Notes;

    [System.NonSerialized]
    private int loopCount;

    public void Update(float time, LayerUI layerUI)
    {
        if (time >= this.StartingTime)
        {
            float timeSinceLayerStart = time - this.StartingTime;
            float timeSinceCurrentLoopStart = timeSinceLayerStart % this.TimesByLoop;

            if ((int)timeSinceLayerStart / (int) this.TimesByLoop > this.loopCount)
            {
                this.loopCount++;
                for (int index = 0; index < this.Notes.Count; index++)
                {
                    this.Notes[index].Reset();
                }
            }

            for (int index = 0; index < this.Notes.Count; index++)
            {
                this.Notes[index].Update(timeSinceCurrentLoopStart, layerUI, index);
            }
        }
    }
}