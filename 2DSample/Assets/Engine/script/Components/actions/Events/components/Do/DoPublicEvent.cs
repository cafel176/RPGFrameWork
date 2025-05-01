using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions
{
    public sealed class DoPublicEvent : DoEvent
    {
        protected override void onAwake()
        {
            a = gameObject.GetComponent<PublicEvent>();
        }

        override public void doSth(TreeNodeInterface toDo)
        {
            base.doSth(toDo);

            eventStruct e = new eventStruct(toDo);

            switch (e.type)
            {
                case eventType.text:
                    if (a.Player)
                    {
                        a.stopUserControl();
                        setActorMove(a.Player, Vector2.zero);
                    }
                    string _name = a.findText(e.str1, getSetting().nowlang);
                    string text = a.findText(e.str2, getSetting().nowlang);
                    if (e.bool1)
                    {
                        showTextPanel(a, _name, text, a.findImg(e.str3), a.findAudio(e.str4), a.findPitch(e.str4));
                    }
                    else
                    {
                        showTextPanel(a, _name, text, null, a.findAudio(e.str4), a.findPitch(e.str4));
                    }
                    break;

               // default 会造成混乱，严禁出现
            }
        }
    }
}
