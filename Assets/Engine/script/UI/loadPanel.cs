using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.UI;
using System.IO;

public class loadPanel : keyBoardMenu
{
    [HideInInspector]
    public GameObject menuPanel;

    public int pageNum = 10;

    public Text[] files;
    public Image[] avators;
    public Text[] infos;//0地图，1时间
    public GameObject tabCursor;
    public Vector3 tabMove;
    public Sprite UIMask;

    private int tabPos = 0, tabNum = 3;
    private Vector3 p, p2;
    private int nowPage = 0;

    override protected void init()
    {
        cursor.SetActive(true);
        p = cursor.transform.localPosition;
        p2 = tabCursor.transform.localPosition;
        optionNum = files.Length;
        doFuncs = new Func[optionNum];
        for (int i = 0; i < optionNum; i++)
        {
            files[i].gameObject.SetActive(true);
            doFuncs[i] = new Func(showChoosePanel);
        }

        drawPanel();

        otherFuncs[0] = new Func(_cancel);
        otherFuncs[1] = new Func(moveLeft);
        otherFuncs[2] = new Func(moveRight);
        otherFuncs[3] = new Func(moveUp);
        otherFuncs[4] = new Func(moveDown);
        menuPanel = UIManager.instance.getMainMenu();

        tabCursor.SetActive(false);
    }

    public void moveLeft()
    {
        if (!tabCursor.activeInHierarchy)
            return;

        tabPos = (tabPos - 1 + tabNum) % tabNum;
        tabCursor.transform.localPosition = p2 + tabPos * tabMove;
        if (tabPos == 0)
            texts[0].text.text = findText("sys.loadhelp", gameManager.instance.setting.nowlang);
        else if (tabPos == 1)
            texts[0].text.text = findText("sys.savehelp", gameManager.instance.setting.nowlang);
        else
            texts[0].text.text = findText("sys.delehelp", gameManager.instance.setting.nowlang);

        drawPanel();
    }

    public void moveRight()
    {
        if (!tabCursor.activeInHierarchy)
            return;

        tabPos = (tabPos + 1) % tabNum;
        tabCursor.transform.localPosition = p2 + tabPos * tabMove;
        if (tabPos == 0)
            texts[0].text.text = findText("sys.loadhelp", gameManager.instance.setting.nowlang);
        else if (tabPos == 1)
            texts[0].text.text = findText("sys.savehelp", gameManager.instance.setting.nowlang);
        else
            texts[0].text.text = findText("sys.delehelp", gameManager.instance.setting.nowlang);

        drawPanel();
    }

    public void moveUp()
    {
        if (tabCursor.activeInHierarchy)
            return;

        if (pageNum > 1 && cursorPos == 0)
        {
            nowPage = (nowPage - 1 + pageNum) % pageNum;
        }

        cursorPos = (cursorPos - 1 + optionNum) % optionNum;
        cursor.transform.localPosition = p + cursorPos * nextMove;

        drawPanel();
    }

    public void moveDown()
    {
        if (tabCursor.activeInHierarchy)
            return;

        if (pageNum > 1 && cursorPos == optionNum - 1)
        {
            nowPage = (nowPage + 1) % pageNum;
        }

        cursorPos = (cursorPos + 1) % optionNum;
        cursor.transform.localPosition = p + cursorPos * nextMove;

        drawPanel();
    }

    private void _cancel()
    {
        if (tabCursor.activeInHierarchy)
        {
            tabCursor.SetActive(false);
        }
        else
        {
            exit();
        }
    }

    private void exit()
    {
        if (menuPanel != null)
            gameManager.instance.changeState(nowState.window, menuPanel);
        else
            gameManager.instance.changeState(nowState.move);
        ableDis();
    }

