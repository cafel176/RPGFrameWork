using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    [System.Serializable]
    public class ListItemData 
    {
        public bool bright = true;
        public Sprite[] pics = null;
        public string[] texts = null;

        public ListItemData(Sprite[] _pics, string[] _texts, bool _bright = true)
        {
            pics = _pics;
            texts = _texts;
            bright = _bright;
        }

        public ListItemData(Sprite[] _pics = null, bool _bright = true)
        {
            pics = _pics;
            bright = _bright;
        }

        public ListItemData(string[] _texts = null)
        {
            texts = _texts;
        }
    }

    public sealed class listItemUI : baseUI, funcable, IPointerEnterHandler
    {
        private string key = "";
        public string Key
        {
            get
            {
                return key;
            }
        }

        private Sprite noneImg = null;
        public Sprite NoneImg
        {
            set
            {
                noneImg = value;
            }
        }

        private string noneText = "";
        public string NoneText
        {
            set
            {
                noneText = value;
            }
        }

        [SerializeField]
        private Image[] imgs;
        [SerializeField]
        private Text[] txts;

        public delegate void listItemFunc(string i);

        public event listItemFunc callback;

        public Func hover;

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (hover != null)
                hover();
        }

        public void setKeyIfNotNull(string _key)
        {
            if(!string.IsNullOrEmpty(_key))
                key = _key;
        }

        public void setDataIfNotNull(ListItemData data)
        {
            clearData();
            if (data == null)
                return;
            if (data.pics != null && imgs != null)
                for (int i = 0; i < data.pics.Length && i < imgs.Length; i++)
                {
                    if (data.pics[i] == null)
                        imgs[i].sprite = noneImg;
                    else
                        imgs[i].sprite = data.pics[i];
                    if (data.bright)
                        imgs[i].color = new Color(1, 1, 1, 1);
                    else
                        imgs[i].color = new Color(0, 0, 0, 0.6f);
                }
            if (data.texts != null && txts != null)
                for (int i = 0; i < data.texts.Length && i < txts.Length; i++)
                {
                    if (string.IsNullOrEmpty(data.texts[i]))
                        txts[i].text = noneText;
                    else
                        txts[i].text = data.texts[i];
                    if (data.bright)
                        txts[i].color = new Color(1, 1, 1, 1);
                    else
                        txts[i].color = new Color(0.4f, 0.4f, 0.4f, 1);
                }
        }

        public void clearKey()
        {
            key = "";
        }

        public void clearData()
        {
            for (int i = 0; i < txts.Length; i++)
            {
                txts[i].text = noneText;
            }
            for (int i = 0; i < imgs.Length; i++)
            {
                imgs[i].sprite = noneImg;
            }
        }

        public void doFunc(string param, GameObject parent)
        {
            if(callback!=null)
                callback(param);
        }

        public void doFunc()
        {
            if (callback != null)
                callback(key);
        }
    }
}
