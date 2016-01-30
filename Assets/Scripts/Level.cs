using UnityEngine;
using System.Collections.Generic;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Level : ScriptableObject
{
    [Tooltip("Battements par minute.")]
    public float Bpm;
    public List<Layer> Layers;

    [System.NonSerialized]
    private float startTime;
    [System.NonSerialized]
    private List<LayerUI> LayersUI;

#if UNITY_EDITOR
    [MenuItem("Assets/Create/Level")]
    public static void Create()
    {
        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath("Assets/Data/Levels/NewLevel.asset");

        Level asset = ScriptableObject.CreateInstance<Level>();
        AssetDatabase.CreateAsset(asset, assetPathAndName);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Selection.activeObject = asset;
        EditorUtility.FocusProjectWindow();
    }
#endif

    public void StartLevel(List<LayerUI> layersUI)
    {
        this.startTime = Time.time;
        this.LayersUI = layersUI;
    }

    public void UpdateLevel()
    {
        float timeDuration = 1 / (this.Bpm / 60);
        float time = (Time.time - this.startTime) / timeDuration;

        for (int index = 0; index < this.Layers.Count; index++)
        {
            this.Layers[index].Update(time, this.LayersUI[index]);
        }
    }
}
