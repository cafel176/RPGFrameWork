using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ManagerSpace;

// =================================== delegates ===================================

public delegate void commonEventCallback();

public delegate void changeSettingCallbaks(settings set);

public delegate void changeStateCallbaks(nowState ns);

// =================================== enums ===================================

public enum changeSceneType
{
    immediately,
    fromBlack,
    fromWhite
}

public enum controlMode
{
    control_2D,
    control_3DFPS,
    control_3D
}

public enum language
{
    chinese,
    english,
    japanese
}

public enum nowState
{
    none = -1,
    auto,
    move,
    window,
    text
}

public enum mouseInput
{
    leftDown,
    rightDown,
    midDown,
    left,
    right,
    mid
}

public enum keyInput
{
    up,
    down,
    left,
    right,
    upOnce,
    downOnce,
    leftOnce,
    rightOnce,
    confirm,
    cancel,
    menu,
    shift,
    ctrl,
    jump
}

public enum splashType
{
    wait,
    confirm,
    button
}

// =================================== structs ===================================

public class symbols
{
    // #v3(id)
    public static string search = @"#(\w)*[(](\w)*[)]";

    //dataDic
    public static string ints = "#i";
    public static string doubles = "#d";
    public static string switchs = "#s";
    public static string vec3s= "#v3";
}

[System.Serializable]
public class systemSetting
{
    public int width = 512;
    public int height = 288;
    public int startSize = 2;
    public int maxSize = 4;
    public int FPS = 60;//限帧

    public float distanceEpsilon = 1e-2f;
    public float moveEpsilon = 1e-3f;
    public float followEpsilon = 0.1f;
    public float runFollowDis = 0.5f;

    public PlayerInfo[] PlayerInfos;
}

[System.Serializable]
public class settings
{
    public bool alwaysRun = true;
    public bool autoMessage = false;
    public float musicValue = 0.5f;
    public float SEValue = 1.0f;
    public int windowSize = 1;
    public language nowlang = language.chinese;

    public static string[] keys = { "alwaysRun", "autoMessage", "windowSize", "musicValue", "SEValue", "nowlang" };
}

[System.Serializable]
public class logoSetting
{
    public GameObject obj;
    public splashType type;
    public Button btn;
}

[System.Serializable]
public class gameSetting
{
    public bool canAttack = true;
}

[System.Serializable]
public struct PlayerInfo
{
    public string name;
    public GameObject playerPrefab;
    public Sprite face;
    public Sprite avator;
}

public class changeSceneDo
{
    public int actorId = -1;
    public Vector3 pos = Vector3.zero;
    public Vector3 rotate = Vector3.zero;
    public turn turn = turn.all;
    public changeSceneType changeType = changeSceneType.fromBlack;

    public changeSceneDo(int actor,Vector3 pos, Vector3 rotate,turn turn, changeSceneType changeType = changeSceneType.fromBlack)
    {
        actorId = actor;
        this.pos = pos;
        this.rotate = rotate;
        this.turn = turn;
        this.changeType = changeType;
    }

    public changeSceneDo(changeSceneType changeType = changeSceneType.fromBlack)
    {
        actorId = -1;
        this.changeType = changeType;
    }
}

public class gameDictionary
{
    public Dictionary<string, object> data = new Dictionary<string, object>();

    public gameDictionary(Dictionary<string, object> data)
    {
        this.data = data;
    }
}

public class dataDictionary
{
    public Dictionary<string, object> data = new Dictionary<string, object>();

    public dataDictionary(Dictionary<string, object> data)
    {
        this.data = data;
    }
}

public class saveData
{
    public string fileName;
    public string version;
    public double playTime;

    //全局变量
    public List<string> dataDics;

    public List<string> gameDics;

    public Dictionary<string, bool[]> independentSwitches = new Dictionary<string, bool[]>();

    //队伍
    public List<int> team;
    public vector3d position;
    public string mapName;
    public vector3d turn;
}

// =================================== interfaces ===================================

public class MIFactory
{
    public static managerInterface getMI()
    {
        return Managers.instance;
    }
}

public interface managerInterface
{
    // gameManager
    void setNowStartSetting(int i);
    systemSetting getSystemSetting();
    gameSetting getGameSetting();
    int getOriSize();
    settings getSetting();
    HashsAndTags getHat();

