using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
}
