using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class mainScene : keyBoardMenusController
    {
        [SerializeField]
        private keyBoardMenuList pressConfirmHint;

        [SerializeField]
        private bool pressConfirmToStart = false;

        [SerializeField]
        private logoSetting[] logos;

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
                logos[i].obj.SetActive(false);
            }
        }

        public void splashCallback()
        {
            StartCoroutine(end());
        }

        IEnumerator end()
        {
            if (pressConfirmToStart)
            {
                while (!data.getInputs().Contains(keyInput.confirm))
                {
                    yield return new WaitForSeconds(0.01f);
                }
                if (pressConfirmHint != null)
                    pressConfirmHint.ableDis(afterDis.hide);
            }
            startPanel.gameObject.SetActive(true);
        }

        override protected void onStart()
        {
            startPanel.canInput = false;
            data.showSplach(logos, splashCallback);

            startPanel.changeSize();
            if (pressConfirmHint != null)
                pressConfirmHint.changeSize();
            data.setMainMenu(startPanel.gameObject);

            base.onStart();

            //初始化
            startPanel.ableDis(afterDis);
            if (pressConfirmHint != null)
            {
                pressConfirmHint.ableDis(afterDis.hide);
                StartCoroutine(showHint());
            }
        }

        IEnumerator showHint()
        {
            yield return new WaitForSeconds(0.7f); 
            pressConfirmHint.gameObject.SetActive(true);
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

        public void endApp()
        {
            startPanel.ableDis(afterDis.hide);
            StartCoroutine(quit());
        }

        IEnumerator quit()
        {
            yield return new WaitForSeconds(0.1f);
            Application.Quit();
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
