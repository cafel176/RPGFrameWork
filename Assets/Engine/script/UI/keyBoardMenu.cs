using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct TextDictionary
{
    public string key;
    public Text text;
}

public delegate void Func();

public class keyBoardMenu : basePanel
{
    [SerializeField]
    protected GameObject Panel=null;
    public GameObject panel
    {
        get
        {
            if (Panel == null)
                return gameObject;
            else
                return Panel;
        }
    }

    public TextDictionary[] texts;
    //一个用键盘控制选择的类，其子类使用时要先给父类成员赋值

    //每个选项对应的函数，需要在使用前赋值
    protected Func[] doFuncs;
    //其他函数，包含0取消键，1左2右3上4下键
    protected Func[] otherFuncs = new Func[5];
    //光标
    public GameObject cursor;
    //每次移动的方向
    public Vector3 nextMove;

    // 相关音效
    protected AudioClip yes;
    protected AudioClip no;
    protected AudioClip unable;
    protected AudioClip move;

    //这个菜单选项总数
    protected int optionNum = 2;
    //记录当前光标位置
    protected int cursorPos = 0;

    protected bool changeScale = false;
    protected Vector3 nowScale = Vector3.one;
    protected Vector3 targetScale = Vector3.one;

    protected float ableTime = 0.1f,ableTimer=0;
    protected Func afterDis;

    virtual protected void init() { }
    virtual protected void update() { }

    protected AudioSource _audio = null;

    private void Start()
    {
        _audio = gameObject.GetComponent<AudioSource>();
        afterDis = delegate { Destroy(panel); };

        yes = gameManager.instance.hat.findAudio("确定");
        no = gameManager.instance.hat.findAudio("取消");
        unable = gameManager.instance.hat.findAudio("无效");
        move = gameManager.instance.hat.findAudio("光标");

        init();
    }

    private void Update()
    {
        if(changeScale)
        {
            panel.transform.localScale += (targetScale- nowScale) / ableTime * Time.deltaTime;
            ableTimer+= Time.deltaTime;
            if (ableTimer >= ableTime)
            {
                panel.transform.localScale = targetScale;
                changeScale = false;
                if(Mathf.Abs(targetScale.y)<=0.001f)
                {
                    if (afterDis != null)
                        afterDis();
                }
            }
        }

        update();
    }

    override protected void ableInit()
    {
        for (int i = 0; i < texts.Length; i++)
        {
            if (texts[i].key!=string.Empty)
            {
                texts[i].text.text = gameManager.instance.hat.find(texts[i].key, gameManager.instance.setting.nowlang);
            }
        }

        nowScale = panel.transform.localScale;
        nowScale.y = 0;
        targetScale = panel.transform.localScale;
        panel.transform.localScale = nowScale;
        ableTimer = 0;
        changeScale = true;
    }
    
    override protected void ableDis()
    {
        nowScale = panel.transform.localScale;
        targetScale = panel.transform.localScale;
        targetScale.y = 0;
        ableTimer = 0;
        changeScale = true;
    }

    public void changeCursorPos(bool next)
    {
        if (optionNum > 0)
        {
            if (next)
            {
                cursorPos = (cursorPos + 1) % optionNum;
                if (cursorPos == 0)
                    cursor.transform.localPosition = cursor.transform.localPosition - (optionNum - 1) * nextMove;
                else
                    cursor.transform.localPosition = cursor.transform.localPosition + nextMove;
            }
            else
            {
                cursorPos = (cursorPos - 1 + optionNum) % optionNum;
                if (cursorPos == optionNum - 1)
                    cursor.transform.localPosition = cursor.transform.localPosition + (optionNum - 1) * nextMove;
                else
                    cursor.transform.localPosition = cursor.transform.localPosition - nextMove;
            }
        }
    }

    public void doOption()
    {
        if (cursorPos < doFuncs.Length && cursorPos >= 0)
            if (doFuncs[cursorPos] != null)
            {
                playSE(yes,true);
                doFuncs[cursorPos]();
            }
    }

    public void cancel()
    {
        playSE(no,true);
        if (otherFuncs[0]!=null)
            otherFuncs[0]();
    }

    public void left()
    {
        if (otherFuncs[1] != null)
        {
            playSE(move);
            otherFuncs[1]();
        }
    }

    public void right()
    {
        if (otherFuncs[2] != null)
        {
            playSE(move);
            otherFuncs[2]();
        }
    }

    public void up()
    {
        if (otherFuncs[3] != null)
        {
            playSE(move);
            otherFuncs[3]();
        }
    }

    public void down()
    {
        if (otherFuncs[4] != null)
        {
            playSE(move);
            otherFuncs[4]();
        }
    }

    protected void playSE(AudioClip clip, bool exit = false)
    {
        if(exit)
        {
            gameManager.instance.playSE(clip);
            return;
        }
        if (_audio != null)
        {
            _audio.volume = gameManager.instance.setting.SEValue;
            _audio.clip = clip;
            _audio.Play();
        }
    }

    protected string findText(string key, language lang)
    {
        return gameManager.instance.hat.find(key, lang);
;   }

    protected Sprite findImg(string key)
    {
        return gameManager.instance.hat.findImg(key);
    }

    protected AudioClip findAudio(string key)
    {
        return gameManager.instance.hat.findAudio(key);
    }
}
