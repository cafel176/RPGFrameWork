using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PicPanel : basePanel
{
    protected float change = 0;
    protected Vector3 target,move;
    protected bool destroy = false;

    private void Update()
    {
        if(move!=Vector3.zero)
        {
            transform.localPosition += move * Time.deltaTime;
            if(Vector3.Distance(transform.localPosition,target)<1f)
            {
                transform.localPosition = target;
                move = Vector3.zero;
                if (destroy)
                    Destroy(gameObject);
            }
        }

        if (gameObject.GetComponent<Image>().color.a >= 1 && change > 0)
        {
            Color color = gameObject.GetComponent<Image>().color;
            color.a = 1;
            gameObject.GetComponent<Image>().color = color;
            change = 0;
        }
        else if (gameObject.GetComponent<Image>().color.a <= 0 && change < 0)
        {
            Color color = gameObject.GetComponent<Image>().color;
            color.a = 0;
            gameObject.GetComponent<Image>().color = color;
            change = 0;
        }
        else if (change != 0)
        {
            Color color = gameObject.GetComponent<Image>().color;
            color.a += change * Time.deltaTime;
            gameObject.GetComponent<Image>().color = color;
        }
    }

    public void setSize(float w, float h)
    {
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(w,h);
    }

    public void setAlpha(float a)
    {
        var c = gameObject.GetComponent<Image>().color;
        c.a = a;
        gameObject.GetComponent<Image>().color = c;
    }

    public void setColor(Color a)
    {
        var c = gameObject.GetComponent<Image>().color;
        c = a;
        gameObject.GetComponent<Image>().color = c;
    }

    public void movePic(Vector3 t, float a, float time,bool _destroy = false)
    {
        var c = gameObject.GetComponent<Image>().color.a;
        float smoothing = Mathf.Abs(c - a) / time;
        destroy = _destroy;
        if (c < a)
        {
            change = smoothing;
        }
        else
        {
            change = -smoothing;
        }

        Vector3 now = transform.localPosition;
        target = t;
        move = (t - now) / time;
    }

    public void setPic(Sprite sp)
    {
        gameObject.GetComponent<Image>().sprite = sp;
    }
}
