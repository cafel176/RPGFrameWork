using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//使用须知
/* 事件管理器共有函数的工具类，包含若干不卡进程函数
 * setPlayerMove来让角色静止来阻隔残留输入
 * startUserControl切换到玩家操作模式，即结束自动事件
 * stopUserControl停止玩家操作，即开始自动事件
 */

public class message
{
    public string name, text;
    public message(string n,string t)
    {
        name = n;text = t;
    }

    public message(string t)
    {
        name = ""; text = t;
    }
}

public class tools : MonoBehaviour
{
    //hash和tag
    protected HashsAndTags hat;
    //控制事件是否可推进
    protected bool canDoSth = true;
    //获取当前控制的角色
    protected GameObject player;

    protected AudioSource _audio = null;
    protected float _time = 1f;
    protected bool finish = true;

    //=====================覆盖函数=================================
    /* 由子类覆写来实现子类个性化 */

    virtual public void pushNow(bool auto = false) { }
    virtual protected void doSth(myEvent toDo) { }

    //=====================卡进程函数===============================
    /* 开始时设置canDoSth = false;
     * 协程结束时设置canDoSth = true;
     * 都可推进流程
     */

    //显示文字，卡进程
    protected void showTextPanel(string name, string text, Sprite sprite, bool _new = false)
    {
        canDoSth = true;
        UIManager.instance.showTextPanel(this.gameObject, name, text, sprite, _new);
    }

    protected void showTextPanel(message m, Sprite sprite, bool _new = false)
    {
        canDoSth = true;
        UIManager.instance.showTextPanel(this.gameObject, m.name, m.text, sprite, _new);
    }

    //用于启用协程，启用的协程必须以showTextPanel为结尾
    protected void myStartCoroutine(IEnumerator a)
    {
        stopUserControl();
        canDoSth = false;
        StartCoroutine(a);
    }

    protected void SbMoveToSw(GameObject sb, Vector2 move, Vector2 turn, string name = null, string text = null, Sprite sprite=null, bool _new = false)
    {
        stopUserControl();
        canDoSth = false;
        StartCoroutine(SbToSw(sb, move,turn, name, text, sprite, _new));
    }

    IEnumerator SbToSw(GameObject sb, Vector2 move, Vector2 turn, string name, string text, Sprite sprite, bool _new)
    {
        Vector2 target = move;
        sb.GetComponent<Player>().MoveTo(target,false);
        Vector2 d = sb.transform.position;
        float distanceEpsilon = sb.GetComponent<Player>().distanceEpsilon;
        while (Vector2.Distance(d,target) > 1.5f * distanceEpsilon)
        {
            yield return new WaitForSeconds(1.0f);
            d = sb.transform.position;
        }
        sb.GetComponent<Player>().changeTurn(turn);
        if (name == null || text == null)
        {
            pushNow();
            canDoSth = true;
        }
        else
        {
            yield return new WaitForSeconds(0.3f);
            showTextPanel(name, text, sprite, _new);
        }
    }

    //=====================卡进程函数===============================
    /* 开始时设置canDoSth = false;
     * 协程结束时设置canDoSth = true;
     * 都可推进流程
     */

    //淡入，卡进程
    protected void beBlack(float time, bool push = true, bool black = true)
    {
        canDoSth = false;
        UIManager.instance.beBlack(0.02f / time, black);
        gameManager.instance.changeState(nowState.auto);
        StartCoroutine(wait(time, push));
    }

    //淡出，卡进程
    protected void beWhite(float time, bool push = true, bool black = true)
    {
        canDoSth = false;
        UIManager.instance.beWhite(0.02f / time, black);
        gameManager.instance.changeState(nowState.auto);
        StartCoroutine(wait(time, push));
    }

    //等待，卡进程
    protected void waitForTime(float time, bool push = true)
    {
        canDoSth = false;
        gameManager.instance.changeState(nowState.auto);
        StartCoroutine(wait(time, push));
    }

    IEnumerator wait(float time, bool push)
    {
        yield return new WaitForSeconds(time);
        if (push)
            pushNow();
        canDoSth = true;
    }

    //显示文字提示，卡进程
    protected void showHint(string text, bool wait = true)
    {
        canDoSth = false;
        UIManager.instance.showHint(text);
        pushNow();
        if (wait)
        {
            gameManager.instance.changeState(nowState.auto);
            StartCoroutine(hint());
        }
    }

    protected void showHint(message m, bool wait = true)
    {
        canDoSth = false;
        UIManager.instance.showHint(m.text);
        pushNow();
        if (wait)
        {
            gameManager.instance.changeState(nowState.auto);
            StartCoroutine(hint());
        }
    }

    IEnumerator hint()
    {
        yield return new WaitForSeconds(3f);
        canDoSth = true;
    }

    //=====================不卡进程函数===============================
    /* 不推进流程，同一个序号里可以有多个 */

    //显示任意菜单，不卡进程
    protected GameObject showAnyPanel(GameObject _panel, Vector2 _pos, bool mainUI = false)
    {
        return UIManager.instance.showAnyPanel(_panel, _pos, mainUI);
    }

    public void stopMusic(float time = -1)
    {
        if (time <= 0)
            gameManager.instance.stopMusic();
        else
        {
            _time = time;
            finish = false;
        }
    }

