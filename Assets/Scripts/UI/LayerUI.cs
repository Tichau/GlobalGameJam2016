using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LayerUI : MonoBehaviour
{
    public Text InputKeyText;
    public GameObject InputKey;

    public void DisplayInputKey(string key)
    {
        this.InputKeyText.text = key;
        this.InputKey.GetComponent<Animator>().SetTrigger("display");
    }
}
