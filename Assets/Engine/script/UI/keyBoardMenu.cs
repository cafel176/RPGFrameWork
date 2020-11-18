using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct TextDictionary
{
    public string key;
    public Text text;
}

public class keyBoardMenu : basePanel
{
    public TextDictionary[] texts;

    //一个用键盘控制选择的类，其子类使用时要先给父类成员赋值
    protected delegate void Func();

    //每个选项对应的函数，需要在使用前赋值
    protected Func[] doFuncs;
    //其他函数，包含0取消键，1左2右3上4下键
    protected Func[] otherFuncs = new Func[5];
    //光标
    public GameObject cursor;
    //每次移动的方向
    public Vector3 nextMove;

    public AudioClip z;
    public AudioClip move;
    //这个菜单选项总数
    protected int optionNum = 2;
    //记录当前光标位置
    protected int cursorPos = 0;

    override protected void ableInit()
    {
        for (int i = 0; i < texts.Length; i++)
        {
            if (texts[i].key!=string.Empty)
            {
                texts[i].text.text = gameManager.instance.hat.find(texts[i].key, gameManager.instance.setting.nowlang);
            }
        }
    }

    public void changeCursorPos(bool next)
    {
        if (optionNum > 0)
        {
            if (next)
            {
                cursorPos = (cursorPos + 1) % optionNum;
                if (cursorPos == 0)
                    cursor.transform.localPosition = cursor.transform.localPosition - (optionNum - 1) * nextMove;
                else
                    cursor.transform.localPosition = cursor.transform.localPosition + nextMove;
            }
            else
            {
                cursorPos = (cursorPos - 1 + optionNum) % optionNum;
                if (cursorPos == optionNum - 1)
                    cursor.transform.localPosition = cursor.transform.localPosition + (optionNum - 1) * nextMove;
                else
                    cursor.transform.localPosition = cursor.transform.localPosition - nextMove;
            }
        }
    }

    public void doOption()
    {
        if (cursorPos < doFuncs.Length && cursorPos >= 0)
            if (doFuncs[cursorPos] != null)
            {
                playSE(z);
                doFuncs[cursorPos]();
            }
    }

    public void cancel()
    { 
        if(otherFuncs[0]!=null)
            otherFuncs[0]();
    }

    public void left()
    {
        if (otherFuncs[1] != null)
        {
            playSE(move);
            otherFuncs[1]();
        }
    }

    public void right()
    {
        if (otherFuncs[2] != null)
        {
            playSE(move);
            otherFuncs[2]();
        }
    }

    public void up()
    {
        if (otherFuncs[3] != null)
        {
            playSE(move);
            otherFuncs[3]();
        }
    }

    public void down()
    {
        if (otherFuncs[4] != null)
        {
            playSE(move);
            otherFuncs[4]();
        }
    }

}
