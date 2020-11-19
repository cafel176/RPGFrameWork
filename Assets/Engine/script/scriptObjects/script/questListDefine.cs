using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum questStatus
{
    doing,
    finished,
    failed,
    all
}

public enum questType
{
    main,
    side,
    hidden
}

[System.Serializable]
public class quest
{
    public int id;
    public questStatus status = questStatus.doing;
    public questType type = questType.main;
    //设置名字，难度和文本
    public string name,hard,text;
    //图标
    public Sprite icon = null;
    // 步骤
    public List<string> steps;
    public int stepNum;
}

[CreateAssetMenu]
public class questListDefine : myList<quest>
{
    questListDefine()
    {
        list = new List<quest>();
    }

    public quest getQuestInfo(int id)
    {
        for(int i=0;i<list.Count;i++)
        {
            if (list[i].id == id)
                return list[i];
        }
        return null;
    }
}
