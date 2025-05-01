using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class questMenuData : basePanelData
    {
        public ListNodeInterface getQuestInfo(string key)
        {
            return MI.getQuestInfo(key);
        }

        public Dictionary<string, int> getQuests()
        {
            return MI.getQuests();
        }

        public int getTotalQuestNum(questStatus type)
        {
            return MI.getTotalQuestNum(type);
        }
    }
}
