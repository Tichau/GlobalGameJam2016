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
}
