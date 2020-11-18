using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class imageDictionary
{
    public string key;
    public Sprite img;
}

[CreateAssetMenu]
public class ImageList : myList<imageDictionary>
{
    public Sprite findImg(string key)
    {
        for(int i=0;i<list.Count;i++)
        {
            if (list[i].key == key)
                return list[i].img;
        }
        return null;
    }
}
