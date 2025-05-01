using BaseData;
using System;
using System.Collections.Generic;
using UnityEngine;

// =================================== enums ===================================

public enum SpecialValue
{
    all = -1
}

public enum questType
{
    all = SpecialValue.all,
    main,
    side,
    hidden
}

public enum itemType
{
    all = SpecialValue.all,
    item,
    notes,
    food,
    gifts
}

public enum questStatus
{
    doing,
    finished,
    failed,
    all
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

public enum calValue
{
    add,
    minus,
    multiply,
    divide,
    set
}

public enum findType
{
    name,
    tag
}

public enum IndependentSwitch
{
    switchA,
    switchB,
    switchC,
    switchD
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
    changeGlobalDouble,
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
    changeTurn2D,
    startQuest,
    changeQuest,
    If,
    setFollow,
    changeTeam,
    changeGlobalVector3,
    choicePanel,
    playVideo,
    changeTurn3D,
    actorPlayAnima,
    enableAttack,
    disableAttack,
    showButton,
    hideButton
}

public enum actorComponentType
{
    basicMove2D,
    basicBattle2D,
    followBehaviour2D,
    basicMove3D,
    basicBattle3D,
    basicMove3DFPS,
    basicBattle3DFPS
}

public enum conditionType
{
    Switch,
    Int,
    Float,
    IndependentSwitch
}

// 出现条件
public enum connect
{
    and,
    or
}

public enum compare
{
    larger,
    less,
    equal,
    notEqual
}

// =================================== hashs ===================================

public class dictionaryName
{
    public static string before = "_";

    // gameDic
    public static string items = "items";
    public static string quests = "quests";

    //dataDic
    public static string ints = "ints";
    public static string doubles = "doubles";
    public static string switchs = "switchs";
    public static string vec3s = "vec3s";

    //others
    public static string video = "video";
    public static string audio = "audio";
    public static string image = "image";
    public static string prefab = "prefab";
}

public class propertyName
{
    public static string name = dictionaryName.before + "name";
    public static string type = dictionaryName.before + "type";
    public static string value = dictionaryName.before + "value";
    public static string pitch = dictionaryName.before + "pitch";
}

public class itemProperty
{
    public static string name = dictionaryName.before + "name";
    public static string text = dictionaryName.before + "text";
    public static string img = dictionaryName.before + "img";
    public static string type = dictionaryName.before + "type";
    public static string times = dictionaryName.before + "times";
    public static string commonEvent = dictionaryName.before + "commonEvent";
    public static string eventNum = dictionaryName.before + "eventNum";
}

public class questProperty
{
    public static string name = dictionaryName.before + "name";
    public static string text = dictionaryName.before + "text";
    public static string img = dictionaryName.before + "img";
    public static string type = dictionaryName.before + "type";
    public static string status = dictionaryName.before + "status";
    public static string hard = dictionaryName.before + "hard";
    public static string steps = dictionaryName.before + "steps";
    public static string stepNum = dictionaryName.before + "stepNum";
}

public class structProperty
{
    public static string type = dictionaryName.before + "type";
    public static string helper = dictionaryName.before + "helper";

    public static string float1 = dictionaryName.before + "float1";
    public static string float2 = dictionaryName.before + "float2";
    public static string float3 = dictionaryName.before + "float3";
    public static string float4 = dictionaryName.before + "float4";

    public static string str1 = dictionaryName.before + "str1";
    public static string str2 = dictionaryName.before + "str2";
    public static string str3 = dictionaryName.before + "str3";
    public static string str4 = dictionaryName.before + "str4";

    public static string int1 = dictionaryName.before + "int1";

    public static string bool1 = dictionaryName.before + "bool1";
    public static string bool2 = dictionaryName.before + "bool2";
    public static string bool3 = dictionaryName.before + "bool3";
    public static string bool4 = dictionaryName.before + "bool4";

    public static string img1 = dictionaryName.before + "img1";
    public static string img2 = dictionaryName.before + "img2";
    public static string img3 = dictionaryName.before + "img3";

    public static string vec1 = dictionaryName.before + "vec1";
    public static string vec2 = dictionaryName.before + "vec2";

