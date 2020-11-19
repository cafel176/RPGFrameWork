using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum nowState
{
    auto,
    move,
    window,
    text
}

public enum language
{
    chinese,
    english,
    japanese
}

public enum keyInput
{
    up = KeyCode.UpArrow,
    down = KeyCode.DownArrow,
    left = KeyCode.LeftArrow,
    right = KeyCode.RightArrow,
    W = KeyCode.W,
    S = KeyCode.S,
    A = KeyCode.A,
    D = KeyCode.D,
    Z = KeyCode.Z,
    X = KeyCode.X,
    menu = KeyCode.Q,
    enter = KeyCode.Return,
    space = KeyCode.Space,
    esc = KeyCode.Escape,
    Lshift = KeyCode.LeftShift,
    Lctrl = KeyCode.LeftControl,
    Rshift = KeyCode.RightShift,
    Rctrl = KeyCode.RightControl
}

public struct changeSceneDo
{
    public int actorId;
    public Vector3 pos;
    public Vector3 turn;
}

[System.Serializable]
public struct publicEventDictionary
{
    public string name;
    public eventStruct eventList;
}

//音量
public struct settings
{
    public bool alwaysRun;
    public bool autoMessage;
    public float musicValue, SEValue;
    //窗口尺寸
    public int windowSize;
    public language nowlang;
}

public class gameManager : MonoBehaviour
{
    // 窗口宽高
    public int width = 512;
    public int height = 288;

    public int StartSize = 2;
    public int maxSize = 4;

    public int FPS = 60;//限帧

    public Vector3 startPos;

    public GameObject[] logos;
    public GameObject Mask;

    public publicEventDictionary[] commonEventLists;

    //=====================不可见变量===============================

    //管理器的实例
    public static gameManager instance;

    //选项
    [HideInInspector]
    public double playTime = 0.0;
    [HideInInspector]
    public settings setting;
    [HideInInspector]
    public float oriSize;
    [HideInInspector]
    public string nowScene = "MainScene";
    //记录当前输入状态
    public nowState nowstate = nowState.window;
    [HideInInspector]
    public HashsAndTags hat;
    //记录当前操作的角色的引用
    [HideInInspector]
    public Player player = null;

    [HideInInspector]
    public AudioSource musicAudio;
    [HideInInspector]
    public AudioSource BGSAudio;
    private AudioSource SEAudio;
    private bool canInput = false;

    //记录当前操作窗口的引用
    private keyBoardMenu nowWindow = null;

    private List<publicEvent> commonEvents = new List<publicEvent>();
    private changeSceneDo csd;

    //用于关闭bgm
    private float time = 1f;
    private bool finishBgm = true;
    private bool finishBgs = true;

    private AudioClip menu;

