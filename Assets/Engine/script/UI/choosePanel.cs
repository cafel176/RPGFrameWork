using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class choosePanel : keyBoardMenu
{
    [HideInInspector]
    public GameObject mainPanel;

    private string chooseText;
    public string ChooseText
    {
        set
        {
            chooseText = value;
            texts[0].text.text = chooseText;
        }
    }

    public Func yesFunc,noFunc;

    override protected void init()
    {
        optionNum = 2;
        doFuncs = new Func[optionNum];
        doFuncs[0] = new Func(_yes);
        doFuncs[1] = new Func(_no);

        otherFuncs[0] = new Func(_no);
        otherFuncs[1] = new Func(moveUp);
        otherFuncs[2] = new Func(moveDown);
        otherFuncs[3] = new Func(moveUp);
        otherFuncs[4] = new Func(moveDown);
        afterDis = delegate { Destroy(gameObject); };
    }

    public void BlackBackground()
    {
        gameObject.GetComponent<Image>().color = new Color(0, 0, 0, (168.0f / 255));
    }

    void _yes()
    {
        exit();
        yesFunc();
    }

    void _no()
    {
        exit();
        noFunc();
    }

    void moveUp()
    {
        changeCursorPos(false);
    }

    void moveDown()
    {
        changeCursorPos(true);
    }

    void exit()
    {
        if (mainPanel != null)
            gameManager.instance.changeState(nowState.window, mainPanel.gameObject);
        else
            gameManager.instance.changeState(nowState.move);
        ableDis();
    }
}
