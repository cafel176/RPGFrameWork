using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class settingPanelData : basePanelData
    {
        public void changeLanguage(bool add)
        {
            MI.changeLanguage(add);
        }

        public void changeBool(string key)
        {
            MI.changeBool(key);
        }

        public void addWindowSize(bool add)
        {
            MI.addWindowSize(add);
        }

        public void changeMusicValue(float num)
        {
            MI.changeMusicValue(num);
        }

        public void changeSEValue(float num)
        {
            MI.changeSEValue(num);
        }

        public void setMusicValue(float v)
        {
            MI.setMusicValue(v);
        }

        public void setSEValue(float v)
        {
            MI.setSEValue(v);
        }

        public void saveSetting()
        {
            MI.saveSetting();
        }

        public void backToMain()
        {
            MI.backToMain();
        }
    }
}
