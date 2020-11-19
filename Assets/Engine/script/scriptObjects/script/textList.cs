using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class stringDictionary
{
    public string key;
    public string cnText;
    public string enText;
    public string jpText;
}

[CreateAssetMenu]
public class textList : myList<stringDictionary>
{
    textList()
    {
        list = new List<stringDictionary>();
    }
}
