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

    [System.NonSerialized]
    public bool IsValid = false;

    public void Update(float time, LayerUI layerUI)
    {
        if (time >= this.StartingTime)
        {
            float timeSinceLayerStart = time - this.StartingTime;
            float timeSinceCurrentLoopStart = timeSinceLayerStart % this.TimesByLoop;

            if ((int)timeSinceLayerStart / (int) this.TimesByLoop > this.loopCount)
            {
                this.IsValid = true;
                for (int index = 0; index < this.Notes.Count; index++)
                {
                    this.IsValid &= this.Notes[index].Accuracy > 0.5f;
                }

                Debug.Log("Layer: " + this.Name + " IsValid: " + this.IsValid);

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