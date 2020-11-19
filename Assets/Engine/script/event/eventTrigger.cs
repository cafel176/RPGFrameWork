using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//使用须知
/* 每个序号必须有且只有一个卡进程函数，可以有若干不卡进程函数
 * setPlayerMove来让角色静止来阻隔残留输入
 * startUserControl切换到玩家操作模式，即结束自动事件
 * stopUserControl停止玩家操作，即开始自动事件
 */

[System.Serializable]
public class eventTriggerStruct
{
    public eventStruct events;
    // 自动执行函数下应该卡进程
    // 是否是并行处理事件,菜单情况下也执行，只有卡进程函数时不执行
    public bool with = false;
}

public class eventTrigger : tools
{
    public string id;
    // 需要玩家朝向
    public turn needTurn;

    public eventTriggerStruct[] eventLists;

    public bool showCanDoHint = true;
    public float Yoffset = 0.5f;
    public string canDoHint = "调 查";

    public npc npc = null;

    //当前正在进行的事件页
    protected int nowList = -1;
    //判断角色是否在范围内
    protected bool inArea=false;
    protected bool canTouch = true;
    protected bool loadInS = false;

    protected Vector2 archerPos;

    protected Vector2 originTurn;

    protected CanDoHint cando;

    void Start()
    {
        _audio = gameObject.GetComponent<AudioSource>();
        _camera = Camera.main.gameObject;
        hat = gameManager.instance.hat;
        archerPos = transform.position;
        if (npc != null)
        {
            originTurn = npc.turn;
            archerPos = npc.transform.position - new Vector3(0, 0.12f, 0);
        }
    }

    private void loadindependentSwitchs()
    {
        if(!loadInS)
        {
            for (int i = 0; i < eventLists.Length; i++)
            {
                eventLists[i].events.eventList.independentSwitchs = dataManager.instance.getIndependentSwitchs(gameManager.instance.nowScene + "#" + id);
            }
            loadInS = true;
        }
    }

