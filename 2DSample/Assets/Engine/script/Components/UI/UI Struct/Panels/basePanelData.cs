using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class basePanelData : dataComponent
    {
        public GameObject getMainMenu()
        {
            return MI.getMainMenu();
        }

        public void setMainMenu(GameObject g)
        {
            MI.setMainMenu(g);
        }

        public GameObject showAnyPanel(GameObject g, Vector2 p, bool b, nowState ns)
        {
            return MI.showAnyPanel(g, p, b, ns);
        }

        public void showChoosePanel(string text, Func yes, Func no, nowState ns, GameObject mainPanel = null, bool black = false)
        {
            MI.showChoosePanel(text, yes, no, ns, mainPanel, black);
        }

        public int getMaxSize()
        {
            return MI.getSystemSetting().maxSize;
        }

        public int getOriSize()
        {
            return MI.getOriSize();
        }

        public int getHeight()
        {
            return MI.getSystemSetting().height;
        }

        public int getWidth()
        {
            return MI.getSystemSetting().width;
        }

        public string findText(string key, language lang)
        {
            return MI.findText(key, lang);
        }

        public Sprite findImg(string key)
        {
            return MI.findImg(key);
        }

        public AudioClip findAudio(string key)
        {
            return MI.findAudio(key);
        }

        public void playMusic(string c)
        {
            MI.playMusic(c);
        }
    }
}