    public void drawPanel()
    {
        for (int i=0;i<optionNum;i++)
        {
            files[i].text = "* File "+ (optionNum * nowPage + i+1);
        }

        if(gameManager.instance.player==null)
        {
            texts[2].text.transform.parent.GetComponent<Button>().interactable= false;
        }
        else
        {
            texts[2].text.transform.parent.GetComponent<Button>().interactable = true;
        }

        string fileName = "file" + (optionNum * nowPage + cursorPos + 1);
        string filePath = Application.dataPath + @"/Save/" + fileName + ".json";
        if(System.IO.File.Exists(filePath))
        {
            texts[1].text.transform.parent.GetComponent<Button>().interactable = true;
            texts[3].text.transform.parent.GetComponent<Button>().interactable = true;

            string txt = File.ReadAllText(filePath);
            saveData data = JsonMapper.ToObject<saveData>(txt);

            double playTime = data.playTime;

            //地图名字
            infos[0].text = findText(data.mapName, gameManager.instance.setting.nowlang);
            // 游戏时间
            int hour = (int)playTime / 3600;
            int minute = ((int)playTime - hour * 3600) / 60;
            int second = (int)playTime - hour * 3600 - minute * 60;
            infos[1].text = string.Format("{0:D2}:{1:D2}:{2:D2}", hour, minute, second);
            // 人物头像
            for (int i = 0; i < avators.Length; i++)
            {
                if (i < data.team.Count)
                {
                    avators[i].sprite = dataManager.instance.playerInfos[data.team[i]].face;
                }
                else
                {
                    avators[i].sprite = UIMask;
                }
            }
        }
        else
        {
            texts[1].text.transform.parent.GetComponent<Button>().interactable = false;
            texts[3].text.transform.parent.GetComponent<Button>().interactable = false;
            infos[0].text = "empty";
            infos[1].text = "empty";
            for (int i = 0; i < avators.Length; i++)
            {
                avators[i].sprite = UIMask;
            }
        }
    }

    public void showChoosePanel()
    {
        if (!tabCursor.activeInHierarchy)
        {
            tabCursor.SetActive(true);
            return;
        }

        string fileName = "file" + (optionNum * nowPage + cursorPos + 1);
        string filePath = Application.dataPath + @"/Save/" + fileName + ".json";

        string text = "";
        Func _yes = delegate { doSth(); }, _no = delegate { };
        if (tabPos == 0)
        {
            if (System.IO.File.Exists(filePath))
            {
                playSE(yes);
                text = findText("sys.loadtext", gameManager.instance.setting.nowlang);
                UIManager.instance.showChoosePanel(text, _yes, _no, gameObject, true);
            }
            else
            {
                playSE(unable);
            }
        }
        else if (tabPos == 1)
        {
            if (gameManager.instance.player != null)
            {
                playSE(yes);
                text = findText("sys.savetext", gameManager.instance.setting.nowlang);
                UIManager.instance.showChoosePanel(text, _yes, _no, gameObject, true);
            }
            else
            {
                playSE(unable);
            }
        }
        else
        {
            if (System.IO.File.Exists(filePath))
            {
                playSE(yes);
                text = findText("sys.deletext", gameManager.instance.setting.nowlang);
                UIManager.instance.showChoosePanel(text, _yes, _no, gameObject, true);
            }
            else
            {
                playSE(unable);
            }
        }
    }

    public void doSth()
    {
        string fileName = "file" + (optionNum * nowPage + cursorPos + 1);
        string filePath = Application.dataPath + @"/Save/" + fileName + ".json";
        // 读取
        if (tabPos==0)
        {
            saveData data = dataManager.instance.loadData("file" + (optionNum * nowPage + cursorPos + 1));
            if (gameManager.instance.player != null)
                Destroy(menuPanel);
            gameManager.instance.LoadLevel(data.mapName, data.team[0], data.position.toVec3(), data.turn.toVec3());
            exit();
        }
        else if (tabPos == 1)// 保存
        {
            dataManager.instance.saveData("file" + (optionNum * nowPage + cursorPos + 1));
            exit();
        }
        else// 删除
        {
            dataManager.instance.deleteFile(filePath);
            drawPanel();
        }
    }
}
