using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public delegate void AddEvent(eventType t);

#if UNITY_EDITOR

[CustomEditor(typeof(eventListDefine))]
public class onceEventInspector : Editor
{
    Editor cacheEditor;
    int index = 0,l = 0;
    List<myEvent> list;

    public void addEvent(eventType t)
    {
        if(index<0)
            list.Add(new myEvent(t));
        else
            list.Insert(index,new myEvent(t));
    }

    public override void OnInspectorGUI()
    {
        // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
        serializedObject.Update();

        //显示eventlist的默认UI
        base.OnInspectorGUI();

        GUILayout.Space(20);
        list = ((eventListDefine)target).list;
        l = 0;
        if (list != null)
            l = list.Count;
        GUILayout.Label("事件列表，事件总数: "+ l);

        GUILayout.Space(10);
        for (int i=0;i<l;i++)
        {
            GUILayout.Space(10);
            var e = list[i];
            if (e != null)
            {
                //创建event的Editor
                GUILayout.Label("事件 "+i+":");
                e.type = (eventType)EditorGUILayout.EnumPopup(myEvent.eventTypeToName(e.type), e.type);
                if (e.type == eventType.text)
                {
                    e.str1 = EditorGUILayout.TextField("名字",e.str1);
                    e.str2 = EditorGUILayout.TextField("文本", e.str2);
                    e.str3 = EditorGUILayout.TextField("立绘", e.str3);
                    e.bool1 = EditorGUILayout.Toggle("是否显示立绘", e.bool1);
                    e.str4 = EditorGUILayout.TextField("文字音效", e.str4);
                }
                else if (e.type == eventType.textSpecial)// 会覆盖黑屏白屏
                {
                    e.str1 = EditorGUILayout.TextField("文本", e.str1);
                    e.vec2_1 = EditorGUILayout.Vector2Field("位置坐标", e.vec2_1);
                    EditorGUILayout.BeginHorizontal();
                    e.helper = (Transform)EditorGUILayout.ObjectField(e.helper, typeof(Transform));
                    if (GUILayout.Button("获取"))
                    {
                        e.vec2_1 = e.helper.position;
                        e.helper = null;
                    }
                    EditorGUILayout.EndHorizontal();

                    e.str2 = EditorGUILayout.TextField("文字音效", e.str2);
                }
                else if (e.type == eventType.beBlack || e.type == eventType.beWhite || e.type == eventType.wait)
                {
                    e.float1 = EditorGUILayout.FloatField("时间", e.float1);
                }
                else if (e.type == eventType.changeGlobalInt)
                {
                    e.str1 = EditorGUILayout.TextField("变量名", e.str1);
                    e.howToCal = (calValue)EditorGUILayout.EnumPopup("计算方式", e.howToCal);
                    e.int1 = EditorGUILayout.IntField("值", e.int1);
                }
                else if (e.type == eventType.changeGlobalFloat)
                {
                    e.str1 = EditorGUILayout.TextField("变量名", e.str1);
                    e.howToCal = (calValue)EditorGUILayout.EnumPopup("计算方式", e.howToCal);
                    e.float1 = EditorGUILayout.FloatField("值", e.float1);
                }
                else if (e.type == eventType.changeGlobalSwith)
                {
                    e.str1 = EditorGUILayout.TextField("变量名", e.str1);
                    e.bool1 = EditorGUILayout.Toggle("值", e.bool1);
                }
                else if (e.type == eventType.debugLog)
                {
                    e.str1 = EditorGUILayout.TextField("文本", e.str1);
                }
                else if (e.type == eventType.playBgm)
                {
                    e.str1 = EditorGUILayout.TextField("BGM名称", e.str1);
                }
                else if (e.type == eventType.playBgs)
                {
                    e.str1 = EditorGUILayout.TextField("BGS名称", e.str1);
                }
                else if (e.type == eventType.playSE)
                {
                    e.str1 = EditorGUILayout.TextField("SE名称", e.str1);
                }
                else if (e.type == eventType.stopBgm)
                {
                    e.float1 = EditorGUILayout.FloatField("时间", e.float1);
                }
                else if (e.type == eventType.stopBgs)
                {
                    e.float1 = EditorGUILayout.FloatField("时间", e.float1);
                }
                else if (e.type == eventType.showPic)
                {
                    e.str1 = EditorGUILayout.TextField("图片编号", e.str1);
                    e.str2 = EditorGUILayout.TextField("图片名", e.str2);
                    e.vec2_1 = EditorGUILayout.Vector2Field("位置坐标", e.vec2_1);
                    EditorGUILayout.BeginHorizontal();
                    e.helper = (Transform)EditorGUILayout.ObjectField(e.helper, typeof(Transform));
                    if (GUILayout.Button("获取"))
                    {
                        e.vec2_1 = e.helper.position;
                        e.helper = null;
                    }
                    EditorGUILayout.EndHorizontal();

                    e.float1 = EditorGUILayout.FloatField("透明度", e.float1);
                }
                else if (e.type == eventType.movePic)
                {
                    e.str1 = EditorGUILayout.TextField("图片编号", e.str1);

                    e.vec2_1 = EditorGUILayout.Vector2Field("移动目标位置", e.vec2_1);
                    EditorGUILayout.BeginHorizontal();
                    e.helper = (Transform)EditorGUILayout.ObjectField(e.helper, typeof(Transform));
                    if (GUILayout.Button("获取"))
                    {
                        e.vec2_1 = e.helper.position;
                        e.helper = null;
                    }
                    EditorGUILayout.EndHorizontal();

                    e.float1 = EditorGUILayout.FloatField("透明度", e.float1);
                    e.float2 = EditorGUILayout.FloatField("时间", e.float2);
                    e.bool1 = EditorGUILayout.Toggle("卡进程", e.bool1);
                }
                else if (e.type == eventType.hidePic)
                {
                    e.str1 = EditorGUILayout.TextField("图片编号", e.str1);
                }
                else if (e.type == eventType.changeScene)
                {
                    e.str1 = EditorGUILayout.TextField("场景名", e.str1);
                    e.vec2_1 = EditorGUILayout.Vector2Field("位置", e.vec2_1);
                    EditorGUILayout.BeginHorizontal();
                    e.helper = (Transform)EditorGUILayout.ObjectField(e.helper, typeof(Transform));
                    if (GUILayout.Button("获取"))
                    {
                        e.vec2_1 = e.helper.position;
                        e.helper = null;
                    }
                    EditorGUILayout.EndHorizontal();

                    e.turn = (turn)EditorGUILayout.EnumPopup("新的朝向", e.turn);
                }
                else if (e.type == eventType.changeIndependentSwitch)
                {
                    e.independentSwitch = (IndependentSwitch)EditorGUILayout.EnumPopup("编号", e.independentSwitch);
                    e.bool1 = EditorGUILayout.Toggle("开关", e.bool1);
                }
                else if (e.type == eventType.playAnima)
                {
                    e.str1 = EditorGUILayout.TextField("动画名", e.str1);

                    e.vec2_1 = EditorGUILayout.Vector2Field("位置", e.vec2_1);
                    EditorGUILayout.BeginHorizontal();
                    e.helper = (Transform)EditorGUILayout.ObjectField(e.helper, typeof(Transform));
                    if (GUILayout.Button("获取"))
                    {
                        e.vec2_1 = e.helper.position;
                        e.helper = null;
                    }
                    EditorGUILayout.EndHorizontal();

                    e.bool1 = EditorGUILayout.Toggle("卡进程", e.bool1);
                }
                else if (e.type == eventType.moveCamera)
                {
                    e.vec2_1 = EditorGUILayout.Vector2Field("移动目标位置", e.vec2_1);
                    EditorGUILayout.BeginHorizontal();
                    e.helper = (Transform)EditorGUILayout.ObjectField(e.helper, typeof(Transform));
                    if (GUILayout.Button("获取"))
                    {
                        e.vec2_1 = e.helper.position;
                        e.helper = null;
                    }
                    EditorGUILayout.EndHorizontal();

                    e.float1 = EditorGUILayout.FloatField("时间", e.float1);
                }
                else if (e.type == eventType.sbMoveToSw)
                {
                    e.findType = (findType)EditorGUILayout.EnumPopup("查找类型", e.findType);
                    e.str1 = EditorGUILayout.TextField("文本", e.str1);
                    e.vec2_1 = EditorGUILayout.Vector2Field("移动目标位置", e.vec2_1);
                    EditorGUILayout.BeginHorizontal();
                    e.helper = (Transform)EditorGUILayout.ObjectField(e.helper, typeof(Transform));
                    if (GUILayout.Button("获取"))
                    {
                        e.vec2_1 = e.helper.position;
                        e.helper = null;
                    }
                    EditorGUILayout.EndHorizontal();

                    e.turn = (turn)EditorGUILayout.EnumPopup("新的朝向", e.turn);
                }
                else if (e.type == eventType.changeTurn)
                {
                    e.findType = (findType)EditorGUILayout.EnumPopup("查找类型", e.findType);
                    e.str1 = EditorGUILayout.TextField("文本", e.str1);
                    e.turn = (turn)EditorGUILayout.EnumPopup("新的朝向", e.turn);
                }
                else if (e.type == eventType.flashScreen)
                {
                    e.color1 = EditorGUILayout.ColorField("颜色", e.color1);
                    e.float1 = EditorGUILayout.FloatField("时间", e.float1);
                    e.bool1 = EditorGUILayout.Toggle("卡进程", e.bool1);
                }
                else if (e.type == eventType.shakeScreen)
                {
                    e.float1 = EditorGUILayout.FloatField("时间", e.float1);
                    e.float2 = EditorGUILayout.FloatField("强度", e.float2);
                    e.bool1 = EditorGUILayout.Toggle("卡进程", e.bool1);
                }
                else if (e.type == eventType.changeWeather)
                {
                    e.weather = (weather)EditorGUILayout.EnumPopup("天气", e.weather);
                    e.float1 = EditorGUILayout.FloatField("强度", e.float1);
                }
                else if (e.type == eventType.changeItem)
                {
                    e.int1 = EditorGUILayout.IntField("道具id", e.int1);
                    e.int2 = EditorGUILayout.IntField("数量", e.int2);
                }
                else if (e.type == eventType.changeThingPos)
                {
                    e.findType = (findType)EditorGUILayout.EnumPopup("查找类型", e.findType);
                    e.str1 = EditorGUILayout.TextField("文本", e.str1);
                    e.vec2_1 = EditorGUILayout.Vector2Field("设置位置", e.vec2_1);
                    EditorGUILayout.BeginHorizontal();
                    e.helper = (Transform)EditorGUILayout.ObjectField(e.helper, typeof(Transform));
                    if (GUILayout.Button("获取"))
                    {
                        e.vec2_1 = e.helper.position;
                        e.helper = null;
                    }
                    EditorGUILayout.EndHorizontal();
                }
                else if (e.type == eventType.changeThingActive)
                {
                    e.findType = (findType)EditorGUILayout.EnumPopup("查找类型", e.findType);
                    e.str1 = EditorGUILayout.TextField("文本", e.str1);
                    e.bool1 = EditorGUILayout.Toggle("可见性", e.bool1);
                }
                else if (e.type == eventType.startQuest)
                {
                    e.int1 = EditorGUILayout.IntField("任务id", e.int1);
                }
                else if (e.type == eventType.changeQuest)
                {
                    e.int1 = EditorGUILayout.IntField("任务id", e.int1);
                    e.questStatus = (questStatus)EditorGUILayout.EnumPopup("任务状态", e.questStatus);
                }
                else if (e.type == eventType.If)
                {
                    EditorGUILayout.LabelField("条件: ");
                    e.condition.type = (conditionType)EditorGUILayout.EnumPopup("条件类型", e.condition.type);
                    e.condition.name = EditorGUILayout.TextField("变量名", e.condition.name);
                    e.condition.compare = (compare)EditorGUILayout.EnumPopup("比较类型", e.condition.compare);
                    e.condition.value = EditorGUILayout.TextField("值", e.condition.value);
                    EditorGUILayout.LabelField("范围: ");
                    e.int1 = EditorGUILayout.IntField("包含事件数", e.int1);
                }
            }
        }

        GUILayout.Space(20);
        index = EditorGUILayout.IntField("添加/删除 事件的位置序号: ", index);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("新建事件"))
        {
            var window = EditorWindow.GetWindow(typeof(ChooseEventTypeWin), true, "请选择事件类型") as ChooseEventTypeWin;
            window.maxSize = new Vector2(400, window.maxSize.y);
            window.e = addEvent;
            window.Show();
        }
        if (GUILayout.Button("删除事件"))
        {
            if (l > 0)
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

#endif