using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class questMenu : keyBoardMenu
{
    [HideInInspector]
    public GameObject menuPanel;
    [HideInInspector]
    public int[,] quests;

    public Image[] questsImg;

    public Sprite UIMask;
    public GameObject tabCursor;
    public Vector3 tabMove;

    public Image title1Icon;
    public Text title1, title2;
    public Text hard, text,steps;

    private int tabPos = 0,tabNum = 4;
    private Vector3 p,p2;
    private int maxNum;
    private int nowpage = 0;
    private questStatus nowStatus;

    override protected void init()
    {
        nowStatus = questStatus.doing;
        cursor.SetActive(true);
        p = cursor.transform.localPosition;
        p2 = tabCursor.transform.localPosition;
        maxNum = questsImg.Length;
        doFuncs = new Func[maxNum];
        quests = new int[maxNum, 2];
        for (int i = 0; i < maxNum; i++)
        {
            questsImg[i].gameObject.SetActive(true);
            doFuncs[i] = new Func(select);
        }

        drawPanel();
        
        otherFuncs[0] = new Func(_cancel);
        otherFuncs[3] = new Func(moveUp);
        otherFuncs[4] = new Func(moveDown);
        menuPanel = UIManager.instance.getMainMenu();

        cursor.SetActive(false);
        afterDis = delegate { Destroy(gameObject); };
    }

    private void select()
    {
        if (!cursor.activeInHierarchy)
        {
            cursor.SetActive(true);
            return;
        }
    }

    private void _cancel()
    {
        if (cursor.activeInHierarchy)
        {
            cursor.SetActive(false);
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

    public void checkOptionNum(int totalNum)
    {
        if ((nowpage + 1) * maxNum - 1 < totalNum)
            optionNum = maxNum;
        else
            optionNum = totalNum % maxNum;
    }

    public void drawPanel()
    {
        int totalNum = dataManager.instance.getTotalQuestNum(nowStatus);
        checkOptionNum(totalNum);
        int start = maxNum * nowpage;
        if(cursorPos>=optionNum)
        {
            cursorPos = 0;
            cursor.transform.localPosition = p + cursorPos * nextMove;
        }

        int num = 0;
        quest q = null;
        for (int i = 0; i < dataManager.instance.quests.Count; i++)
        {
            if (nowStatus == questStatus.all|| dataManager.instance.quests[i][1] == (int)nowStatus)
            {
                if(num>=start)
                {
                    if (num-start < optionNum)
                    {
                        quests[num - start, 0] = dataManager.instance.quests[i][0];
                        quests[num - start, 1] = dataManager.instance.quests[i][1];
                        setQuest(num - start, quests[num - start, 0]);
                        if(q==null)
                            q = dataManager.instance.questlist.getQuestInfo(quests[num - start, 0]);
                    }
                    else
                    {
                        break;
                    }
                }
                num++;
            }
        }
        for (int i = optionNum; i < maxNum; i++)
        {
            removeQuest(i);
        }

        if (q!=null)
        {
            title1.text = findText(q.name, gameManager.instance.setting.nowlang);
            title2.text = findText(q.name, gameManager.instance.setting.nowlang);
            title1Icon.sprite = q.icon;
            hard.text = q.hard;
            text.text = findText(q.text, gameManager.instance.setting.nowlang);
            string txt = "";
            for(int i=0;i<q.steps.Count;i++)
            {
                txt+= findText(q.steps[i], gameManager.instance.setting.nowlang);
                txt += "\n";
            }
            steps.text = txt;
        }
        else
        {
            title1Icon.sprite = UIMask;
            title1.text = "";
            title2.text = "";
            hard.text = "";
            text.text = "";
            steps.text = "";
        }
    }

    public void setQuest(int index, int id)
    {
        quest i = dataManager.instance.questlist.getQuestInfo(id);
        questsImg[index].sprite = i.icon;
        Text txt = questsImg[index].gameObject.GetComponentInChildren<Text>();
        txt.text = findText(i.name, gameManager.instance.setting.nowlang);
    }

    public void removeQuest(int index)
    {
        questsImg[index].sprite = UIMask;
        Text txt = questsImg[index].gameObject.GetComponentInChildren<Text>();
        txt.text = "";
    }

    void moveUp()
    {
        if (cursor.activeInHierarchy)
        {
            if (optionNum > 0)
            {
                if (cursorPos == 0)
                {
                    int allpage = dataManager.instance.quests.Count / maxNum + 1;
                    if (allpage > 1)
                    {
                        nowpage = (nowpage - 1 + allpage) % allpage;

                    }
                    cursorPos = (cursorPos - 1 + optionNum) % optionNum;
                    cursor.transform.localPosition = p + cursorPos * nextMove;
                }
                else
                {
                    cursorPos = (cursorPos - 1 + optionNum) % optionNum;
                    cursor.transform.localPosition = cursor.transform.localPosition - nextMove;
                }
                drawPanel();
            }
        }
        else
        {
            tabPos = (tabPos - 1 + tabNum) % tabNum;
            tabCursor.transform.localPosition = p2 + tabPos * tabMove;
            nowStatus = (questStatus)tabPos;

            drawPanel();
        }

    }

    void moveDown()
    {
        if (cursor.activeInHierarchy)
        {
            if (optionNum > 0)
            {
                if (cursorPos == optionNum - 1)
                {
                    cursorPos = 0;
                    cursor.transform.localPosition = p + cursorPos * nextMove;
                    int allpage = dataManager.instance.quests.Count / maxNum + 1;
                    if (allpage > 1)
                    {
                        nowpage = (nowpage + 1) % allpage;
                        drawPanel();
                    }
                }
                else
                {
                    cursorPos = (cursorPos + 1) % optionNum;
                    cursor.transform.localPosition = cursor.transform.localPosition + nextMove;
                }
                drawPanel();
            }
        }
        else
        {
            tabPos = (tabPos + 1) % tabNum;
            tabCursor.transform.localPosition = p2 + tabPos * tabMove;
            nowStatus = (questStatus)tabPos;

            drawPanel();
        }
    }
}
