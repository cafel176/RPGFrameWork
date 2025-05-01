using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class gameUIData : basePanelData
    {
        public int getInt(string key)
        {
            return MI.getInt(key);
        }

        public bool getSwitch(string key)
        {
            return MI.getSwitch(key);
        }

        public void setSwitch(string key,bool value)
        {
            MI.setSwitch(key,value);
        }
    }
}

