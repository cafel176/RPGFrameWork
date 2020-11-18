using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//列表模板
public class myList<T> : ScriptableObject
{
    public List<T> list;
}

[CustomEditor(typeof(intList))]
public class intInspector : Editor
{
    private int index = 0;

    public override void OnInspectorGUI()
    {
        // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
        serializedObject.Update();

        var list = ((intList)target).list;
        GUILayout.Label("edit list, total num: " + list.Count);
        for (int i = 0; i < list.Count; i++)
        {
            GUILayout.Space(10);
            var e = list[i];
            if (e != null)
            {
                GUILayout.Label("element " + i+":");
                e.key = EditorGUILayout.TextField("key", e.key);
                e.value = EditorGUILayout.IntField("value", e.value);
            }
        }

        GUILayout.Space(10);
        if (GUILayout.Button("Add Element"))
        {
            list.Add(new VarIntDictionary());
        }
        EditorGUILayout.BeginHorizontal();
        index = EditorGUILayout.IntField("delete N0.", index);
        if (GUILayout.Button("delete"))
        {
            if (list.Count > 0)
                list.RemoveAt(index);
        }
        EditorGUILayout.EndHorizontal();


        // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
        //需要在OnInspectorGUI之前修改属性，否则无法修改值
        serializedObject.ApplyModifiedProperties();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}

[CustomEditor(typeof(floatList))]
public class floatInspector : Editor
{
    private int index = 0;

    public override void OnInspectorGUI()
    {
        // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
        serializedObject.Update();

        var list = ((floatList)target).list;
        GUILayout.Label("edit list, total num: " + list.Count);
        for (int i = 0; i < list.Count; i++)
        {
            GUILayout.Space(10);
            var e = list[i];
            if (e != null)
            {
                GUILayout.Label("element " + i + ":");
                e.key = EditorGUILayout.TextField("key", e.key);
                e.value = EditorGUILayout.FloatField("value", e.value);
            }
        }

        GUILayout.Space(10);
        if (GUILayout.Button("Add Element"))
        {
            list.Add(new VarFloatDictionary());
        }
        EditorGUILayout.BeginHorizontal();
        index = EditorGUILayout.IntField("delete N0.", index);
        if (GUILayout.Button("delete"))
        {
            if (list.Count > 0)
                list.RemoveAt(index);
        }
        EditorGUILayout.EndHorizontal();

        // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
        //需要在OnInspectorGUI之前修改属性，否则无法修改值
        serializedObject.ApplyModifiedProperties();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}

[CustomEditor(typeof(switchList))]
public class switchInspector : Editor
{
    private int index = 0;

    public override void OnInspectorGUI()
    {
        // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
        serializedObject.Update();

        var list = ((switchList)target).list;
        GUILayout.Label("edit list, total num: " + list.Count);
        for (int i = 0; i < list.Count; i++)
        {
            GUILayout.Space(10);
            var e = list[i];
            if (e != null)
            {
                GUILayout.Label("element " + i + ":");
                e.key = EditorGUILayout.TextField("key", e.key);
                e.value = EditorGUILayout.Toggle("value", e.value);
            }
        }

        GUILayout.Space(10);
        if (GUILayout.Button("Add Element"))
        {
            list.Add(new switchDictionary());
        }
        EditorGUILayout.BeginHorizontal();
        index = EditorGUILayout.IntField("delete N0.", index);
        if (GUILayout.Button("delete"))
        {
            if (list.Count > 0)
                list.RemoveAt(index);
        }
        EditorGUILayout.EndHorizontal();

        // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
        //需要在OnInspectorGUI之前修改属性，否则无法修改值
        serializedObject.ApplyModifiedProperties();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}

[CustomEditor(typeof(textList))]
public class textInspector : Editor
{
    private int index = 0;

