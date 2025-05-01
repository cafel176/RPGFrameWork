using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions
{
    public class PublicEvent : EventAction, publicEventInterface
    {
        //当前正在进行的事件页
        private eventStruct nowList = null;
        public eventStruct NowList
        {
            get
            {
                return nowList;
            }
        }

        protected nowState ns = nowState.none;

        override protected void onAwake()
        {
            _audio = gameObject.GetComponent<AudioSource>();
            _camera = Camera.main.gameObject;
        }

        protected override void onStart()
        {
            if (nowList == null || nowList.eventList == null)
                Debug.LogError("存在一个未赋值的公共事件");
            else
                nowList.eventList.init();
        }

        private void Update()
        {
            if (!getCanEventsDo())
                return;

            doEveryFrame();
        }

        public void setNowList(eventStruct e)
        {
            nowList = e;
        }

        override public void doEveryFrame()
        {
            // 条件判断
            if (nowList == null || !canDoSth)
                return;

            if (_do.CanDoSth)
            {
                _run.interact(nowList);
            }
            
        }

        public override void pushNow(bool auto = false)
        {
            if (nowList.ThisNow == null)
            {
                nowList.ThisNow = nowList.eventList.getRoot();
                return;
            }

            var children = nowList.ThisNow.getChildren();
            if (children.Count == 0)
            {
                nowList.Finish = true;
                return;
            }

            if ((eventType)nowList.ThisNow.getNodeType() == eventType.If)
            {
                string branch1 = nowList.ThisNow.getData<string>(structProperty.str3);
                string branch2 = nowList.ThisNow.getData<string>(structProperty.str4);

                if (checkCondition(nowList))
                    nowList.ThisNow = nowList.ThisNow.getChild(branch1);
                else
                    nowList.ThisNow = nowList.ThisNow.getChild(branch2);
            }
            else
            {
                nowList.ThisNow = children[0];
            }
        }

        public void startEvent(string index, nowState n)
        {
            ns = n;
            _start.start(index);
        }

        public override bool checkEnd()
        {
            if (nowList.Finish)
            {
                if (nowList.loop)
                {
                    nowList.ThisNow = nowList.eventList.getRoot();
                    nowList.Finish = false;
                    return false;
                }
                else
                {
                    changeState(ns);
                    nowList.Remove = true;
                    canDoSth = false;
                    return true;
                }
            }
            else
                return false;
        }

        // ========================================== 接口函数 ==========================================

        public void resetNowList()
        {
            nowList.reset();
        }

        public bool canNowListRemove()
        {
            return nowList.Remove;
        }

        public bool checkNowListEventConditions()
        {
            return nowList.EventListInterface.checkEventConditions();
        }

        public void exit()
        {
            Destroy(gameObject);
        }
    }
}
