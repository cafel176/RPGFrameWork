using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// 出现条件
public enum connect
{
    and,
    or
}

public enum conditionType
{
    Switch,
    Int,
    Float,
    IndependentSwitch
}

public enum compare
{
    larger,
    less,
    equal,
    notEqual
}

[System.Serializable]
public struct conditionDictionary
{
    public conditionType type;
    public string name;
    public connect thisConnect;
    public compare compare;
    public string value;
}

//触发条件
public enum start
{
    Z,
    playerTouch,
    eventTouch,
    auto
}

// 需要玩家朝向
public enum turn
{
    up,
    down,
    left,
    right,
    all
}

public enum findType
{
    name,
    tag
}

// 天气
public enum weather
{
    sunny,
    rain,
    snow
}

//事件类型
// 加的时候从尾部加，否则会把之前做好的顺序打乱
public enum eventType
{
    none,
    debugLog,
    text,
    wait,
    beBlack,
    beWhite,
    startUserControl,
    stopUserControl,
    changeGlobalSwith,
    changeGlobalInt,
    changeGlobalFloat,
    playBgm,
    stopBgm,
    showPic,
    movePic,
    hidePic,
    changeScene,
    changeIndependentSwitch,
    commonEvent,
    textSpecial,
    playAnima,
    moveCamera,
    sbMoveToSw,
    flashScreen,
    shakeScreen,
    changeWeather,
    showSavePanel,
    changeItem,
    changeThingPos,
    playSE,
    playBgs,
    stopBgs,
    changeThingActive,
    changeTurn,
    startQuest,
    changeQuest,
    If
}

// 事件基类
[System.Serializable]
public class myEvent
{
    public eventType type;

    public myEvent(eventType _type)
    {
        type = _type;
    }

    public static string eventTypeToName(eventType _type)
    {
        string t = string.Empty;
        switch (_type)
        {
            case eventType.text: t = "显示文本框文字"; break;
            case eventType.textSpecial: t = "显示特殊文字"; break;
            case eventType.beBlack: t = "黑屏"; break;
            case eventType.beWhite: t = "白屏"; break;
            case eventType.changeGlobalFloat: t = "浮点变量操作"; break;
            case eventType.changeGlobalInt: t = "整数变量操作"; break;
            case eventType.changeGlobalSwith: t = "开关操作"; break;
            case eventType.changeIndependentSwitch: t = "独立开关操作"; break;
            case eventType.changeScene: t = "场景移动"; break;
            case eventType.commonEvent: t = "公共事件"; break;
            case eventType.debugLog: t = "控制台输出"; break;
            case eventType.hidePic: t = "隐藏图片"; break;
            case eventType.movePic: t = "移动图片"; break;
            case eventType.playBgm: t = "播放BGM"; break;
            case eventType.playBgs: t = "播放BGS"; break;
            case eventType.playSE: t = "播放SE"; break;
            case eventType.showPic: t = "显示图片"; break;
            case eventType.startUserControl: t = "开启角色控制"; break;
            case eventType.stopBgm: t = "停止BGM"; break;
            case eventType.stopBgs: t = "停止BGS"; break;
            case eventType.stopUserControl: t = "关闭角色控制"; break;
            case eventType.wait: t = "等待"; break;
            case eventType.playAnima: t = "播放动画"; break;
            case eventType.moveCamera: t = "移动相机"; break;
            case eventType.sbMoveToSw: t = "设置移动路线"; break;
            case eventType.flashScreen: t = "闪烁屏幕"; break;
            case eventType.shakeScreen: t = "震动屏幕"; break;
            case eventType.changeWeather: t = "改变天气"; break;
            case eventType.showSavePanel: t = "打开存档界面"; break;
            case eventType.changeItem: t = "增减道具"; break;
            case eventType.changeThingPos: t = "设置物体位置"; break;
            case eventType.changeTurn: t = "设置人物朝向"; break;
            case eventType.changeThingActive: t = "设置物体可见性"; break;
            case eventType.startQuest: t = "任务开始"; break;
            case eventType.changeQuest: t = "更改任务状态"; break;
            case eventType.If: t = "如果"; break;
            default: t = "错误，未找到"; break;
        }
        return t;
    }