    public override void OnInspectorGUI()
    {
        // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
        serializedObject.Update();

        var list = ((textList)target).list;
        GUILayout.Label("edit list, total num: " + list.Count);
        for (int i = 0; i < list.Count; i++)
        {
            GUILayout.Space(10);
            var e = list[i];
            if (e != null)
            {
                GUILayout.Label("element " + i + ":");
                e.key = EditorGUILayout.TextField("key", e.key);
                GUILayout.Label("chinese text");
                e.chnText = EditorGUILayout.TextArea(e.chnText, GUILayout.Height(50));
                GUILayout.Label("english text");
                e.engText = EditorGUILayout.TextArea(e.engText, GUILayout.Height(50));
            }
        }

        GUILayout.Space(10);
        if (GUILayout.Button("Add Element"))
        {
            list.Add(new stringDictionary());
        }
        EditorGUILayout.BeginHorizontal();
        index = EditorGUILayout.IntField("delete N0.", index);
        if (GUILayout.Button("delete"))
        {
            if (list.Count > 0)
                list.RemoveAt(index);
        }
        EditorGUILayout.EndHorizontal();

        // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
        //需要在OnInspectorGUI之前修改属性，否则无法修改值
        serializedObject.ApplyModifiedProperties();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}

[CustomEditor(typeof(ImageList))]
public class ImgInspector : Editor
{
    private int index = 0;

    public override void OnInspectorGUI()
    {
        // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
        serializedObject.Update();

        var list = ((ImageList)target).list;
        GUILayout.Label("edit list, total num: " + list.Count);
        for (int i = 0; i < list.Count; i++)
        {
            GUILayout.Space(10);
            var e = list[i];
            if (e != null)
            {
                GUILayout.Label("element " + i + ":");
                e.key = EditorGUILayout.TextField("key", e.key);
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("image");
                e.img = (Sprite)EditorGUILayout.ObjectField(e.img, typeof(Sprite));
                EditorGUILayout.EndHorizontal();
            }
        }

        GUILayout.Space(10);
        if (GUILayout.Button("Add Element"))
        {
            list.Add(new imageDictionary());
        }
        EditorGUILayout.BeginHorizontal();
        index = EditorGUILayout.IntField("delete N0.", index);
        if (GUILayout.Button("delete"))
        {
            if (list.Count > 0)
                list.RemoveAt(index);
        }
        EditorGUILayout.EndHorizontal();

        // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
        //需要在OnInspectorGUI之前修改属性，否则无法修改值
        serializedObject.ApplyModifiedProperties();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}

[CustomEditor(typeof(itemListDefine))]
public class itemInspector : Editor
{
    private int index = 0;

    public override void OnInspectorGUI()
    {
        // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
        serializedObject.Update();

        var list = ((itemListDefine)target).list;
        GUILayout.Label("edit list, total num: " + list.Count);
        for (int i = 0; i < list.Count; i++)
        {
            GUILayout.Space(10);
            var e = list[i];
            if (e != null)
            {
                GUILayout.Label("element " + i + ":");
                e.id = EditorGUILayout.IntField("id", e.id);
                e.type = (itemType)EditorGUILayout.EnumPopup("type", e.type);
                e._name = EditorGUILayout.TextField("name", e._name);
                e.text = EditorGUILayout.TextField("text", e.text);
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("image");
                e.img = (Sprite)EditorGUILayout.ObjectField(e.img, typeof(Sprite));
                EditorGUILayout.EndHorizontal();
                e.times = EditorGUILayout.IntField("times", e.times);
            }
        }

        GUILayout.Space(10);
        if (GUILayout.Button("Add Element"))
        {
            list.Add(new item());
        }
        EditorGUILayout.BeginHorizontal();
        index = EditorGUILayout.IntField("delete N0.", index);
        if (GUILayout.Button("delete"))
        {
            if (list.Count > 0)
                list.RemoveAt(index);
        }
        EditorGUILayout.EndHorizontal();

        // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
        //需要在OnInspectorGUI之前修改属性，否则无法修改值
        serializedObject.ApplyModifiedProperties();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}