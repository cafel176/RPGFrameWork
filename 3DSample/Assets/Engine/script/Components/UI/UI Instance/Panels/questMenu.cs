using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UI
{

    public class questMenu : keyBoardMenusController
    {
        private questMenuData data = new questMenuData();

        [SerializeField]
        private keyBoardMenuList list;

        [SerializeField]
        private constText[] constText;

        override protected void onStart()
        {
            base.onStart();

            //处理显示回调
            startPanel.afterUpdateItems += list.updateItems;
            list.afterUpdateItems += setInfo;
            list.afterUpdateItems += setTitle;

            //初始化
            setConstText();

            list.updateItems(startPanel.getItemKey(startPanel.getCursorPosIndex()));
        }

        override public List<ListItemData> getDatas(int index, string param)
        {
            List<ListItemData> a = new List<ListItemData>();
            if (index == Array.IndexOf(children, list))
            {
                var p = data.getQuests();
                var keys = new List<string>(p.Keys);
                var values = new List<int>(p.Values);
                for (int i = 0; i < keys.Count; i++)
                {
                    var it = data.getQuestInfo(keys[i]);
                    if (param == Enum.GetName(typeof(questStatus), questStatus.all) || Enum.GetName(typeof(questStatus), values[i]) == param)
                        a.Add(new ListItemData(
                        new Sprite[] { it.getData<Sprite>(questProperty.img) },
                        new string[] { findText(it.getData<string>(questProperty.name), data.getSetting().nowlang) }
                        ));
                }
            }

            return a;
        }

        override public List<string> getKeys(int index, string param)
        {
            List<string> a = new List<string>();
            if (index == Array.IndexOf(children, list))
            {
                var p = data.getQuests();
                var keys = new List<string>(p.Keys);
                var values = new List<int>(p.Values);
                for (int i = 0; i < keys.Count; i++)
                    if (param == Enum.GetName(typeof(questStatus), questStatus.all) || Enum.GetName(typeof(questStatus), values[i]) == param)
                        a.Add(keys[i]);

            }
            else if (index == Array.IndexOf(children, startPanel))
            {
                Array values = Enum.GetNames(typeof(questStatus));
                for (int i = 0; i < values.Length; i++)
                {
                    string v = (string)values.GetValue(i);
                    if (!string.IsNullOrEmpty(v))
                        a.Add(v);
                }
            }

            return a;
        }

        private void setTitle(string i)
        {
            string key = i;
            var it = data.getQuestInfo(key);
            if(it!=null)
                setShow(new ListItemData(
            new Sprite[] { it.getData<Sprite>(questProperty.img) },
            new string[] { findText(it.getData<string>(questProperty.name), data.getSetting().nowlang) }
            ), showPanels[0]);
            else
                setShow(new ListItemData(null,null), showPanels[0]);
        }

        private void setInfo(string index)
        {
            string key = index;
            var it = data.getQuestInfo(key);
            string[] txt = { null, null, null, null };
            if (it != null)
            {
                setConstText();
                var q = it.getData<List<string>>(questProperty.steps);
                string steps = "";
                if (q != null)
                    for (int i = 0; i < q.Count; i++)
                    {
                        steps += findText(q[i], data.getSetting().nowlang);
                        steps += "\n";
                    }
                txt[0] = findText(it.getData<string>(questProperty.name), data.getSetting().nowlang);
                txt[1] = findText(it.getData<string>(questProperty.hard), data.getSetting().nowlang);
                txt[2] = findText(it.getData<string>(questProperty.text), data.getSetting().nowlang);
                txt[3] = steps;
            }
            else
                clearConstText();

            setShow(new ListItemData(txt), showPanels[1]);
        }


        private void setConstText()
        {
            for (int i = 0; i < constText.Length; i++)
            {
                constText[i].text.text = findText(constText[i].txt, getSetting().nowlang);
            }
        }

        private void clearConstText()
        {
            for (int i = 0; i < constText.Length; i++)
            {
                constText[i].text.text = "";
            }
        }
    }
}
