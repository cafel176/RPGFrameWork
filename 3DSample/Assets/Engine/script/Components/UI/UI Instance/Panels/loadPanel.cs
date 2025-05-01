using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace UI
{
    public class loadPanel : keyBoardMenusController
    {
        private loadPanelData data = new loadPanelData();

        override protected void onStart()
        {
            base.onStart();

            startPanel.cancelCallback += exit;
        }

        public void doSelect(string key)
        {
            menuPanel = null;
            exit();
            data.setNowStartSetting(int.Parse(key));
            data.startGame();
        }

        override public List<ListItemData> getDatas(int index, string param)
        {
            List<ListItemData> e = new List<ListItemData>();
            return e;
        }

        override public List<string> getKeys(int index, string param)
        {
            List<string> e = new List<string>();
            return e;
        }
    }
}
