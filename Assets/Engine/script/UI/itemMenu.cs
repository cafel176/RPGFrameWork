using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class itemMenu : keyBoardMenu
{
    [HideInInspector]
    public GameObject menuPanel;
    [HideInInspector]
    public int[,] items;

    public Image[] itemsImg;

    public Sprite UIMask;
    public Vector3 lineMove;
    public GameObject tabCursor;
    public Vector3 tabMove;
    public int everyLineNum = 2;

    private int tabPos = 0,tabNum = 4;
    private Vector3 p,p2;
    private int maxNum;
    private int nowpage = 0;
    private itemType nowType;

    override protected void init()
    {
        nowType = itemType.item;
        cursor.SetActive(true);
        p = cursor.transform.localPosition;
        p2 = tabCursor.transform.localPosition;
        maxNum = itemsImg.Length;
        doFuncs = new Func[maxNum];
        items = new int[maxNum, 2];
        for (int i = 0; i < maxNum; i++)
        {
            itemsImg[i].gameObject.SetActive(true);
            doFuncs[i] = new Func(useItem);
        }

        drawPanel();
        texts[0].text.text = gameManager.instance.hat.find(dataManager.instance.itemlist.getItemInfo(items[cursorPos, 0]).text, gameManager.instance.setting.nowlang);
        
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
        nowType = (itemType)tabPos;

        drawPanel();
        if (optionNum > 0)
            texts[0].text.text = gameManager.instance.hat.find(dataManager.instance.itemlist.getItemInfo(items[cursorPos, 0]).text, gameManager.instance.setting.nowlang);
        else
            texts[0].text.text = "";
    }

    public void moveRight()
    {
        tabPos = (tabPos + 1) % tabNum;
        tabCursor.transform.localPosition = p2 + tabPos * tabMove;
        nowType = (itemType)tabPos;

        drawPanel();
        if (optionNum > 0)
            texts[0].text.text = gameManager.instance.hat.find(dataManager.instance.itemlist.getItemInfo(items[cursorPos, 0]).text, gameManager.instance.setting.nowlang);
        else
            texts[0].text.text = "";
    }

    private void exit()
    {
        if (menuPanel != null)
            gameManager.instance.changeState(nowState.window, menuPanel);
        else
            gameManager.instance.changeState(nowState.move);
        Destroy(gameObject);
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
        int totalNum = dataManager.instance.getTotalItemNum(nowType);
        checkOptionNum(totalNum);
        int start = maxNum * nowpage;
        if(cursorPos>=optionNum)
        {
            cursorPos = 0;
            cursor.transform.localPosition = p + (cursorPos / everyLineNum) * lineMove;
        }

        int num = 0;
        for (int i = 0; i < dataManager.instance.items.Count; i++)
        {
            if (dataManager.instance.itemlist.getItemInfo(dataManager.instance.items[i][0]).type == nowType)
            {
                if(num>=start)
                {
                    if (num-start < optionNum)
                    {
                        items[num - start, 0] = dataManager.instance.items[i][0];
                        items[num - start, 1] = dataManager.instance.items[i][1];
                        setItem(num - start, items[num - start, 0], items[num - start, 1]);

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
                removeItem(i);
        }
    }

    public void setItem(int index, int id, int num)
    {
        itemsImg[index].sprite = dataManager.instance.itemlist.getItemInfo(id).img;
        Text[] txt = itemsImg[index].gameObject.GetComponentsInChildren<Text>();
        txt[0].text = gameManager.instance.hat.find(dataManager.instance.itemlist.getItemInfo(id)._name, gameManager.instance.setting.nowlang);
        txt[1].text = "x" + num;
    }

    public void removeItem(int index)
    {
        itemsImg[index].sprite = UIMask;
        Text[] txt = itemsImg[index].gameObject.GetComponentsInChildren<Text>();
        txt[0].text = "";
        txt[1].text = "";
    }

    void moveUp()
    {
        if (optionNum > 0)
        {
            if (cursorPos == 0)
            {
                int allpage = dataManager.instance.items.Count / maxNum + 1;
                if (allpage > 1)
                {
                    nowpage = (nowpage - 1 + allpage) % allpage;
                    drawPanel();
                }
                cursorPos = (cursorPos - 1 + optionNum) % optionNum;
                if (cursorPos == optionNum - 1)
                {
                    cursor.transform.localPosition = p + (cursorPos / everyLineNum) * lineMove + (optionNum - 1) % everyLineNum * nextMove;
                }
                else
                    cursor.transform.localPosition = p + (cursorPos / everyLineNum) * lineMove + (everyLineNum - 1) * nextMove;
            }
            else if (cursorPos % everyLineNum == 0)
            {
                cursorPos = (cursorPos - 1 + optionNum) % optionNum;
                cursor.transform.localPosition = p + (cursorPos / everyLineNum) * lineMove + (everyLineNum - 1) * nextMove;
            }
            else
            {
                cursorPos = (cursorPos - 1 + optionNum) % optionNum;
                cursor.transform.localPosition = cursor.transform.localPosition - nextMove;
            }
            texts[0].text.text = gameManager.instance.hat.find(dataManager.instance.itemlist.getItemInfo(items[cursorPos, 0]).text, gameManager.instance.setting.nowlang);
        }
    }

    void moveDown()
    {
        if (optionNum > 0)
        {
            if(cursorPos == optionNum - 1)
            {
                cursorPos = 0;
                cursor.transform.localPosition = p + (cursorPos / everyLineNum) * lineMove;
                int allpage = dataManager.instance.items.Count / maxNum+1;
                if(allpage>1)
                {
                    nowpage = (nowpage + 1) % allpage;
                    drawPanel();
                }
            }
            else if ((cursorPos + 1) % everyLineNum == 0)
            {
                cursorPos = (cursorPos + 1) % optionNum;
                cursor.transform.localPosition = p + (cursorPos / everyLineNum) * lineMove;
            }
            else
            {
                cursorPos = (cursorPos + 1) % optionNum;
                cursor.transform.localPosition = cursor.transform.localPosition + nextMove;
            }
            texts[0].text.text = gameManager.instance.hat.find(dataManager.instance.itemlist.getItemInfo(items[cursorPos, 0]).text, gameManager.instance.setting.nowlang);
        }
    }

    void useItem()
    {
        int id = items[cursorPos,0];
        bool success = dataManager.instance.useItem(id);
        if (success)
        {

        }

        int allpage = dataManager.instance.items.Count / maxNum + 1;
        if (nowpage >= allpage && nowpage>0)
            nowpage--;
        drawPanel();
        texts[0].text.text = gameManager.instance.hat.find(dataManager.instance.itemlist.getItemInfo(items[cursorPos, 0]).text, gameManager.instance.setting.nowlang);
    }
}
