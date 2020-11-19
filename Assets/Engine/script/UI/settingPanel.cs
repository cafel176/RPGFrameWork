using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class settingPanel : keyBoardMenu
{
    public Text runText, autoMessageText, sizeText, musicText, SEText,langText;

    [HideInInspector]
    public basePanel mainPanel;

    override protected void init()
    {
        optionNum = 7;
        doFuncs = new Func[optionNum];
        doFuncs[0] = new Func(changeBool);
        doFuncs[1] = new Func(changeBool);
        doFuncs[2] = new Func(addValue);
        doFuncs[3] = new Func(addValue);
        doFuncs[4] = new Func(addValue);
        doFuncs[5] = new Func(changeLanguage);
        doFuncs[6] = new Func(exit);
        otherFuncs[0] = new Func(_cancel);
        otherFuncs[1] = new Func(_cancel);
        otherFuncs[2] = new Func(_right);
        otherFuncs[3] = new Func(moveUp);
        otherFuncs[4] = new Func(moveDown);
        mainPanel = UIManager.instance.getMainMenu().GetComponent<basePanel>();

        //初始化UI数值
        musicText.text = Mathf.RoundToInt(gameManager.instance.setting.musicValue * 10) + "0%";
        SEText.text = Mathf.RoundToInt(gameManager.instance.setting.SEValue * 10) + "0%";
        if (gameManager.instance.setting.windowSize == gameManager.instance.maxSize)
            sizeText.text = "全屏";
        else
            sizeText.text = "x "+gameManager.instance.setting.windowSize;
        if (gameManager.instance.setting.alwaysRun)
            runText.text = "ON";
        else
            runText.text = "OFF";
        if (gameManager.instance.setting.autoMessage)
            autoMessageText.text = "ON";
        else
            autoMessageText.text = "OFF";
        langText.text = gameManager.instance.setting.nowlang.ToString();

        afterDis = delegate { Destroy(gameObject); };
    }

    void _right()
    {
        if (cursorPos == 5)
        {
            changeLanguage();
        }
        else if (cursorPos == 0 || cursorPos == 1)
        {
            changeBool();
        }
        else
        {
            addValue();
        }
    }

    void _cancel()
    {
        if (cursorPos == 5)
        {
            gameManager.instance.setting.nowlang = gameManager.instance.changeLanguage(false);
            langText.text = gameManager.instance.setting.nowlang.ToString();
            ableInit();
        }
        else if (cursorPos == 0|| cursorPos == 1)
        {
            changeBool();
        }
        else
        {
            reduceValue();
        }
    }

    void changeLanguage()
    {
        gameManager.instance.setting.nowlang = gameManager.instance.changeLanguage(true);
        langText.text = gameManager.instance.setting.nowlang.ToString();
        ableInit();
    }

    void changeBool()
    {
        if (cursorPos == 0)
        {
            gameManager.instance.changeBool("alwaysrun");
            if (gameManager.instance.setting.alwaysRun)
                runText.text = "ON";
            else
                runText.text = "OFF";
        }
        else if (cursorPos == 1)
        {
            gameManager.instance.changeBool("autoMessage");
            if (gameManager.instance.setting.autoMessage)
                autoMessageText.text = "ON";
            else
                autoMessageText.text = "OFF";
        }
    }

    void addValue()
    {
        if (cursorPos==2)
        {
            gameManager.instance.addWindowSize(true);
            if (gameManager.instance.setting.windowSize == gameManager.instance.maxSize)
                sizeText.text = "全屏";
            else
                sizeText.text = "x " + gameManager.instance.setting.windowSize;
            mainPanel.changeSize();
        }
        else if (cursorPos == 3)
        {
            if (gameManager.instance.setting.musicValue < 0.95)
            {
                gameManager.instance.changeMusicValue(0.1f);
                musicText.text = Mathf.RoundToInt(gameManager.instance.setting.musicValue * 10) + "0%";
            }
            else
            {
                gameManager.instance.setting.musicValue = 1;
                musicText.text = "100%";
            }
        }
        else if (cursorPos == 4)
        {
            if (gameManager.instance.setting.SEValue < 0.95)
            {
                gameManager.instance.changeSEValue(0.1f); 
                SEText.text = Mathf.RoundToInt(gameManager.instance.setting.SEValue * 10) + "0%";
            }
            else
            {
                gameManager.instance.setting.SEValue = 1;
                SEText.text = "100%";
            }
        }
    }

    void reduceValue()
    {
        if (cursorPos == 2)
        {
            gameManager.instance.addWindowSize(false);
            if (gameManager.instance.setting.windowSize == gameManager.instance.maxSize)
                sizeText.text = "全屏";
            else
                sizeText.text = "x " + gameManager.instance.setting.windowSize;
            mainPanel.changeSize();
        }
        else if (cursorPos == 3)
        {
            if (gameManager.instance.setting.musicValue > 0.05)
            {
                gameManager.instance.changeMusicValue(-0.1f);
                musicText.text = Mathf.RoundToInt(gameManager.instance.setting.musicValue * 10) + "0%";
            }
            else
            {
                gameManager.instance.setting.musicValue =0;
                musicText.text = "0%";
            }
        }
        else if (cursorPos == 4)
        {
            if (gameManager.instance.setting.SEValue > 0.05)
            {
                gameManager.instance.changeSEValue(-0.1f);
                SEText.text = Mathf.RoundToInt(gameManager.instance.setting.SEValue * 10) + "0%";
            }
            else
            {
                gameManager.instance.setting.SEValue = 0;
                SEText.text = "0%";
            }

        }
    }

    void exit()
    {
        gameManager.instance.saveSetting();
        if(mainPanel!=null)
            gameManager.instance.changeState(nowState.window, mainPanel.gameObject);
        else
            gameManager.instance.changeState(nowState.move);
        ableDis();
    }

    void moveUp()
    {
        changeCursorPos(false);
    }

    void moveDown()
    {
        changeCursorPos(true);
    }
}
