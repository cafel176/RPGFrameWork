using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TextPanel : basePanel
    {
        private float y;

        public float TextPanelXForAvator;
        public float TextYForName;
        public Text mainText, nameText;
        public GameObject textPanel;
        public GameObject avatorPanel;
        public Image Avator;

        override protected void onAwake()
        {
            base.onAwake();

            y = mainText.rectTransform.localPosition.y;
        }

        public void setMainText(string txt)
        {
            if (mainText.gameObject.activeInHierarchy)
                mainText.text = txt;
        }

        public void setNameText(string txt)
        {
            var p = mainText.rectTransform.localPosition;
            nameText.text = txt;
            if (txt == "")
            {
                mainText.transform.localPosition = new Vector3(p.x, y, p.z);
            }
            else
            {
                mainText.transform.localPosition = new Vector3(p.x, y + TextYForName, p.z);
            }
        }

        public void setAvator(Sprite avator)
        {
            var p = textPanel.transform.localPosition;
            if (avator == null)
            {
                avatorPanel.SetActive(false);
                textPanel.transform.localPosition = new Vector3(0, p.y, p.z);
            }
            else
            {    
                textPanel.transform.localPosition = new Vector3(TextPanelXForAvator, p.y, p.z);
                avatorPanel.SetActive(true);
                Avator.sprite = avator;
            }
        }
    }
}
