using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    //淡入淡出蒙版
    public class BlackPanel : basePanel
    {

        private float change = 0;

        private void Update()
        {
            doEveryFrame();
        }

        public override void doEveryFrame()
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
                transform.SetAsFirstSibling();
            }
            else if (change != 0)
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
                change = 1.0f / time;
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
}
