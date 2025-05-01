using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions
{
    public class startpublicEvent : startEvents
    {
        private PublicEvent a;

        protected override void onAwake()
        {
            a = gameObject.GetComponent<PublicEvent>();
        }

        //开始一个事件页
        override public void start(string index)
        {
            var nowList = getCommonEvent(index);
            a.setNowList(nowList);
            a.setCanDo(false);
        }
    }
}
