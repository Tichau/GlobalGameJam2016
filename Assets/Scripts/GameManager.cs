using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour
{
    public Camera gameCamera;
    public LayerUI InitialLayer;
    public GameObject FxPrefab;
    public GameObject FxPrefabError;
    public ParticleSystem SmokeFxLeft;
    public ParticleSystem SmokeFxRight;
    public Animator BassFx1;
    public Animator BassFx2;
    public Animator BassFx3;
    public Animator BassFx11;
    public Animator BassFx22;
    public Animator BassFx33;

    private float rotationSpeed;

    public static GameManager Instance
    {
        get;
        private set;
    }

    public bool GameEnded
    {
        get;
        private set;
    }

    public Level Level;

    [System.NonSerialized]
    public Color[] Colors = new[]
    {
        ColorUtils.FromHex("1abc9c"),
        ColorUtils.FromHex("2ecc71"),
        ColorUtils.FromHex("3498db"),
        ColorUtils.FromHex("9b59b6"),
       // ColorUtils.FromHex("34495e"),
        ColorUtils.FromHex("16a085"),
        ColorUtils.FromHex("27ae60"),
        ColorUtils.FromHex("2980b9"),
        ColorUtils.FromHex("8e44ad"),
        //ColorUtils.FromHex("2c3e50"),
        ColorUtils.FromHex("f1c40f"),
        ColorUtils.FromHex("e67e22"),
        ColorUtils.FromHex("e74c3c"),
        ColorUtils.FromHex("f39c12"),
        ColorUtils.FromHex("d35400"),
        ColorUtils.FromHex("c0392b"),
    };

    private int currentColorIndex = 0;
    private List<LayerUI> layersUI = new List<LayerUI>();

    public void ChangeBackgroundColor()
    {
        this.currentColorIndex = (this.currentColorIndex + 1) % this.Colors.Length;
        this.gameCamera.backgroundColor = this.Colors[this.currentColorIndex];
    }

    public void EndGame()
    {
        this.GameEnded = true;
        this.rotationSpeed = 0.5f;
    }

    public void StartGame()
    {
        this.Level.StartLevel(layersUI);

        for (int index = 0; index < this.layersUI.Count; index++)
        {
            //this.layersUI[index].
        }
    }

    private void Awake()
    {
        Instance = this;

        foreach (Layer layer in this.Level.Layers)
        {
            if (!layer.NeedInteraction)
            {
                continue;
            }

            GameObject newLayer = Instantiate(InitialLayer.gameObject) as GameObject;
            this.layersUI.Add(newLayer.GetComponent<LayerUI>());
            newLayer.transform.SetParent(InitialLayer.transform.parent);
            newLayer.transform.localScale = Vector3.one;

            this.layersUI[this.layersUI.Count - 1].Init(layer.Notes[0].InputKey.ToString(), 0);
            for (int i = 1; i < layer.Notes.Count; ++i)
            {
                GameObject note = Instantiate(newLayer.transform.GetChild(0).gameObject) as GameObject;
                note.transform.SetParent(newLayer.transform);
                note.transform.localPosition = Vector3.zero;
                note.transform.localScale = Vector3.one;

                this.layersUI[this.layersUI.Count - 1].Init(layer.Notes[i].InputKey.ToString(), i);
            }

            this.layersUI[this.layersUI.Count - 1].gameObject.SetActive(false);
        }

        this.InitialLayer.gameObject.SetActive(false);
    }

    internal void PlaySmokeLeft()
    {
        this.SmokeFxLeft.startColor = this.Colors[this.currentColorIndex];
        this.SmokeFxLeft.Emit(10);
    }

    internal void PlaySmokeRight()
    {
        this.SmokeFxRight.startColor = this.Colors[this.currentColorIndex];
        this.SmokeFxRight.Emit(10);
    }

    private void Update()
    {
        if (this.Level.IsStarted)
        {
            this.Level.UpdateLevel(this.FxPrefab, this.FxPrefabError);
        }

        if (this.rotationSpeed > 0f)
        {
            this.gameCamera.transform.Rotate(Vector3.forward, this.rotationSpeed);
        }
    }

    internal void EmitBassParticle(int index)
    {
        // this.BassFx.startColor = this.Colors[(this.currentColorIndex + (this.Colors.Length / 2)) % this.Colors.Length];
        switch (index)
        {
            case 0:
                this.BassFx1.SetTrigger("play");
                this.BassFx11.SetTrigger("play");
                break;
            case 1:
                this.BassFx2.SetTrigger("play");
                this.BassFx22.SetTrigger("play");
                break;
            case 2:
                this.BassFx3.SetTrigger("play");
                this.BassFx33.SetTrigger("play");
                break;

            default:
                break;
        }
    }
}
