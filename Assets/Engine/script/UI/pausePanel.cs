using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pausePanel : keyBoardMenu
{
    [HideInInspector]
    public GameObject mainPanel;

    override protected void init()
    {
        optionNum = 2;
        doFuncs = new Func[optionNum];
        doFuncs[0] = new Func(yes);
        doFuncs[1] = new Func(no);

        otherFuncs[0] = new Func(no);
        otherFuncs[3] = new Func(moveUp);
        otherFuncs[4] = new Func(moveDown);
        mainPanel = UIManager.instance.getMainMenu();
    }

    void yes()
    {
        Application.Quit();
    }

    void no()
    {
        exit();
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
        Destroy(gameObject);
    }
}
