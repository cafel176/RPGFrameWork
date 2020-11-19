using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class audioDictionary
{
    public string key;
    public float pitch = 1.0f;
    public AudioClip audio;
}

[CreateAssetMenu]
public class AudioList : myList<audioDictionary>
{
    AudioList()
    {
        list = new List<audioDictionary>();
    }

    public AudioClip findAudio(string key)
    {
        for(int i=0;i<list.Count;i++)
        {
            if (list[i].key == key)
                return list[i].audio;
        }
        return null;
    }

    public float findPitch(string key)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].key == key)
                return list[i].pitch;
        }
        return -1;
    }
}
