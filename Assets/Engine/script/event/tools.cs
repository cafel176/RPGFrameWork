using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//使用须知
/* 事件管理器共有函数的工具类，包含若干不卡进程函数
 * setPlayerMove来让角色静止来阻隔残留输入
 * startUserControl切换到玩家操作模式，即结束自动事件
 * stopUserControl停止玩家操作，即开始自动事件
 */

public enum IndependentSwitch
{
    switchA,
    switchB,
    switchC,
    switchD
}

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
    protected bool finishBgm = true;
    protected bool finishBgs = true;
    //获取场景相机
    protected GameObject _camera;

    protected float autoMessageTime = 1.0f, autoMessageTimer = 0;

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
    protected void showTextPanel(string name, string text, Sprite sprite, AudioClip se = null, float p = 1.0f, bool _new = false)
    {
        canDoSth = true;
        UIManager.instance.showTextPanel(this, name, text, sprite, se, p, _new);
    }

    protected void showTextPanel(message m, Sprite sprite, AudioClip se = null,float p=1.0f, bool _new = false)
    {
        canDoSth = true;
        UIManager.instance.showTextPanel(this, m.name, m.text, sprite, se, p, _new);
    }

    protected void showSpecialTextPanel(string text,Vector2 pos,AudioClip se = null, float p = 1.0f)
    {
        canDoSth = true;
        UIManager.instance.showSpecialTextPanel(this, text,pos, se,p);
    }

    protected void showSpecialTextPanel(message m, Vector2 pos, AudioClip se = null, float p = 1.0f)
    {
        canDoSth = true;
        UIManager.instance.showSpecialTextPanel(this, m.text,pos, se,p);
    }

    //用于启用协程，启用的协程必须以showTextPanel为结尾
    protected void myStartCoroutine(IEnumerator a)
    {
        stopUserControl();
        canDoSth = false;
        StartCoroutine(a);
    }

    protected void CameraMoveToSw(Vector2 move, float time)
    {
        stopUserControl();
        canDoSth = false;
        GameObject c = Camera.main.gameObject;
        StartCoroutine(CameraToSw(c, move,time));
    }

    IEnumerator CameraToSw(GameObject sb, Vector2 move,float time)
    {
        Vector2 target = move;
        var cf = sb.GetComponent<CameraFollow>();
        cf.Active = false;
        cf.MoveTo(target,time);
        yield return new WaitForSeconds(time+1.0f);
        pushNow(true);
        canDoSth = true;
    }

    protected void ShakeCamera(float hard, float time)
    {
        GameObject c = Camera.main.gameObject;
        var cf = c.GetComponent<CameraFollow>();
        cf.Active = false;
        cf.shake(hard, time);
    }

    protected void SbMoveToSw(GameObject sb, Vector2 move, Vector2 turn)
    {
        canDoSth = false;
        var p = sb.GetComponent<Player>();
        if (!p)
        {
            p = sb.transform.GetChild(0).gameObject.GetComponent<Player>();
            if (!p)
                return;
        }
        StartCoroutine(SbToSw(p, move,turn));
    }

    IEnumerator SbToSw(Player sb, Vector2 move, Vector2 turn)
    {
        Vector2 target = move;
        sb.MoveTo(target,false);
        Vector2 d = sb.transform.position;
        float distanceEpsilon = sb.distanceEpsilon;
        while (Vector2.Distance(d,target) > 1.5f * distanceEpsilon)
        {
            yield return new WaitForSeconds(1.0f);
            d = sb.transform.position;
        }
        sb.changeTurn(turn);

        pushNow(true);
        canDoSth = true;
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
        UIManager.instance.beBlack(time, black);
        gameManager.instance.changeState(nowState.auto);
        StartCoroutine(wait(time, push));
    }

    //淡出，卡进程
    protected void beWhite(float time, bool push = true, bool black = true)
    {
        canDoSth = false;
        UIManager.instance.beWhite(time, black);
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
            pushNow(true);
        canDoSth = true;
    }

    //显示文字提示，卡进程
    protected void showHint(string text, bool wait = true)
    {
        canDoSth = false;
        UIManager.instance.showHint(text);
        pushNow(true);
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
        pushNow(true);
        if (wait)
        {
            gameManager.instance.changeState(nowState.auto);
            StartCoroutine(hint());
        }
    }

    IEnumerator hint()
    {
        yield return new WaitForSeconds(2f);
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
            finishBgm = false;
        }
    }

    public void stopBGS(float time = -1)
    {
        if (time <= 0)
            gameManager.instance.stopBGS();
        else
        {
            _time = time;
            finishBgs = false;
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

    protected void changeThingPos(GameObject g, Vector2 t, AudioClip step = null, AudioClip runStep = null)
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
        var p = _player.GetComponent<Player>();
        if (p==null)
        {
            p = _player.transform.GetChild(0).gameObject.GetComponent<Player>();
            if (p == null)
                return;
        }
        if (_turn == Vector2.zero)
            p.Move(_player.GetComponent<Player>().turn);
        else
            p.GetComponent<Player>().Move(_turn);
        p.GetComponent<Player>().Move(Vector2.zero);
    }

    protected void loadLevel(string level, int actorId = -1, Vector3 pos = default(Vector3), Vector3 turn = default(Vector3), bool stopMusic = true)
    {
        gameManager.instance.LoadLevel(level, actorId, pos,turn, stopMusic);
    }

    protected GameObject spawnPrefab(string name,Vector2 pos)
    {
        var g = gameManager.instance.hat.findPrefab(name);
        GameObject go = Instantiate(g,pos,g.transform.rotation) as GameObject;

        return go;
    }

    protected void playSE(AudioClip clip,float p,bool loop=false)
    {
        if (_audio != null)
        {
            _audio.loop = loop;
            _audio.volume = gameManager.instance.setting.SEValue;
            _audio.pitch = p;
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

    protected void playBGS(AudioClip clip)
    {
        gameManager.instance.playBGS(clip);
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

    // 公用的写在这里，各自个性化的写在dosth里
    protected void toolsDoSth(myEvent toDo)
    {
        switch (toDo.type)
        {
            case eventType.beBlack: beBlack(toDo.float1); break;
            case eventType.beWhite: beWhite(toDo.float1); break;
            case eventType.wait: waitForTime(toDo.float1); break;
            case eventType.startUserControl:pushNow(true); startUserControl(); break;
            case eventType.stopUserControl:pushNow(true); stopUserControl(); break;
            case eventType.changeGlobalInt:
                calValue cal = toDo.howToCal;
                int u = dataManager.instance.getInt(toDo.str1);
                if (cal == calValue.add)
                    dataManager.instance.setInt(toDo.str1, u + toDo.int1);
                else if (cal == calValue.minus)
                    dataManager.instance.setInt(toDo.str1, u - toDo.int1);
                else if (cal == calValue.multiply)
                    dataManager.instance.setInt(toDo.str1, u * toDo.int1);
                else if (cal == calValue.divide)
                    dataManager.instance.setInt(toDo.str1, u / toDo.int1);
                else
                    dataManager.instance.setInt(toDo.str1, toDo.int1);
                pushNow(true); break;
            case eventType.changeGlobalFloat:
                cal = toDo.howToCal;
                float y = dataManager.instance.getFloat(toDo.str1);
                if (cal == calValue.add)
                    dataManager.instance.setFloat(toDo.str1, y + toDo.float1);
                else if (cal == calValue.minus)
                    dataManager.instance.setFloat(toDo.str1, y - toDo.float1);
                else if (cal == calValue.multiply)
                    dataManager.instance.setFloat(toDo.str1, y * toDo.float1);
                else if (cal == calValue.divide)
                    dataManager.instance.setFloat(toDo.str1, y / toDo.float1);
                else
                    dataManager.instance.setFloat(toDo.str1, toDo.float1);
                pushNow(true); break;
            case eventType.changeGlobalSwith:
                dataManager.instance.setSwitch(toDo.str1, toDo.bool1);
                pushNow(true); break;
            case eventType.debugLog:
                Debug.Log(toDo.str1);
                pushNow(true); break;
            case eventType.playBgm:
                playMusic(gameManager.instance.hat.findAudio(toDo.str1));
                pushNow(true); break;
            case eventType.stopBgm:
                finishBgm = false;
                pushNow(true); break;
            case eventType.playBgs:
                playBGS(gameManager.instance.hat.findAudio(toDo.str1));
                pushNow(true); break;
            case eventType.stopBgs:
                finishBgs = false;
                pushNow(true); break;
            case eventType.playSE:
                playSE(gameManager.instance.hat.findAudio(toDo.str1), gameManager.instance.hat.findPitch(toDo.str1));
                pushNow(true); break;
            case eventType.showPic:
                UIManager.instance.showPicPanel(toDo.str1, toDo.vec2_1, gameManager.instance.hat.findImg(toDo.str2), toDo.float1/255.0f);
                pushNow(true); break;
            case eventType.movePic:
                UIManager.instance.movePicPanel(toDo.str1, toDo.vec2_1,toDo.float1/255.0f,toDo.float2);
                if(toDo.bool1)
                    waitForTime(toDo.float2);
                else
                    pushNow(true);
                break;
            case eventType.hidePic:pushNow(true);
                UIManager.instance.hidePicPanel(toDo.str1);break;
            case eventType.textSpecial:
                string text = gameManager.instance.hat.find(toDo.str1, gameManager.instance.setting.nowlang);
                showSpecialTextPanel(text, toDo.vec2_1, gameManager.instance.hat.findAudio(toDo.str2), gameManager.instance.hat.findPitch(toDo.str2));
                break;
            case eventType.changeScene:
                pushNow(true);
                Vector2 t = new Vector2(0,1);
                if(toDo.turn==turn.down)
                    t = new Vector2(0, -1);
                else if (toDo.turn == turn.left)
                    t = new Vector2(-1, 0);
                else if (toDo.turn == turn.right)
                    t = new Vector2(1, 0);
                loadLevel(toDo.str1, dataManager.instance.team[0], toDo.vec2_1,t);            
                break;
            case eventType.playAnima:
                var a = spawnPrefab(toDo.str1,toDo.vec2_1);
                var animator = a.GetComponent<Animator>();
                AnimationClip clip = animator.runtimeAnimatorController.animationClips[0];
                var speed = animator.GetCurrentAnimatorStateInfo(0).speed;
                var time = clip.length / (speed * speed);
                if (toDo.bool1)
                    waitForTime(time);
                else
                    pushNow(true);
                break;
            case eventType.moveCamera:
                CameraMoveToSw(toDo.vec2_1, toDo.float1);
                break;
            case eventType.sbMoveToSw:
                GameObject g = null;
                if (toDo.findType == findType.tag)
                    g = GameObject.FindGameObjectWithTag(toDo.str1);
                else if (toDo.findType == findType.name)
                    g = GameObject.Find(toDo.str1);
                t = new Vector2(0, 1);
                if (toDo.turn == turn.down)
                    t = new Vector2(0, -1);
                else if (toDo.turn == turn.left)
                    t = new Vector2(-1, 0);
                else if (toDo.turn == turn.right)
                    t = new Vector2(1, 0);
                if (g.GetComponent<Player>()==null)
                    g = g.transform.GetChild(0).gameObject;
                SbMoveToSw(g, toDo.vec2_1, t);
                break;
            case eventType.changeThingPos:
                pushNow(true); g = null;
                if (toDo.findType == findType.tag)
                    g = GameObject.FindGameObjectWithTag(toDo.str1);
                else if (toDo.findType == findType.name)
                    g = GameObject.Find(toDo.str1);
                changeThingPos(g, toDo.vec2_1);          
                break;
            case eventType.changeTurn:
                g = null;
                if (toDo.findType == findType.tag)
                    g = GameObject.FindGameObjectWithTag(toDo.str1);
                else if (toDo.findType == findType.name)
                    g = GameObject.Find(toDo.str1);
                t = new Vector2(0, 1); pushNow(true);
                if (toDo.turn == turn.down)
                    t = new Vector2(0, -1);
                else if (toDo.turn == turn.left)
                    t = new Vector2(-1, 0);
                else if (toDo.turn == turn.right)
                    t = new Vector2(1, 0);
                if (g.GetComponent<Player>()==null)
                    g = g.transform.GetChild(0).gameObject;
                changePlayerTurn(g, t);
                break;
            case eventType.changeThingActive:
                pushNow(true); g = null; // 注意find函数无法查找隐藏的物体，需要给物体添加父物体以设置其可见性，且父子物体不可重名
                if (toDo.findType == findType.tag)
                {
                    if(toDo.str1==gameManager.instance.hat.player)
                        g = player;
                    else
                        g = GameObject.FindGameObjectWithTag(toDo.str1).transform.GetChild(0).gameObject;
                }
                else if (toDo.findType == findType.name)
                    g = GameObject.Find(toDo.str1).transform.GetChild(0).gameObject;
                if (g != null)
                    g.SetActive(toDo.bool1);
                break;
            case eventType.showSavePanel:
                pushNow();
                UIManager.instance.showSavePanel();                
                break;
            case eventType.changeItem:
                getItem(toDo.int1, toDo.int2);
                var n = dataManager.instance.itemlist.getItemInfo(toDo.int1)._name;
                showHint("获得道具:"+ gameManager.instance.hat.find(n, gameManager.instance.setting.nowlang) +" x"+ toDo.int2, true);
                break;
            case eventType.flashScreen:
                UIManager.instance.showFlash(toDo.color1,toDo.float1);
                if (toDo.bool1)
                    waitForTime(toDo.float1);
                else
                    pushNow(true);
                break;
            case eventType.shakeScreen:
                ShakeCamera(toDo.float2, toDo.float1);
                if (toDo.bool1)
                    waitForTime(toDo.float1);
                else
                    pushNow(true);
                break;
            case eventType.startQuest:
                dataManager.instance.addQuest(toDo.int1);
                pushNow(true);
                break;
            case eventType.changeQuest:
                dataManager.instance.changeQuest(toDo.int1,toDo.questStatus);
                pushNow(true);
                break;
            case eventType.changeWeather:

                pushNow(true);
                break;


            default: break;
        }

        doSth(toDo);// 子类实现各自的操作
    }
}
