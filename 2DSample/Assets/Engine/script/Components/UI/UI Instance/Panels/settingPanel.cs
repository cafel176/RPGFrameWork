using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UI
{
    public class settingPanel : keyBoardMenusController
    {
        private settingPanelData data = new settingPanelData();

        private string[] values = { "", "", "", "", "", "" };

        override protected void onStart()
        {
            base.onStart();

            setValues();
            for(int i=0;i<values.Length;i++)
            {
                setInfo(i);
            }

            startPanel.callbackRight = delegate (string index) { changeSetting(index, true); };
            startPanel.callbackLeft = delegate (string index) { changeSetting(index, false); };

            List<listItemUI> l = startPanel.getlistItems();
            for (int i = 0; i < l.Count; i++)
            {
                l[i].callback += save;
            }
        }
        

        private void save(string i)
        {
            data.saveSetting();
            exit();
        }

        override public List<ListItemData> getDatas(int index, string param)
        {
            List<ListItemData> a = new List<ListItemData>();
            if (index == Array.IndexOf(children, startPanel))
            {
                for (int i = 0; i < startPanel.getlistLength(); i++)
                {
                    a.Add(new ListItemData(new string[] { settings.keys[i] }));
                }
            }

            return a;
        }

        override public List<string> getKeys(int index, string param)
        {
            List<string> a = new List<string>();
            if (index == Array.IndexOf(children, startPanel))
            {
                for (int i = 0; i < startPanel.getlistLength(); i++)
                {
                    a.Add(settings.keys[i]);
                }
            }

            return a;
        }

        private void changeSetting(string index, bool add)
        {
            if (index == settings.keys[5])
            {
                changeLanguage(add);
            }
            else if (index == settings.keys[0] || index == settings.keys[1])
            {
                changeBool(index);
            }
            else
            {
                changeValue(index, add);
            }

            setValues();
            setInfo(Array.IndexOf(settings.keys, index));
        }

        void changeLanguage(bool add)
        {
            data.changeLanguage(add);
            startPanel.updateItems();
        }

        void changeBool(string index)
        {
            if (index == settings.keys[0])
            {
                data.changeBool(settings.keys[0]);
            }
            else if (index == settings.keys[1])
            {
                data.changeBool(settings.keys[1]);
            }
        }

        void changeValue(string index, bool add)
        {
            if (index == settings.keys[2])
            {
                data.addWindowSize(add);
            }
            else if (index == settings.keys[3])
            {
                if (add)
                {
                    if (data.getSetting().musicValue < 0.95)
                    {
                        data.changeMusicValue(0.1f);
                    }
                    else
                    {
                        data.setMusicValue(1);
                    }
                }
                else
                {
                    if (data.getSetting().musicValue > 0.05)
                    {
                        data.changeMusicValue(-0.1f);
                    }
                    else
                    {
                        data.setMusicValue(0);
                    }
                }
            }
            else if (index == settings.keys[4])
            {
                if (add)
                {
                    if (data.getSetting().SEValue < 0.95)
                    {
                        data.changeSEValue(0.1f);
                    }
                    else
                    {
                        data.setSEValue(1);
                    }
                }
                else
                {
                    if (data.getSetting().SEValue > 0.05)
                    {
                        data.changeSEValue(-0.1f);
                    }
                    else
                    {
                        data.setSEValue(0);
                    }
                }

            }
        }

        private void setInfo(int i)
        {
            setShow(new ListItemData(new string[] { values[i] }), showPanels[i]);
        }

        private void setValues()
        {
            if (data.getSetting().alwaysRun)
                values[0] = "ON";
            else
                values[0] = "OFF";

            if (data.getSetting().autoMessage)
                values[1] = "ON";
            else
                values[1] = "OFF";

            if (data.getSetting().windowSize == data.getMaxSize())
                values[2] = "全屏";
            else
                values[2] = "x " + data.getSetting().windowSize;

            if(menuPanel!=null)
                menuPanel.GetComponent<basePanel>().changeSize();

            if (data.getSetting().musicValue > 0.95)
            {
                values[3] = "100%";
            }
            else if (data.getSetting().musicValue < 0.05)
            {
                values[3] = "0%";
            }
            else
            {
                values[3] = Mathf.RoundToInt(data.getSetting().musicValue * 10) + "0%";
            }

            if (data.getSetting().SEValue > 0.95)
            {
                values[4] = "100%";
            }
            else if (data.getSetting().SEValue < 0.05)
            {
                values[4] = "0%";
            }
            else
            {
                values[4] = Mathf.RoundToInt(data.getSetting().SEValue * 10) + "0%";
            }

            values[5] = data.getSetting().nowlang.ToString();
        }
    }
}
