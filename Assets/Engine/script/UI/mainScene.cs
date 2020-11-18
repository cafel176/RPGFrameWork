using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mainScene : keyBoardMenu
{
    public GameObject loadPanel;
    public GameObject settingPanel;

    private float time = 1f;
    private bool finish = true;

    override protected void init()
    {
        if (gameManager.instance.musicAudio != null)
        {
            UIManager.instance.beWhite(0.02f, true);
            //gameManager.instance.playMusic(gameManager.instance.main);
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
    }

    private void Update()
    {
        if (!finish)
        {
            finish = gameManager.instance.reduceMusicVolume(Time.deltaTime / time);
        }
    }

    public void startGame()
    {
        finish = false;
        gameManager.instance.LoadLevel(1,0,new Vector3(0,-1,0));
    }

    public void loadGame()
    {
        UIManager.instance.showAnyPanel(loadPanel, Vector2.zero,true);
    }

    public void setting()
    {
        UIManager.instance.showAnyPanel(settingPanel, Vector2.zero, true);
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
