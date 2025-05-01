using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions
{
    public class runPublicEvent : RunEvents
    {
        private PublicEvent a;

        protected float autoMessageTime = 1.0f, autoMessageTimer = 0;

        protected override void onAwake()
        {
            a = gameObject.GetComponent<PublicEvent>();
        }

        override public void interact(eventStruct e)
        {
            if (getNowState() == nowState.move)
            {
                forAutoEvent(e);
            }
            else if (getNowState() == nowState.window)
            {
                forAutoEvent(e);
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
                else
                {
                    //自动事件
                    forAutoEvent(e);
                }
            }
            else if (getNowState() == nowState.auto)
            {
                //自动事件
                forAutoEvent(e);
            }
        }

        private void forAutoEvent(eventStruct e)
        {
            if (e.ThisNow == null && !e.Finish)//首次触发
            {
                a.pushNow();
                a.doSth(e.ThisNow);
            }
            else
            {
                //自动事件
                if (e.ThisNow != e.eventList.getRoot() && !a.checkEnd())
                    a.doSth(e.ThisNow);
            }
        }

        private void forTextEvent(eventStruct e)
        {
            hideTextPanel();
            a.startUserControl();
            a.pushNow();
            if (e.ThisNow != e.eventList.getRoot() && !a.checkEnd())
                a.doSth(e.ThisNow);
        }
    }
}
