using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using UnityEditor;
using UnityEngine.SceneManagement;

/*
 * 注意：

使用LitJson解析时，解析类时
若包含Dictionary结构，则key的类型必须是string，而不能是int类型（如需表示id等），否则无法正确解析！
若需要小数，要使用double类型，而不能使用float，可后期在代码里再显式转换为float类型。
 */

//LitJs 处理float会出错，用double代替
public struct vector3d
{
    public double x;
    public double y;
    public double z;

    public vector3d(Vector3 v)
    {
        x = v.x;
        y = v.y;
        z = v.z;
    }

    public Vector3 toVec3()
    {
        return new Vector3((float)x, (float)y, (float)z);
    }
}

public class saveData
{
    public string version;
    public double playTime;

    //全局变量
    public Dictionary<string, bool> switchD = new Dictionary<string, bool>();
    public Dictionary<string, int> intD = new Dictionary<string, int>();
    public Dictionary<string, double> floatD = new Dictionary<string, double>();

    //道具
    public List<int[]> items = new List<int[]>();

    //队伍
    public List<int> team;
    public vector3d position;
    public int mapId;
    public vector3d turn;
}

public class dataManager : MonoBehaviour
{
    public itemListDefine itemlist;

    // 全局变量
    public switchList switchs;
    public intList ints;
    public floatList floats;

    public GameObject[] playerPrefabs;
    public Sprite[] faces;

    //=====================不可见变量===============================

    //管理器的实例
    public static dataManager instance;

    public Dictionary<string, bool> switchD = new Dictionary<string, bool>();
    public Dictionary<string, int> intD = new Dictionary<string, int>();
    public Dictionary<string, double> floatD = new Dictionary<string, double>();

    //记录各个道具位放了些什么，2位数组，0号位是编号，1号位是数量
    [HideInInspector]
    public List<int[]> items = new List<int[]>();
    //队伍
    [HideInInspector]
    public List<int> team = new List<int>();

    private void Awake()
    {
        //创造管理器实例
        if (dataManager.instance == null)
        {
            dataManager.instance = this;
            DontDestroyOnLoad(dataManager.instance.gameObject);
        }
        else if (dataManager.instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        for(int i=0;i<switchs.list.Count; i++)
        {
            switchD.Add(switchs.list[i].key, switchs.list[i].value);
        }
        for (int i = 0; i < ints.list.Count; i++)
        {
            intD.Add(ints.list[i].key, ints.list[i].value);
        }
        for (int i = 0; i < floats.list.Count; i++)
        {
            floatD.Add(floats.list[i].key, floats.list[i].value);
        }
    }

    public int getTotalItemNum(itemType type)
    {
        int num = 0;
        for(int i=0;i<items.Count;i++)
        {
            if(type==itemType.all||itemlist.getItemInfo(items[i][0]).type==type)
            {
                num++;
            }
        }
        return num;
    }

    // i是物品id
    public void getItem(int id,int num)
    {
        for(int j=0;j<items.Count;j++)
        {
            if(items[j][0]==id)
            {
                items[j][1] += num;
                return;
            }
        }
        items.Add(new int[2]{ id,num});
    }

    public bool useItem(int id)
    {
        switch (id)
        {
            case 0:
                {
                    return true;
                }
            default:
                {
                    //removeItem(index);消耗品需要移除
                }
                break;
        }
        return false;
    }

    public void removeItem(int id,int num)
    {
        for (int j = 0; j < items.Count; j++)
        {
            if (items[j][0] == id)
            {
                items[j][1] -= num;
                return;
            }
        }
    }

    public bool getSwitch(string key)
    {
        return switchD[key];
    }

    public void setSwitch(string key, bool _switch)
    {
        switchD[key] = _switch;
    }

    public int getInt(string key)
    {
        return intD[key];
    }

    public void setInt(string key, int _int)
    {
        intD[key] = _int;
    }

    public float getFloat(string key)
    {
        return (float)floatD[key];
    }

    public void setFloat(string key, float _float)
    {
        floatD[key] = _float;
    }

    public void saveData(string fileName)
    {
        saveData data = new saveData();
        data.version = Application.version;
        data.playTime = gameManager.instance.playTime;

        data.switchD = switchD;
        data.intD = intD;
        data.floatD = floatD;

        data.items = items;

        data.mapId = SceneManager.GetActiveScene().buildIndex;
        data.position = new vector3d(gameManager.instance.player.transform.position);
        data.team = team;
        data.turn = new vector3d(gameManager.instance.player.turn);

        string filePath = Application.dataPath + @"/Save/"+ fileName + ".json";
        //找到当前路径
        FileInfo file = new FileInfo(filePath);
        //判断有没有文件，有则打开文件，，没有创建后打开文件
        StreamWriter sw = file.CreateText();
        //ToJson接口将你的列表类传进去，，并自动转换为string类型
        string json = JsonMapper.ToJson(data);
        //将转换好的字符串存进文件，
        sw.WriteLine(json);
        //注意释放资源
        sw.Close();
        sw.Dispose();
        AssetDatabase.Refresh();
    }

    public saveData loadData(string fileName)
    {
        string txt = File.ReadAllText(Application.dataPath + @"/Save/" + fileName + ".json");
        saveData data = JsonMapper.ToObject<saveData>(txt);

        gameManager.instance.playTime += data.playTime;

        switchD = data.switchD;
        intD = data.intD;
        floatD = data.floatD;

        items = data.items;

        team = data.team;

        return data;
    }

    public void deleteFile(string fileName)
    {
        File.Delete(fileName);
    }

    //角色入队
    public void inTeam(int id)
    {
        if (!dataManager.instance.team.Contains(id))
            dataManager.instance.team.Add(id);
    }

    //角色离队
    public void outTeam(int id)
    {
        if (dataManager.instance.team.Contains(id))
            dataManager.instance.team.Remove(id);
    }
}
