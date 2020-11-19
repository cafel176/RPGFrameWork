using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HashsAndTags : MonoBehaviour
{
    public textList[] texts;
    public ImageList[] imgs;
    public AudioList[] audios;
    public PrefabList[] Prefabs;

    //tags
    public string player = "Player";
    public string mainCamera = "MainCamera";
    public string sceneManager = "sceneManager";
    public string npc = "npc";

    //hashs
    public string doOnce = "do", doOnce2 = "do2", doOnce3 = "do3", back = "back", speed = "speed";
    public string Xspeed = "Xspeed", YspeedUp = "YspeedUp", YspeedDown = "YspeedDown",
        runX = "runX", runY = "runY", idle = "back", idleAnima = "idle", rushX = "rushX", rushY = "rushY";
    public string start = "Start", end = "End";

    public Dictionary<string, string> cnStrings = new Dictionary<string, string>();
    public Dictionary<string, string> enStrings = new Dictionary<string, string>();
    public Dictionary<string, string> jpStrings = new Dictionary<string, string>();

    public GameObject findPrefab(string key)
    {
        try
        {
            for (int j = 0; j < Prefabs.Length; j++)
            {
                GameObject s = Prefabs[j].findPrefab(key);
                if (s != null)
                    return s;
            }
        }
        catch
        {
            Debug.Log("找不到Prefab，key:" + key);
        }
        return null;
    }

    public AudioClip findAudio(string key)
    {
        try
        {
            for (int j = 0; j < audios.Length; j++)
            {
                AudioClip s = audios[j].findAudio(key);
                if (s != null)
                    return s;
            }
        }
        catch
        {
            Debug.Log("找不到音效，key:" + key);
        }
        return null;
    }

    public float findPitch(string key)
    {
        try
        {
            for (int j = 0; j < audios.Length; j++)
            {
                float s = audios[j].findPitch(key);
                if (s > 0)
                    return s;
            }
        }
        catch
        {
            Debug.Log("找不到音效，key:" + key);
        }
        return 1.0f;
    }

    public Sprite findImg(string key)
    {
        try
        {
            for (int j = 0; j < imgs.Length; j++)
            {
                Sprite s = imgs[j].findImg(key);
                if (s!=null)
                    return s;
            }
        }
        catch
        {
            Debug.Log("找不到图片，key:" + key);
        }
        return null;
    }

    public string find(string key,language lang)
    {
        string txt = "can not find";
        try
        {
            if (lang == language.chinese)
                txt = cnStrings[key];
            else if (lang == language.english)
                txt = enStrings[key];
            else if (lang == language.japanese)
                txt = jpStrings[key];
            else
                txt = cnStrings[key];
        }
        catch
        {
            //Debug.Log("找不到文本，key:"+key);
        }
        return txt;
    }

    public void add(string key,string text, language lang)
    {
        if (lang == language.chinese)
            cnStrings.Add(key,text);
        else if (lang == language.english)
            enStrings.Add(key, text);
        else if (lang == language.japanese)
            jpStrings.Add(key, text);
    }

    private void Awake()
    {
        for (int j = 0; j < texts.Length; j++)
            for (int i = 0; i < texts[j].list.Count; i++)
            {
                cnStrings.Add(texts[j].list[i].key, texts[j].list[i].cnText);
                enStrings.Add(texts[j].list[i].key, texts[j].list[i].enText);
                jpStrings.Add(texts[j].list[i].key, texts[j].list[i].jpText);
            }
    }
}
