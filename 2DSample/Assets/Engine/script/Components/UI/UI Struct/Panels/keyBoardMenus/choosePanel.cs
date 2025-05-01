using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace UI
{
    public class choosePanel : keyBoardMenusController
    {
        public Func yesFunc, noFunc;

        public string ChooseText
        {
            set
            {
                setShow(new ListItemData(new string[] { value }), showPanels[0]);
            }
        }

        override protected void onStart()
        {
            base.onStart();

            startPanel.cancelCallback += _no;

            //处理功能回调
            List<listItemUI> l = startPanel.getlistItems();
            for (int i = 0; i < l.Count; i++)
            {
                l[i].callback += doSelect;
            }
        }

        private void doSelect(string key)
        {
            if (key == "yes")
                _yes();
            else if (key == "no")
                _no();
        }

        public void BlackBackground()
        {
            gameObject.GetComponent<Image>().color = new Color(0, 0, 0, (168.0f / 255));
        }

        void _yes()
        {
            exit();
            if(yesFunc!=null)
                yesFunc();
        }

        void _no()
        {
            exit();
            if(noFunc!=null)
                noFunc();
        }

        override public List<ListItemData> getDatas(int index, string param)
        {
            return null;
        }

        override public List<string> getKeys(int index, string param)
        {
            return null;
        }
    }
}
