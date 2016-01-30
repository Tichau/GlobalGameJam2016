﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public Camera gameCamera;
    public LayerUI InitialLayer;

    public static GameManager Instance
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
        ColorUtils.FromHex("34495e"),
        ColorUtils.FromHex("16a085"),
        ColorUtils.FromHex("27ae60"),
        ColorUtils.FromHex("2980b9"),
        ColorUtils.FromHex("8e44ad"),
        ColorUtils.FromHex("2c3e50"),
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

    private void Awake()
    {
        Instance = this;

        foreach (var layer in this.Level.Layers)
        {
            GameObject newLayer = Instantiate(InitialLayer.gameObject) as GameObject;
            this.layersUI.Add(newLayer.GetComponent<LayerUI>());
            newLayer.transform.SetParent(InitialLayer.transform.parent);
            newLayer.transform.localScale = Vector3.one;

            for (int i = 1; i < layer.Notes.Count; ++i)
            {
                GameObject note = Instantiate(newLayer.transform.GetChild(0).gameObject) as GameObject;
                note.transform.SetParent(newLayer.transform);
                note.transform.localScale = Vector3.one;
            }
        }

        this.InitialLayer.gameObject.SetActive(false);
    }

	private void Start ()
	{
	    this.Level.StartLevel(layersUI);
	}
	
	private void Update ()
	{
	    this.Level.UpdateLevel();
	}
}
