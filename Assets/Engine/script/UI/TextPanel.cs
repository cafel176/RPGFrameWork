using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextPanel : basePanel
{  
    public Text mainText, mainText2, nameText;
    public GameObject avatorPanel;
    public Image Avator;

    public void setMainText(string txt)
    {
        if(mainText.gameObject.activeInHierarchy)
            mainText.text = txt;
        if (mainText2.gameObject.activeInHierarchy)
            mainText2.text = txt;
    }

    public void setNameText(string txt)
    {
         nameText.text = txt;
    }

    public void setAvator(Sprite avator)
    {
        if (avator == null)
        {
            avatorPanel.SetActive(false);
            mainText2.gameObject.SetActive(true);
            mainText.gameObject.SetActive(false);
        }
        else
        {
            avatorPanel.SetActive(true);
            mainText.gameObject.SetActive(true);
            mainText2.gameObject.SetActive(false);
            Avator.sprite = avator;
        }
    }
}
