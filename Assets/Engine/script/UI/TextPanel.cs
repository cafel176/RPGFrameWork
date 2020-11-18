using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextPanel : basePanel
{  
    public Text mainText,nameText;
    public GameObject avatorPanel;
    public Image Avator;

    public void setMainText(string txt)
    {
        mainText.text = txt;
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
        }
        else
        {
            avatorPanel.SetActive(true);
            Avator.sprite = avator;
        }
    }
}
