using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions
{
    [System.Serializable]
    public class eventTriggerStruct : eventStruct
    {
        public int index = -1;
        // 自动执行函数下应该卡进程
        // 是否是并行处理事件,菜单情况下也执行，只有卡进程函数时不执行
        public bool with = false;
        public bool pre = false;
    }

    public class RuntEventTrigger : RunEvents
    {
        private EventTrigger a;

        protected float autoMessageTime = 1.0f, autoMessageTimer = 0;

        protected override void onStart()
        {
            a = gameObject.GetComponent<EventTrigger>();
        }

        override public void interact(eventStruct _e)
        {
            eventTriggerStruct e = (eventTriggerStruct)_e;
            if (getNowState() == nowState.move)
            {
                if (a.CanDoSth || e.EventListInterface.getHowToStart() != start.playerTouch)
                {
                    if ((e.EventListInterface.getHowToStart() != start.Z) || getInputs().Contains(keyInput.confirm))
                    {
                        if (e.ThisNow == null)//首次触发
                        {
                            if (e.EventListInterface.getHowToStart() == start.auto
                                || e.EventListInterface.getHowToStart() == start.eventTouch
                                || a.checkPlayerPos(e.EventListInterface.getHowToStart()))
                            {
                                forMoveEventFirst(e);
                            }
                        }
                        else
                            forAutoEvent(e);
                    }
                    else if (e.with)
                    {
                        //自动事件
                        if (e.ThisNow == null)//首次触发
                        {
                            forAutoEventFirst(e);
                        }
                        else
                            forAutoEvent(e);
                    }
                }
            }
            else if (getNowState() == nowState.window)
            {
                if (e.with)
                {
                    //自动事件
                    if (e.ThisNow == null)//首次触发
                    {
                        forAutoEventFirst(e);
                    }
                    else
                        forAutoEvent(e);
                }
            }
            else if (getNowState() == nowState.text)
            {
                if (checkTextRequet(a))
                {
                    if (getSetting().autoMessage || getInputs().Contains(keyInput.confirm))
                    {
                        if (getShowFinish())
                        {
                            if (getCanHind())
                            {
                                if (getSetting().autoMessage)
                                {
                                    autoMessageTimer += Time.deltaTime;
                                    if (autoMessageTimer >= autoMessageTime)
                                    {
                                        autoMessageTimer = 0;
                                        forTextEvent(e);
                                    }
                                }
                                else
                                {
                                    forTextEvent(e);
                                }
                            }
                        }
                        else
                        {
                            if (!getSetting().autoMessage)
                                skip();
                        }
                    }
                }
                else if (e.with)
                {
                    if (e.ThisNow == null)//首次触发
                    {
                        forAutoEventFirst(e);
                    }
                    else
                    {
                        forAutoEvent(e);
                    }
                }
            }
            else if (getNowState() == nowState.auto)
            {
                if (e.with)
                {
                    if (e.ThisNow == null)//首次触发
                    {
                        forAutoEventFirst(e);
                    }
                    else
                        forAutoEvent(e);
                }
                else
                {
                    forAutoEvent(e);
                }
            }
        }

        private void forAutoEvent(eventTriggerStruct e)
        {
            if (e.ThisNow != e.eventList.getRoot() && !a.checkEnd())
                a.doSth(e.ThisNow);
        }

        private void forAutoEventFirst(eventTriggerStruct e)
        {
            a.setNowList(e);
            a.pushNow();
            var p = a.Player;
            if (p)
                a.Player = p.gameObject;

            a.doSth(e.ThisNow);
        }

        private void forMoveEventFirst(eventTriggerStruct e)
        {
            if (e.EventListInterface.getHowToStart() == start.auto || e.with)
            {
                var p = a.Player;
                if (p)
                    a.Player = p.gameObject;
            }
            else
            {
                stopUserControl();
            }

            a.setNowList(e);
            if (e.EventListInterface.getHowToStart() != start.auto)
                a.stopPlayer(a.Player);
            a.pushNow();
            a.doSth(e.ThisNow);
        }

        private void forTextEvent(eventTriggerStruct e)
        {
            hideTextPanel();
            a.pushNow();
            if (e.ThisNow != e.eventList.getRoot() && !a.checkEnd())
                a.doSth(e.ThisNow);
        }
    }
}
