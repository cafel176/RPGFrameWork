using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VarIntDictionary
{
    public string key;
    public int value;
}

[CreateAssetMenu]
public class intList : myList<VarIntDictionary>
{
    intList()
    {
        list = new List<VarIntDictionary>();
    }
}