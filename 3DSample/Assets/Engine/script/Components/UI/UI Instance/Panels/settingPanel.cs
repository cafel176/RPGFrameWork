using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace UI
{
    public class settingPanel : keyBoardMenusController
    {
        private settingPanelData data = new settingPanelData();

        private int musicValue;
        private int SEValue;

        [SerializeField]
        private Image music;

        [SerializeField]
        private Sprite[] musicImgs;

        [SerializeField]
        private Image SE;

        [SerializeField]
        private Sprite[] SEImgs;

        override protected void onStart()
        {
            base.onStart();

            startPanel.cancelCallback += exit;

            float m = data.getSetting().musicValue;
            if(m<0.3f)
                musicValue = 0;
            else if (m < 0.6f)
                musicValue = 1;
            else if (m < 0.9f)
                musicValue = 2;
            else
                musicValue = 3;
            music.sprite = musicImgs[musicValue];

            float s = data.getSetting().SEValue;
            if (s < 0.3f)
                SEValue = 0;
            else if (s < 0.6f)
                SEValue = 1;
            else if (s < 0.9f)
                SEValue = 2;
            else
                SEValue = 3;
            SE.sprite = SEImgs[SEValue];
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

        public void apply()
        {
            data.saveSetting();
            exit();
        }

        public void resume()
        {
            exit();
        }

        public void backToMain()
        {
            exit();
            data.backToMain();
        }

        public void changeMusic()
        {
            switch(musicValue)
            {
                case 0: data.setMusicValue(0.33f); musicValue = 1; break;
                case 1: data.setMusicValue(0.66f); musicValue = 2; break;
                case 2: data.setMusicValue(1); musicValue = 3; break;
                case 3: data.setMusicValue(0); musicValue = 0; break;
            }
            music.sprite = musicImgs[musicValue];
        }

        public void changeSE()
        {
            switch (SEValue)
            {
                case 0: data.setSEValue(0.33f); SEValue = 1; break;
                case 1: data.setSEValue(0.66f); SEValue = 2; break;
                case 2: data.setSEValue(1); SEValue = 3; break;
                case 3: data.setSEValue(0); SEValue = 0; break;
            }
            SE.sprite = SEImgs[SEValue];
        }
    }
}
