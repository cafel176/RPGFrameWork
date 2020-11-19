using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR

public class ChooseEventTypeWin : EditorWindow
{
    private Vector2 scrollPosition;

    public AddEvent e;
    public string[] names;

    private void OnGUI()
    {
         ShowEditorGUI();
    }

    private void ShowEditorGUI()
    {
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        GUILayout.Label("信息");
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(myEvent.eventTypeToName(eventType.text)))
        {
            e(eventType.text);
        }
        if (GUILayout.Button(myEvent.eventTypeToName(eventType.textSpecial)))
        {
            e(eventType.textSpecial);
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(20);

        GUILayout.Label("游戏进程");
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(myEvent.eventTypeToName(eventType.changeGlobalSwith)))
        {
            e(eventType.changeGlobalSwith);
        }
        if (GUILayout.Button(myEvent.eventTypeToName(eventType.changeIndependentSwitch)))
        {
            e(eventType.changeIndependentSwitch);
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(myEvent.eventTypeToName(eventType.changeGlobalFloat)))
        {
            e(eventType.changeGlobalFloat);
        }
        if (GUILayout.Button(myEvent.eventTypeToName(eventType.changeGlobalInt)))
        {
            e(eventType.changeGlobalInt);
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(20);

        GUILayout.Label("流程控制");
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(myEvent.eventTypeToName(eventType.commonEvent)))
        {
            e(eventType.commonEvent);
        }
        if (GUILayout.Button(myEvent.eventTypeToName(eventType.If)))
        {
            e(eventType.If);
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(20);

        GUILayout.Label("队伍和角色");
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(myEvent.eventTypeToName(eventType.startUserControl)))
        {
            e(eventType.startUserControl);
        }
        if (GUILayout.Button(myEvent.eventTypeToName(eventType.stopUserControl)))
        {
            e(eventType.stopUserControl);
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(myEvent.eventTypeToName(eventType.playAnima)))
        {
            e(eventType.playAnima);
        }
        if (GUILayout.Button(myEvent.eventTypeToName(eventType.changeItem)))
        {
            e(eventType.changeItem);
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(myEvent.eventTypeToName(eventType.startQuest)))
        {
            e(eventType.startQuest);
        }
        if (GUILayout.Button(myEvent.eventTypeToName(eventType.changeQuest)))
        {
            e(eventType.changeQuest);
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(20);

        GUILayout.Label("移动");
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(myEvent.eventTypeToName(eventType.changeScene)))
        {
            e(eventType.changeScene);
        }
        if (GUILayout.Button(myEvent.eventTypeToName(eventType.moveCamera)))
        {
            e(eventType.moveCamera);
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(myEvent.eventTypeToName(eventType.sbMoveToSw)))
        {
            e(eventType.sbMoveToSw);
        }
        if (GUILayout.Button(myEvent.eventTypeToName(eventType.changeTurn)))
        {
            e(eventType.changeTurn);
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(myEvent.eventTypeToName(eventType.changeThingActive)))
        {
            e(eventType.changeThingActive);
        }
        if (GUILayout.Button(myEvent.eventTypeToName(eventType.changeThingPos)))
        {
            e(eventType.changeThingPos);
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(20);

        GUILayout.Label("图片");
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(myEvent.eventTypeToName(eventType.showPic)))
        {
            e(eventType.showPic);
        }
        if (GUILayout.Button(myEvent.eventTypeToName(eventType.hidePic)))
        {
            e(eventType.hidePic);
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(myEvent.eventTypeToName(eventType.movePic)))
        {
            e(eventType.movePic);
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(20);

        GUILayout.Label("计时");
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(myEvent.eventTypeToName(eventType.wait)))
        {
            e(eventType.wait);
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(20);

        GUILayout.Label("画面");
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(myEvent.eventTypeToName(eventType.beBlack)))
        {
            e(eventType.beBlack);
        }
        if (GUILayout.Button(myEvent.eventTypeToName(eventType.beWhite)))
        {
            e(eventType.beWhite);
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(myEvent.eventTypeToName(eventType.flashScreen)))
        {
            e(eventType.flashScreen);
        }
        if (GUILayout.Button(myEvent.eventTypeToName(eventType.shakeScreen)))
        {
            e(eventType.shakeScreen);
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(myEvent.eventTypeToName(eventType.changeWeather)))
        {
            e(eventType.changeWeather);
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(20);

        GUILayout.Label("音频");
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(myEvent.eventTypeToName(eventType.playBgm)))
        {
            e(eventType.playBgm);
        }
        if (GUILayout.Button(myEvent.eventTypeToName(eventType.stopBgm)))
        {
            e(eventType.stopBgm);
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(myEvent.eventTypeToName(eventType.playBgs)))
        {
            e(eventType.playBgs);
        }
        if (GUILayout.Button(myEvent.eventTypeToName(eventType.stopBgs)))
        {
            e(eventType.stopBgs);
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(myEvent.eventTypeToName(eventType.playSE)))
        {
            e(eventType.playSE);
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(20);

        GUILayout.Label("场景控制");
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(myEvent.eventTypeToName(eventType.showSavePanel)))
        {
            e(eventType.showSavePanel);
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(20);

        GUILayout.Label("其他");
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(myEvent.eventTypeToName(eventType.debugLog)))
        {
            e(eventType.debugLog);
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(20);

        GUILayout.EndScrollView();

        GUIUtility.ExitGUI();
    }
}

#endif