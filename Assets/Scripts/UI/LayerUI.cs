using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LayerUI : MonoBehaviour
{
    public void DisplayInputKey(string key, int noteIndex)
    {
        this.transform.GetChild(noteIndex).gameObject.GetComponentInChildren<Text>().text = key;
        this.transform.GetChild(noteIndex).GetComponent<Animator>().SetTrigger("display");
    }
}
