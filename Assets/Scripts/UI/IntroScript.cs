using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IntroScript : MonoBehaviour
{
    public float FadeDuration;

    public Text TextObject;

    private Image titleBackground;
    private float fadeStart = float.NaN;
    private float startBackgroundAlpha;
    private float startTextAlpha;

    private void Start()
    {
        this.titleBackground = this.GetComponent<Image>();
        this.startBackgroundAlpha = this.titleBackground.color.a;
        this.startTextAlpha = this.TextObject.color.a;
    }

    private void Update()
    {
        if (Input.anyKey)
        {
            this.StartFadeOut();
        }

        if (!float.IsNaN(this.fadeStart))
        {
            float remainingTime = this.FadeDuration - (Time.time - this.fadeStart);

            if (remainingTime <= float.Epsilon)
            {
                this.gameObject.SetActive(false);
                GameManager.Instance.StartGame();
            }
            else
            {
                float ratio = remainingTime / this.FadeDuration;
                this.titleBackground.color = new Color(this.titleBackground.color.r, this.titleBackground.color.g, this.titleBackground.color.b, this.startBackgroundAlpha * ratio);
                this.TextObject.color = new Color(this.TextObject.color.r, this.TextObject.color.g, this.TextObject.color.b, this.startTextAlpha * ratio);
            }
        }
	}

    private void StartFadeOut()
    {
        if (float.IsNaN(this.fadeStart))
        {
            this.fadeStart = Time.time;
        }
    }
}
