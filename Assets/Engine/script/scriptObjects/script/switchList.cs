using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class switchDictionary
{
    public string key;
    public bool value;
}

[CreateAssetMenu]
public class switchList : myList<switchDictionary>
{
    switchList()
    {
        list = new List<switchDictionary>();
    }
}
