using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UI
{
    public class keyBoardMenuImages : keyBoardMenuList
    {
        [System.Serializable]
        public class ImageDictionary
        {
            public string key = "";
            public string img;
            public string onHover;
        }

        public ImageDictionary[] images;

        public listItemUI[] children;

        public override void spawanItems()
        {
            for (int i = 0; i < children.Length; i++)
            {
                listItems.Add(children[i]);
            }

            for (int i = 0; i < listItems.Count && i < images.Length; i++)
            {
                setKey(i, images[i].key);
            }
        }

        override protected void doUpdateItems(string _param = "")
        {
            string p = string.IsNullOrEmpty(_param) ? param : _param;

            allPage = (int)Math.Ceiling((float)images.Length / getMaxNum());
            if (page > allPage)
            {
                page = 0;
                resetCurosr();
            }
            int startIndex = page * getMaxNum();
            clearAll();
            for (int i = 0; i < images.Length; i++)
            {
                setKey(i, images[i].key);
                setData(i, new ListItemData(new Sprite[] { findImg(images[i].img) }));
            }

            int j = getCursorPosIndex();
            setData(j, new ListItemData(new Sprite[] { findImg(images[j].onHover) }));
        }
    }
}