    public static string color1 = dictionaryName.before + "color1";

    public static string howToCal = dictionaryName.before + "howToCal";
    public static string turn = dictionaryName.before + "turn";
    public static string findType = dictionaryName.before + "findType";
    public static string questStatus = dictionaryName.before + "questStatus";
    public static string independentSwitch = dictionaryName.before + "independentSwitch";
    public static string compare = dictionaryName.before + "compare";
    public static string conditionType = dictionaryName.before + "conditionType";
}

// =================================== structs ===================================

[System.Serializable]
public class eventStruct
{
    public dataTree eventList;

    // 是否可以反复执行
    public bool loop = false;

    public EventListInterface EventListInterface
    {
        get
        {
            return (EventListInterface)eventList;
        }
    }

    //本事件现有进度
    private TreeNodeInterface thisNow = null;
    public TreeNodeInterface ThisNow
    {
        get
        {
            return thisNow;
        }

        set
        {
            thisNow = value;
        }
    }

    //事件页是否完成
    private bool finish = false;
    public bool Finish
    {
        get
        {
            return finish;
        }

        set
        {
            finish = value;
        }
    }

    //事件页是否完成
    private bool remove = false;
    public bool Remove
    {
        get
        {
            return remove;
        }

        set
        {
            remove = value;
        }
    }

    public void reset()
    {
        thisNow = null;
        finish = false;
        remove = false;
    }
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

// =================================== interfaces ===================================

public class turnTool
{
    public static turn get(Vector3 v)
    {
        return get(new Vector2Int((int)v.x, (int)v.y));
    }

    public static turn get(Vector2Int turn)
    {
        if (turn == new Vector2Int(0, -1))
            return global::turn.down;
        else if (turn == new Vector2Int(0, 1))
            return global::turn.up;
        else if (turn == new Vector2Int(-1, 0))
            return global::turn.left;
        else if (turn == new Vector2Int(1, 0))
            return global::turn.right;
        else
            return global::turn.all;
    }

    public static Vector2Int get(turn t)
    {
        if (t == global::turn.down)
            return new Vector2Int(0, -1);
        else if (t == global::turn.up)
            return new Vector2Int(0, 1);
        else if (t == global::turn.left)
            return new Vector2Int(-1, 0);
        else if (t == global::turn.right)
            return new Vector2Int(1, 0);
        else
            return Vector2Int.zero;
    }
}

public interface dataNodeInterface
{
    Type getType(string name);
    object getData(string name);
    T getData<T>(string name);
}

public interface ListNodeInterface : dataNodeInterface
{
    string getId();
    int getNodeType();
}

public interface TreeNodeInterface : ListNodeInterface
{
    TreeNodeInterface getChild(string id);
    void addChild(TreeNodeInterface c);
    bool removeChild(TreeNodeInterface c);
    List<TreeNodeInterface> getChildren();

    TreeNodeInterface getParent(string id);
    void addParent(TreeNodeInterface p);
    bool removeParent(TreeNodeInterface c);
    List<TreeNodeInterface> getParents();
      
}

public class dataList : ScriptableObject
{
    protected List<ListNodeInterface> list = new List<ListNodeInterface>();

    protected Func toInterfaceFunc;

    public void init()
    {
        if (toInterfaceFunc != null)
            toInterfaceFunc();
    }

    public List<ListNodeInterface> getList()
    {
        return list;
    }

    public ListNodeInterface getListNode(string id)
    {
        foreach (var i in list)
        {
            if (i.getId() == id)
                return i;
        }
        return null;
    }
}

public class dataTree : ScriptableObject
{
    protected TreeNodeInterface root;

    protected Func toInterfaceFunc;

    public void init()
    {
        if (toInterfaceFunc != null)
            toInterfaceFunc();
    }
    public TreeNodeInterface getRoot()
    {
        return root;
    }
}

public interface EventListInterface
{
    start getHowToStart();
    bool[] getIndependentSwitchs();
    void setIndependentSwitchs(bool[] ind);
    bool checkEventConditions();
    bool checkCondition(conditionDictionary condition, bool[] independentSwitchs);
}

