using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class stringDictionary
{
    public string key;
    public string chnText;
    public string engText;
}

[CreateAssetMenu]
public class textList : myList<stringDictionary>
{
}
