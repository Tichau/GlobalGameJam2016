using UnityEngine;
using UnityEngine.UI;

public class UILayerControl : MonoBehaviour
{
    public float FadeDuration;
    private Text[] textObjects;
    private Image[] textBackgrounds;
    private float fadeStart = float.NaN;
    private bool visible = false;

    public void SetVisible(bool visible)
    {
        this.visible = visible;
        this.fadeStart = Time.time;
    }

	private void Start ()
	{
	    this.textBackgrounds = this.GetComponentsInChildren<Image>();
	    this.textObjects = this.GetComponentsInChildren<Text>();
	}
	
	private void Update ()
    {
        //if (!float.IsNaN(this.fadeStart))
        //{
        //    float remainingTime = this.FadeDuration - (Time.time - this.fadeStart);
        //    float ratio = remainingTime / this.FadeDuration;

        //    for (int index = 0; index < this.textBackgrounds.Length; index++)
        //    {
        //        Image image = this.textBackgrounds[index];
        //        image.color = new Color(image.color.r, image.color.g, image.color.b, ratio);
        //    }

        //    for (int index = 0; index < this.textObjects.Length; index++)
        //    {
        //        Image image = this.textBackgrounds[index];
        //        image.color = new Color(image.color.r, image.color.g, image.color.b, ratio);
        //    }
        //}
    }
}
