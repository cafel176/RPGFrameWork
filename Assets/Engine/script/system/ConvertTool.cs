using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System.Text;

//RMMV转换工具
public class ConvertTool : MonoBehaviour
{
    private void Start()
    {
        //SpawnList("System");
        //SpawnTextsList(new string[]{"cn", "en", "jp"});
        SpawnItemsList();
    }

    public void SpawnItemsList()
    {
        string filePath = Application.dataPath + @"/Save/Items.json";
        if (!File.Exists(filePath))
            return;

        string txt = File.ReadAllText(filePath);
        JsonData data = JsonMapper.ToObject(txt);

        string List = SpawnItemList(data);

        filePath = Application.dataPath + @"/Save/itemList.txt";
        FileInfo file = new FileInfo(filePath);
        StreamWriter sw = file.CreateText();
        sw.WriteLine(List);
        sw.Close();
        sw.Dispose();
        Debug.Log("finish3");
    }

    public string SpawnItemList(JsonData data)
    {
        string List = "";
        foreach (JsonData v in data)
        {
            if (v == null)
                continue;

            List += "  - id: " + v["id"] + " ";
            List += "\n    type: " + ((int)v["itypeId"]-1) + " ";
            string n = v["name"].ToString();
            if (n != string.Empty)
                n = n.Substring(2, n.Length - 3);
            List += "\n    _name: \"" + n + "\" ";
            n = v["description"].ToString();
            if (n != string.Empty)
                n = n.Substring(2, n.Length - 3);
            List += "\n    text: \"" + n + "\" ";
            List += "\n    img: {fileID: 21300034, guid: be9ed3894f119b344b6c69fe5a6745ee, type: 3}";
            int times = 1;//-2表示无限，-1表示不可使用
            if ((int)v["occasion"] == 3)
                times = -1;
            else if(!(bool)v["consumable"])
                times = -2;
            List += "\n    times: " + times + " ";
            List += "\n    commonEvent:";
            List += "\n    eventNum: 0 ";
            List += "\n";
        }
        return List;
    }

    public void SpawnList(string fileName)
    {
        string filePath = Application.dataPath + @"/Save/" + fileName + ".json";
        if (!File.Exists(filePath))
            return;

        string txt = File.ReadAllText(filePath);
        JsonData data = JsonMapper.ToObject(txt);

        string intList = SpawnIntList(data);
        string switchList = SpawnSwitchList(data);

        filePath = Application.dataPath + @"/Save/intList.txt";
        FileInfo file = new FileInfo(filePath);
        StreamWriter sw = file.CreateText();
        sw.WriteLine(intList);
        sw.Close();
        sw.Dispose();

        filePath = Application.dataPath + @"/Save/switchList.txt";
        file = new FileInfo(filePath);
        sw = file.CreateText();
        sw.WriteLine(switchList);
        sw.Close();
        sw.Dispose();
        Debug.Log("finish1");
    }

    public string SpawnIntList(JsonData data)
    {
        JsonData variables = data["variables"];
        string List = "";
        foreach (JsonData v in variables)
        {
            string key = v.ToString();
            if (key != string.Empty)
            {
                List += "  - key: \"" + HexCharToString(key) + "\" \n    value: 0 \n";
            }
        }
        return List;
    }

    public string SpawnSwitchList(JsonData data)
    {
        JsonData variables = data["switches"];
        string List = "";
        foreach (JsonData v in variables)
        {
            string key = v.ToString();
            if (key != string.Empty)
            {
                List += "  - key: \"" + HexCharToString(key) + "\" \n    value: 0 \n";
            }
        }
        return List;
    }

    // 汉字转utf8编码
    public string HexCharToString(string c)
    {
        c = c.Replace("\\", "\\\\");
        c = c.Replace("\n", "\\n");
        c = c.Replace("\'", "\\\'");
        c = c.Replace("\"", "\\\"");
        string t = string.Empty;
        for (int i=0;i<c.Length;i++)
        {
            if(c[i]>=0x4e00 && c[i] <= 0x9fbb)//是汉字
            {
                t += "\\u" + System.Convert.ToString((int)c[i], 16).ToUpper();
            }
            else if (c[i] >= 0x3040 && c[i] <= 0x309f)//是平假名
            {
                t += "\\u" + System.Convert.ToString((int)c[i], 16).ToUpper();
            }
            else if (c[i] >= 0x30a0 && c[i] <= 0x30ff)//是片假名
            {
                t += "\\u" + System.Convert.ToString((int)c[i], 16).ToUpper();
            }
            else
            {
                t += c[i];
            }
        }        
        return t;
    }

    public void SpawnTextsList(string[] fileList)
    {
        string filePath;
        List<string> keys = new List<string>();
        List<List<string>> langs = new List<List<string>>();
        for (int i=0;i<fileList.Length;i++)
        {
            filePath = Application.dataPath + @"/Save/"+fileList[i]+".json";
            if (!File.Exists(filePath))
                return;

            string txt = File.ReadAllText(filePath);
            langs.Add(new List<string>());

            Dictionary<string,JsonData> data = JsonMapper.ToObject<Dictionary<string, JsonData>>(txt);
            foreach (var v in data)
            {
                copyContent(v.Value, langs[i]);
                if(i==0)
                    copyKey(v.Key, v.Value, keys);
            }
        }

        string List = string.Empty;
        for (int i=0;i<langs[0].Count;i++)
        {
            List += "  - key: \"" + keys[i] + "\" ";
            for (int j=0;j< fileList.Length; j++)
            {
                List += "\n    " + fileList[j] + "Text: \"" + HexCharToString(langs[j][i]) + "\" ";
            }
            List += "\n";
        }

        filePath = Application.dataPath + @"/Save/TextList.txt";
        FileInfo file = new FileInfo(filePath);
        StreamWriter sw = file.CreateText();
        sw.WriteLine(List);
        sw.Close();
        sw.Dispose();
        Debug.Log("finish2");
    }

    private void copyContent(JsonData data, List<string> list)
    {
        Dictionary<string, string> d = JsonMapper.ToObject<Dictionary<string, string>>(data.ToJson());
        foreach (var v in d)
        {
            list.Add(v.Value);
        }
    }

    private void copyKey(string key, JsonData data, List<string> list)
    {
        Dictionary<string, string> d = JsonMapper.ToObject<Dictionary<string, string>>(data.ToJson());
        foreach (var v in d)
        {
            list.Add(key+"."+v.Key);
        }
    }
}
