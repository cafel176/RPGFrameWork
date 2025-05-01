using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions
{
    public sealed class DoEventTrigger : DoEvent
    {
        protected override void onStart()
        {
            a = gameObject.GetComponent<EventTrigger>();
        }

        override public void doSth(TreeNodeInterface toDo)
        {
            base.doSth(toDo);

            eventStruct e = new eventStruct(toDo);

            switch (e.type)
            {
                case eventType.text:
                    if (((EventTrigger)a).NPC != null)
                    {
                        var v = turnTool.get(getActorTurn(a.Player));
                        turn t = turnTool.get(new Vector2Int(-v.x,-v.y));
                        setActorTurn(((EventTrigger)a).NPC, t);
                    }
                        
                    string _name = a.findText(e.str1, getSetting().nowlang);
                    string text = a.findText(e.str2, getSetting().nowlang);
                    if (e.bool1)
                    {
                        showTextPanel(a,_name, text, a.findImg(e.str3), a.findAudio(e.str4), a.findPitch(e.str4));
                    }
                    else
                    {
                        showTextPanel(a, _name, text, null, a.findAudio(e.str4), a.findPitch(e.str4));
                    }
                    break;
                case eventType.changeIndependentSwitch:
                    ((EventTrigger)a).setIns(e.bool1, (int)e.independentSwitch);
                    pushNow(true); break;
                case eventType.commonEvent:
                    if(toDo.getChildren().Count==0)
                        startPublicEvent(e.str1,nowState.move);
                    else
                        startPublicEvent(e.str1, nowState.auto);
                    break;

              // default 会造成混乱，严禁出现
            }
        }
    }
}
