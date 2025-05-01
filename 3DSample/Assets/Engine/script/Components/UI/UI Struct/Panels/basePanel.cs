using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class basePanel : baseUI
    {
        private basePanelData data = new basePanelData();
        protected Vector3 startScale = new Vector3(1, 1, 1);
        protected Vector3 startPosition = new Vector3(0, 0, 0);

        override protected void onAwake()
        {
            base.onAwake();

            startScale = transform.localScale;
            startPosition = transform.localPosition;
        }

        override protected void ableInit()
        {
            base.ableInit();

            if (data.canUse())
                changeSize();
        }

        public void changeStartScale(Vector3 v)
        {
            startScale = v;
        }

        public void changeStartPos(float x, float y)
        {
            startPosition = new Vector3(x, y, startPosition.z);
        }

        public void changeSize()
        {
            basePanel menu = null;
            try
            {
                menu = transform.parent.gameObject.GetComponent<basePanel>();
                menu.changeSize();
            }
            catch
            {
                if (menu == null)
                {
                    float s = data.getSetting().windowSize;
                    if (data.getSetting().windowSize == data.getMaxSize())
                    {
                        float w = Screen.width / data.getWidth();
                        float h = Screen.height / data.getHeight();
                        s = (w > h) ? w : h;
                    }
                    transform.localScale = startScale / data.getOriSize() * s;
                    transform.localPosition = startPosition / data.getOriSize() * s;
                }
            }
        }

        protected settings getSetting()
        {
            return data.getSetting();
        }

        protected HashsAndTags getHat()
        {
            return data.getHat();
        }

        protected void changeState(nowState ns, GameObject g = null)
        {
            data.changeState(ns, g);
        }

        protected void showAnyPanel(GameObject g, Vector2 p, bool b)
        {
            data.showAnyPanel(g, p, b, nowState.window);
        }

        protected void setMainMenu(GameObject g)
        {
            data.setMainMenu(g);
        }

        protected GameObject getMainMenu()
        {
            return data.getMainMenu();
        }

        protected string findText(string key, language lang)
        {
            return data.findText(key, lang);
        }

        protected Sprite findImg(string key)
        {
            return data.findImg(key);
        }

        protected AudioClip findAudio(string key)
        {
            return data.findAudio(key);
        }
    }
}
