using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class menuPanel : keyBoardMenu
{
    public GameObject itemMenu, optionMenu,loadMenu,pauseMenu;
    public string[] keys;

    override protected void init()
    {
        optionNum = 9;
        doFuncs = new Func[optionNum];
        doFuncs[0] = new Func(select);
        doFuncs[1] = new Func(select);
        doFuncs[2] = new Func(select);
        doFuncs[3] = new Func(select);
        doFuncs[4] = new Func(select);
        doFuncs[5] = new Func(select);
        doFuncs[6] = new Func(select);
        doFuncs[7] = new Func(select);
        doFuncs[8] = new Func(select);
        otherFuncs[0] = new Func(exit);
        otherFuncs[1] = new Func(moveLeft);
        otherFuncs[2] = new Func(moveRight);
        texts[0].text.text = gameManager.instance.hat.find(keys[cursorPos], gameManager.instance.setting.nowlang);
    }

    private void select()
    {
        if (cursorPos == 0)
        {
            showItemMenu();
        }
        else if (cursorPos == 6)
        {
            showOptionMenu();
        }
        else if (cursorPos == 7)
        {
            showLoadMenu();
        }
        else if (cursorPos == 8)
        {
            showPauseMenu();
        }
    }

    public void showOptionMenu()
    {
        UIManager.instance.showAnyPanel(optionMenu, Vector2.zero, true);
    }

    public void showLoadMenu()
    {
        UIManager.instance.showAnyPanel(loadMenu, Vector2.zero, true);
    }

    public void showItemMenu()
    {
        UIManager.instance.showAnyPanel(itemMenu, Vector2.zero, true);
    }

    public void showPauseMenu()
    {
        UIManager.instance.showAnyPanel(pauseMenu, Vector2.zero, true);
    }

    private void exit()
    {
        gameManager.instance.changeState(nowState.move);
        Destroy(gameObject);
    }

    void moveLeft()
    {
        changeCursorPos(false);
        texts[0].key = keys[cursorPos];
        texts[0].text.text = gameManager.instance.hat.find(keys[cursorPos],gameManager.instance.setting.nowlang);
    }

    void moveRight()
    {
        changeCursorPos(true);
        texts[0].key = keys[cursorPos];
        texts[0].text.text = gameManager.instance.hat.find(keys[cursorPos], gameManager.instance.setting.nowlang);
    }

    public void exit2(GameObject g)
    {
        gameManager.instance.changeState(nowState.move);
        Destroy(g);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        UIManager.instance.setMainMenu(null);
    }
}
