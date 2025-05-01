using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BaseData
{
#if UNITY_EDITOR

    public class ChooseEventTypeWin : ChooseTypeWin
    {
        protected override void doShow()
        {
            GUILayout.Label("信息");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(myEvent.eventTypeToName(eventType.text)))
            {
                e((int)eventType.text);
            }
            if (GUILayout.Button(myEvent.eventTypeToName(eventType.textSpecial)))
            {
                e((int)eventType.textSpecial);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(20);

            GUILayout.Label("游戏进程");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(myEvent.eventTypeToName(eventType.changeGlobalSwith)))
            {
                e((int)eventType.changeGlobalSwith);
            }
            if (GUILayout.Button(myEvent.eventTypeToName(eventType.changeIndependentSwitch)))
            {
                e((int)eventType.changeIndependentSwitch);
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(myEvent.eventTypeToName(eventType.changeGlobalDouble)))
            {
                e((int)eventType.changeGlobalDouble);
            }
            if (GUILayout.Button(myEvent.eventTypeToName(eventType.changeGlobalInt)))
            {
                e((int)eventType.changeGlobalInt);
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(myEvent.eventTypeToName(eventType.changeGlobalVector3)))
            {
                e((int)eventType.changeGlobalVector3);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(20);

            GUILayout.Label("流程控制");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(myEvent.eventTypeToName(eventType.commonEvent)))
            {
                e((int)eventType.commonEvent);
            }
            if (GUILayout.Button(myEvent.eventTypeToName(eventType.If)))
            {
                e((int)eventType.If);
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(myEvent.eventTypeToName(eventType.showButton)))
            {
                e((int)eventType.showButton);
            }
            if (GUILayout.Button(myEvent.eventTypeToName(eventType.hideButton)))
            {
                e((int)eventType.hideButton);
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(myEvent.eventTypeToName(eventType.choicePanel)))
            {
                e((int)eventType.choicePanel);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(20);

            GUILayout.Label("队伍和角色");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(myEvent.eventTypeToName(eventType.startUserControl)))
            {
                e((int)eventType.startUserControl);
            }
            if (GUILayout.Button(myEvent.eventTypeToName(eventType.stopUserControl)))
            {
                e((int)eventType.stopUserControl);
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(myEvent.eventTypeToName(eventType.enableAttack)))
            {
                e((int)eventType.enableAttack);
            }
            if (GUILayout.Button(myEvent.eventTypeToName(eventType.disableAttack)))
            {
                e((int)eventType.disableAttack);
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(myEvent.eventTypeToName(eventType.playAnima)))
            {
                e((int)eventType.playAnima);
            }
            if (GUILayout.Button(myEvent.eventTypeToName(eventType.actorPlayAnima)))
            {
                e((int)eventType.actorPlayAnima);
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(myEvent.eventTypeToName(eventType.startQuest)))
            {
                e((int)eventType.startQuest);
            }
            if (GUILayout.Button(myEvent.eventTypeToName(eventType.changeQuest)))
            {
                e((int)eventType.changeQuest);
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(myEvent.eventTypeToName(eventType.setFollow)))
            {
                e((int)eventType.setFollow);
            }
            if (GUILayout.Button(myEvent.eventTypeToName(eventType.changeTeam)))
            {
                e((int)eventType.changeTeam);
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(myEvent.eventTypeToName(eventType.changeItem)))
            {
                e((int)eventType.changeItem);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(20);

            GUILayout.Label("移动");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(myEvent.eventTypeToName(eventType.changeScene)))
            {
                e((int)eventType.changeScene);
            }
            if (GUILayout.Button(myEvent.eventTypeToName(eventType.moveCamera)))
            {
                e((int)eventType.moveCamera);
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(myEvent.eventTypeToName(eventType.changeTurn3D)))
            {
                e((int)eventType.changeTurn3D);
            }
            if (GUILayout.Button(myEvent.eventTypeToName(eventType.changeTurn2D)))
            {
                e((int)eventType.changeTurn2D);
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(myEvent.eventTypeToName(eventType.changeThingActive)))
            {
                e((int)eventType.changeThingActive);
            }
            if (GUILayout.Button(myEvent.eventTypeToName(eventType.changeThingPos)))
            {
                e((int)eventType.changeThingPos);
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(myEvent.eventTypeToName(eventType.sbMoveToSw)))
            {
                e((int)eventType.sbMoveToSw);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(20);

            GUILayout.Label("图片");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(myEvent.eventTypeToName(eventType.showPic)))
            {
                e((int)eventType.showPic);
            }
            if (GUILayout.Button(myEvent.eventTypeToName(eventType.hidePic)))
            {
                e((int)eventType.hidePic);
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(myEvent.eventTypeToName(eventType.movePic)))
            {
                e((int)eventType.movePic);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(20);

            GUILayout.Label("计时");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(myEvent.eventTypeToName(eventType.wait)))
            {
                e((int)eventType.wait);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(20);

            GUILayout.Label("画面");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(myEvent.eventTypeToName(eventType.beBlack)))
            {
                e((int)eventType.beBlack);
            }
            if (GUILayout.Button(myEvent.eventTypeToName(eventType.beWhite)))
            {
                e((int)eventType.beWhite);
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(myEvent.eventTypeToName(eventType.flashScreen)))
            {
                e((int)eventType.flashScreen);
            }
            if (GUILayout.Button(myEvent.eventTypeToName(eventType.shakeScreen)))
            {
                e((int)eventType.shakeScreen);
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(myEvent.eventTypeToName(eventType.changeWeather)))
            {
                e((int)eventType.changeWeather);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(20);

            GUILayout.Label("音频");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(myEvent.eventTypeToName(eventType.playBgm)))
            {
                e((int)eventType.playBgm);
            }
            if (GUILayout.Button(myEvent.eventTypeToName(eventType.stopBgm)))
            {
                e((int)eventType.stopBgm);
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(myEvent.eventTypeToName(eventType.playBgs)))
            {
                e((int)eventType.playBgs);
            }
            if (GUILayout.Button(myEvent.eventTypeToName(eventType.stopBgs)))
            {
                e((int)eventType.stopBgs);
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(myEvent.eventTypeToName(eventType.playSE)))
            {
                e((int)eventType.playSE);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(20);

            GUILayout.Label("视频");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(myEvent.eventTypeToName(eventType.playVideo)))
            {
                e((int)eventType.playVideo);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(20);

            GUILayout.Label("场景控制");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(myEvent.eventTypeToName(eventType.showSavePanel)))
            {
                e((int)eventType.showSavePanel);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(20);

            GUILayout.Label("其他");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(myEvent.eventTypeToName(eventType.debugLog)))
            {
                e((int)eventType.debugLog);
            }
            if (GUILayout.Button(myEvent.eventTypeToName(eventType.none)))
            {
                e((int)eventType.none);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(20);
        }
    }

#endif
}

