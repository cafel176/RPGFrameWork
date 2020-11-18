using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HashsAndTags : MonoBehaviour
{
    public textList[] texts;
    public ImageList[] imgs;

    //tags
    public string player = "Player";
    public string mainCamera = "MainCamera";
    public string sceneManager = "sceneManager";
    public string npc = "npc";

    //hashs
    public string doOnce = "do", doOnce2 = "do2", doOnce3 = "do3", back = "back", speed = "speed";
    public string Xspeed = "Xspeed", YspeedUp = "YspeedUp", YspeedDown = "YspeedDown",
        runX = "runX", runY = "runY", idle = "back", idleAnima = "idle", rushX = "rushX", rushY = "rushY";

    public Dictionary<string, string> chnStrings = new Dictionary<string, string>();
    public Dictionary<string, string> engStrings = new Dictionary<string, string>();

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
        try
        {
            if (lang == language.chinese)
                return chnStrings[key];
            else if (lang == language.english)
                return engStrings[key];
            else
                return chnStrings[key];
        }
        catch
        {
            Debug.Log("找不到文本，key:"+key);
        }
        return "can not find";
    }

    public void add(string key,string text, language lang)
    {
        if (lang == language.chinese)
            chnStrings.Add(key,text);
        else if (lang == language.english)
            engStrings.Add(key, text);
    }

    private void Awake()
    {
        for (int j = 0; j < texts.Length; j++)
            for (int i = 0; i < texts[j].list.Count; i++)
            {
                chnStrings.Add(texts[j].list[i].key, texts[j].list[i].chnText);
                engStrings.Add(texts[j].list[i].key, texts[j].list[i].engText);
            }
    }
}
