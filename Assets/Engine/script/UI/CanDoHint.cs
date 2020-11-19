using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanDoHint : basePanel
{
    Animator anima;
    public Text txt;
    [HideInInspector]
    public Vector3 pos;
    [HideInInspector]
    public bool show = false;

    void Awake()
    {
        anima = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        if(show)
        {
            Vector2 cPos = Camera.main.WorldToScreenPoint(pos);
            Vector2 uipos = new Vector2(cPos.x - Screen.width * 0.5f, cPos.y - 0.5f * Screen.height);
            gameObject.GetComponent<RectTransform>().localPosition = uipos;
            transform.SetAsFirstSibling();
        }
    }

    public void Show(string t)
    {
        txt.text = gameManager.instance.hat.find(t, gameManager.instance.setting.nowlang);
        show = true;
        anima.SetTrigger(gameManager.instance.hat.start);
    }

    public void Hide()
    {
        anima.SetTrigger(gameManager.instance.hat.end);
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
