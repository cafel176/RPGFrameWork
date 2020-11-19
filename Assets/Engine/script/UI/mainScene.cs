using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mainScene : keyBoardMenu
{
    public GameObject loadPanel;
    public GameObject settingPanel;

    override protected void init()
    {
        if (gameManager.instance.musicAudio != null)
        {
            UIManager.instance.beWhite(1.0f, true);
        }

        changeSize();
        optionNum = 3;
        doFuncs = new Func[optionNum];
        doFuncs[0] = new Func(startGame);
        doFuncs[1] = new Func(loadGame);
        doFuncs[2] = new Func(setting);
        //doFuncs[3] = new Func(exit);
        otherFuncs[1] = new Func(moveUp);
        otherFuncs[2] = new Func(moveDown);
        otherFuncs[3] = new Func(moveUp);
        otherFuncs[4] = new Func(moveDown);
        gameManager.instance.changeState(nowState.window, this.gameObject);
        UIManager.instance.setMainMenu(gameObject);
        afterDis = delegate { panel.SetActive(false); panel.transform.localScale = nowScale; enabled = false; };
    }

    public void startGame()
    {
        playSE(yes);
        gameManager.instance.LoadLevel("Garden prologue", 0,gameManager.instance.startPos);
        ableDis();
    }

    public void loadGame()
    {
        playSE(yes);
        UIManager.instance.showAnyPanel(loadPanel, Vector2.zero,true);
        ableDis();
    }

    public void setting()
    {
        playSE(yes);
        UIManager.instance.showAnyPanel(settingPanel, Vector2.zero, true);
        ableDis();
    }

    public void exit()
    {
        Application.Quit();
    }

    void moveUp()
    {
        changeCursorPos(false);
    }

    void moveDown()
    {
        changeCursorPos(true);
    }

    private void OnDestroy()
    {
        UIManager.instance.setMainMenu(null);
    }
}
