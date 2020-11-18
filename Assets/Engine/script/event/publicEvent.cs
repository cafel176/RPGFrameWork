using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct publicEventDictionary
{
    public string name;
    public eventStruct eventList;
}

public class publicEvent : tools
{
    public publicEventDictionary[] eventLists;

    //当前正在进行的事件页
    protected eventStruct nowList = null;

    //获取场景相机
    protected GameObject _camera;

    protected Vector2 archerPos;

    void Start()
    {
        _audio = gameObject.GetComponent<AudioSource>();
        _camera = Camera.main.gameObject;
        hat = gameManager.instance.hat;
        archerPos = transform.position;
    }

    void Update()
    {
        // 用于处理音乐的
        if (!finish)
        {
            finish = gameManager.instance.reduceMusicVolume(Time.deltaTime / _time);
        }
        // 条件判断
        if (nowList==null)
            return;

        if (gameManager.instance.nowstate == nowState.move)
        {
            if (nowList.thisNow == -1)//首次触发
            {
                stopUserControl();
                if (nowList.eventList.howToStart != start.auto)
                    player.GetComponent<Player>().Move(Vector2.zero);
                pushNow();
                doSth(nowList.eventList.list[nowList.thisNow]);
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
                        doSth(nowList.eventList.list[nowList.thisNow]);
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
                doSth(nowList.eventList.list[nowList.thisNow]);
        }
    }

    //开始一个事件页
    public void startEvent(int index)
    {
        if (!eventLists[index].eventList.eventList.checkCondition())
            return;
        nowList = eventLists[index].eventList;
    }

    //开始一个事件页
    public void startEvent(string name)
    {
        for(int i=0;i< eventLists.Length;i++)
        {
            if(eventLists[i].name==name)
            {
                if (!eventLists[i].eventList.eventList.checkCondition())
                    return;
                nowList = eventLists[i].eventList;
                break;
            }
        }
    }

    public bool checkEnd()
    {
        if (nowList.thisNow >= nowList.eventList.list.Count)
        {
            nowList.thisNow = -1;
            startUserControl();
            return true;
        }
        else
            return false;
    }

    override protected void doSth(myEvent toDo)
    {
        switch (toDo.type)
        {
            default: break;
        }
      }
}
