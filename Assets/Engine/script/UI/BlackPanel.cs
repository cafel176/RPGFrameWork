using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//淡入淡出蒙版
public class BlackPanel : MonoBehaviour {

    private float change=0;

    private void Update()
    {
        if (this.gameObject.GetComponent<RawImage>().color.a >= 1 && change > 0)
        {
            Color color = this.gameObject.GetComponent<RawImage>().color;
            color.a = 1;
            this.gameObject.GetComponent<RawImage>().color = color;
            change = 0;
        }
        else if (this.gameObject.GetComponent<RawImage>().color.a <= 0 && change < 0)
        {
            Color color = this.gameObject.GetComponent<RawImage>().color;
            color.a = 0;
            this.gameObject.GetComponent<RawImage>().color = color;
            change = 0;
        }
        else if(change!=0)
        {
            Color color = this.gameObject.GetComponent<RawImage>().color;
            color.a += change * Time.deltaTime;
            this.gameObject.GetComponent<RawImage>().color = color;
            transform.SetAsLastSibling();
        }
    }

    public void beBlack(float time)
    {
        if (change <= 0)
        {
            change = 1.0f/time;
        }
    }

    public void beWhite(float time)
    {
        if (change >= 0)
        {
            change = -1.0f / time;
        }
    }
}
