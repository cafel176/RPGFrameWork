using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(eventListDefine))]
public class onceEventInspector : Editor
{
    Editor cacheEditor;
    int index = 0;

    public override void OnInspectorGUI()
    {
        // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
        serializedObject.Update();

        //显示eventlist的默认UI
        base.OnInspectorGUI();

        GUILayout.Space(20);
        var list = ((eventListDefine)target).list;
        GUILayout.Label("edit event list, total num: "+ list.Count);

        GUILayout.Space(10);
        for (int i=0;i<list.Count;i++)
        {
            GUILayout.Space(10);
            var e = list[i];
            if (e != null)
            {
                //创建event的Editor
                GUILayout.Label("event "+i+":");
                e.type = (eventType)EditorGUILayout.EnumPopup("type", e.type);
                if (e.type == eventType.text)
                {
                    e._name = EditorGUILayout.TextField("name",e._name);
                    e.text = EditorGUILayout.TextField("text", e.text);
                    e.sp = EditorGUILayout.TextField("face", e.sp);
                    e.showFace = EditorGUILayout.Toggle("show face", e.showFace);
                }
                else if (e.type == eventType.beBlack || e.type == eventType.beWhite || e.type == eventType.wait)
                {
                    e.time = EditorGUILayout.FloatField("time", e.time);
                }
                else if (e.type == eventType.changeGlobalInt)
                {
                    e.key = EditorGUILayout.TextField("key", e.key);
                    e.howToCal = (calValue)EditorGUILayout.EnumPopup("howToCal", e.howToCal);
                    e.globalInt = EditorGUILayout.IntField("value", e.globalInt);
                }
                else if (e.type == eventType.changeGlobalFloat)
                {
                    e.key = EditorGUILayout.TextField("key", e.key);
                    e.howToCal = (calValue)EditorGUILayout.EnumPopup("howToCal", e.howToCal);
                    e.globalFloat = EditorGUILayout.FloatField("value", e.globalFloat);
                }
                else if (e.type == eventType.changeGlobalSwith)
                {
                    e.key = EditorGUILayout.TextField("key", e.key);
                    e.globalSwitch = EditorGUILayout.Toggle("switch", e.globalSwitch);
                }
            }
        }

        GUILayout.Space(20);
        if (GUILayout.Button("Add Event"))
        {
            list.Add(new myEvent());
        }
        EditorGUILayout.BeginHorizontal();
        index = EditorGUILayout.IntField("delete N0.", index);
        if (GUILayout.Button("delete"))
        {
            if (list.Count > 0)
                list.RemoveAt(index);
        }
        EditorGUILayout.EndHorizontal();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }

        serializedObject.ApplyModifiedProperties();
    }
}

