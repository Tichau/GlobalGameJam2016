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
    private float startTime = float.NaN;
    [System.NonSerialized]
    private List<LayerUI> LayersUI;

    public bool IsStarted
    {
        get { return !float.IsNaN(startTime); }
    }

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

    public void UpdateLevel(GameObject fx, GameObject fxError)
    {
        float timeDuration = 1 / (this.Bpm / 60);
        float time = (Time.time - this.startTime) / timeDuration;

        for (int index = 0; index < this.Layers.Count; index++)
        {
            Layer layer = this.Layers[index];
            if (!layer.IsPlaying)
            {
                if (index < this.LayersUI.Count)
                {
                    this.LayersUI[index].GetComponent<UILayerControl>().SetVisible(true);
                }

                layer.StartLayer(time);
            }

            layer.UpdateLayer(time, index < this.LayersUI.Count ? this.LayersUI[index] : null, fx, fxError);

            if (!layer.IsValid && !GameManager.Instance.GameEnded)
            {
                for (int index2 = index + 1; index2 < this.Layers.Count; index2++)
                {
                    this.Layers[index2].StopLayer();
                    if (index2 < this.LayersUI.Count)
                    {
                        this.LayersUI[index2].GetComponent<UILayerControl>().SetVisible(false);
                    }
                }

                break;
            }
        }
    }
}
