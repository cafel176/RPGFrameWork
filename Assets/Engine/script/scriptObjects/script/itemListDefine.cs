using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum itemType
{
    all = -1,
    item,
    weapon,
    clothes,
    keyItem
}

[System.Serializable]
public class item
{
    public int id;
    public itemType type;
    //设置名字和文本
    public string _name, text;
    public Sprite img = null;
    // 使用次数，-1表示无限
    public int times;
}

[CreateAssetMenu]
public class itemListDefine : myList<item>
{
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
