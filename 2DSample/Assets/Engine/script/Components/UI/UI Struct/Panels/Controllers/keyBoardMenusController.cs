using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [System.Serializable]
    public class constText
    {
        public string txt;
        public Text text;
    }

    [RequireComponent(typeof(AudioSource))]
    public abstract class keyBoardMenusController : basePanel, funcable
    {
        public afterDis afterDis = afterDis.destroy;
        public showType showType = showType.all;

        protected GameObject menuPanel;

        [SerializeField]
        protected Sprite noneImg = null;
        public Sprite NoneImg
        {
            get
            {
                return noneImg;
            }
        }

        [SerializeField]
        protected string noneText = "";
        public string NoneText
        {
            get
            {
                return noneText;
            }
        }

        [SerializeField]
        protected keyBoardMenuList startPanel;

        [SerializeField]
        protected listItemUI[] showPanels;

        protected AudioClip yes;
        protected AudioClip no;
        protected AudioClip unable;

        protected keyBoardMenuList[] children;

        protected AudioSource _audio = null;

        protected override void onAwake()
        {
            base.onAwake();

            for (int i = 0; i < showPanels.Length; i++)
            {
                showPanels[i].NoneImg = noneImg;
                showPanels[i].NoneText = noneText;
                setShow(new ListItemData(null,null), showPanels[i]);
            }

            setMenuPanel();
        }

        override protected void onStart()
        {
            base.onStart();

            yes = findAudio("确定");
            no = findAudio("取消");
            unable = findAudio("无效");

            _audio = gameObject.GetComponent<AudioSource>();
            children = gameObject.GetComponentsInChildren<keyBoardMenuList>();
            for (int i = 0; i < children.Length; i++)
            {
                children[i].setDataSource(i, this);
                children[i].spawanItems();
                children[i].updateItems();
                if(showType == showType.tree)
                {
                    children[i].gameObject.SetActive(false);
                }
            }

            startPanel.gameObject.SetActive(true);
            startPanel.doFunc("", gameObject);
        }

        public void setMenuPanel(GameObject g = null)
        {
            if(g==null)
                menuPanel = getMainMenu();
            else
                menuPanel = g;
        }

        public void doFunc(string _param, GameObject _parent)
        {
            doFunc();
        }

        public void doFunc()
        {
            exit();
        }

        protected virtual void exit()
        {
            if (menuPanel != null)
                changeState(nowState.window, menuPanel);
            else
                changeState(nowState.move);

            startPanel.ableDis(afterDis);
        }

        public abstract List<ListItemData> getDatas(int index, string param);

        public abstract List<string> getKeys(int index, string param);

        protected void setShow(ListItemData data, listItemUI panel)
        {
            if (panel != null)
                panel.setDataIfNotNull(data);
        }

        public void playSE(AudioClip clip, bool exit = false)
        {
            if (exit)
            {
                playSE(clip);
                return;
            }
            if (_audio != null)
            {
                _audio.volume = getSetting().SEValue;
                _audio.clip = clip;
                _audio.Play();
            }
        }
    }
}
