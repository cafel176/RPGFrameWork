using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class menuPanelSimple : keyBoardMenusController
    {
        private mainMenuData data = new mainMenuData();

        [SerializeField]
        private GameObject[] menus;

        private string[] keys = { "item", "quest", "think", "option" };

        override protected void onStart()
        {
            base.onStart();

            //处理功能回调
            List<listItemUI> l = startPanel.getlistItems();
            for (int i = 0; i < l.Count; i++)
            {
                l[i].callback += select;
            }

            startPanel.callbackUp = delegate(string i) { startPanel.setCursorPos(0, 0); };
            startPanel.callbackDown = delegate (string i) { startPanel.setCursorPos(0, 3); };
            startPanel.callbackLeft = delegate (string i) { startPanel.setCursorPos(0, 1); };
            startPanel.callbackRight = delegate (string i) { startPanel.setCursorPos(0, 2); };

            //初始化
            setMainMenu(startPanel.gameObject);
        }

        private void select(string index)
        {
            if (index == keys[0])
            {
                showItemMenu();
            }
            else if (index == keys[1])
            {
                showQuestMenu();
            }
            else if (index == keys[2])
            {
                showThink();
            }
            else if (index == keys[3])
            {
                showOptionMenu();
            }
        }

        public void showOptionMenu()
        {
            showAnyPanel(menus[2], Vector2.zero, true);
            startPanel.ableDis(afterDis.hide);
        }

        public void showItemMenu()
        {
            showAnyPanel(menus[0], Vector2.zero, true);
            startPanel.ableDis(afterDis.hide);
        }

        public void showQuestMenu()
        {
            showAnyPanel(menus[1], Vector2.zero, true);
            startPanel.ableDis(afterDis.hide);
        }

        public void showThink()
        {
            data.startPublicEvent(getThink(),nowState.window ,delegate() { });
            startPanel.ableDis(afterDis.hide);
        }

        private string getThink()
        {
            //人物id#公共事件名   如：0#charlotte_think
            int i = data.getInt("剧情编号");// 定义一个跟随剧情的think变量
            string text = findText("think." + i, getSetting().nowlang);
            string nowThink = "夏洛特TALK";
            if (text == "can not find")
            {
                text = findText("vane." + i, getSetting().nowlang);
                nowThink = "瓦妮莎TALK";
            }
            if (text == "can not find")
            {
                nowThink = "伊万TALK";
            }
            return nowThink;
        }

        protected override void exit()
        {
            changeState(nowState.move);

            startPanel.ableDis(afterDis);
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
