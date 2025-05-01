using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class mainScene : keyBoardMenusController
    {
        [SerializeField]
        private GameObject[] logos;

        [SerializeField]
        private GameObject[] panels;

        [SerializeField]
        private string[] keys = { "start", "load", "setting" };

        private MainSceneData data = new MainSceneData();

        override protected void onAwake()
        {
            base.onAwake();

            for (int i = 0; i < logos.Length; i++)
            {
                logos[i].SetActive(false);
            }
        }

        override protected void onStart()
        {
            data.showSplach(logos);

            startPanel.changeSize();
            data.setMainMenu(startPanel.gameObject);

            base.onStart();

            //处理功能回调
            List<listItemUI> l = startPanel.getlistItems();
            for (int i = 0; i < l.Count; i++)
            {
                l[i].callback += doSelect;
            }

            //初始化
            startPanel.ableDis(afterDis);

            StartCoroutine(wait());
        }

        IEnumerator wait()
        {
            yield return new WaitForSeconds(4*logos.Length + 0.8f);
            startPanel.gameObject.SetActive(true);
        }

        private void doSelect(string key)
        {
            if (key == keys[0])
            {
                startGame();
            }
            else if (key == keys[1])
            {
                loadGame();
            }
            else if (key == keys[2])
            {
                setting();
            }
        }

        public void startGame()
        {
            data.startGame();
            startPanel.ableDis(afterDis.hide);
        }

        public void loadGame()
        {
            data.showAnyPanel(panels[0], Vector2.zero, true, nowState.window);
            startPanel.ableDis(afterDis.hide);
        }

        public void setting()
        {
             data.showAnyPanel(panels[1], Vector2.zero, true, nowState.window);
             startPanel.ableDis(afterDis.hide);
        }

        protected override void exit()
        {
            startPanel.doFunc("", gameObject);
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
