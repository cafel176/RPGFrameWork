using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PrefabDictionary
{
    public string key;
    public GameObject Prefab;
}

[CreateAssetMenu]
public class PrefabList : myList<PrefabDictionary>
{
    PrefabList()
    {
        list = new List<PrefabDictionary>();
    }

    public GameObject findPrefab(string key)
    {
        for(int i=0;i<list.Count;i++)
        {
            if (list[i].key == key)
                return list[i].Prefab;
        }
        return null;
    }
}
