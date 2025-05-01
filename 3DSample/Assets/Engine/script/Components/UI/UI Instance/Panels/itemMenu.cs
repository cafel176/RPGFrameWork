using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace UI
{
    public class itemMenu : keyBoardMenusController
    {
        private itemMenuData data = new itemMenuData();

        private AudioClip use;

        [SerializeField]
        private keyBoardMenuList list;

        override protected void onStart()
        {
            base.onStart();

            //其他操作
            use = data.findAudio("使用物品");

            //处理显示回调
            startPanel.afterUpdateItems += list.updateItems;
            list.afterUpdateItems += setInfo;

            //处理功能回调
            List<listItemUI> l = list.getlistItems();
            for (int i = 0; i < l.Count; i++)
            {
                l[i].callback += useItem;
            }

            //初始化
            list.updateItems(startPanel.getItemKey(startPanel.getCursorPosIndex()));
        }

        override public List<ListItemData> getDatas(int index, string param)
        {
            List<ListItemData> a = new List<ListItemData>();
            if (index == Array.IndexOf(children, list))
            {
                var p = data.getItems();
                var keys = new List<string>(p.Keys);
                var values = new List<int>(p.Values);
                for (int i = 0; i < keys.Count; i++)
                {
                    var it = data.getItemInfo(keys[i]);
                    if (Enum.GetName(typeof(itemType), it.getData<itemType>(itemProperty.type)) == param)
                        a.Add(new ListItemData(
                            new Sprite[] { it.getData<Sprite>(itemProperty.img) },
                            new string[] { data.findText(it.getData<string>(itemProperty.name), data.getSetting().nowlang), "x" + values[i] },
                            it.getData<int>(itemProperty.times) == -1 ? false : true));
                }
            }

            return a;
        }

        override public List<string> getKeys(int index, string param)
        {
            List<string> a = new List<string>();
            if (index == Array.IndexOf(children, list))
            {
                var p = data.getItems();
                var keys = new List<string>(p.Keys);
                for (int i = 0; i < keys.Count; i++)
                    if (Enum.GetName(typeof(itemType), data.getItemInfo(keys[i]).getData<itemType>(itemProperty.type)) == param)
                        a.Add(keys[i]);

            }
            else if (index == Array.IndexOf(children, startPanel))
            {
                Array values = Enum.GetNames(typeof(itemType));
                for (int i = 0; i < values.Length; i++)
                {
                    string v = (string)values.GetValue(i);
                    if (!string.IsNullOrEmpty(v))
                        a.Add(v);
                }
            }

            return a;
        }

        private void setInfo(string i)
        {
            string key = i;
            string txt = "";
            if (!string.IsNullOrEmpty(key))
                txt = data.findText(data.getItemInfo(key).getData<string>(itemProperty.text), data.getSetting().nowlang);
            setShow(new ListItemData(new string[] { txt }), showPanels[0]);
        }

        private void useItem(string i)
        {
            bool success = data.useItem(i,delegate() { changeState(nowState.window, startPanel.gameObject); });
            if (success)
            {
                playSE(use);
            }
            list.updateItems();
            startPanel.ableDis(afterDis.hide);
        }
    }
}
