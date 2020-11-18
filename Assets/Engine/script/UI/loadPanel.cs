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
    public Text[] infos;
    public GameObject tabCursor;
    public Vector3 tabMove;
    public Sprite UIMask;

    public GameObject[] masks;

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
            doFuncs[i] = new Func(doSth);
        }

        drawPanel();

        otherFuncs[0] = new Func(exit);
        otherFuncs[1] = new Func(moveLeft);
        otherFuncs[2] = new Func(moveRight);
        otherFuncs[3] = new Func(moveUp);
        otherFuncs[4] = new Func(moveDown);
        menuPanel = UIManager.instance.getMainMenu();
    }

    public void moveLeft()
    {
        tabPos = (tabPos - 1 + tabNum) % tabNum;
        tabCursor.transform.localPosition = p2 + tabPos * tabMove;

        drawPanel();
    }

    public void moveRight()
    {
        tabPos = (tabPos + 1) % tabNum;
        tabCursor.transform.localPosition = p2 + tabPos * tabMove;

        drawPanel();
    }

    public void moveUp()
    {
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
        if (pageNum > 1 && cursorPos == optionNum - 1)
        {
            nowPage = (nowPage + 1) % pageNum;
        }

        cursorPos = (cursorPos + 1) % optionNum;
        cursor.transform.localPosition = p + cursorPos * nextMove;

        drawPanel();
    }

    private void exit()
    {
        if (menuPanel != null)
            gameManager.instance.changeState(nowState.window, menuPanel);
        else
            gameManager.instance.changeState(nowState.move);
        Destroy(gameObject);
    }

    public void drawPanel()
    {
        for (int i=0;i<optionNum;i++)
        {
            files[i].text = "* File "+ (optionNum * nowPage + i+1);
        }

        if(gameManager.instance.player==null)
        {
            masks[2].SetActive(true);
        }
        else
        {
            masks[2].SetActive(false);
        }

        string fileName = "file" + (optionNum * nowPage + cursorPos + 1);
        string filePath = Application.dataPath + @"/Save/" + fileName + ".json";
        if(System.IO.File.Exists(filePath))
        {
            masks[0].SetActive(false);
            masks[1].SetActive(false);

            string txt = File.ReadAllText(filePath);
            saveData data = JsonMapper.ToObject<saveData>(txt);

            double playTime = data.playTime;

            // 游戏时间
            int hour = (int)playTime / 3600;
            int minute = ((int)playTime - hour * 3600) / 60;
            int second = (int)playTime - hour * 3600 - minute * 60;
            infos[0].text = string.Format("{0:D2}:{1:D2}:{2:D2}", hour, minute, second);
            // 人物头像
            for (int i = 0; i < avators.Length; i++)
            {
                if (i < data.team.Count)
                {
                    avators[i].sprite = dataManager.instance.faces[data.team[i]];
                }
                else
                {
                    avators[i].sprite = UIMask;
                }
            }
        }
        else
        {
            masks[0].SetActive(true);
            masks[1].SetActive(true);
            infos[0].text = "empty";
            for (int i = 0; i < avators.Length; i++)
            {
                avators[i].sprite = UIMask;
            }
        }
    }

    public void doSth()
    {
        string fileName = "file" + (optionNum * nowPage + cursorPos + 1);
        string filePath = Application.dataPath + @"/Save/" + fileName + ".json";
        if (tabPos==0)
        {
            if (System.IO.File.Exists(filePath))
            {
                saveData data = dataManager.instance.loadData("file" + (optionNum * nowPage + cursorPos + 1));
                if (gameManager.instance.player != null)
                    Destroy(menuPanel);
                gameManager.instance.LoadLevel(data.mapId,data.team[0],data.position.toVec3(),data.turn.toVec3());
                Destroy(gameObject);
            }
        }
        else if (tabPos == 1)
        {
            if (gameManager.instance.player != null)
            {
                dataManager.instance.saveData("file" + (optionNum * nowPage + cursorPos + 1));
                exit();
            }           
        }
        else
        {
            if (System.IO.File.Exists(filePath))
            {
                dataManager.instance.deleteFile(filePath);
                drawPanel();
            }
            else
            {

            }
        }
    }
}
