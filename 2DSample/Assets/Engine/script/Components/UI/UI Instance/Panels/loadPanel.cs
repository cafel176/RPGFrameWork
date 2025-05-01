using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UI
{
    public class loadPanel : keyBoardMenusController
    {
        private loadPanelData data = new loadPanelData();

        [SerializeField]
        private keyBoardMenuList operate;

        [SerializeField]
        private int saveNum = 100;

        private string file = "";

        [SerializeField]
        private string[] texts = { "load", "save", "delete" };

        override protected void onStart()
        {
            base.onStart();

            //处理显示回调
            startPanel.afterUpdateItems += setInfo;
            startPanel.afterUpdateItems += setTitle1;
            operate.afterUpdateItems += setTitle2;

            //处理功能回调
            List<listItemUI> l = operate.getlistItems();
            for (int i = 0; i < l.Count; i++)
            {
                l[i].callback += showChoosePanel;
            }

            //初始化
            startPanel.updateItems();
        }

        override public List<ListItemData> getDatas(int index, string param)
        {
            List<ListItemData> a = new List<ListItemData>();
            if (index == Array.IndexOf(children, startPanel))
            {
                for (int i = 0; i < saveNum; i++)
                {
                    a.Add(new ListItemData(new string[] { "* File " + (i + 1) }));
                }
            }
            else if (index == Array.IndexOf(children, operate))
            {

            }

            return a;
        }

        override public List<string> getKeys(int index, string param)
        {
            List<string> a = new List<string>();
            if (index == Array.IndexOf(children, startPanel))
            {
                for (int i = 0; i < saveNum; i++)
                {
                    a.Add("file" + (i + 1));
                }

            }
            else if (index == Array.IndexOf(children, operate))
            {
                for (int i = 0; i < operate.getlistLength(); i++)
                {
                    a.Add(texts[i]);
                }
            }

            return a;
        }

        private void setTitle1(string i = "")
        {
            setShow(new ListItemData(
                        new string[] { findText("sys.selehelp", data.getSetting().nowlang) }
                        ), showPanels[0]);
        }

        private void setTitle2(string i)
        {
            string key = i;
            string t = "";
            if (key == texts[0])
                t = "sys.loadhelp";
            else if (key == texts[1])
                t = "sys.savehelp";
            else if (key == texts[2])
                t = "sys.delehelp";
            setShow(new ListItemData(
                        new string[] { findText(t, data.getSetting().nowlang) }
                        ), showPanels[0]);
        }

        private void setInfo(string index = "")
        {
            operate.changeBtn(0, true);
            operate.changeBtn(1, true);
            operate.changeBtn(2, true);

            string fileName = string.IsNullOrEmpty(index)? file : index;
            string[] txt = { "empty", "empty" };
            Sprite[] imgs = { null, null, null, null };
            saveData savedata = data.readFile(fileName);
            if (savedata != null)
            {
                double playTime = savedata.playTime;

                //地图名字
                txt[0] = findText(savedata.mapName, data.getSetting().nowlang);
                // 游戏时间
                int hour = (int)playTime / 3600;
                int minute = ((int)playTime - hour * 3600) / 60;
                int second = (int)playTime - hour * 3600 - minute * 60;
                txt[1] = string.Format("{0:D2}:{1:D2}:{2:D2}", hour, minute, second);
                // 人物头像
                for (int i = 0; i < imgs.Length; i++)
                {
                    if (i < savedata.team.Count)
                    {
                        imgs[i] = data.getPlayerInfos()[savedata.team[i]].face;
                    }
                }
            }
            else
            {
                operate.changeBtn(0, false);
                operate.changeBtn(2, false);
            }
            file = fileName;
            setShow(new ListItemData(imgs, txt), showPanels[1]);

            if (data.getPlayer() == null)
                operate.changeBtn(1,false);
        }

        private void showChoosePanel(string key)
        {
            string t = "";
            if (key == texts[0])
                t = "sys.loadtext";
            else if (key == texts[1])
                t = "sys.savetext";
            else if (key == texts[2])
                t = "sys.deletext";

            var d = data.readFile(file);
            if (((key != texts[1]) && (d != null)) || ((key == texts[1]) && (data.getPlayer() != null)))
            {
                playSE(yes);
                string text = findText(t, data.getSetting().nowlang);
                Func _yes = delegate { doSelect(key, d); operate.cancel(); }, _no = delegate { operate.cancel(); };
                data.showChoosePanel(text, _yes, _no, nowState.window, startPanel.gameObject, true);
                startPanel.ableDis(afterDis.hide);
            }
            else
            {
                playSE(unable);
            }
        }

        private void doSelect(string key, saveData savedata)
        {
            // 读取
            if (key == texts[0])
            {
                if (data.getPlayer() != null)
                    Destroy(menuPanel);
                changeSceneDo csd = new changeSceneDo(savedata.team[0], savedata.position.toVec3(), turnTool.get(savedata.turn.toVec3()));
                data.loadData(savedata.fileName);
                data.LoadLevel(savedata.mapName, csd);
                exit();
            }
            else if (key == texts[1])// 保存
            {
                data.saveData(file);
            }
            else if (key == texts[2])// 删除
            {
                data.deleteData(file);
            }

            setInfo();
        }
    }
}
