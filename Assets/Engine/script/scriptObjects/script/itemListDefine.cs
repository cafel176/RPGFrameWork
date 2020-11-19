using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum itemType
{
    all = -1,
    item,
    notes,
    food,
    gifts
}

[System.Serializable]
public class item
{
    public int id;
    public itemType type;
    //设置名字和文本
    public string _name, text;
    public Sprite img = null;
    // 使用次数，-2表示无限，-1表示不可使用
    public int times;
    public List<string> commonEvent;
    public int eventNum = 0;
}

[CreateAssetMenu]
public class itemListDefine : myList<item>
{
    itemListDefine()
    {
        list = new List<item>();
    }

    public item getItemInfo(int id)
    {
        for(int i=0;i<list.Count;i++)
        {
            if (list[i].id == id)
                return list[i];
        }
        return null;
    }
}
