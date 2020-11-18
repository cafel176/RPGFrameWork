using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//使用须知
/* 每个序号必须有且只有一个卡进程函数，可以有若干不卡进程函数
 * setPlayerMove来让角色静止来阻隔残留输入
 * startUserControl切换到玩家操作模式，即结束自动事件
 * stopUserControl停止玩家操作，即开始自动事件
 */

public class eventTrigger : tools
{
    // 需要玩家朝向
    public turn needTurn;

    public eventStruct[] eventLists;

    public npc npc = null;

    //当前正在进行的事件页
    protected int nowList = -1;
    //判断角色是否在范围内
    protected bool inArea=false;
    protected bool s = true;
    //获取场景相机
    protected GameObject _camera;

    protected Vector2 archerPos;

    protected Vector2 originTurn;

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

    void Update()
    {
        // 用于处理音乐的
        if (!finish)
        {
            finish = gameManager.instance.reduceMusicVolume(Time.deltaTime / _time);
        }
        // 条件判断
        for (int r = 0; r < eventLists.Length; r++)
        {
            if (eventLists[r].finish)
                continue;
            if (!eventLists[r].eventList.checkCondition() && eventLists[r].thisNow == -1)
                continue;
            if (canDoSth &&
                (((eventLists[r].eventList.howToStart == start.playerTouch || eventLists[r].eventList.howToStart == start.eventTouch) && inArea)
                || (eventLists[r].eventList.howToStart == start.auto)
                || (eventLists[r].eventList.howToStart == start.Z && inArea)
                ))
            {
                if (gameManager.instance.nowstate == nowState.move)
                {
                    if (s)
                    {
                        if (eventLists[r].thisNow == -1)//首次触发
                        {
                            if ((eventLists[r].eventList.howToStart != start.Z) || (Input.GetKeyDown((KeyCode)keyInput.Z) || Input.GetKeyDown((KeyCode)keyInput.enter) || Input.GetKeyDown((KeyCode)keyInput.space)))
                            {
                                if (eventLists[r].eventList.howToStart == start.auto||(checkTurn(eventLists[r]) && checkIn()))
                                {
                                    nowList = r;
                                    stopUserControl();
                                    if (eventLists[nowList].eventList.howToStart != start.auto)
                                        player.GetComponent<Player>().Move(Vector2.zero);
                                    pushNow();                                   
                                    doSth(eventLists[r].eventList.list[eventLists[r].thisNow]);
                                    s = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (eventLists[r].eventList.howToStart != start.auto)
                            s = true;
                    }

                }
                else if (gameManager.instance.nowstate == nowState.text && UIManager.instance.checkTextRequet(this.gameObject))
                {
                    if (Input.GetKeyDown((KeyCode)keyInput.Z) || Input.GetKeyDown((KeyCode)keyInput.enter) || Input.GetKeyDown((KeyCode)keyInput.space))
                    {
                        if (UIManager.instance.showFinish)
                        {
                            UIManager.instance.hideTextPanel();
                            pushNow();
                            if (!checkEnd())
                                doSth(eventLists[r].eventList.list[eventLists[r].thisNow]);
                        }
                        else
                        {
                            UIManager.instance.skip();
                        }
                    }
                }
                else if (gameManager.instance.nowstate == nowState.auto)
                {
                    //自动事件
                    if (!checkEnd())
                        doSth(eventLists[r].eventList.list[eventLists[r].thisNow]);
                }
                break;
            }
        }
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
            return false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            inArea = true;
            s = true;
            player = collision.gameObject;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            inArea = true;
            player = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            inArea = false;
            StartCoroutine(aaa());
        }
    }

    IEnumerator aaa()
    {
        yield return new WaitForSeconds(0.11f);
        if (gameManager.instance.nowstate == nowState.text && UIManager.instance.checkTextRequet(gameObject))
        {
            UIManager.instance.hideTextPanel();
            startUserControl();
        }
    }

    protected bool checkEnd()
    {
        if (eventLists[nowList].thisNow >= eventLists[nowList].eventList.list.Count)
        {
            eventLists[nowList].finish = true;
            nowList = -1;
            startUserControl();
            return true;
        }
        else
            return false;        
    }

    //推动本事件进度，不卡进程
    override public void pushNow(bool auto=false)
    {
        eventLists[nowList].thisNow++;
        if (auto)
            gameManager.instance.changeState(nowState.auto);
    }

    override protected void doSth(myEvent toDo)
    {
        switch (toDo.type)
        {
            case eventType.reUseEvent:
                eventLists[nowList].thisNow = -1;
                nowList = -1;
                startUserControl();
                break;
            case eventType.text:
                if (npc != null)
                    npc.changeTurn(-player.GetComponent<Player>().turn);
                string _name = gameManager.instance.hat.find(toDo._name, gameManager.instance.setting.nowlang);
                string text = gameManager.instance.hat.find(toDo.text, gameManager.instance.setting.nowlang);
                if (toDo.showFace)
                {
                    showTextPanel(_name, text, gameManager.instance.hat.findImg(toDo.sp));
                }
                else
                {
                    showTextPanel(_name, text, null);
                }
                break;
            case eventType.beBlack:beBlack(toDo.time); break;
            case eventType.beWhite: beWhite(toDo.time); break;
            case eventType.wait:waitForTime(toDo.time); break;
            case eventType.startUserControl:startUserControl(); pushNow(); break;
            case eventType.stopUserControl:stopUserControl(); pushNow(true); break;
            case eventType.changeGlobalInt:
                calValue cal = toDo.howToCal;
                int u = dataManager.instance.getInt(toDo.key);
                if (cal == calValue.add)
                    dataManager.instance.setInt(toDo.key, u + toDo.globalInt);
                else if (cal == calValue.minus)
                    dataManager.instance.setInt(toDo.key, u - toDo.globalInt);
                else if (cal == calValue.multiply)
                    dataManager.instance.setInt(toDo.key, u * toDo.globalInt);
                else if (cal == calValue.divide)
                    dataManager.instance.setInt(toDo.key, u / toDo.globalInt);
                else
                    dataManager.instance.setInt(toDo.key, toDo.globalInt);
                pushNow(true); break;
            case eventType.changeGlobalFloat:
                cal = toDo.howToCal;
                float y = dataManager.instance.getFloat(toDo.key);
                if (cal == calValue.add)
                    dataManager.instance.setFloat(toDo.key, y + toDo.globalFloat);
                else if (cal == calValue.minus)
                    dataManager.instance.setFloat(toDo.key, y - toDo.globalFloat);
                else if (cal == calValue.multiply)
                    dataManager.instance.setFloat(toDo.key, y * toDo.globalFloat);
                else if (cal == calValue.divide)
                    dataManager.instance.setFloat(toDo.key, y / toDo.globalFloat);
                else
                    dataManager.instance.setFloat(toDo.key, toDo.globalFloat);
                pushNow(true); break;
            case eventType.changeGlobalSwith:
                dataManager.instance.setSwitch(toDo.key, toDo.globalSwitch);
                pushNow(true); break;
            default: break;
        }
    }
}
//getItem(0, 2);
//getItem(1, 1);