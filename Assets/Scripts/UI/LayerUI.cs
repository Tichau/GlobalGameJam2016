using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class LayerUI : MonoBehaviour
{
    public void Init(string key, int noteIndex)
    {
        this.transform.GetChild(noteIndex).gameObject.GetComponentInChildren<Text>().text = key;
    }

    public void DisplayInputKey(int noteIndex)
    {
        this.transform.GetChild(noteIndex).GetComponent<Animator>().SetTrigger("display");
    }

    public void DisplayFx(int noteIndex, float fxPlacement, GameObject fx)
    {
        var particleSystem = Instantiate(fx) as GameObject;
        particleSystem.transform.parent = this.transform.GetChild(noteIndex).transform;
        particleSystem.transform.localPosition = new Vector3( 75f * fxPlacement, 0f, -20f);
        particleSystem.transform.localScale = new Vector3(1.5f, 1, 1.5f);
    }
}
