using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class HashsAndTags : MonoBehaviour
{
    //tags
    private static string[] tags;

    [Header("自定义的tags")]
    public static string player = "Player";
    public static string mainCamera = "MainCamera";
    public static string specialText = "specialText";

    // Layers
    private static string[] layers;

    [Header("自定义的layers")]
    public static string npc = "npc";
    public static string obj = "object";
    public static string follow = "follow";
    public static string Collider = "collider";

    // Sorting Layers
    private static string[] sortingLayers;

    [Header("自定义的sortingLayers")]
    public static string character = "character";
    public static string front = "front";

    // hashs
    [Header("自定义的hashs")]
    public string doOnce = "do";
    public string doOnce2 = "do2", doOnce3 = "do3", back = "back", speed = "speed";
    public string Xspeed = "Xspeed", YspeedUp = "YspeedUp", YspeedDown = "YspeedDown",
        runX = "runX", runY = "runY", idle = "back", idleAnima = "idle", rushX = "rushX", rushY = "rushY";
    public string start = "Start", end = "End";

    [MenuItem("工具/初始化")]
    public static void loadSettings()
    {
        tags = new string[] { specialText };
        for (int i = 0; i < tags.Length; i++)
        {
            AddTagIfNotHas(tags[i]);
        }
        Debug.Log("tags自动添加完成。");

        layers = new string[] { player, npc, obj, follow, Collider };
        for (int i = 0; i < layers.Length; i++)
        {
            AddLayerIfNotHas(layers[i]);
        }
        Debug.Log("layers自动添加完成。");

        sortingLayers = new string[] { obj, character, front };
        for (int i = 0; i < sortingLayers.Length; i++)
        {
            AddSortingLayerIfNotHas(sortingLayers[i]);
        }
        Debug.Log("sortingLayers自动添加完成。");

        //设置层碰撞关系
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(player), LayerMask.NameToLayer(player), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(player), LayerMask.NameToLayer(follow), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(follow), LayerMask.NameToLayer(follow), true);
        Debug.Log("layers碰撞关系自动设置完成。");

        loadAllScenes();
        Debug.Log("相关场景添加到 Build Setting 已完成。");
    }

    public static void loadAllScenes()
    {
        string[] scenePath = { @"/Engine/Scenes/" };

        List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>();
        for (int i = 0; i < scenePath.Length; i++)
        {
            string path = Application.dataPath + scenePath[i];
            string[] files = Directory.GetFiles(path, "*.unity", SearchOption.AllDirectories);

            for (int j = 0; j < files.Length; ++j)
            {
                string scenepath = files[j].Substring(files[j].IndexOf("Assets"));
                scenes.Add(new EditorBuildSettingsScene(scenepath, true));
            }
        }

        if (scenes.Count > 0)
        {
            EditorBuildSettings.scenes = scenes.ToArray();
        }
    }

    public static void AddTagIfNotHas(string tag)
    {
        if (!isHasTag(tag))
        {
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty it = tagManager.GetIterator();
            while (it.NextVisible(true))
            {
                if (it.name == "tags")
                {
                    it.InsertArrayElementAtIndex(it.arraySize);
                    SerializedProperty dataPoint = it.GetArrayElementAtIndex(it.arraySize - 1);
                    dataPoint.stringValue = tag;
                    tagManager.ApplyModifiedProperties();
                    return;
                }
            }
        }
    }

    public static void AddSortingLayerIfNotHas(string sortingLayer)
    {
        if (!isHasSortingLayer(sortingLayer))
        {
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty it = tagManager.GetIterator();
            while (it.NextVisible(true))
            {
                if (it.name == "m_SortingLayers")
                {
                    it.InsertArrayElementAtIndex(it.arraySize);
                    SerializedProperty dataPoint = it.GetArrayElementAtIndex(it.arraySize - 1);
                    while (dataPoint.NextVisible(true))
                    {
                        if (dataPoint.name == "name")
                        {
                            dataPoint.stringValue = sortingLayer;
                            tagManager.ApplyModifiedProperties();
                            return;
                        }
                    }
                }
            }
        }
    }

    public static void AddLayerIfNotHas(string layer)
    {
        if (!isHasLayer(layer))
        {
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty it = tagManager.GetIterator();
            while (it.NextVisible(true))
            {
                if (it.name == "layers")
                {
                    for (int i = 0; i < it.arraySize; i++)
                    {
                        if (i == 3 || i == 6 || i == 7) continue;
                        SerializedProperty dataPoint = it.GetArrayElementAtIndex(i);
                        if (string.IsNullOrEmpty(dataPoint.stringValue))
                        {
                            dataPoint.stringValue = layer;
                            tagManager.ApplyModifiedProperties();
                            return;
                        }
                    }
                }
            }
        }
    }

    public static bool isHasTag(string tag)
    {
        for (int i = 0; i < UnityEditorInternal.InternalEditorUtility.tags.Length; i++)
        {
            if (UnityEditorInternal.InternalEditorUtility.tags[i].Contains(tag))
                return true;
        }
        return false;
    }

    public static bool isHasSortingLayer(string sortingLayer)
    {
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty it = tagManager.GetIterator();
        while (it.NextVisible(true))
        {
            if (it.name == "m_SortingLayers")
            {
                for (int i = 0; i < it.arraySize; i++)
                {
                    SerializedProperty dataPoint = it.GetArrayElementAtIndex(i);
                    while (dataPoint.NextVisible(true))
                    {
                        if (dataPoint.name == "name")
                        {
                            if (dataPoint.stringValue == sortingLayer) return true;
                        }
                    }
                }
            }
        }

        return false;
    }

    public static bool isHasLayer(string layer)
    {
        for (int i = 0; i < UnityEditorInternal.InternalEditorUtility.layers.Length; i++)
        {
            if (UnityEditorInternal.InternalEditorUtility.layers[i].Contains(layer))
                return true;
        }
        return false;
    }

}
