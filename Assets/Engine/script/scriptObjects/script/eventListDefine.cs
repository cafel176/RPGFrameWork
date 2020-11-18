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
    _switch,
    _int,
    _float
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
    public string name;
    public connect thisConnect;
    public conditionType type;
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

//事件类型
// 加的时候从尾部加，否则会把之前做好的顺序打乱
public enum eventType
{
    none,
    reUseEvent,
    text,
    wait,
    beBlack,
    beWhite,
    startUserControl,
    stopUserControl,
    changeGlobalSwith,
    changeGlobalInt,
    changeGlobalFloat
}

[System.Serializable]
public class eventStruct
{
    public eventListDefine eventList;
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
    // 事件页条件
    public conditionDictionary[] conditions;
    // 事件页触发方式
    public start howToStart;
    // float误差
    public float floatEpsilon = 1e-3f;
    // 事件列表
    [HideInInspector]
    public List<myEvent> list;

    // 检查条件是否满足
    public bool checkCondition()
    {
        bool checkBool = true;
        for (int i = 0; i < conditions.Length; i++)
        {
            bool b = true;
            if (conditions[i].type == conditionType._switch)
            {
                bool a = dataManager.instance.getSwitch(conditions[i].name);
                if (conditions[i].compare == compare.equal)
                {
                    if (Convert.ToBoolean(conditions[i].value) == a)
                        b = true;
                    else
                        b = false;
                }
                else
                {
                    if (Convert.ToBoolean(conditions[i].value) != a)
                        b = true;
                    else
                        b = false;
                }
            }
            else if (conditions[i].type == conditionType._int)
            {
                int a = dataManager.instance.getInt(conditions[i].name);
                if (conditions[i].compare == compare.equal)
                {
                    if (Convert.ToInt32(conditions[i].value) == a)
                        b = true;
                    else
                        b = false;
                }
                else if (conditions[i].compare == compare.larger)
                {
                    if (a > Convert.ToInt32(conditions[i].value))
                        b = true;
                    else
                        b = false;
                }
                else if (conditions[i].compare == compare.less)
                {
                    if (a < Convert.ToInt32(conditions[i].value))
                        b = true;
                    else
                        b = false;
                }
                else
                {
                    if (Convert.ToInt32(conditions[i].value) != a)
                        b = true;
                    else
                        b = false;
                }
            }
            else
            {
                float a = dataManager.instance.getFloat(conditions[i].name);
                if (conditions[i].compare == compare.equal)
                {
                    if (Math.Abs(Convert.ToDouble(conditions[i].value) - a) < floatEpsilon)
                        b = true;
                    else
                        b = false;
                }
                else if (conditions[i].compare == compare.larger)
                {
                    if (a > Convert.ToDouble(conditions[i].value))
                        b = true;
                    else
                        b = false;
                }
                else if (conditions[i].compare == compare.less)
                {
                    if (a < Convert.ToDouble(conditions[i].value))
                        b = true;
                    else
                        b = false;
                }
                else
                {
                    if (Math.Abs(Convert.ToDouble(conditions[i].value) - a) > floatEpsilon)
                        b = true;
                    else
                        b = false;
                }
            }

            if (conditions[i].thisConnect == connect.or)
                checkBool = (checkBool || b);
            else
                checkBool = (checkBool && b);
        }
        return checkBool;
    }
}

// 事件基类
[System.Serializable]
public class myEvent
{
    public eventType type;
    //设置名字和文本
    public string _name, text;
    public string sp;
    public bool showFace = true;

    public float time;

    public string key;
    public calValue howToCal;
    public int globalInt;
    public float globalFloat;
    public bool globalSwitch;
}

public enum calValue
{
    add,
    minus,
    multiply,
    divide,
    set
}