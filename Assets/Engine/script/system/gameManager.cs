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
    english
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
    enter = KeyCode.Return,
    space = KeyCode.Space,
    esc = KeyCode.Escape,
    Lshift = KeyCode.LeftShift,
    Lctrl = KeyCode.LeftControl,
    Rshift = KeyCode.RightShift,
    Rctrl = KeyCode.RightControl
}

//音量
public struct settings
{
    public bool alwaysRun;
    public bool commandRemember;
    public float musicValue, SEValue;
    //窗口尺寸
    public int windowSize;
    public language nowlang;
}

public class gameManager : MonoBehaviour
{
    // 窗口宽高
    public int width = 384;
    public int height = 192;

    public GameObject[] logos;

    //=====================不可见变量===============================

    //管理器的实例
    public static gameManager instance;

    //选项
    [HideInInspector]
    public double playTime = 0.0;
    [HideInInspector]
    public settings setting;
    [HideInInspector]
    public float oriSize = 3.0f;
    //记录当前输入状态
    [HideInInspector]
    public nowState nowstate = nowState.window;
    [HideInInspector]
    public HashsAndTags hat;
    [HideInInspector]
    public publicEvent publicevent;
    //记录当前操作的角色的引用
    [HideInInspector]
    public Player player = null;

    [HideInInspector]
    public AudioSource musicAudio;
    private AudioSource SEAudio;
    private bool canInput = false;

    //记录当前操作窗口的引用
    private keyBoardMenu nowWindow = null;

    private bool canControl = true;

    private void Awake()
    {
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
        musicAudio.volume = setting.musicValue;
        musicAudio.loop = true;
        SEAudio.volume = setting.SEValue;
        publicevent = gameObject.GetComponent<publicEvent>();

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
            UIManager.instance.beWhite(0.02f, true);
            yield return new WaitForSeconds(3f);
            UIManager.instance.beBlack(0.02f, true);
            yield return new WaitForSeconds(1f);
            logos[i].SetActive(false);
        }
        musicAudio.Play();
        UIManager.instance.beWhite(0.02f, true);
        canInput = true;
    }

    // Update is called once per frame
    void Update()
    {
        playTime += Time.deltaTime;
        if (canInput)
        {
            checkInput();
        }
    }

    public void loadSetting()
    {
    //窗口尺寸
        if (PlayerPrefs.HasKey("windowSize"))
            setting.windowSize = PlayerPrefs.GetInt("windowSize");
        else
            setting.windowSize = 3;
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
        if (PlayerPrefs.HasKey("commandRemember"))
            setting.commandRemember = (PlayerPrefs.GetInt("commandRemember") == 0 ? true : false);
        else
            setting.commandRemember = true;
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
        PlayerPrefs.SetInt("commandRemember", gameManager.instance.setting.commandRemember ? 0 : 1);
        PlayerPrefs.SetInt("language", (int)gameManager.instance.setting.nowlang);
    }

    //用于改变当前的交互状态
    public void changeState(nowState state, GameObject _object = null)
    {
        nowstate = state;
        if (state == nowState.window)
        {
            if (_object != null)
                nowWindow = _object.GetComponent<keyBoardMenu>();
        }
        else if (state == nowState.move)
        {
            if (_object != null)
                player = _object.GetComponent<Player>();
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
                if (canControl)
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
                    else if (Input.GetKeyDown((KeyCode)keyInput.X)|| Input.GetKeyDown((KeyCode)keyInput.esc))
                    {
                        player.Move(Vector2.zero, run, rush);
                        UIManager.instance.showMenuPanel();
                    }
                }
            }
        }
    }

    public void startUserControl(GameObject _player = null)
    {
        gameManager.instance.changeState(nowState.move, _player);
        canControl = true;
    }

    //停止玩家控制，不卡进程
    public void stopUserControl()
    {
        gameManager.instance.changeState(nowState.auto);
        canControl = false;
    }

    //0代表首页
    public void LoadLevel(int level, int actorId=-1, Vector3 pos = default(Vector3), 
        Vector3 turn = default(Vector3), bool stopMusic = true)
    {
        canInput = false;
        UIManager.instance.beBlack(0.02f, true);
        UIManager.instance.hideTextPanel();
        stopSE();

        StartCoroutine(StartLoading(level, actorId, pos,turn));
    }

    private IEnumerator StartLoading(int scene, int actorId, Vector3 pos, Vector3 turn)
    {
        yield return new WaitForSeconds(1);
        AsyncOperation op = SceneManager.LoadSceneAsync(scene);
        op.allowSceneActivation = false;
        while (op.progress < 0.9f)
        {
        }
        op.allowSceneActivation = true;
        yield return new WaitForSeconds(0.03f);//这个时间需要耐心调整
        UIManager.instance.beWhite(0.02f, true);

        if(actorId!=-1)
        {
            GameObject go = GameObject.Instantiate(dataManager.instance.playerPrefabs[actorId]) as GameObject;
            go.transform.position = pos;
            player = go.GetComponent<Player>();
            player.changeTurn(turn);
            dataManager.instance.inTeam(actorId);
            startUserControl(go);
        }
        else
            changeState(nowState.move);
        canInput = true;
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
        else if(key == "commandremember")
        {
            setting.commandRemember = !setting.commandRemember;
        }
    }

    //改变游戏窗口尺寸
    public void addWindowSize(bool add)
    {
        if (add)
            setting.windowSize = setting.windowSize % 5 + 1;
        else
            setting.windowSize = (setting.windowSize - 1) > 0 ? setting.windowSize - 1 : 5;
        if (setting.windowSize == 5)
            Screen.SetResolution(width * setting.windowSize, height * setting.windowSize, true);
        else
            Screen.SetResolution(width * setting.windowSize, height * setting.windowSize, false);
        nowWindow.changeSize();
    }

    //设置音乐音量
    public void changeMusicValue(float num)
    {
        setting.musicValue += num;
        musicAudio.volume = setting.musicValue;
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

    //改变音效音量
    public void changeSEValue(float num)
    {
        setting.SEValue += num;
        SEAudio.volume = setting.SEValue;
    }

    public void playMusic(AudioClip clip)
    {
        if (!musicAudio.isPlaying)
        {
            musicAudio.clip = clip;
            gameManager.instance.changeMusicValue(0);
            musicAudio.Play();
        }
    }

    public void stopMusic()
    {
        musicAudio.Stop();
    }

    public void playSE(AudioClip clip, bool loop = false)
    {
        SEAudio.loop = loop;
        SEAudio.clip = clip;
        SEAudio.Play();
    }

    public void stopSE()
    {
        SEAudio.loop = false;
        SEAudio.Stop();
    }

    public void startPublicEvent(int index)
    {
        publicevent.startEvent(index);
    }

    public void startPublicEvent(string name)
    {
        publicevent.startEvent(name);
    }
}