    private void Awake()
    {
        Application.targetFrameRate = FPS;

        Mask.SetActive(false);
        oriSize = StartSize;
        loadSetting();
        hat = gameObject.GetComponent<HashsAndTags>();

        if (gameManager.instance == null)
        {
            Screen.SetResolution(width * setting.windowSize, height * setting.windowSize, false);
            //创造管理器实例
            gameManager.instance = this;
            DontDestroyOnLoad(gameManager.instance.gameObject);
        }
        else if (gameManager.instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        AudioSource[] a = gameObject.GetComponents<AudioSource>();
        musicAudio = a[0];
        SEAudio = a[1];
        BGSAudio = a[2];
        musicAudio.volume = setting.musicValue;
        musicAudio.loop = true;
        BGSAudio.volume = setting.musicValue;
        BGSAudio.loop = true;
        SEAudio.volume = setting.SEValue;
        menu = gameManager.instance.hat.findAudio("确定");

        for (int i = 0; i < commonEventLists.Length; i++)
        {
            if(commonEventLists[i].eventList.eventList.howToStart== start.auto)
            {
                startPublicEvent(i);
            }
        }

        for (int i = 0; i < logos.Length; i++)
        {
            logos[i].SetActive(false);
        }
        StartCoroutine(splash());
    }

    IEnumerator splash()
    {
        for (int i = 0; i < logos.Length; i++)
        {
            logos[i].SetActive(true);
            UIManager.instance.beWhite(1.0f, true);
            yield return new WaitForSeconds(3f);
            UIManager.instance.beBlack(1.0f, true);
            yield return new WaitForSeconds(1f);
            logos[i].SetActive(false);
        }
        AudioClip title = gameManager.instance.hat.findAudio("标题");
        musicAudio.clip = title;
        musicAudio.Play();
        UIManager.instance.beWhite(1.0f, true);
        yield return new WaitForSeconds(0.8f);
        canInput = true;
    }

    // Update is called once per frame
    void Update()
    {
        playTime += Time.deltaTime;
        if (!finishBgm)
        {
            finishBgm = reduceMusicVolume(setting.musicValue * Time.deltaTime / time);
        }
        if (!finishBgs)
        {
            finishBgs = reduceBGSVolume(setting.musicValue * Time.deltaTime / time);
        }
        if (canInput)
        {
            checkInput();
        }
        checkCommonEvent();

        if(nowScene != SceneManager.GetActiveScene().name)
        {
            nowScene = SceneManager.GetActiveScene().name;
            if (csd.actorId != -1)
            {
                GameObject go = GameObject.Instantiate(dataManager.instance.playerInfos[csd.actorId].playerPrefab) as GameObject;
                go.transform.position = csd.pos;
                player = go.GetComponent<Player>();
                player.changeTurn(csd.turn);
                dataManager.instance.inTeam(csd.actorId);
                startUserControl(go);
            }
            else
                changeState(nowState.move);
            canInput = true;
            StartCoroutine(StartDo());
        }
    }

    private IEnumerator StartDo()
    {
        yield return new WaitForSeconds(0.3f);
        Mask.SetActive(false);
    }

    public void loadSetting()
    {
    //窗口尺寸
        if (PlayerPrefs.HasKey("windowSize"))
            setting.windowSize = PlayerPrefs.GetInt("windowSize");
        else
            setting.windowSize = StartSize;
        if (PlayerPrefs.HasKey("musicValue"))
            setting.musicValue = PlayerPrefs.GetFloat("musicValue");
        else 
            setting.musicValue = 0.5f;
        if (PlayerPrefs.HasKey("SEValue"))
            setting.SEValue = PlayerPrefs.GetFloat("SEValue");
        else
            setting.SEValue = 1;
        if (PlayerPrefs.HasKey("alwaysRun"))
            setting.alwaysRun = (PlayerPrefs.GetInt("alwaysRun") == 0 ? true : false);
        else
            setting.alwaysRun = true;
        if (PlayerPrefs.HasKey("autoMessage"))
            setting.autoMessage = (PlayerPrefs.GetInt("autoMessage") == 0 ? true : false);
        else
            setting.autoMessage = false;
        if (PlayerPrefs.HasKey("language"))
            setting.nowlang = (language)PlayerPrefs.GetInt("language");
        else
            setting.nowlang = language.chinese;
    }

    public void saveSetting()
    {
        PlayerPrefs.SetFloat("musicValue", gameManager.instance.setting.musicValue);
        PlayerPrefs.SetFloat("SEValue", gameManager.instance.setting.SEValue);
        PlayerPrefs.SetInt("windowSize", gameManager.instance.setting.windowSize);
        PlayerPrefs.SetInt("alwaysRun", gameManager.instance.setting.alwaysRun?0:1);
        PlayerPrefs.SetInt("autoMessage", gameManager.instance.setting.autoMessage ? 0 : 1);
        PlayerPrefs.SetInt("language", (int)gameManager.instance.setting.nowlang);
    }

    //用于改变当前的交互状态
    public void changeState(nowState state, GameObject _object = null)
    {
        nowstate = state;
        if (state == nowState.window)
        {
            if (_object != null)
            {
                nowWindow = _object.GetComponent<keyBoardMenu>();
                nowWindow.panel.SetActive(true);
                nowWindow.enabled = true;

            }
        }
        else if (state == nowState.move)
        {
            if (_object != null)
                player = _object.GetComponent<Player>();
            if(player != null)
            {
                CameraFollow c = Camera.main.GetComponent<CameraFollow>();
                if (c != null)
                    c.player = player.gameObject;
            }

        }
    }
    
    //玩家输入控制
    void checkInput()
    {
        if (nowstate == nowState.window)
        {
            if (nowWindow != null)
            {
                if (Input.GetKeyDown((KeyCode) keyInput.up)|| Input.GetKeyDown((KeyCode)keyInput.W))
                {
                    nowWindow.up();
                }
                else if (Input.GetKeyDown((KeyCode)keyInput.down)|| Input.GetKeyDown((KeyCode)keyInput.S))
                {
                    nowWindow.down();
                }
                else if (Input.GetKeyDown((KeyCode)keyInput.left)|| Input.GetKeyDown((KeyCode)keyInput.A))
                {
                    nowWindow.left();
                }
                else if (Input.GetKeyDown((KeyCode)keyInput.right)|| Input.GetKeyDown((KeyCode)keyInput.D))
                {
                    nowWindow.right();
                }
                else if (Input.GetKeyDown((KeyCode)keyInput.Z) || Input.GetKeyDown((KeyCode)keyInput.enter) || Input.GetKeyDown((KeyCode)keyInput.space))
                {
                    nowWindow.doOption();
                }
                else if (Input.GetKeyDown((KeyCode)keyInput.X) || Input.GetKeyDown((KeyCode)keyInput.esc))
                {
                    nowWindow.cancel();
                }
            }
        }
        else if (nowstate == nowState.move)
        {
            if (player != null)
            {

                    bool run = false;
                    if(setting.alwaysRun)
                    {
                        run = true;
                        if (Input.GetKey((KeyCode)keyInput.Lshift)|| Input.GetKey((KeyCode)keyInput.Rshift))
                            run = false;
                    }
                    else
                    {
                        run = false;
                        if (Input.GetKey((KeyCode)keyInput.Lshift) || Input.GetKey((KeyCode)keyInput.Rshift))
                            run = true;
                    }
                    bool rush = (Input.GetKeyDown((KeyCode)keyInput.Lctrl)|| Input.GetKeyDown((KeyCode)keyInput.Rctrl));
                    if (Input.GetKey((KeyCode)keyInput.up)|| Input.GetKey((KeyCode)keyInput.W))
                    {
                        player.Move(new Vector2(0, 1), run, rush);
                    }
                    else if (Input.GetKey((KeyCode)keyInput.down)|| Input.GetKey((KeyCode)keyInput.S))
                    {
                        player.Move(new Vector2(0, -1), run, rush);
                    }
                    else if (Input.GetKey((KeyCode)keyInput.left)|| Input.GetKey((KeyCode)keyInput.A))
                    {
                        player.Move(new Vector2(-1, 0), run, rush);
                    }
                    else if (Input.GetKey((KeyCode)keyInput.right)|| Input.GetKey((KeyCode)keyInput.D))
                    {
                        player.Move(new Vector2(1, 0), run, rush);
                    }
                    else
                    {
                        player.Move(Vector2.zero, run, rush);
                    }

                    if (Input.GetKeyDown((KeyCode)keyInput.Z)|| Input.GetKeyDown((KeyCode)keyInput.enter)|| Input.GetKeyDown((KeyCode)keyInput.space))
                    {

                    }
                    else if (Input.GetKeyDown((KeyCode)keyInput.menu) || Input.GetKeyDown((KeyCode)keyInput.esc))
                    {
                        player.Move(Vector2.zero, run, rush);
                        playSE(menu);
                        UIManager.instance.showMenuPanel();
                    }
            }
        }
    }

    public void startUserControl(GameObject _player = null)
    {
        gameManager.instance.changeState(nowState.move, _player);
    }

    //停止玩家控制，不卡进程
    public void stopUserControl()
    {
        gameManager.instance.changeState(nowState.auto);
    }

    //0代表首页
    public void LoadLevel(string level, int actorId=-1, Vector3 pos = default(Vector3), 
        Vector3 turn = default(Vector3), bool stopMusic = true)
    {
        canInput = false;
        UIManager.instance.beBlack(1.0f, true);
        UIManager.instance.hideTextPanel();
        if (stopMusic)
            finishBgm = false;
        stopSE();

        csd.actorId = actorId;
        csd.pos = pos;
        csd.turn = turn;

        StartCoroutine(StartLoading(level));
    }

    private IEnumerator StartLoading(string scene)
    {
        yield return new WaitForSeconds(1);
        Mask.SetActive(true);
        nowScene = string.Empty;

        /*
         * //异步加载
        AsyncOperation op = SceneManager.LoadSceneAsync(scene);
        op.allowSceneActivation = false;
        while(op.progress < 0.9f)
        {
        }
        op.allowSceneActivation = true;
        */

        // 直接加载
        SceneManager.LoadScene(scene);
    }

    public language changeLanguage(bool add)
    {
        int j = (int)setting.nowlang;
        int length = System.Enum.GetValues(typeof(language)).Length;
        if (add)
            return (language)((j + 1)% length);
        else
            return (language)((j - 1 + length) % length);
    }

    public void changeBool(string key)
    {
        if(key=="alwaysrun")
        {
            setting.alwaysRun = !setting.alwaysRun;
        }
        else if(key == "autoMessage")
        {
            setting.autoMessage = !setting.autoMessage;
        }
    }

    //改变游戏窗口尺寸
    public void addWindowSize(bool add)
    {
        if (add)
            setting.windowSize = setting.windowSize % maxSize + 1;
        else
            setting.windowSize = (setting.windowSize - 1) > 0 ? setting.windowSize - 1 : maxSize;
        if (setting.windowSize == maxSize)
            Screen.SetResolution(Screen.width, Screen.height, true);
        else
            Screen.SetResolution(width * setting.windowSize, height * setting.windowSize, false);
        nowWindow.changeSize();
    }

    //设置音乐音量
    public void changeMusicValue(float num)
    {
        setting.musicValue += num;
        musicAudio.volume = setting.musicValue;
        BGSAudio.volume = setting.musicValue;
    }

    //改变音乐音量，用于淡入淡出
    public bool reduceMusicVolume(float num)
    {
        if (musicAudio.volume > 0)
        {
            musicAudio.volume -= num * setting.musicValue;
            return false;
        }
        else
        {
            musicAudio.Stop();
            musicAudio.volume = setting.musicValue;
            return true;
        }
    }

    public bool reduceBGSVolume(float num)
    {
        if (BGSAudio.volume > 0)
        {
            BGSAudio.volume -= num * setting.musicValue;
            return false;
        }
        else
        {
            BGSAudio.Stop();
            BGSAudio.volume = setting.musicValue;
            return true;
        }
    }

    //改变音效音量
    public void changeSEValue(float num)
    {
        setting.SEValue += num;
        SEAudio.volume = setting.SEValue;
    }

    public void playMusic(AudioClip clip)
    {
        if (musicAudio.isPlaying)
        {
            musicAudio.Stop();
            musicAudio.volume = setting.musicValue;
            finishBgm = true;
        }

        musicAudio.clip = clip;
        gameManager.instance.changeMusicValue(0);
        musicAudio.Play();
    }

    public void stopMusic()
    {
        musicAudio.Stop();
    }

    public void playBGS(AudioClip clip)
    {
        if (BGSAudio.isPlaying)
        {
            BGSAudio.Stop();
            BGSAudio.volume = setting.musicValue;
            finishBgs = true;
        }

        BGSAudio.clip = clip;
        gameManager.instance.changeMusicValue(0);
        BGSAudio.Play();
    }

    public void stopBGS()
    {
        BGSAudio.Stop();
    }

    public void playSE(AudioClip clip,float pitch=1.0f, bool loop = false)
    {
        SEAudio.loop = loop;
        SEAudio.pitch = pitch;
        SEAudio.clip = clip;
        SEAudio.Play();
    }

    public void stopSE()
    {
        SEAudio.loop = false;
        SEAudio.Stop();
    }

    // 开始进行公共事件
    public void startPublicEvent(int index)
    {
        var e = gameObject.AddComponent<publicEvent>();
        e.startEvent(index);
        commonEvents.Add(e);
    }

    public void startPublicEvent(string name)
    {
        var e = gameObject.AddComponent<publicEvent>();
        e.startEvent(name);
        commonEvents.Add(e);
    }

    // 循环检查公共事件
    private void checkCommonEvent()
    {
        for(int i=0;i<commonEvents.Count;i++)
        {
            if(commonEvents[i].nowList.thisNow==-2)
            {
                commonEvents[i].nowList.thisNow = -1;
                commonEvents.Remove(commonEvents[i]);
                i--;
            }
            else
            {
                commonEvents[i].canDo = commonEvents[i].nowList.eventList.checkConditions();
            }
        }
    }

    // 获取公共事件
    public publicEventDictionary getCommonEvent(int index)
    {
        return commonEventLists[index];
    }

    public publicEventDictionary getCommonEvent(string name)
    {
        for (int i = 0; i < commonEventLists.Length; i++)
        {
            if (commonEventLists[i].name == name)
            {
                return commonEventLists[i];
            }
        }
        return new publicEventDictionary();
    }
}
