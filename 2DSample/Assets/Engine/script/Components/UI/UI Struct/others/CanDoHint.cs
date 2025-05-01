using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UI;

public class CanDoHint : basePanel
{
    public Text txt;

    private Vector3 pos;
    public Vector3 Pos
    {
        set
        {
            pos = value;
        }
    }

    private bool show = false;
    public bool canShow
    {
        get
        {
            return show;
        }
    }

    private Animator anima;

    override protected void onAwake()
    {
        anima = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        doEveryFrame();
    }

    override public void doEveryFrame()
    {
        if (show)
        {
            Vector2 cPos = Camera.main.WorldToScreenPoint(pos);
            Vector2 uipos = new Vector2(cPos.x - Screen.width * 0.5f, cPos.y - 0.5f * Screen.height);
            gameObject.GetComponent<RectTransform>().localPosition = uipos;
            transform.SetAsFirstSibling();
        }
    }

    public void Show(string t)
    {
        txt.text = findText(t, getSetting().nowlang);
        show = true;
        anima.SetTrigger(getHat().start);
    }

    public void Hide()
    {
        anima.SetTrigger(getHat().end);
    }

    public void showAnimaEnd()
    {

    }

    public void hideAnimaEnd()
    {
        show = false;
        gameObject.SetActive(false);
    }
}
