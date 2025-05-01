using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Actions
{
    [RequireComponent(typeof(DoEventTrigger))]
    [RequireComponent(typeof(RuntEventTrigger))]
    public class EventTrigger : EventAction
    {
        public string id;

        protected int nextList = 0;

        protected int nowList = -1;
        public int NowList
        {
            get
            {
                return nowList;
            }
        }

        [SerializeField]
        protected eventTriggerStruct[] eventLists;
        public eventTriggerStruct[] EventLists
        {
            get
            {
                return eventLists;
            }
        }

        [SerializeField]
        protected GameObject npc = null;
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

        [SerializeField]
        protected bool ShowCanDoHint = true;
        [SerializeField]
        protected float Yoffset = 0.5f;
        [SerializeField]
        protected float scale = 1f;
        [SerializeField]
        protected string canDoHint = "";

        protected CanDoHint cando;
        protected Camera cam;

        protected bool isVisible = false;
        public bool IsVisible
        {
            set
            {
                isVisible = value;
                if (isVisible)
                    showCanDo();                  
                else
                    hideCanDo(true);
            }
        }

        protected override void onAwake()
        {
            _audio = gameObject.GetComponent<AudioSource>();

            for (int i=0;i<eventLists.Length;i++)
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
            _camera = Camera.main.gameObject;
            cam = _camera.GetComponent<Camera>();

            loadins();
        }

        private void Update()
        {
            doEveryFrame();
        }

        public override void pushNow(bool auto = false)
        {
            if(toBeNow!=null)
            {
                eventLists[nowList].ThisNow = toBeNow;
            }

            if (eventLists[nowList].ThisNow == null)
            {
                eventLists[nowList].ThisNow = eventLists[nowList].eventList.getRoot();
                return;
            }

            var children = eventLists[nowList].ThisNow.getChildren();
            if (children.Count == 0)
            {
                if ((eventLists[nowList].loop && getNowState()!=nowState.auto) && 
                   (eventLists[nowList].with || eventLists[nowList].eventList.getRoot() == eventLists[nowList].ThisNow))
                    eventLists[nowList].Finish = false;
                else
                    eventLists[nowList].Finish = true;

                eventLists[nowList].ThisNow = null;

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
            else if ((eventType)eventLists[nowList].ThisNow.getNodeType() == eventType.choicePanel)
            {
                var node = eventLists[nowList].ThisNow;
                string str1 = node.getData<string>(structProperty.str1);
                string str2 = node.getData<string>(structProperty.str2);
                string str3 = node.getData<string>(structProperty.str3);
                string str4 = node.getData<string>(structProperty.str4);
                Func[] funcs = 
                {
                    delegate { eventLists[nowList].ThisNow = node.getChild(str2.Split('#')[1]);changeState(nowState.auto); },
                    delegate { eventLists[nowList].ThisNow = node.getChild(str3.Split('#')[1]);changeState(nowState.auto); },
                    delegate { eventLists[nowList].ThisNow = node.getChild(str4.Split('#')[1]);changeState(nowState.auto); }
                };
                string[] texts = { str2.Split('#')[0], str3.Split('#')[0], str4.Split('#')[0] };
                showEventChoosePanel(findText(str1, getSetting().nowlang),funcs,texts,false);
            }
            else if ((eventType)eventLists[nowList].ThisNow.getNodeType() == eventType.showButton)
            {
                string branch = eventLists[nowList].ThisNow.getData<string>(structProperty.str3);
                if (toBeNow != null)
                {
                    eventLists[nowList].ThisNow = toBeNow.getChild(branch);
                    toBeNow = null;
                }
                else
                {
                    bool check = false;
                    for (int i = 0; i < children.Count; i++)
                    {
                        if (children[i].getId() != branch)
                        {
                            eventLists[nowList].ThisNow = children[i];
                            check = true;
                            break;
                        }
                    }
                    if (!check)
                        eventLists[nowList].ThisNow = children[0];
                }
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
                cando = showCanDoHint(pos,scale).GetComponent<CanDoHint>();
                cando.gameObject.SetActive(false);
            }

            if (!cando.canShow && isVisible &&
                _start.checkStart(eventLists[nextList].EventListInterface.getHowToStart()) && 
                !eventLists[nextList].Finish &&
                eventLists[nextList].EventListInterface.checkEventConditions())
            {
                cando.gameObject.SetActive(true);
                cando.Show(canDoHint);
            }

        }

        public void hideCanDo(bool immediately = false)
        {
            if (!ShowCanDoHint)
                return;

            if (cando != null && cando.canShow)
            {
                cando.Hide(immediately);
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
