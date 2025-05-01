using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System.Text.RegularExpressions;

namespace ManagerSpace
{
    [System.Serializable]
    public class CommonEventList
    {
        public string Key;
        public eventStruct list;
    }

    [DisallowMultipleComponent]
    public class dataManager : MonoBehaviour
    {
        // 自定义数据表，键值可重复，按照名字包含key的来搜索
        [SerializeField]
        private CommonEventList[] commonEventLists;
        [SerializeField]
        private dataList[] listsForDataDictionary;
        [SerializeField]
        private dataList[] listsForGameDictionary;
        [SerializeField]
        private dataList[] listsForOther;

        //=====================不可见变量===============================

        //管理器的实例
        public static dataManager instance;

        private Dictionary<string,Dictionary<string, object>> datadictionary = new Dictionary<string, Dictionary<string, object>>();
        private Dictionary<string, Dictionary<string, object>> gamedictionary = new Dictionary<string, Dictionary<string, object>>();

        private void Awake()
        {
            //创造管理器实例
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(instance.gameObject);
            }
            else if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }

        public void init()
        {
            clear();

            initCommonEvenList();

            initDataDic();
            initGameDic();
            initOtherList();
        }

        private void initCommonEvenList()
        {
            for (int i = 0; i < commonEventLists.Length; i++)
            {
                commonEventLists[i].list.eventList.init();
            }
        }

        private void initDataDic()
        {
            for(int n=0;n< listsForDataDictionary.Length;n++)
            {
                listsForDataDictionary[n].init();
                List<ListNodeInterface> lists = listsForDataDictionary[n].getList();
                for (int i = 0; i < lists.Count; i++)
                {
                    string Key = lists[i].getId();
                    dataList value = lists[i].getData<dataList>(propertyName.value);
                    value.init();

                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    var list = value.getList();
                    for (int j = 0; j < list.Count; j++)
                    {
                        dic.Add(list[j].getId(), list[j].getData(propertyName.value));
                    }

                    if (datadictionary.ContainsKey(Key))
                    {
                        var keys = new List<string>(dic.Keys);
                        var values = new List<object>(dic.Values);
                        for (int k = 0; k < keys.Count; k++)
                            datadictionary[Key].Add(keys[k], values[k]);
                    }
                    else
                        datadictionary.Add(Key, dic);
                }
            }
        }

        private void initGameDic()
        {
            for (int n = 0; n < listsForGameDictionary.Length; n++)
            {
                listsForGameDictionary[n].init();
                List<ListNodeInterface> lists = listsForGameDictionary[n].getList();
                for (int i = 0; i < lists.Count; i++)
                {
                    string Key = lists[i].getId();
                    dataList value = lists[i].getData<dataList>(propertyName.value);
                    value.init();
                    if (!gamedictionary.ContainsKey(Key))
                    {
                        gamedictionary.Add(Key, new Dictionary<string, object>());
                    }
                }
            }
        }

        private void initOtherList()
        {
            for (int n = 0; n < listsForOther.Length; n++)
            {
                listsForOther[n].init();
                List<ListNodeInterface> lists = listsForOther[n].getList();
                for (int i = 0; i < lists.Count; i++)
                {
                    dataList value = lists[i].getData<dataList>(propertyName.value);
                    value.init();
                }
            }
        }

        public T getValueDataDictionary<T>(string listkey, string key)
        {
            if (datadictionary.ContainsKey(listkey))
                if (datadictionary[listkey].ContainsKey(key))
                {
                    return (T)datadictionary[listkey][key];
                }                
            return default;
        }

        public void setValueDataDictionary(string listkey, string key, object value)
        {
            if (datadictionary.ContainsKey(listkey))
                if (datadictionary[listkey].ContainsKey(key))
                    datadictionary[listkey][key] = value;
        }

        public Dictionary<string, Dictionary<string, object>> getDataDictionary()
        {
            return datadictionary;
        }

        public void loadDataDictionary(string name, dataDictionary data)
        {
            datadictionary.Add(name, data.data);
        }

