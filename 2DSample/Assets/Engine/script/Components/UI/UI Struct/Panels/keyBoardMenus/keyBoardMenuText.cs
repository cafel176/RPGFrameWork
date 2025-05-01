using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace UI
{
    public class keyBoardMenuText : keyBoardMenuList
    {
        [System.Serializable]
        public class TextDictionary
        {
            public string key = "";
            public string text = "";
        }

        public TextDictionary[] texts;

        public override void spawanItems()
        {
            base.spawanItems();

            for (int i = 0; i < listItems.Count && i<texts.Length; i++)
            {
                setKey(i,texts[i].key);
            }
        }

        override protected void doUpdateItems(string _param = "")
        {
            string p = string.IsNullOrEmpty(_param)?param: _param;

            allPage = (int)Math.Ceiling((float)texts.Length / getMaxNum());
            if (page > allPage)
            {
                page = 0;
                resetCurosr();
            }
            int startIndex = page * getMaxNum();
            clearAll();
            for (int i = 0; i < texts.Length; i++)
            {
                if (texts[i].text == string.Empty)
                    texts[i].text = "empty";

                setKey(i, texts[i].key);
                setData(i, new ListItemData(new string[] { findText(texts[i].text, getSetting().nowlang) }));
            }
        }
    }
}