    void Update()
    {
        // 放在start中会导致加载出现问题
        loadindependentSwitchs();

        // 用于处理音乐的
        if (!finishBgm)
        {
            finishBgm = gameManager.instance.reduceMusicVolume(gameManager.instance.setting.musicValue * Time.deltaTime / _time);
        }
        if (!finishBgs)
        {
            finishBgs = gameManager.instance.reduceBGSVolume(gameManager.instance.setting.musicValue * Time.deltaTime / _time);
        }
        // 条件判断
        for (int r = 0; r < eventLists.Length; r++)
        {
            if (eventLists[r].events.finish)
                continue;
            if (!eventLists[r].events.eventList.checkConditions() && eventLists[r].events.thisNow == -1)
                continue;
            if (canDoSth &&
                (((eventLists[r].events.eventList.howToStart != start.auto) && inArea)
                || (eventLists[r].events.eventList.howToStart == start.auto)))
            {
                if (gameManager.instance.nowstate == nowState.move)
                {
                    if (canTouch || eventLists[r].events.eventList.howToStart != start.playerTouch)
                    {
                        if ((eventLists[r].events.eventList.howToStart != start.Z) || (Input.GetKeyDown((KeyCode)keyInput.Z) || Input.GetKeyDown((KeyCode)keyInput.enter) || Input.GetKeyDown((KeyCode)keyInput.space)))
                        {
                            if (eventLists[r].events.thisNow == -1)//首次触发
                            {
                                if (eventLists[r].events.eventList.howToStart == start.auto
                                    || eventLists[r].events.eventList.howToStart == start.eventTouch
                                    || (checkTurn(eventLists[r].events) && checkIn()))
                                {
                                    if (eventLists[r].events.eventList.howToStart == start.auto||eventLists[r].with)
                                    {
                                        var p = gameManager.instance.player;
                                        if (p)
                                            player = p.gameObject;
                                    }
                                    else
                                    {
                                        stopUserControl();
                                    }
                                    
                                    nowList = r;
                                    if (eventLists[nowList].events.eventList.howToStart != start.auto)
                                        player.GetComponent<Player>().Move(Vector2.zero);
                                    pushNow();
                                    toolsDoSth(eventLists[r].events.eventList.list[eventLists[r].events.thisNow]);
                                }
                            }
                            else if (eventLists[r].events.thisNow > 0 && !checkEnd())
                                toolsDoSth(eventLists[r].events.eventList.list[eventLists[r].events.thisNow]);
                        }
                        else if (eventLists[r].with)
                        {
                            if (eventLists[r].events.thisNow == -1)//首次触发
                            {
                                nowList = r;
                                pushNow();
                                var p = gameManager.instance.player;
                                if (p)
                                    player = p.gameObject;
                                toolsDoSth(eventLists[r].events.eventList.list[eventLists[r].events.thisNow]);
                            }
                            else if (eventLists[r].events.thisNow > 0 && !checkEnd())
                                toolsDoSth(eventLists[r].events.eventList.list[eventLists[r].events.thisNow]);
                        }
                    }
                }
                else if (gameManager.instance.nowstate == nowState.window)
                {
                    if (eventLists[r].with)
                    {
                        //自动事件
                        if (eventLists[r].events.thisNow == -1)//首次触发
                        {
                            nowList = r;
                            pushNow();
                            var p = gameManager.instance.player;
                            if (p)
                                player = p.gameObject;
                            toolsDoSth(eventLists[r].events.eventList.list[eventLists[r].events.thisNow]);
                        }
                        else if (eventLists[r].events.thisNow > 0 && !checkEnd())
                            toolsDoSth(eventLists[r].events.eventList.list[eventLists[r].events.thisNow]);
                    }
                }
                else if (gameManager.instance.nowstate == nowState.text)
                {
                    if (UIManager.instance.checkTextRequet(this))
                    {
                        if (gameManager.instance.setting.autoMessage || Input.GetKeyDown((KeyCode)keyInput.Z) || Input.GetKeyDown((KeyCode)keyInput.enter) || Input.GetKeyDown((KeyCode)keyInput.space))
                        {
                            if (UIManager.instance.showFinish)
                            {
                                if (UIManager.instance.canHide)
                                {
                                    if (gameManager.instance.setting.autoMessage)
                                    {
                                        autoMessageTimer += Time.deltaTime;
                                        if (autoMessageTimer >= autoMessageTime)
                                        {
                                            autoMessageTimer = 0;
                                            UIManager.instance.hideTextPanel();
                                            pushNow();
                                            if (eventLists[r].events.thisNow > 0 && !checkEnd())
                                                toolsDoSth(eventLists[r].events.eventList.list[eventLists[r].events.thisNow]);
                                        }
                                    }
                                    else
                                    {
                                        UIManager.instance.hideTextPanel();
                                        pushNow();
                                        if (eventLists[r].events.thisNow > 0 && !checkEnd())
                                            toolsDoSth(eventLists[r].events.eventList.list[eventLists[r].events.thisNow]);
                                    }
                                }
                            }
                            else
                            {
                                if (!gameManager.instance.setting.autoMessage)
                                    UIManager.instance.skip();
                            }
                        }
                    }
                    else if (eventLists[r].with)
                    {
                        if (eventLists[r].events.thisNow == -1)//首次触发
                        {
                            nowList = r;
                            pushNow();
                            var p = gameManager.instance.player;
                            if (p)
                                player = p.gameObject;
                            toolsDoSth(eventLists[r].events.eventList.list[eventLists[r].events.thisNow]);
                        }
                        else if (eventLists[r].events.thisNow > 0 && !checkEnd())
                            toolsDoSth(eventLists[r].events.eventList.list[eventLists[r].events.thisNow]);
                    }                  
                }
                else if (gameManager.instance.nowstate == nowState.auto)
                {
                    if(eventLists[r].with)
                    {
                        if (eventLists[r].events.thisNow == -1)//首次触发
                        {
                            nowList = r;
                            pushNow();
                            var p = gameManager.instance.player;
                            if (p)
                                player = p.gameObject;
                            toolsDoSth(eventLists[r].events.eventList.list[eventLists[r].events.thisNow]);
                        }
                        else if (eventLists[r].events.thisNow > 0 && !checkEnd())
                            toolsDoSth(eventLists[r].events.eventList.list[eventLists[r].events.thisNow]);
                    }
                    else
                    {
                        if (eventLists[r].events.thisNow > 0 && !checkEnd())
                            toolsDoSth(eventLists[r].events.eventList.list[eventLists[r].events.thisNow]);
                    }
                }
                break;
            }
        }
    }

    // 设置独立开关
    public void setIndependentSwitch(bool s, int index)
    {
        dataManager.instance.setIndependentSwitchs(gameManager.instance.nowScene+"#"+id,index,s);
        for (int i = 0; i < eventLists.Length; i++)
        {
            eventLists[i].events.eventList.independentSwitchs = dataManager.instance.getIndependentSwitchs(gameManager.instance.nowScene + "#" + id);
        }
    }

    //0-4 A-D
    public bool getIndependentSwitch(int index)
    {
        return dataManager.instance.getIndependentSwitchs(gameManager.instance.nowScene + "#" + id)[index];
    }

