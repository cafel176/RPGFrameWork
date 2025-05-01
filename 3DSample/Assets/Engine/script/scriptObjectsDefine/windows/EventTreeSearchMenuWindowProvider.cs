using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
#endif

namespace BaseData
{
#if UNITY_EDITOR

    public class EventTreeSearchMenuWindowProvider : myTreeSearchMenuWindowProvider<myEvent>
    {
        protected override void menuDetails(List<SearchTreeEntry> entries)
        {
            entries.Add(new SearchTreeGroupEntry(new GUIContent("信息")) { level = 1 });//添加了一个二级菜单
            entries.Add(new SearchTreeEntry(new GUIContent(myEvent.eventTypeToName(eventType.text))) { level = 2, userData = eventType.text });
            entries.Add(new SearchTreeEntry(new GUIContent(myEvent.eventTypeToName(eventType.textSpecial))) { level = 2, userData = eventType.textSpecial });

            entries.Add(new SearchTreeGroupEntry(new GUIContent("游戏进程")) { level = 1 });
            entries.Add(new SearchTreeEntry(new GUIContent(myEvent.eventTypeToName(eventType.changeGlobalSwith))) { level = 2, userData = eventType.changeGlobalSwith });
            entries.Add(new SearchTreeEntry(new GUIContent(myEvent.eventTypeToName(eventType.changeIndependentSwitch))) { level = 2, userData = eventType.changeIndependentSwitch });
            entries.Add(new SearchTreeEntry(new GUIContent(myEvent.eventTypeToName(eventType.changeGlobalDouble))) { level = 2, userData = eventType.changeGlobalDouble });
            entries.Add(new SearchTreeEntry(new GUIContent(myEvent.eventTypeToName(eventType.changeGlobalInt))) { level = 2, userData = eventType.changeGlobalInt });
            entries.Add(new SearchTreeEntry(new GUIContent(myEvent.eventTypeToName(eventType.changeGlobalVector3))) { level = 2, userData = eventType.changeGlobalVector3 });

            entries.Add(new SearchTreeGroupEntry(new GUIContent("流程控制")) { level = 1 });
            entries.Add(new SearchTreeEntry(new GUIContent(myEvent.eventTypeToName(eventType.commonEvent))) { level = 2, userData = eventType.commonEvent });
            entries.Add(new SearchTreeEntry(new GUIContent(myEvent.eventTypeToName(eventType.If))) { level = 2, userData = eventType.If });
            entries.Add(new SearchTreeEntry(new GUIContent(myEvent.eventTypeToName(eventType.showButton))) { level = 2, userData = eventType.showButton });
            entries.Add(new SearchTreeEntry(new GUIContent(myEvent.eventTypeToName(eventType.hideButton))) { level = 2, userData = eventType.hideButton });
            entries.Add(new SearchTreeEntry(new GUIContent(myEvent.eventTypeToName(eventType.choicePanel))) { level = 2, userData = eventType.choicePanel });

            entries.Add(new SearchTreeGroupEntry(new GUIContent("队伍和角色")) { level = 1 });
            entries.Add(new SearchTreeEntry(new GUIContent(myEvent.eventTypeToName(eventType.startUserControl))) { level = 2, userData = eventType.startUserControl });
            entries.Add(new SearchTreeEntry(new GUIContent(myEvent.eventTypeToName(eventType.stopUserControl))) { level = 2, userData = eventType.stopUserControl });
            entries.Add(new SearchTreeEntry(new GUIContent(myEvent.eventTypeToName(eventType.enableAttack))) { level = 2, userData = eventType.enableAttack });
            entries.Add(new SearchTreeEntry(new GUIContent(myEvent.eventTypeToName(eventType.disableAttack))) { level = 2, userData = eventType.disableAttack });
            entries.Add(new SearchTreeEntry(new GUIContent(myEvent.eventTypeToName(eventType.playAnima))) { level = 2, userData = eventType.playAnima });
            entries.Add(new SearchTreeEntry(new GUIContent(myEvent.eventTypeToName(eventType.actorPlayAnima))) { level = 2, userData = eventType.actorPlayAnima });
            entries.Add(new SearchTreeEntry(new GUIContent(myEvent.eventTypeToName(eventType.changeItem))) { level = 2, userData = eventType.changeItem });
            entries.Add(new SearchTreeEntry(new GUIContent(myEvent.eventTypeToName(eventType.startQuest))) { level = 2, userData = eventType.startQuest });
            entries.Add(new SearchTreeEntry(new GUIContent(myEvent.eventTypeToName(eventType.changeQuest))) { level = 2, userData = eventType.changeQuest });
            entries.Add(new SearchTreeEntry(new GUIContent(myEvent.eventTypeToName(eventType.setFollow))) { level = 2, userData = eventType.setFollow });
            entries.Add(new SearchTreeEntry(new GUIContent(myEvent.eventTypeToName(eventType.changeTeam))) { level = 2, userData = eventType.changeTeam});

            entries.Add(new SearchTreeGroupEntry(new GUIContent("移动")) { level = 1 });
            entries.Add(new SearchTreeEntry(new GUIContent(myEvent.eventTypeToName(eventType.changeScene))) { level = 2, userData = eventType.changeScene });
            entries.Add(new SearchTreeEntry(new GUIContent(myEvent.eventTypeToName(eventType.moveCamera))) { level = 2, userData = eventType.moveCamera });
            entries.Add(new SearchTreeEntry(new GUIContent(myEvent.eventTypeToName(eventType.sbMoveToSw))) { level = 2, userData = eventType.sbMoveToSw });
            entries.Add(new SearchTreeEntry(new GUIContent(myEvent.eventTypeToName(eventType.changeTurn2D))) { level = 2, userData = eventType.changeTurn2D });
            entries.Add(new SearchTreeEntry(new GUIContent(myEvent.eventTypeToName(eventType.changeTurn3D))) { level = 2, userData = eventType.changeTurn3D });
            entries.Add(new SearchTreeEntry(new GUIContent(myEvent.eventTypeToName(eventType.changeThingActive))) { level = 2, userData = eventType.changeThingActive });
            entries.Add(new SearchTreeEntry(new GUIContent(myEvent.eventTypeToName(eventType.changeThingPos))) { level = 2, userData = eventType.changeThingPos });

            entries.Add(new SearchTreeGroupEntry(new GUIContent("图片")) { level = 1 });
            entries.Add(new SearchTreeEntry(new GUIContent(myEvent.eventTypeToName(eventType.showPic))) { level = 2, userData = eventType.showPic});
            entries.Add(new SearchTreeEntry(new GUIContent(myEvent.eventTypeToName(eventType.hidePic))) { level = 2, userData = eventType.hidePic });
            entries.Add(new SearchTreeEntry(new GUIContent(myEvent.eventTypeToName(eventType.movePic))) { level = 2, userData = eventType.movePic });

            entries.Add(new SearchTreeGroupEntry(new GUIContent("计时")) { level = 1 });
            entries.Add(new SearchTreeEntry(new GUIContent(myEvent.eventTypeToName(eventType.wait))) { level = 2, userData = eventType.wait });

            entries.Add(new SearchTreeGroupEntry(new GUIContent("画面")) { level = 1 });
            entries.Add(new SearchTreeEntry(new GUIContent(myEvent.eventTypeToName(eventType.beBlack))) { level = 2, userData = eventType.beBlack });
            entries.Add(new SearchTreeEntry(new GUIContent(myEvent.eventTypeToName(eventType.beWhite))) { level = 2, userData = eventType.beWhite });
            entries.Add(new SearchTreeEntry(new GUIContent(myEvent.eventTypeToName(eventType.flashScreen))) { level = 2, userData = eventType.flashScreen });
            entries.Add(new SearchTreeEntry(new GUIContent(myEvent.eventTypeToName(eventType.shakeScreen))) { level = 2, userData = eventType.shakeScreen });
            entries.Add(new SearchTreeEntry(new GUIContent(myEvent.eventTypeToName(eventType.changeWeather))) { level = 2, userData = eventType.changeWeather });

            entries.Add(new SearchTreeGroupEntry(new GUIContent("音频")) { level = 1 });
            entries.Add(new SearchTreeEntry(new GUIContent(myEvent.eventTypeToName(eventType.playBgm))) { level = 2, userData = eventType.playBgm });
            entries.Add(new SearchTreeEntry(new GUIContent(myEvent.eventTypeToName(eventType.stopBgm))) { level = 2, userData = eventType.stopBgm });
            entries.Add(new SearchTreeEntry(new GUIContent(myEvent.eventTypeToName(eventType.playBgs))) { level = 2, userData = eventType.playBgs });
            entries.Add(new SearchTreeEntry(new GUIContent(myEvent.eventTypeToName(eventType.stopBgs))) { level = 2, userData = eventType.stopBgs });
            entries.Add(new SearchTreeEntry(new GUIContent(myEvent.eventTypeToName(eventType.playSE))) { level = 2, userData = eventType.playSE });

            entries.Add(new SearchTreeGroupEntry(new GUIContent("视频")) { level = 1 });
            entries.Add(new SearchTreeEntry(new GUIContent(myEvent.eventTypeToName(eventType.playVideo))) { level = 2, userData = eventType.playVideo });

            entries.Add(new SearchTreeGroupEntry(new GUIContent("场景控制")) { level = 1 });
            entries.Add(new SearchTreeEntry(new GUIContent(myEvent.eventTypeToName(eventType.showSavePanel))) { level = 2, userData = eventType.showSavePanel });

            entries.Add(new SearchTreeGroupEntry(new GUIContent("其他")) { level = 1 });
            entries.Add(new SearchTreeEntry(new GUIContent(myEvent.eventTypeToName(eventType.debugLog))) { level = 2, userData = eventType.debugLog});
            entries.Add(new SearchTreeEntry(new GUIContent(myEvent.eventTypeToName(eventType.none))) { level = 2, userData = eventType.none });
        }

    }

#endif
}