        public int getInt(string key)
        {
            return getValueDataDictionary<int>(dictionaryName.ints, key);
        }

        public double getDouble(string key)
        {
            return getValueDataDictionary<double>(dictionaryName.doubles, key);
        }

        public bool getSwitch(string key)
        {
            return getValueDataDictionary<bool>(dictionaryName.switchs, key);
        }

        public Vector3 getVec3(string key)
        {
            return getValueDataDictionary<Vector3>(dictionaryName.vec3s, key);
        }

        public void setInt(string key, int v)
        {
            setValueDataDictionary(dictionaryName.ints, key, v);
        }

        public void setDouble(string key, double v)
        {
            setValueDataDictionary(dictionaryName.doubles, key, v);
        }

        public void setSwitch(string key, bool v)
        {
            setValueDataDictionary(dictionaryName.switchs, key, v);
        }

        public void setVec3(string key, Vector3 v)
        {
            setValueDataDictionary(dictionaryName.vec3s, key, v);
        }

        public T getValueGameDictionary<T>(string listkey, string key)
        {
            if (gamedictionary.ContainsKey(listkey))
                if (gamedictionary[listkey].ContainsKey(key))
                    return (T)gamedictionary[listkey][key];
            return default;
        }

        public void setValueGameDictionary(string listkey, string key, object value)
        {
            if (gamedictionary.ContainsKey(listkey))
            {
                if (gamedictionary[listkey].ContainsKey(key))
                    gamedictionary[listkey][key] = value;
                else
                    gamedictionary[listkey].Add(key, value);
            }
        }

        public bool removeValueGameDictionary(string listkey, string key)
        {
            if (gamedictionary.ContainsKey(listkey))
            {
                if (gamedictionary[listkey].ContainsKey(key))
                    return gamedictionary[listkey].Remove(key);
            }
            return true;
        }

        public Dictionary<string, T> getListInGameDictionary<T>(string name)
        {
            Dictionary<string, T> dic = new Dictionary<string, T>();
            var d = gamedictionary[name];
            List<string> keys = new List<string>(d.Keys);
            List<object> values = new List<object>(d.Values);
            for(int i=0;i<keys.Count;i++)
            {
                dic.Add(keys[i], (T)values[i]);
            }
            return dic;
        }

        public Dictionary<string, Dictionary<string, object>> getGameDictionary()
        {
            return gamedictionary;
        }

        public void loadGameDictionary(string name, gameDictionary data)
        {
            gamedictionary.Add(name, data.data);
        }

        // i是物品id
        public void addItem(string id, int n)
        {
            int num = getValueGameDictionary<int>(dictionaryName.items, id);
            num += n;
            setValueGameDictionary(dictionaryName.items, id, num);
        }

        public void removeItem(string id, int n)
        {
            int num = getValueGameDictionary<int>(dictionaryName.items, id);
            num -= n;
            if(num<=0)
                removeValueGameDictionary(dictionaryName.items, id);
            else
                setValueGameDictionary(dictionaryName.items, id, num);
        }

        // i是id
        public void addQuest(string id)
        {
            setValueGameDictionary(dictionaryName.quests, id, (int)questStatus.doing);
        }

        public void changeQuest(string id, questStatus type)
        {
            setValueGameDictionary(dictionaryName.quests, id, (int)type);
        }

        public void clear()
        {
            datadictionary.Clear();
            gamedictionary.Clear();
        }



        public int getTotalNumInList(string key, int type)
        {
            int num = 0;

            var all = new List<dataList[]> { listsForOther, listsForGameDictionary };

            for(int k=0;k<all.Count;k++)
                for (int n = 0; n < all[k].Length; n++)
                {
                    List<ListNodeInterface> lists = all[k][n].getList();
                    for (int i = 0; i < lists.Count; i++)
                    {
                        string Key = lists[i].getId();
                        if (Key == key)
                        {
                            var list = lists[i].getData<dataList>(propertyName.value).getList();

                            for (int j = 0; j < list.Count; j++)
                            {
                                if (type == (int)SpecialValue.all || (int)list[i].getData(propertyName.type) == type)
                                {
                                    num++;
                                }
                            }
                        }
                    }
                }


            return num;

        }