    public calValue howToCal;
    public IndependentSwitch independentSwitch;
    public turn turn;
    public weather weather;
    public findType findType;
    public questStatus questStatus;
    public conditionDictionary condition;

    public Transform helper;

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
    public int int2;

    public Vector2 vec2_1;
    public Vector2 vec2_2;

    public Color color1;
}

public enum calValue
{
    add,
    minus,
    multiply,
    divide,
    set
}

[System.Serializable]
public class eventStruct
{
    public eventListDefine eventList;
    // 是否可以反复执行
    public bool loop = false;
    //本事件现有进度
    [HideInInspector]
    public int thisNow = -1;
    //事件页是否完成
    [HideInInspector]
    public bool finish = false;
}

// 事件列表
[CreateAssetMenu]
public class eventListDefine : ScriptableObject
{
    // float误差
    public static float floatEpsilon = 1e-3f;

    // 事件页条件
    public conditionDictionary[] conditions;
    // 事件页触发方式
    public start howToStart;
    // 事件列表
    [HideInInspector]
    public List<myEvent> list;
    // 独立开关
    [HideInInspector]
    public bool[] independentSwitchs;

    eventListDefine()
    {
        list = new List<myEvent>();
    }

    // 检查条件是否满足
    public bool checkConditions()
    {
        bool checkBool = true;
        for (int i = 0; i < conditions.Length; i++)
        {
            bool b = checkCondition(conditions[i], independentSwitchs);

            if (conditions[i].thisConnect == connect.or)
                checkBool = (checkBool || b);
            else
                checkBool = (checkBool && b);
        }
        return checkBool;
    }


    public static bool checkCondition(conditionDictionary condition, bool[] independentSwitchs)
    {
        bool b = true;
        if (condition.type == conditionType.Switch)
        {
            bool a = dataManager.instance.getSwitch(condition.name);
            if (condition.compare == compare.equal)
            {
                if (Convert.ToBoolean(condition.value) == a)
                    b = true;
                else
                    b = false;
            }
            else
            {
                if (Convert.ToBoolean(condition.value) != a)
                    b = true;
                else
                    b = false;
            }
        }
        else if (condition.type == conditionType.IndependentSwitch)
        {
            bool a = false;
            switch (condition.name)
            {
                case "A": a = independentSwitchs[0]; break;
                case "B": a = independentSwitchs[1]; break;
                case "C": a = independentSwitchs[2]; break;
                case "D": a = independentSwitchs[3]; break;
                default: break;
            }
            if (condition.compare == compare.equal)
            {
                if (Convert.ToBoolean(condition.value) == a)
                    b = true;
                else
                    b = false;
            }
            else
            {
                if (Convert.ToBoolean(condition.value) != a)
                    b = true;
                else
                    b = false;
            }
        }
        else if (condition.type == conditionType.Int)
        {
            int a = dataManager.instance.getInt(condition.name);
            if (condition.compare == compare.equal)
            {
                if (Convert.ToInt32(condition.value) == a)
                    b = true;
                else
                    b = false;
            }
            else if (condition.compare == compare.larger)
            {
                if (a > Convert.ToInt32(condition.value))
                    b = true;
                else
                    b = false;
            }
            else if (condition.compare == compare.less)
            {
                if (a < Convert.ToInt32(condition.value))
                    b = true;
                else
                    b = false;
            }
            else
            {
                if (Convert.ToInt32(condition.value) != a)
                    b = true;
                else
                    b = false;
            }
        }
        else
        {
            float a = dataManager.instance.getFloat(condition.name);
            if (condition.compare == compare.equal)
            {
                if (Math.Abs(Convert.ToDouble(condition.value) - a) < floatEpsilon)
                    b = true;
                else
                    b = false;
            }
            else if (condition.compare == compare.larger)
            {
                if (a > Convert.ToDouble(condition.value))
                    b = true;
                else
                    b = false;
            }
            else if (condition.compare == compare.less)
            {
                if (a < Convert.ToDouble(condition.value))
                    b = true;
                else
                    b = false;
            }
            else
            {
                if (Math.Abs(Convert.ToDouble(condition.value) - a) > floatEpsilon)
                    b = true;
                else
                    b = false;
            }
        }

        return b;
    }

}
