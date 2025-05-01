using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManagerSpace
{
    public enum globalDataType
    {
        _int,
        _double,
        _switch
    }

    [System.Serializable]
    public class globalData
    {
        public globalDataType type;
        public string key;
        public string value;
    }

    [System.Serializable]
    public class startSetting
    {
        public Vector3 StartPos;
        public Vector3 StartRotate;
        public turn StartTurn;
        public string StartScene;
        public controlMode startMode;
        public List<int> startActor = new List<int>();
        public List<globalData> datas = new List<globalData>();
    }

    [System.Serializable]
    public class publicEventStruct
    {
        public Component com;
        public commonEventCallback callback;
        public publicEventInterface events;

        public publicEventStruct(publicEventInterface events, commonEventCallback callback = null,Component cm = null)
        {
            this.events = events;
            this.callback = callback;
            this.com = cm;
        }
    }

    [DisallowMultipleComponent]
    public class gameManager : MonoBehaviour
    {
        public static gameManager instance;

        private List<publicEventStruct> commonEvents = new List<publicEventStruct>();

        [SerializeField]
        private GameObject publicEventPrefab;

        private Dictionary<string, bool[]> independentSwitches = new Dictionary<string, bool[]>();
        public Dictionary<string, bool[]> IndependentSwitches
        {
            get
            {
                return independentSwitches;
            }
        }

        //队伍
        private List<int> team = new List<int>();
        public List<int> Team
        {
            get
            {
                return team;
            }
        }

        public systemSetting SystemSetting = new systemSetting();

        public int nowStartSetting = 0;
        public startSetting[] StartSettings = new startSetting[1];

        public gameSetting GameSetting = new gameSetting();

        [SerializeField]
        private settings setting = new settings();
        public settings Setting
        {
            get
            {
                return setting;
            }
        }

        private int oriSize;
        public int OriSize
        {
            get
            {
                return oriSize;
            }
        }

        private double playTime = 0.0;
        public double PlayTime
        {
            get
            {
                return playTime;
            }
            set
            {
                playTime = value;
            }
        }

        public event changeSettingCallbaks callbacks;


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
            Application.targetFrameRate = SystemSetting.FPS;
            loadSetting();
            oriSize = SystemSetting.startSize;
            Screen.SetResolution(SystemSetting.width * setting.windowSize, SystemSetting.height * setting.windowSize, false);
        }

        public void doEveryFrame()
        {
            playTime += Time.deltaTime;

            checkCommonEvent();
        }

        public void startPublicEvent(string name, nowState ns, commonEventCallback c = null, Component cm = null)
        {
            if(cm!=null)
            {
                for(int i=0;i<commonEvents.Count;i++)
                {
                    if (commonEvents[i].com == cm)
                        return;
                }
            }

            GameObject g = Instantiate(publicEventPrefab);
            var e = g.GetComponent<publicEventInterface>();
            e.startEvent(name,ns);
            commonEvents.Add(new publicEventStruct(e,c,cm));
        }

        // 循环检查公共事件
        private void checkCommonEvent()
        {
            for (int i = 0; i < commonEvents.Count; i++)
            {
                if (commonEvents[i].events.canNowListRemove())
                {
                    commonEvents[i].events.resetNowList();
                    commonEvents[i].events.exit();
                    if (commonEvents[i].callback != null)
                        commonEvents[i].callback();
                    commonEvents.RemoveAt(i);
                    i--;
                }
                else
                {
                    commonEvents[i].events.setCanDo(commonEvents[i].events.checkNowListEventConditions());
                }
            }
        }

        public void loadSetting()
        {
            //窗口尺寸
            if (PlayerPrefs.HasKey(settings.keys[0]))
                setting.alwaysRun = (PlayerPrefs.GetInt(settings.keys[0]) == 0 ? true : false);
            if (PlayerPrefs.HasKey(settings.keys[1]))
                setting.autoMessage = (PlayerPrefs.GetInt(settings.keys[1]) == 0 ? true : false);
            if (PlayerPrefs.HasKey(settings.keys[2]))
                setting.windowSize = PlayerPrefs.GetInt(settings.keys[2]);
            else
                setting.windowSize = SystemSetting.startSize;
            if (PlayerPrefs.HasKey(settings.keys[3]))
                setting.musicValue = PlayerPrefs.GetFloat(settings.keys[3]);
            if (PlayerPrefs.HasKey(settings.keys[4]))
                setting.SEValue = PlayerPrefs.GetFloat(settings.keys[4]);
            if (PlayerPrefs.HasKey(settings.keys[5]))
                setting.nowlang = (language)PlayerPrefs.GetInt(settings.keys[5]);

            if(callbacks!=null)
                callbacks(setting);
        }

        public void saveSetting()
        {
            PlayerPrefs.SetInt(settings.keys[0], setting.alwaysRun ? 0 : 1);
            PlayerPrefs.SetInt(settings.keys[1], setting.autoMessage ? 0 : 1);
            PlayerPrefs.SetInt(settings.keys[2], setting.windowSize);
            PlayerPrefs.SetFloat(settings.keys[3], setting.musicValue);
            PlayerPrefs.SetFloat(settings.keys[4], setting.SEValue);
            PlayerPrefs.SetInt(settings.keys[5], (int)setting.nowlang);

            if (callbacks != null)
                callbacks(setting);
        }

        public void changeLanguage(bool add)
        {
            int j = (int)setting.nowlang;
            int length = System.Enum.GetValues(typeof(language)).Length;
            if (add)
                setting.nowlang = (language)((j + 1) % length);
            else
                setting.nowlang = (language)((j - 1 + length) % length);
        }

        public void changeBool(string key)
        {
            if (key == settings.keys[0])
            {
                setting.alwaysRun = !setting.alwaysRun;
            }
            else if (key == settings.keys[1])
            {
                setting.autoMessage = !setting.autoMessage;
            }
        }

        //改变游戏窗口尺寸
        public void addWindowSize(bool add)
        {
            if (add)
                setting.windowSize = setting.windowSize % SystemSetting.maxSize + 1;
            else
                setting.windowSize = (setting.windowSize - 1) > 0 ? setting.windowSize - 1 : SystemSetting.maxSize;
            if (setting.windowSize == SystemSetting.maxSize)
                Screen.SetResolution(Screen.width, Screen.height, true);
            else
                Screen.SetResolution(SystemSetting.width * setting.windowSize, SystemSetting.height * setting.windowSize, false);
        }

        public void loadIndependentSwitchs(Dictionary<string, bool[]> a)
        {
            independentSwitches = a;
        }

        public void loadTeam(List<int> team)
        {
            this.team = team;
        }

        public bool[] getIndependentSwitchs(string key)
        {
            if (independentSwitches.ContainsKey(key))
            {
                return independentSwitches[key];
            }
            else
            {
                return new bool[] { false, false, false, false };
            }
        }

        public void setIndependentSwitchs(string key, int index, bool ind)
        {
            if (!independentSwitches.ContainsKey(key))
            {
                independentSwitches.Add(key, new bool[] { false, false, false, false });
            }
            independentSwitches[key][index] = ind;
        }

        //角色入队
        public void inTeamIfNotContain(int id)
        {
            if (!team.Contains(id))
                team.Add(id);
        }

        //角色离队
        public void outTeamIfContain(int id)
        {
            if (team.Contains(id))
                team.Remove(id);
        }
    }
}