        public List<dataList> findList(string key)
        {
            List<dataList> l = new List<dataList>();
            var all = new List<dataList[]> { listsForOther, listsForGameDictionary };

            for (int k = 0; k < all.Count; k++)
                for (int n = 0; n < all[k].Length; n++)
                {
                    List<ListNodeInterface> lists = all[k][n].getList();
                    for (int i = 0; i < lists.Count; i++)
                    {
                        string Key = lists[i].getId();
                        if (Key == key)
                        {
                            var list = lists[i].getData<dataList>(propertyName.value);
                            l.Add(list);
                        }
                    }
                }

            return l;
        }

        public ListNodeInterface findObjectInList(string list, string id)
        {
            var t = findList(list);
            if (t.Count > 0)
            {
                for(int i=0;i<t.Count;i++)
                {
                    var n = t[i].getListNode(id);
                    if (n != null)
                        return n;
                }
            }

            return null;
        }

        public T findValueInList<T>(string list,string id,string key)
        {
            ListNodeInterface node = findObjectInList(list, id);
            if (node != null)
            {
                T o = (T)node.getData(key);
                if (o != null)
                    return o;
            }

            return default;
        }

        public ListNodeInterface findItemInList(string id)
        {
            return findObjectInList(dictionaryName.items, id);
        }

        public ListNodeInterface findQuestInList(string id)
        {
            return findObjectInList(dictionaryName.quests, id);
        }

        public GameObject findPrefabInList(string id)
        {
            return findValueInList<GameObject>(dictionaryName.prefab, id, propertyName.value);
        }

        public AudioClip findAudioInList(string id)
        {
            return findValueInList<AudioClip>(dictionaryName.audio, id, propertyName.value);
        }

        public VideoClip findVideoInList(string id)
        {
            return findValueInList<VideoClip>(dictionaryName.video, id, propertyName.value);
        }

        public float findPitchInList(string id)
        {
            return findValueInList<float>(dictionaryName.audio, id, propertyName.pitch);
        }

        public Sprite findImgInList(string id)
        {
            return findValueInList<Sprite>(dictionaryName.image, id, propertyName.value);
        }

        public string findTextInList(string id, language lang)
        {
            string txt = id;
            string list = Enum.GetName(typeof(language), lang);
            txt = findValueInList<string>(list, id, propertyName.value);
            if(txt==default)
                return id;
            txt = changeText(txt);
            return txt;
        }

        public CommonEventList[] getCommonEventList()
        {
            return commonEventLists;
        }

        public eventStruct getCommonEvent(string list)
        {
            foreach (var t in commonEventLists)
                if (t.Key == list)
                {
                    return t.list;
                }

            return null;
        }

        private string changeText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            string txt = text;
            Regex regex = new Regex(symbols.search);
            var g = regex.Matches(txt);
            for (int j = 0; j < g.Count; j++)
            {
                string content = g[j].Value;
                if (string.IsNullOrEmpty(content) || !content.Contains("(") || !content.Contains("#"))
                    continue;

                string type = content.Substring(0, content.IndexOf('('));
                if (string.IsNullOrEmpty(type))
                    continue;

                if (type == symbols.ints)
                {
                    string id = content.Substring(3, content.Length - 4);
                    txt = txt.Replace(content, getInt(id).ToString());
                }
                else if (type == symbols.doubles)
                {
                    string id = content.Substring(3, content.Length - 4);
                    txt = txt.Replace(content, getDouble(id).ToString());
                }
                else if (type == symbols.switchs)
                {
                    string id = content.Substring(3, content.Length - 4);
                    txt = txt.Replace(content, getSwitch(id).ToString());
                }
                else if (type == symbols.vec3s)
                {
                    string id = content.Substring(4, content.Length - 5);
                    var v = getVec3(id);
                    txt = txt.Replace(content, "("+ v.x +"," + v.y + "," + v.z +")");
                }
            }
            return txt;
        }
    }

}
