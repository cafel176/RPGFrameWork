using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class itemMenuData : basePanelData
    {
        public ListNodeInterface getItemInfo(string key)
        {
            return MI.getItemInfo(key);
        }

        public Dictionary<string, int> getItems()
        {
            return MI.getItems();
        }

        public int getTotalItemNum(itemType type)
        {
            return MI.getTotalItemNum(type);
        }

        public bool useItem(string id,commonEventCallback c = null)
        {
            return MI.useItem(id,c);
        }
    }
}
