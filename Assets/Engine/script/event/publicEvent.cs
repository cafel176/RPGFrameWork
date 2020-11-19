using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class publicEvent : tools
{
    //当前正在进行的事件页
    public eventStruct nowList = null;
    // 由外界进行更改的变量
    public bool canDo = false;

    protected float timer = 0;
    protected Func afterTime;

    void Start()
    {
        _audio = gameObject.GetComponent<AudioSource>();
        _camera = Camera.main.gameObject;
        hat = gameManager.instance.hat;
    }

    void Update()
    {
        // 条件判断
        if (nowList == null||!canDo)
            return;

        // 用于处理音乐的
        if (!finishBgm)
        {
            finishBgm = gameManager.instance.reduceMusicVolume(gameManager.instance.setting.musicValue * Time.deltaTime / _time);
        }
        if (!finishBgs)
        {
            finishBgs = gameManager.instance.reduceBGSVolume(gameManager.instance.setting.musicValue * Time.deltaTime / _time);
        }

        if (timer>0)
        {
            timer -= Time.deltaTime;
            if(timer<=0)
            {
                timer = 0;
                afterTime();
            }
        }

        if (gameManager.instance.nowstate == nowState.move)
        {
            if (nowList.thisNow == -1)//首次触发
            {
                pushNow();
                var p = gameManager.instance.player;
                if (p)
                    player = p.gameObject;
                toolsDoSth(nowList.eventList.list[nowList.thisNow]);
            }
            else
            {
                //自动事件
                if (!checkEnd() && nowList.thisNow > 0)
                    toolsDoSth(nowList.eventList.list[nowList.thisNow]);
            }
        }
        else if (gameManager.instance.nowstate == nowState.window)
        {
            if (nowList.thisNow == -1)//首次触发
            {
                pushNow();
                var p = gameManager.instance.player;
                if(p)
                    player = p.gameObject;
                toolsDoSth(nowList.eventList.list[nowList.thisNow]);
            }
            else
            {
                //自动事件
                if (!checkEnd() && nowList.thisNow > 0)
                    toolsDoSth(nowList.eventList.list[nowList.thisNow]);
            }
        }
        else if (gameManager.instance.nowstate == nowState.text)
        {
            if(UIManager.instance.checkTextRequet(this))
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
                                    startUserControl();
                                    pushNow();
                                    if (!checkEnd() && nowList.thisNow > 0)
                                        toolsDoSth(nowList.eventList.list[nowList.thisNow]);
                                }
                            }
                            else
                            {
                                UIManager.instance.hideTextPanel();
                                startUserControl();
                                pushNow();
                                if (!checkEnd() && nowList.thisNow > 0)
                                    toolsDoSth(nowList.eventList.list[nowList.thisNow]);
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
            else
            {
                //自动事件
                if (nowList.thisNow == -1)//首次触发
                {
                    pushNow();
                    var p = gameManager.instance.player;
                    if (p)
                        player = p.gameObject;
                    toolsDoSth(nowList.eventList.list[nowList.thisNow]);
                }
                else if (!checkEnd() && nowList.thisNow > 0)
                    toolsDoSth(nowList.eventList.list[nowList.thisNow]);
            }
        }
        else if (gameManager.instance.nowstate == nowState.auto)
        {
            //自动事件
            if (nowList.thisNow == -1)//首次触发
            {
                pushNow();
                var p = gameManager.instance.player;
                if (p)
                    player = p.gameObject;
                toolsDoSth(nowList.eventList.list[nowList.thisNow]);
            }
            else if (!checkEnd() && nowList.thisNow>0)
                toolsDoSth(nowList.eventList.list[nowList.thisNow]);
        }
    }

    //推动本事件进度，不卡进程
    override public void pushNow(bool auto = false)
    {
        nowList.thisNow++;
    }

    //开始一个事件页
    public void startEvent(int index)
    {
        var e = gameManager.instance.getCommonEvent(index);
        nowList = e.eventList;

        nowList.thisNow = -1;
        canDo = false;
        timer = 0;
    }

    //开始一个事件页
    public void startEvent(string name)
    {
        var e = gameManager.instance.getCommonEvent(name);
        if (e.eventList==null)
            return;
        nowList = e.eventList;

        nowList.thisNow = -1;
        canDo = false;
        timer = 0;
    }

    public bool checkEnd()
    {
        if (nowList.thisNow >= nowList.eventList.list.Count)
        {
            if(nowList.loop)
            {
                nowList.thisNow = -1;
                return false;
            }
            else
            {
                // 标记为-2进行移除
                nowList.thisNow = -2;
                canDo = false;
                timer = 0;
                return true;
            }
        }
        else
            return false;
    }

    override protected void doSth(myEvent toDo)
    {
        // 公共事件默认不阻塞进程，需要阻塞进程的事件需要在这里手动设置
        // 阻塞进程的事件由协程实现，这些事件在执行时，自动执行事件执行频率会被影响
        switch (toDo.type)
        {
            case eventType.beBlack:
                if (player)
                {
                    stopUserControl();
                    player.GetComponent<Player>().Move(Vector2.zero);
                }
                afterTime = delegate { startUserControl(); }; timer = toDo.float1; break;
            case eventType.beWhite:
                if (player)
                {
                    stopUserControl();
                    player.GetComponent<Player>().Move(Vector2.zero);
                }
                afterTime = delegate { startUserControl(); }; timer = toDo.float1; break;
            case eventType.wait:
                if (player)
                {
                    stopUserControl();
                    player.GetComponent<Player>().Move(Vector2.zero);
                }
                afterTime = delegate { startUserControl(); }; timer = toDo.float1; break;
            case eventType.text:
                if(player)
                {
                    stopUserControl();
                    player.GetComponent<Player>().Move(Vector2.zero);
                }
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
            case eventType.If:
                if (eventListDefine.checkCondition(toDo.condition, null))
                    pushNow(true);
                else
                    nowList.thisNow += (toDo.int1 + 1);
                break;


            default: break;
        }
      }
}
