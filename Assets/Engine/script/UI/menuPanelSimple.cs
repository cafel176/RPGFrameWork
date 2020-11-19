using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class menuPanelSimple : keyBoardMenu
{
    public GameObject itemMenu, optionMenu,questMenu;
    public string[] keys;
    public Button[] btns;

    protected string nowThink;

    override protected void init()
    {
        optionNum = 4;
        doFuncs = new Func[optionNum];
        doFuncs[0] = new Func(select);
        doFuncs[1] = new Func(select);
        doFuncs[2] = new Func(select);
        doFuncs[3] = new Func(select);
        otherFuncs[0] = new Func(exit);
        otherFuncs[1] = new Func(moveLeft);
        otherFuncs[2] = new Func(moveRight);
        otherFuncs[3] = new Func(moveUp);
        otherFuncs[4] = new Func(moveDown);
        btns[cursorPos].image.sprite = findImg(keys[cursorPos*2]);
        afterDis = delegate { Destroy(gameObject); UIManager.instance.setMainMenu(null); };

        //人物id#公共事件名   如：0#charlotte_think
        int i = dataManager.instance.getInt("剧情编号");// 定义一个跟随剧情的think变量
        string text = findText("think." + i, gameManager.instance.setting.nowlang);
        int id = 0;
        nowThink = "夏洛特TALK";
        if (text == "can not find")
        {
            text = findText("vane." + i, gameManager.instance.setting.nowlang);
            id = 1;
            nowThink = "瓦妮莎TALK";
        }
        if (text == "can not find")
        {
            id = 2;
            nowThink = "伊万TALK";
        }

        keys[4] = id + "_think";
        keys[5] = id + "_choose";

        btns[0].image.sprite = findImg("item_choose");
        btns[2].image.sprite = findImg(keys[4]);
    }

    private void select()
    {
        if (cursorPos == 0)
        {
            showItemMenu();
        }
        else if (cursorPos == 1)
        {
            showQuestMenu();
        }
        else if (cursorPos == 2)
        {
            showThink();
        }
        else if (cursorPos == 3)
        {
            showOptionMenu();
        }
    }

    public void showOptionMenu()
    {
        UIManager.instance.showAnyPanel(optionMenu, Vector2.zero, true);
        ableDis();
    }

    public void showItemMenu()
    {
        UIManager.instance.showAnyPanel(itemMenu, Vector2.zero, true);
        ableDis();
    }

    public void showQuestMenu()
    {
        UIManager.instance.showAnyPanel(questMenu, Vector2.zero, true);
        ableDis();
    }

    public void showThink()
    {
        gameManager.instance.startPublicEvent(nowThink);
        ableDis();
    }

    private void exit()
    {
        gameManager.instance.changeState(nowState.move);
        ableDis();
    }

    void moveLeft()
    {
        if (cursorPos != 1)
        {
            btns[cursorPos].image.sprite = findImg(keys[cursorPos * 2]);
            cursorPos=1;
            btns[cursorPos].image.sprite = findImg(keys[cursorPos*2+1]);
        }
    }

    void moveRight()
    {
        if (cursorPos != 2)
        {
            btns[cursorPos].image.sprite = findImg(keys[cursorPos * 2]);
            cursorPos=2;
            btns[cursorPos].image.sprite = findImg(keys[cursorPos*2+1]);
        }
    }

    void moveUp()
    {
        if (cursorPos != 0)
        {
            btns[cursorPos].image.sprite = findImg(keys[cursorPos * 2]);
            cursorPos =0;
            btns[cursorPos].image.sprite = findImg(keys[cursorPos * 2 + 1]);
        }
    }

    void moveDown()
    {
        if (cursorPos != 3)
        {
            btns[cursorPos].image.sprite = findImg(keys[cursorPos * 2]);
            cursorPos = 3;
            btns[cursorPos].image.sprite = findImg(keys[cursorPos*2+1]);
        }
    }
}