    protected bool checkTurn(eventStruct eventLists)
    {
        if (needTurn==turn.all)
        {
            if (eventLists.eventList.howToStart==start.auto)
                return true;
            Vector2 l = player.transform.position - new Vector3(0, 0.12f, 0);
            l = archerPos - l;
            Vector2 k = player.GetComponent<Player>().turn;
            if (l.x * k.x + l.y * k.y > 0)
                return true;
        }
        Vector2 t = new Vector2(0, 0);
        if (needTurn == turn.left)
            t.x = -1;
        else if (needTurn == turn.right)
            t.x = 1;
        else if (needTurn == turn.down)
            t.y = -1;
        else
            t.y = 1;
        if (player.GetComponent<Player>().turn.x == t.x && player.GetComponent<Player>().turn.y == t.y)
            return true;
        return false;
    }

    protected bool checkIn()
    {
        BoxCollider2D c = gameObject.GetComponent<BoxCollider2D>();
        Vector2 d, d1 = transform.TransformPoint(c.offset), d2 = player.transform.position;
        d2.y -= 0.09f ;
        d = d1 - d2;
        float bias = 0.01f;
        if (Mathf.Abs(d.x) < c.size.x+0.05f+ bias && Mathf.Abs(d.y) < c.size.y + 0.04f+ bias)
        {
            return true;
        }
        else
        {
            if(inArea)
                inArea = false;
            hideCanDo();
            return false;
        }
    }

    public void showCanDo()
    {
        if (!showCanDoHint)
            return;

        if (cando==null)
        {
            Vector3 pos = transform.position + new Vector3(0, Yoffset, 0);
            cando = UIManager.instance.showCanDoHint(pos).GetComponent<CanDoHint>();
        }

        if (!cando.show)
        {
            cando.gameObject.SetActive(true);        
            cando.Show(canDoHint);
        }

    }

    public void hideCanDo()
    {
        if (!showCanDoHint)
            return;

        if (cando != null && cando.show)
        {
            cando.Hide();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            inArea = true;
            canTouch = true;
            showCanDo();
            player = collision.gameObject;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            inArea = true;
            showCanDo();
            player = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            inArea = false;
            canTouch = true;
            hideCanDo();
            StartCoroutine(aaa());
        }
    }

    IEnumerator aaa()
    {
        yield return new WaitForSeconds(0.11f);
        if (gameManager.instance.nowstate == nowState.text && UIManager.instance.checkTextRequet(this))
        {
            UIManager.instance.hideTextPanel();
            startUserControl();
        }
    }

    protected bool checkEnd()
    {
        if (eventLists[nowList].events.thisNow >= eventLists[nowList].events.eventList.list.Count)
        {
            eventLists[nowList].events.thisNow = -1;
            if (canTouch)
                canTouch = false;
            if (!eventLists[nowList].events.loop)
            {
                eventLists[nowList].events.finish = true;
            }
            nowList = -1;
            if (gameManager.instance.nowstate == nowState.auto||gameManager.instance.nowstate == nowState.window)
                startUserControl();
            return true;
        }
        else
            return false;        
    }

    //推动本事件进度，不卡进程
    override public void pushNow(bool auto=false)
    {
        eventLists[nowList].events.thisNow++;
        if (auto && !eventLists[nowList].with)
            gameManager.instance.changeState(nowState.auto);
    }

    override protected void doSth(myEvent toDo)
    {
        switch (toDo.type)
        {
            case eventType.text:
                if (npc != null)
                    npc.changeTurn(-player.GetComponent<Player>().turn);
                string _name = gameManager.instance.hat.find(toDo.str1, gameManager.instance.setting.nowlang);
                string text = gameManager.instance.hat.find(toDo.str2, gameManager.instance.setting.nowlang);
                if (toDo.bool1)
                {
                    showTextPanel(_name, text, gameManager.instance.hat.findImg(toDo.str3), gameManager.instance.hat.findAudio(toDo.str4), gameManager.instance.hat.findPitch(toDo.str4));
                }
                else
                {
                    showTextPanel(_name, text, null, gameManager.instance.hat.findAudio(toDo.str4), gameManager.instance.hat.findPitch(toDo.str4));
                }
                break;
            case eventType.changeIndependentSwitch:
                setIndependentSwitch(toDo.bool1,(int)toDo.independentSwitch);
                pushNow(true); break;
            case eventType.If:
                if (eventListDefine.checkCondition(toDo.condition, eventLists[nowList].events.eventList.independentSwitchs))
                    pushNow(true);
                else
                    eventLists[nowList].events.thisNow += (toDo.int1+1);
                break;

            default: break;
        }
    }
}