    //获得道具，不卡进程
    protected void getItem(int i,int num)
    {
        dataManager.instance.getItem(i,num);
    }

    protected void useItem(int i)
    {
        dataManager.instance.useItem(i);
    }

    protected void removeItem(int i,int num)
    {
        dataManager.instance.removeItem(i,num);
    }

    //移动物体，不卡进程
    protected void changeThingPos(GameObject g, Transform t,AudioClip step = null, AudioClip runStep = null)
    {
        if (t != null)
        {
            g.transform.position = new Vector3(t.transform.position.x, t.transform.position.y, g.transform.position.z);
            if (g.tag == hat.player)
            {
                Vector2 e = g.GetComponent<Player>().turn;
                if (step != null)
                {
                    g.GetComponent<Player>().step = step;
                    g.GetComponent<Player>().runStep = runStep;
                }
                int num = g.GetComponent<Player>().follows.Count;
                for(int i = 0; i < num; i++)
                {
                    g.GetComponent<Player>().follows[i].gameObject.transform.position= new Vector3(t.transform.position.x - e.x * 0.1f, t.transform.position.y - e.y * 0.1f, g.transform.position.z);
                    g.GetComponent<Player>().follows[i].changeTurn(g.GetComponent<Player>().turn);
                    if (g.GetComponent<Player>().follows[i].step != null)
                    {
                        g.GetComponent<Player>().follows[i].step = step;
                        g.GetComponent<Player>().follows[i].runStep = runStep;
                    }
                }
            }
        }
    }

    protected void changeThingPos(GameObject g, Vector3 t, AudioClip step = null, AudioClip runStep = null)
    {
        g.transform.position = new Vector3(t.x, t.y, g.transform.position.z);
        if (g.tag == hat.player)
        {
            Vector2 e = g.GetComponent<Player>().turn;
            g.GetComponent<Player>().Move(e);
            g.GetComponent<Player>().Move(Vector2.zero);
            if (step != null)
            {
                g.GetComponent<Player>().step = step;
                g.GetComponent<Player>().runStep = runStep;
            }
            int num = g.GetComponent<Player>().follows.Count;
            for (int i = 0; i < num; i++)
            {
                g.GetComponent<Player>().follows[i].gameObject.transform.position = new Vector3(t.x-e.x * 0.1f, t.y-e.y*0.1f, g.transform.position.z);
                g.GetComponent<Player>().follows[i].changeTurn(g.GetComponent<Player>().turn);
                if (g.GetComponent<Player>().follows[i].step != null)
                {
                    g.GetComponent<Player>().follows[i].step = step;
                    g.GetComponent<Player>().follows[i].runStep = runStep;
                }
            }
        }
    }

    //给某个物体设置某个触发器，不卡进程
    protected void setTrigger(GameObject _object, string hash)
    {
        Animator anima = _object.GetComponent<Animator>();
        anima.SetTrigger(hash);
    }

    //给某个物体设置某个开关，不卡进程
    protected void setBool(GameObject _object, string hash, bool value)
    {
        Animator anima = _object.GetComponent<Animator>();
        anima.SetBool(hash,value);
    }

    //控制给定的角色开始移动，null则使用原本角色，不卡进程
    public void startUserControl(GameObject _player = null)
    {
        gameManager.instance.startUserControl(_player);
        if (_player != null)
        {
            player = _player;
            Camera.main.GetComponent<CameraFollow>().player = player;
        }
    }

    //停止玩家控制，不卡进程
    public void stopUserControl()
    {
        gameManager.instance.stopUserControl();
    }

    //设置人物速度，不卡进程
    protected void setPlayerMove(GameObject _player, Vector2 _move)
    {
        _player.GetComponent<Player>().Move(_move);
    }

    //设置人物朝向，不卡进程
    protected void changePlayerTurn(GameObject _player, Vector2 _turn)
    {
        if (_turn == Vector2.zero)
            _player.GetComponent<Player>().Move(_player.GetComponent<Player>().turn);
        else
            _player.GetComponent<Player>().Move(_turn);
        _player.GetComponent<Player>().Move(Vector2.zero);
    }

    protected void loadLevel(int level, int actorId = -1, Vector3 pos = default(Vector3), Vector3 turn = default(Vector3), bool stopMusic = true)
    {
        gameManager.instance.LoadLevel(level, actorId, pos,turn, stopMusic);
    }

    protected void playSE(AudioClip clip,bool loop=false)
    {
        if (_audio != null)
        {
            _audio.loop = loop;
            _audio.volume = gameManager.instance.setting.SEValue;
            _audio.clip = clip;
            _audio.Play();
        }
    }

    protected void stopSE()
    {
        if (_audio != null)
        {
            _audio.loop = false;
            _audio.Stop();
        }
    }

    protected void playMusic(AudioClip clip)
    {
        gameManager.instance.playMusic(clip);
    }

    public void changeCanDo(bool can)
    {
        canDoSth = can;
    }

    //角色入队
    protected void inTeam(int id)
    {
        dataManager.instance.inTeam(id);
    }

    //角色离队
    protected void outTeam(int id)
    {
        dataManager.instance.outTeam(id);
    }
}
