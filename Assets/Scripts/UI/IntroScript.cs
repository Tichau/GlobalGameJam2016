using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IntroScript : MonoBehaviour
{
    public float FadeDuration;

    private Text[] textObjects;
    private Image titleBackground;
    private float fadeStart = float.NaN;
    private float startBackgroundAlpha;
    private float startTextAlpha;

    private void Start()
    {
        this.textObjects = this.GetComponentsInChildren<Text>();
        this.titleBackground = this.GetComponent<Image>();
        this.startBackgroundAlpha = this.titleBackground.color.a;
        this.startTextAlpha = 1;
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

                for (int index = 0; index < this.textObjects.Length; index++)
                {
                    Text text = this.textObjects[index];
                    text.color = new Color(text.color.r, text.color.g, text.color.b, this.startTextAlpha * ratio);
                }
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
