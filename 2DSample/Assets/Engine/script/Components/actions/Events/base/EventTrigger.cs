using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Actions
{
    public class EventTrigger : EventAction
    {
        public string id;

        private int nextList = 0;

        private int nowList = -1;
        public int NowList
        {
            get
            {
                return nowList;
            }
        }

        [SerializeField]
        private eventTriggerStruct[] eventLists;
        public eventTriggerStruct[] EventLists
        {
            get
            {
                return eventLists;
            }
        }

        [SerializeField]
        private GameObject npc = null;
        public GameObject NPC
        {
            get
            {
                return npc;
            }
            set
            {
                npc = value;
            }
        }

        private Vector2 archerPos;
        public Vector2 ArcherPos
        {
            get
            {
                return archerPos;
            }
        }

        private turn originTurn;

        [SerializeField]
        private bool ShowCanDoHint = true;
        [SerializeField]
        private float Yoffset = 0.5f;
        [SerializeField]
        private string canDoHint = "";

        private CanDoHint cando;

        override protected void onAwake()
        {
            _audio = gameObject.GetComponent<AudioSource>();
            _camera = Camera.main.gameObject;
            archerPos = transform.position;
            if (npc != null)
            {
                originTurn = getActorTurn(npc);
                archerPos = npc.transform.position - new Vector3(0, 0.12f, 0);
            }

            for(int i=0;i<eventLists.Length;i++)
            {
                eventLists[i].index = i;
                if (eventLists[i].eventList == null)
                    Debug.LogError("存在一个未赋值的事件");
                else
                    eventLists[i].eventList.init();
            }
        }

        protected override void onStart()
        {
            loadins();
        }

        private void Update()
        {
            doEveryFrame();
        }

        public override void pushNow(bool auto = false)
        {
            if (eventLists[nowList].ThisNow == null)
            {
                eventLists[nowList].ThisNow = eventLists[nowList].eventList.getRoot();
                return;
            }

            var children = eventLists[nowList].ThisNow.getChildren();
            if (children.Count == 0)
            {
                eventLists[nowList].ThisNow = null;
                if (eventLists[nowList].loop && eventLists[nowList].with)
                    eventLists[nowList].Finish = false;
                else
                    eventLists[nowList].Finish = true;

                return;
            }

            if ((eventType)eventLists[nowList].ThisNow.getNodeType() == eventType.If)
            {
                string branch1 = eventLists[nowList].ThisNow.getData<string>(structProperty.str3);
                string branch2 = eventLists[nowList].ThisNow.getData<string>(structProperty.str4);

                if (checkCondition(eventLists[nowList]))
                    eventLists[nowList].ThisNow = eventLists[nowList].ThisNow.getChild(branch1);
                else
                    eventLists[nowList].ThisNow = eventLists[nowList].ThisNow.getChild(branch2);
            }
            else
            {
                eventLists[nowList].ThisNow = children[0];
            }

            if (auto && !eventLists[nowList].with)
                changeState(nowState.auto);
        }

        override public void doEveryFrame()
        {
            for (int r = 0; r < eventLists.Length; r++)
            {
                if (eventLists[r].Remove)
                    continue;
                if (!eventLists[r].EventListInterface.checkEventConditions() && eventLists[r].ThisNow == null && !eventLists[r].Finish)
                    continue;

                nextList = r;
                if (_do.CanDoSth &&
                    (((eventLists[r].EventListInterface.getHowToStart() != start.auto) && _start.CanDoSth)
                    || (eventLists[r].EventListInterface.getHowToStart() == start.auto && (getCanEventsDo()|| eventLists[r].pre))))
                {
                    _run.interact(eventLists[r]);
                }
            }
        }

        public override void changeState(nowState ns, GameObject g = null)
        {
            if(!eventLists[nowList].with)
                base.changeState(ns, g);
        }

        public bool checkPlayerPos(start howToStart)
        {
            return _start.checkStart(howToStart);
        }

        public override bool checkEnd()
        {
            if (eventLists[nowList].Finish)
            {
                if (canDoSth)
                    canDoSth = false;

                if (eventLists[nowList].loop)
                {
                    eventLists[nowList].Finish = false;
                }
                else
                {
                    eventLists[nowList].Remove = true;
                }

                nowList = -1;
                startUserControl();
                return true;
            }
            else
                return false;
        }

        public void setNowList(eventTriggerStruct e)
        {
            nowList = e.index;
        }

        public void showCanDo()
        {
            if (!ShowCanDoHint)
                return;

            if (cando == null)
            {
                Vector3 pos = transform.position + new Vector3(0, Yoffset, 0);
                cando = showCanDoHint(pos).GetComponent<CanDoHint>();
                cando.gameObject.SetActive(false);
            }

            if (!cando.canShow && 
                _start.checkStart(eventLists[nextList].EventListInterface.getHowToStart()) && 
                !eventLists[nextList].Finish &&
                eventLists[nextList].EventListInterface.checkEventConditions())
            {
                cando.gameObject.SetActive(true);
                cando.Show(canDoHint);
            }

        }

        public void hideCanDo()
        {
            if (!ShowCanDoHint)
                return;

            if (cando != null && cando.canShow)
            {
                cando.Hide();
            }
        }

        private void loadins()
        {
            for (int i = 0; i < EventLists.Length; i++)
            {
                EventLists[i].EventListInterface.setIndependentSwitchs(getIndependentSwitchs(getNowScene() + "#" + id));
            }
        }

        // 设置独立开关
        public void setIns(bool s, int index)
        {
            setIndependentSwitchs(getNowScene() + "#" + id, index, s);
            for (int i = 0; i < EventLists.Length; i++)
            {
                EventLists[i].EventListInterface.setIndependentSwitchs(getIndependentSwitchs(getNowScene() + "#" + id));
            }
        }

        //0-4 A-D
        public bool getIns(int index)
        {
            return getIndependentSwitchs(getNowScene() + "#" + id)[index];
        }
    }
}
