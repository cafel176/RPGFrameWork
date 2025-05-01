using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions
{
    public class DoEvent : DoBasicEvents
    {
        protected class eventStruct
        {
            public eventType type;
            public calValue howToCal;
            public turn turn;
            public findType findType;
            public questStatus questStatus;
            public IndependentSwitch independentSwitch;
            public conditionType conditionType;
            public compare compare;

            //设置名字和文本
            public string str1;
            public string str2;
            public string str3;
            public string str4;
            public bool bool1 = false;
            public bool bool2 = false;
            public float float1;
            public float float2;
            public int int1;
            public Vector3 vec1;
            public Vector3 vec2;
            public Color color1;

            public eventStruct(TreeNodeInterface toDo)
            {
                type = toDo.getData<eventType>(structProperty.type);
                float1 = toDo.getData<float>(structProperty.float1);
                float2 = toDo.getData<float>(structProperty.float2);
                str1 = toDo.getData<string>(structProperty.str1);
                str2 = toDo.getData<string>(structProperty.str2);
                str3 = toDo.getData<string>(structProperty.str3);
                str4 = toDo.getData<string>(structProperty.str4);
                int1 = toDo.getData<int>(structProperty.int1);
                bool1 = toDo.getData<bool>(structProperty.bool1);
                bool2 = toDo.getData<bool>(structProperty.bool2);
                vec1 = toDo.getData<Vector3>(structProperty.vec1);
                vec2 = toDo.getData<Vector3>(structProperty.vec2);
                howToCal = toDo.getData<calValue>(structProperty.howToCal);
                turn = toDo.getData<turn>(structProperty.turn);
                findType = toDo.getData<findType>(structProperty.findType);
                color1 = toDo.getData<Color>(structProperty.color1);
                questStatus = toDo.getData<questStatus>(structProperty.questStatus);
                independentSwitch = toDo.getData<IndependentSwitch>(structProperty.independentSwitch);
                compare = toDo.getData<compare>(structProperty.compare);
                conditionType = toDo.getData<conditionType>(structProperty.conditionType);
            }
        }

        // 公用的写在这里，各自个性化的写在dosth里
        override public void doSth(TreeNodeInterface toDo)
        {
            eventStruct e = new eventStruct(toDo);

            switch (e.type)
            {
                // =============================== 空操作 ===============================
                case eventType.none: pushNow(true); break;
                case eventType.If: pushNow(true); break;

                // =============================== 其他操作 ===============================
                case eventType.beBlack: beBlack(e.float1); break;
                case eventType.beWhite: beWhite(e.float1); break;
                case eventType.wait: waitForTime(e.float1); break;
                case eventType.startUserControl: pushNow(true); startUserControl(); break;
                case eventType.stopUserControl: pushNow(true); a.stopUserControl(); break;
                case eventType.changeGlobalInt:
                    calValue cal = e.howToCal;
                    int u = a.getInt(e.str1);
                    if (cal == calValue.add)
                        a.setInt(e.str1, u + e.int1);
                    else if (cal == calValue.minus)
                        a.setInt(e.str1, u - e.int1);
                    else if (cal == calValue.multiply)
                        a.setInt(e.str1, u * e.int1);
                    else if (cal == calValue.divide)
                        a.setInt(e.str1, u / e.int1);
                    else
                        a.setInt(e.str1, e.int1);
                    pushNow(true); break;
                case eventType.changeGlobalDouble:
                    cal = e.howToCal;
                    double y = a.getDouble(e.str1);
                    if (cal == calValue.add)
                        a.setDouble(e.str1, y + e.float1);
                    else if (cal == calValue.minus)
                        a.setDouble(e.str1, y - e.float1);
                    else if (cal == calValue.multiply)
                        a.setDouble(e.str1, y * e.float1);
                    else if (cal == calValue.divide)
                        a.setDouble(e.str1, y / e.float1);
                    else
                        a.setDouble(e.str1, e.float1);
                    pushNow(true); break;
                case eventType.changeGlobalVector3:
                    cal = e.howToCal;
                    Vector3 v = a.getVec3(e.str1);
                    if (cal == calValue.add)
                        a.setVec3(e.str1, v + e.vec1);
                    else if (cal == calValue.minus)
                        a.setVec3(e.str1, v - e.vec1);
                    else if (cal == calValue.multiply)
                        a.setVec3(e.str1, v * e.vec1.x);
                    else if (cal == calValue.divide)
                        a.setVec3(e.str1, v / e.vec1.x);
                    else
                        a.setVec3(e.str1, e.vec1);
                    pushNow(true); break;
                case eventType.changeGlobalSwith:
                    a.setSwitch(e.str1, e.bool1);
                    pushNow(true); break;
                case eventType.debugLog:
                    Debug.Log(a.findText(e.str1,language.none));
                    pushNow(true); break;
                case eventType.playBgm:
                    a.playMusic(a.findAudio(e.str1));
                    pushNow(true); break;
                case eventType.stopBgm:
                    a.stopMusic();
                    pushNow(true); break;
                case eventType.playBgs:
                    a.playBGS(a.findAudio(e.str1));
                    pushNow(true); break;
                case eventType.stopBgs:
                    a.stopBGS();
                    pushNow(true); break;
                case eventType.playSE:
                    a.playSE(a.findAudio(e.str1), a.findPitch(e.str1));
                    pushNow(true); break;
                case eventType.showPic:
                    a.showPicPanel(e.str1, e.vec1, a.findImg(e.str2), e.float1 / 255.0f);
                    pushNow(true); break;
                case eventType.movePic:
                    a.movePicPanel(e.str1, e.vec1, e.float1 / 255.0f, e.float2);
                    if (e.bool1)
                        waitForTime(e.float2);
                    else
                        pushNow(true);
                    break;
                case eventType.hidePic:
                    pushNow(true);
                    a.hidePicPanel(e.str1); break;
                case eventType.textSpecial:
                    string text = a.findText(e.str1, getSetting().nowlang);
                    showSpecialTextPanel(a, text, e.vec1, a.findAudio(e.str2), a.findPitch(e.str2));
                    break;
                case eventType.changeScene:
                    pushNow(true);
                    changeSceneDo csd = new changeSceneDo(a.getTeam()[0], e.vec1, e.turn);
                    a.loadLevel(e.str1, csd);
                    break;
                case eventType.playAnima:
                    var prefab = spawnPrefab(e.str1, e.vec1);
                    var animator = prefab.GetComponent<Animator>();
                    AnimationClip clip = animator.runtimeAnimatorController.animationClips[0];
                    var speed = animator.GetCurrentAnimatorStateInfo(0).speed;
                    var time = clip.length / (speed * speed);
                    if (e.bool1)
                        waitForTime(time);
                    else
                        pushNow(true);
                    break;
                case eventType.moveCamera:
                    CameraMoveToSw(e.vec1, e.float1);
                    break;
                case eventType.sbMoveToSw:
                    GameObject g = null;
                    if (e.findType == findType.tag)
                        g = GameObject.FindGameObjectWithTag(e.str1);
                    else if (e.findType == findType.name)
                        g = GameObject.Find(e.str1);
                    if (g.GetComponent<ActorInterface>() == null)
                        g = g.transform.GetChild(0).gameObject;
                    if (g == null)
                        Debug.LogError("未找到物体");
                    else
                    {
                        SbMoveToSw(g, e.vec1, e.turn);
                    }
                    break;
                case eventType.changeThingPos:
                    pushNow(true); g = null;
                    if (e.findType == findType.tag)
                        g = GameObject.FindGameObjectWithTag(e.str1);
                    else if (e.findType == findType.name)
                        g = GameObject.Find(e.str1);
                    if (g == null)
                        Debug.LogError("未找到物体");
                    else
                    {
                        changeThingPos(g, e.vec1);
                    }
                    break;
                case eventType.changeTurn:
                    g = null;
                    if (e.findType == findType.tag)
                        g = GameObject.FindGameObjectWithTag(e.str1);
                    else if (e.findType == findType.name)
                        g = GameObject.Find(e.str1);

                    if (g == null)
                        Debug.LogError("未找到物体");
                    else
                    {
                        if (g.GetComponent<ActorInterface>() == null)
                            g = g.transform.GetChild(0).gameObject;
                        setActorTurn(g, e.turn);
                    }
                    break;
                case eventType.changeThingActive:
                    pushNow(true); g = null; // 注意find函数无法查找隐藏的物体，需要给物体添加父物体以设置其可见性，且父子物体不可重名
                    if (e.findType == findType.tag)
                    {
                        if (e.str1 == HashsAndTags.player)
                            g = a.Player.gameObject;
                        else
                            g = GameObject.FindGameObjectWithTag(e.str1).transform.GetChild(0).gameObject;
                    }
                    else if (e.findType == findType.name)
                        g = GameObject.Find(e.str1).transform.GetChild(0).gameObject;

                    if (g == null)
                        Debug.LogError("未找到物体");
                    else
                        g.SetActive(e.bool1);
                    break;
                case eventType.showSavePanel:
                    pushNow();
                    a.showSavePanel();
                    break;
                case eventType.changeItem:
                    a.getItem(e.str1, e.int1);
                    var n = a.getItemInfo(e.str1).getData<string>(itemProperty.name);
                    int num = Mathf.Abs(e.int1);
                    if(num>0)
                        showHint(a.findText(n, getSetting().nowlang) + (e.int1>0?" +": " -") + num);
                    break;
                case eventType.flashScreen:
                    a.showFlash(e.color1, e.float1);
                    if (e.bool1)
                        waitForTime(e.float1);
                    else
                        pushNow(true);
                    break;
                case eventType.shakeScreen:
                    ShakeCamera(e.float2, e.float1);
                    if (e.bool1)
                        waitForTime(e.float1);
                    else
                        pushNow(true);
                    break;
                case eventType.startQuest:
                    a.addQuest(e.str1);
                    pushNow(true);
                    break;
                case eventType.changeQuest:
                    a.changeQuest(e.str1, e.questStatus);
                    pushNow(true);
                    break;
                case eventType.changeWeather:

                    pushNow(true);
                    break;
                case eventType.setFollow:
                    g = null;
                    if (e.findType == findType.tag)
                    {
                        g = GameObject.FindGameObjectWithTag(e.str1);
                    }
                    else if (e.findType == findType.name)
                        g = GameObject.Find(e.str1);

                    if(g==null)
                        Debug.LogError("未找到物体");
                    else
                    {
                        GameObject g2 = Instantiate(getSystemSetting().PlayerInfos[e.int1].playerPrefab);
                        g2.transform.position = g.transform.position;
                        setFollow(g2, e.bool1);
                    }                  
                    pushNow(true);
                    break;
                case eventType.changeTeam:
                    n = a.getSystemSetting().PlayerInfos[e.int1].name;
                    if (e.bool1)
                    {
                        inTeam(e.int1);
                        showHint(a.findText(n, getSetting().nowlang) + a.findText("sys.inTeam", getSetting().nowlang));
                    }
                    else
                    {
                        outTeam(e.int1);
                        showHint(a.findText(n, getSetting().nowlang) + a.findText("sys.outTeam", getSetting().nowlang));
                    }
                    break;

               // default 会造成混乱，严禁出现
            }
        }
    }
}
