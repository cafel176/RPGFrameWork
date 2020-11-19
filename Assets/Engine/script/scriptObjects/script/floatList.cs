using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VarFloatDictionary
{
    public string key;
    public float value;
}

[CreateAssetMenu]
public class floatList : myList<VarFloatDictionary>
{
    floatList()
    {
        list = new List<VarFloatDictionary>();
    }
}