    void LoadLevel(string level, changeSceneDo csd, bool stopMusic = true);
    void startGame();
    void changeLanguage(bool add);
    void changeBool(string key);
    void addWindowSize(bool add);
    void saveSetting();
    void addToSettingCallbacks(changeSettingCallbaks cb);

    void startPublicEvent(string name, nowState ns, commonEventCallback c=null, Component cm = null);
    void showSplach(logoSetting[] logos, Func call);
    bool[] getIndependentSwitchs(string key);
    void setIndependentSwitchs(string key, int index, bool ind);
    bool useItem(string id, commonEventCallback c = null);
    bool getCanEventsDo();
    void getItem(string id, int num);
    void removeItem(string id, int num);
    void deleteFile(string fileName);
    void saveData(string fileName);
    void loadData(string fileName);
    void deleteData(string fileName);
    saveData readFile(string fileName);
    void addQuest(string id);
    void changeQuest(string id, questStatus type);
    void inTeam(int id);
    void outTeam(int id);
    Dictionary<string, int> getItems();
    Dictionary<string, int> getQuests();
    List<int> getTeam();

    //dataManager
    eventStruct getCommonEvent(string name);
    int getTotalItemNum(itemType type);
    int getTotalQuestNum(questStatus type);
    ListNodeInterface getItemInfo(string key);
    ListNodeInterface getQuestInfo(string key);
    int getInt(string key);
    double getDouble(string key);
    bool getSwitch(string key);
    Vector3 getVec3(string key);
    void setInt(string key, int v);
    void setDouble(string key, double v);
    void setSwitch(string key, bool v);
    void setVec3(string key, Vector3 v);
    GameObject findPrefab(string id);
    AudioClip findAudio(string id);
    float findPitch(string id);
    Sprite findImg(string id);
    string findText(string id, language lang);

    //ControlManager
    controlMode GetControlMode();
    nowState getNowState();
    GameObject getPlayer();
    List<keyInput> getInputs();
    List<mouseInput> getMouseInputs();

    void startUserControl(GameObject _player = null);
    void stopUserControl();
    void changeState(nowState state, GameObject _object = null);

    // UIManager
    void showGameUI();
    void hideGameUI();
    void showVideoPanel(string name, Func call);    
    void enableAttack();
    void disableAttack();
    bool getShowFinish();
    bool getCanHind();
    GameObject getMainMenu();
    void setMainMenu(GameObject menu);

    void showEventChoosePanel(string maintext, Func[] choices, string[] texts, nowState nowstate, GameObject mainPanel = null, bool black = false);
    void showChoosePanel(string text, Func yes, Func no, nowState ns, GameObject mainPanel = null, bool black = false);
    GameObject showAnyPanel(GameObject _panel, Vector2 _pos, bool mainUI, nowState ns);
    GameObject showPicPanel(string index, Vector2 _pos, Sprite sp, float a);
    GameObject showButton(string index, Vector2 _pos, GameObject btn, Func call);
    void showHint(string text);
    void showTextPanel(Component _object, string name, string text, Sprite sprite, AudioClip se = null, float p = 1.0f, bool _new = false);
    void showSpecialTextPanel(Component _object, string text, Vector2 pos, AudioClip se = null, float p = 1.0f);
    void showSavePanel(Func call);
    GameObject showCanDoHint(Vector3 _pos, float scale);
    GameObject showFlash(Color c, float time);

    void hideTextPanel();
    void hidePicPanel(string index);
    void hideButton(string index);

    void movePicPanel(string index, Vector2 pos, float a, float time);
    void beWhite(float time, bool black);
    void beBlack(float time, bool black);
    bool checkTextRequet(Component _object);
    void skip();


    // audioManager
    void playSE(string name, float pitch = 1.0f, bool loop = false);
    void playMusic(string name);
    void playBGS(string name);
    void stopMusic(bool now = false);
    void stopBGS(bool now = false);
    bool reduceBGSVolume(float num);
    bool reduceMusicVolume(float num);
    void changeMusicValue(float num);
    void changeSEValue(float num);
    void setMusicValue(float v);
    void setSEValue(float v);

    // scenesManager
    string getNowScene();
    void backToMain();
}
