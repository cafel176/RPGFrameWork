using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class mainMenuData : basePanelData
    {
        public void startPublicEvent(string name, nowState ns, commonEventCallback c = null)
        {
            MI.startPublicEvent(name,ns,c);
        }

        public int getInt(string name)
        {
            return MI.getInt(name);
        }
    }
}
