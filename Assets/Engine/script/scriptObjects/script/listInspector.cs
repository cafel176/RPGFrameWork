using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//列表模板
public class myList<T> : ScriptableObject
{
    public List<T> list;
}

#if UNITY_EDITOR

[CustomEditor(typeof(intList))]
public class intInspector : Editor
{
    private int index = 0;
    private int showPage = 1;
    private int showNum = 50;

    public override void OnInspectorGUI()
    {
        // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
        serializedObject.Update();

        var list = ((intList)target).list;
        int l = 0;
        if (list != null)
            l = list.Count;

        if (GUILayout.Button("添加元素"))
        {
            list.Insert(0, new VarIntDictionary());
        }
        EditorGUILayout.BeginHorizontal();
        index = EditorGUILayout.IntField("删除元素，编号: ", index);
        if (GUILayout.Button("确认删除"))
        {
            if (l > 0)
                list.RemoveAt(index);
        }
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(10);

        GUILayout.Label("编辑列表，元素总数: " + l);
        int num = l > showPage * showNum ? showPage * showNum : l;
        for (int i = 0; i < num; i++)
        {
            GUILayout.Space(10);
            var e = list[i];
            if (e != null)
            {
                GUILayout.Label("元素 " + i+":");
                e.key = EditorGUILayout.TextField("变量名", e.key);
                e.value = EditorGUILayout.IntField("值", e.value);
            }
        }

        if (l > showPage * showNum)
        {
            GUILayout.Space(10);
            if (GUILayout.Button("显示更多"))
            {
                showPage++;
                OnInspectorGUI();
                return;
            }
        }

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
    private int showPage = 1;
    private int showNum = 50;

    public override void OnInspectorGUI()
    {
        // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
        serializedObject.Update();

        var list = ((floatList)target).list;
        int l = 0;
        if (list != null)
            l = list.Count;

        if (GUILayout.Button("添加元素"))
        {
            list.Insert(0, new VarFloatDictionary());
        }
        EditorGUILayout.BeginHorizontal();
        index = EditorGUILayout.IntField("删除元素，编号: ", index);
        if (GUILayout.Button("确认删除"))
        {
            if (l > 0)
                list.RemoveAt(index);
        }
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(10);

        GUILayout.Label("编辑列表，元素总数: " + l);
        int num = l > showPage * showNum ? showPage * showNum : l;
        for (int i = 0; i < num; i++)
        {
            GUILayout.Space(10);
            var e = list[i];
            if (e != null)
            {
                GUILayout.Label("元素 " + i + ":");
                e.key = EditorGUILayout.TextField("变量名", e.key);
                e.value = EditorGUILayout.FloatField("值", e.value);
            }
        }

        if (l > showPage * showNum)
        {
            GUILayout.Space(10);
            if (GUILayout.Button("显示更多"))
            {
                showPage++;
                OnInspectorGUI();
                return;
            }
        }

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
    private int showPage = 1;
    private int showNum = 50;

    public override void OnInspectorGUI()
    {
        // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
        serializedObject.Update();

        var list = ((switchList)target).list;
        int l = 0;
        if (list != null)
            l = list.Count;

        if (GUILayout.Button("添加元素"))
        {
            list.Insert(0, new switchDictionary());
        }
        EditorGUILayout.BeginHorizontal();
        index = EditorGUILayout.IntField("删除元素，编号: ", index);
        if (GUILayout.Button("确认删除"))
        {
            if (l > 0)
                list.RemoveAt(index);
        }
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(10);

        GUILayout.Label("编辑列表，元素总数: " + l);
        int num = l > showPage * showNum ? showPage * showNum : l;
        for (int i = 0; i < num; i++)
        {
            GUILayout.Space(10);
            var e = list[i];
            if (e != null)
            {
                GUILayout.Label("元素 " + i + ":");
                e.key = EditorGUILayout.TextField("变量名", e.key);
                e.value = EditorGUILayout.Toggle("值", e.value);
            }
        }

        if (l > showPage * showNum)
        {
            GUILayout.Space(10);
            if (GUILayout.Button("显示更多"))
            {
                showPage++;
                OnInspectorGUI();
                return;
            }
        }

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
    private int showPage = 1;
    private int showNum = 50;

    public override void OnInspectorGUI()
    {
        // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
        serializedObject.Update();

        var list = ((textList)target).list;
        int l = 0;
        if (list != null)
            l = list.Count;

        if (GUILayout.Button("添加元素"))
        {
            list.Insert(0, new stringDictionary());
        }
        EditorGUILayout.BeginHorizontal();
        index = EditorGUILayout.IntField("删除元素，编号: ", index);
        if (GUILayout.Button("确认删除"))
        {
            if (l > 0)
                list.RemoveAt(index);
        }
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(10);

        GUILayout.Label("编辑列表，元素总数: " + l);
        int num = l > showPage*showNum ? showPage * showNum : l;
        for (int i = 0; i < num; i++)
        {
            GUILayout.Space(10);
            var e = list[i];
            if (e != null)
            {
                GUILayout.Label("元素 " + i + ":");
                e.key = EditorGUILayout.TextField("文本名", e.key);
                GUILayout.Label("中文翻译");
                e.cnText = EditorGUILayout.TextArea(e.cnText, GUILayout.Height(40), GUILayout.Width(300));
                GUILayout.Label("English text");
                e.enText = EditorGUILayout.TextArea(e.enText, GUILayout.Height(40), GUILayout.Width(300));
                GUILayout.Label("日本語の翻訳");
                e.jpText = EditorGUILayout.TextArea(e.jpText, GUILayout.Height(40), GUILayout.Width(300));
            }
        }

        if(l > showPage * showNum)
        {
            GUILayout.Space(10);
            if (GUILayout.Button("显示更多"))
            {
                showPage++;
                OnInspectorGUI();
                return;
            }
        }

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
    private int showPage = 1;
    private int showNum = 50;

    public override void OnInspectorGUI()
    {
        // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
        serializedObject.Update();

        var list = ((ImageList)target).list;
        int l = 0;
        if (list != null)
            l = list.Count;

        if (GUILayout.Button("添加元素"))
        {
            list.Insert(0, new imageDictionary());
        }
        EditorGUILayout.BeginHorizontal();
        index = EditorGUILayout.IntField("删除元素，编号: ", index);
        if (GUILayout.Button("确认删除"))
        {
            if (l > 0)
                list.RemoveAt(index);
        }
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(10);

        GUILayout.Label("编辑列表，元素总数: " + l);
        int num = l > showPage * showNum ? showPage * showNum : l;
        for (int i = 0; i < num; i++)
        {
            GUILayout.Space(10);
            var e = list[i];
            if (e != null)
            {
                GUILayout.Label("元素 " + i + ":");
                e.key = EditorGUILayout.TextField("图片名", e.key);
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("图片");
                e.img = (Sprite)EditorGUILayout.ObjectField(e.img, typeof(Sprite));
                EditorGUILayout.EndHorizontal();
            }
        }

        if (l > showPage * showNum)
        {
            GUILayout.Space(10);
            if (GUILayout.Button("显示更多"))
            {
                showPage++;
                OnInspectorGUI();
                return;
            }
        }


        // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
        //需要在OnInspectorGUI之前修改属性，否则无法修改值
        serializedObject.ApplyModifiedProperties();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}

[CustomEditor(typeof(AudioList))]
public class AudioInspector : Editor
{
    private int index = 0;
    private int showPage = 1;
    private int showNum = 50;

    public override void OnInspectorGUI()
    {
        // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
        serializedObject.Update();

        var list = ((AudioList)target).list;
        int l = 0;
        if (list != null)
            l = list.Count;

        if (GUILayout.Button("添加元素"))
        {
            list.Insert(0, new audioDictionary());
        }
        EditorGUILayout.BeginHorizontal();
        index = EditorGUILayout.IntField("删除元素，编号: ", index);
        if (GUILayout.Button("确认删除"))
        {
            if (l > 0)
                list.RemoveAt(index);
        }
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(10);

        GUILayout.Label("编辑列表，元素总数: " + l);
        int num = l > showPage * showNum ? showPage * showNum : l;
        for (int i = 0; i < num; i++)
        {
            GUILayout.Space(10);
            var e = list[i];
            if (e != null)
            {
                GUILayout.Label("元素 " + i + ":");
                e.key = EditorGUILayout.TextField("音乐/音效名", e.key);
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("音乐/音效");
                e.audio = (AudioClip)EditorGUILayout.ObjectField(e.audio, typeof(AudioClip));
                EditorGUILayout.EndHorizontal();
                e.pitch = EditorGUILayout.FloatField("音调", e.pitch);
            }
        }

        if (l > showPage * showNum)
        {
            GUILayout.Space(10);
            if (GUILayout.Button("显示更多"))
            {
                showPage++;
                OnInspectorGUI();
                return;
            }
        }

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
    private int showPage = 1;
    private int showNum = 50;

    public override void OnInspectorGUI()
    {
        // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
        serializedObject.Update();

        var list = ((itemListDefine)target).list;
        int l = 0;
        if (list != null)
            l = list.Count;

        if (GUILayout.Button("添加元素"))
        {
            list.Insert(0, new item());
        }
        EditorGUILayout.BeginHorizontal();
        index = EditorGUILayout.IntField("删除元素，编号: ", index);
        if (GUILayout.Button("确认删除"))
        {
            if (l > 0)
                list.RemoveAt(index);
        }
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(10);

        GUILayout.Label("编辑列表，元素总数: " + l);
        int n = l > showPage * showNum ? showPage * showNum : l;
        for (int i = 0; i < n; i++)
        {
            GUILayout.Space(10);
            var e = list[i];
            if (e != null)
            {
                GUILayout.Label("元素 " + i + ":");
                e.id = EditorGUILayout.IntField("道具id", e.id);
                e.type = (itemType)EditorGUILayout.EnumPopup("道具类型", e.type);
                e._name = EditorGUILayout.TextField("道具名", e._name);
                e.text = EditorGUILayout.TextField("描述文字", e.text);
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("图片");
                e.img = (Sprite)EditorGUILayout.ObjectField(e.img, typeof(Sprite));
                EditorGUILayout.EndHorizontal();
                e.times = EditorGUILayout.IntField("使用次数", e.times);

                EditorGUILayout.BeginHorizontal();
                e.eventNum = EditorGUILayout.IntField("事件总数:", e.eventNum);
                bool b = GUILayout.Button("确定");
                EditorGUILayout.EndHorizontal();

                int num = 0;
                if (e.commonEvent != null)
                    num = e.commonEvent.Count;

                if (b)
                {
                    if (e.eventNum > num)
                    {
                        for (int w = num; w < e.eventNum; w++)
                        {
                            e.commonEvent.Add("");
                        }
                    }
                    else
                    {
                        for (int w = e.eventNum; w < num;)
                        {
                            e.commonEvent.RemoveAt(w);
                            num = e.commonEvent.Count;
                        }
                    }
                }

                EditorGUILayout.BeginVertical();
                for (int w = 0; w < num; w++)
                {
                    e.commonEvent[w] = EditorGUILayout.TextField("公共事件 " + w, e.commonEvent[w]);
                }
                EditorGUILayout.EndVertical();
            }
        }

        if (l > showPage * showNum)
        {
            GUILayout.Space(10);
            if (GUILayout.Button("显示更多"))
            {
                showPage++;
                OnInspectorGUI();
                return;
            }
        }

        // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
        //需要在OnInspectorGUI之前修改属性，否则无法修改值
        serializedObject.ApplyModifiedProperties();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}

[CustomEditor(typeof(questListDefine))]
public class questInspector : Editor
{
    private int index = 0;
    private int showPage = 1;
    private int showNum = 50;

    public override void OnInspectorGUI()
    {
        // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
        serializedObject.Update();

        var list = ((questListDefine)target).list;
        int l = 0;
        if (list != null)
            l = list.Count;

        if (GUILayout.Button("添加元素"))
        {
            list.Insert(0, new quest());
        }
        EditorGUILayout.BeginHorizontal();
        index = EditorGUILayout.IntField("删除元素，编号: ", index);
        if (GUILayout.Button("确认删除"))
        {
            if (l > 0)
                list.RemoveAt(index);
        }
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(10);

        GUILayout.Label("编辑列表，元素总数: " + l);
        int n = l > showPage * showNum ? showPage * showNum : l;
        for (int i = 0; i < n; i++)
        {
            GUILayout.Space(10);
            var e = list[i];
            if (e != null)
            {
                GUILayout.Label("元素 " + i + ":");
                e.id = EditorGUILayout.IntField("任务id", e.id);
                e.status = (questStatus)EditorGUILayout.EnumPopup("任务状态", e.status);
                e.type = (questType)EditorGUILayout.EnumPopup("任务类型", e.type);
                e.name = EditorGUILayout.TextField("名字", e.name);
                e.hard = EditorGUILayout.TextField("难度", e.hard);
                e.text = EditorGUILayout.TextField("描述文字", e.text);

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("图片");
                e.icon = (Sprite)EditorGUILayout.ObjectField(e.icon, typeof(Sprite));
                EditorGUILayout.EndHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.BeginHorizontal();
                e.stepNum = EditorGUILayout.IntField("步骤总数:", e.stepNum);
                bool b = GUILayout.Button("确定");
                EditorGUILayout.EndHorizontal();

                int num = 0;
                if (e.steps != null)
                    num = e.steps.Count;

                if (b)
                {
                    if (e.stepNum > num)
                    {
                        for (int w = num; w < e.stepNum; w++)
                        {
                            e.steps.Add("");
                        }
                    }
                    else
                    {
                        for (int w = e.stepNum; w < num; )
                        {
                            e.steps.RemoveAt(w);
                            num = e.steps.Count;
                        }
                    }
                }

                EditorGUILayout.BeginVertical();
                for (int w = 0; w < num; w++)
                {
                    e.steps[w] = EditorGUILayout.TextField("步骤 "+w, e.steps[w]);
                }
                EditorGUILayout.EndVertical();
            }
        }

        if (l > showPage * showNum)
        {
            GUILayout.Space(10);
            if (GUILayout.Button("显示更多"))
            {
                showPage++;
                OnInspectorGUI();
                return;
            }
        }

        // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
        //需要在OnInspectorGUI之前修改属性，否则无法修改值
        serializedObject.ApplyModifiedProperties();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}

[CustomEditor(typeof(PrefabList))]
public class PrefabInspector : Editor
{
    private int index = 0;
    private int showPage = 1;
    private int showNum = 50;

    public override void OnInspectorGUI()
    {
        // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
        serializedObject.Update();

        var list = ((PrefabList)target).list;
        int l = 0;
        if (list != null)
            l = list.Count;

        if (GUILayout.Button("添加元素"))
        {
            list.Insert(0, new PrefabDictionary());
        }
        EditorGUILayout.BeginHorizontal();
        index = EditorGUILayout.IntField("删除元素，编号: ", index);
        if (GUILayout.Button("确认删除"))
        {
            if (l > 0)
                list.RemoveAt(index);
        }
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(10);

        GUILayout.Label("编辑列表，元素总数: " + l);
        int num = l > showPage * showNum ? showPage * showNum : l;
        for (int i = 0; i < num; i++)
        {
            GUILayout.Space(10);
            var e = list[i];
            if (e != null)
            {
                GUILayout.Label("元素 " + i + ":");
                e.key = EditorGUILayout.TextField("Prefab名", e.key);
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Prefab");
                e.Prefab = (GameObject)EditorGUILayout.ObjectField(e.Prefab, typeof(GameObject));
                EditorGUILayout.EndHorizontal();
            }
        }

        if (l > showPage * showNum)
        {
            GUILayout.Space(10);
            if (GUILayout.Button("显示更多"))
            {
                showPage++;
                OnInspectorGUI();
                return;
            }
        }

        // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
        //需要在OnInspectorGUI之前修改属性，否则无法修改值
        serializedObject.ApplyModifiedProperties();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}

#endif