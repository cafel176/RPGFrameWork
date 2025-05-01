using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System;

/*
* 注意：

使用LitJson解析时，解析类时
若包含Dictionary结构，则key的类型必须是string，而不能是int类型（如需表示id等），否则无法正确解析！
若需要小数，要使用double类型，而不能使用float，可后期在代码里再显式转换为float类型。
*/

//LitJs 处理float会出错，用double代替

namespace ManagerSpace
{
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

        public vector3d(float x, float y, float z = 0)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3 toVec3()
        {
            return new Vector3((float)x, (float)y, (float)z);
        }
    }

    [DisallowMultipleComponent]
    [RequireComponent(typeof(HashsAndTags))]
    public class Managers : MonoBehaviour, managerInterface
    {
        //管理器的实例
        public static Managers instance;

        [Header("过场遮挡")]
        [SerializeField]
        private GameObject Mask;

        [Header("首页UI组")]
        [SerializeField]
        private GameObject UIMenu;

        private bool canInput = false;

        private changeSceneDo csd;

        private HashsAndTags hat;

        private bool canEventsDo = true;
        private bool canSplash = false;

        private void Awake()
        {
            //创造管理器实例
            if (Managers.instance == null)
            {
                Managers.instance = this;
                DontDestroyOnLoad(Managers.instance.gameObject);
            }
            else if (Managers.instance != this)
            {
                Destroy(this.gameObject);
            }

            hat = gameObject.GetComponent<HashsAndTags>();
            Mask.SetActive(false);
        }

        private void init()
        {
            gameManager.instance.init();
            audioManager.instance.init();
            ControlManager.instance.init();
            UIManager.instance.init();
            scenesManager.instance.init();
            dataManager.instance.init();

            var list = dataManager.instance.getCommonEventList();
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i].list.EventListInterface.getHowToStart() == start.auto)
                {
                    gameManager.instance.startPublicEvent(list[i].Key,nowState.none);
                }
            }
        }

        private void Start()
        {
            scenesManager.instance.callbacks += afterLoadScene;
            audioManager.instance.MusicValue = gameManager.instance.Setting.musicValue;
            audioManager.instance.SEValue = gameManager.instance.Setting.SEValue;
            UIManager.instance.stopSE = audioManager.instance.stopSE;
            UIManager.instance.playSE = audioManager.instance.playSE;
            UIManager.instance.changeState = ControlManager.instance.changeState;
            ControlManager.instance.callbackState += changeStateCallbaks;

            gameManager.instance.callbacks += audioManager.instance.changeValue;

            init();

            UIMenu.SetActive(true);
        }

        private void Update()
        {
            doEveryFrame();
        }

        private void doEveryFrame()
        {
            audioManager.instance.doEveryFrame(gameManager.instance.Setting.musicValue, gameManager.instance.Setting.SEValue);
            if (canInput)
            {
                ControlManager.instance.doEveryFrame();
                if (ControlManager.instance.NowState == nowState.window)
                {
                    checkWindow();
                }
                else if (ControlManager.instance.NowState == nowState.move)
                {
                    checkPlayer();
                }
            }

            gameManager.instance.doEveryFrame();
        }


        // =================================== Logo闪屏处理 ===================================

        public void showSplach(logoSetting[] logos, Func call)
        {
            StartCoroutine(splash(logos,call));
        }

        public void changeCanSplash(bool can)
        {
            canSplash = can;
        }

        IEnumerator splash(logoSetting[] logos,Func call)
        {
            for (int i = 0; i < logos.Length; i++)
            {
                logos[i].obj.SetActive(true);
                UIManager.instance.beWhite(1.0f, true);
                yield return new WaitForSeconds(1.0f);
                if (logos[i].type==splashType.button)
                {
                    logos[i].btn.onClick.AddListener(delegate () {
                        changeCanSplash(true);
                    });
                    while (!canSplash)
                    {
                        yield return new WaitForSeconds(0.01f);
                    }
                }
                else if(logos[i].type == splashType.confirm)
                {
                    while(!ControlManager.instance.Inputs.Contains(keyInput.confirm))
                    {
                        ControlManager.instance.doEveryFrame();
                        yield return new WaitForSeconds(0.01f);
                    }
                    ControlManager.instance.clearInput();
                }
                else
                {
                    yield return new WaitForSeconds(2f);
                }
                UIManager.instance.beBlack(1.0f, true);
                yield return new WaitForSeconds(1f);
                logos[i].obj.SetActive(false);
                canSplash = false;
            }
            audioManager.instance.playMusic(dataManager.instance.findAudioInList("标题"));
            UIManager.instance.beWhite(1.0f, true);
            yield return new WaitForSeconds(0.8f);

            canInput = true;
            call();
        }


        // =================================== 数据存取处理 ===================================

        private void setGlobalDataToGame(globalData data)
        {
            switch (data.type)
            {
                case globalDataType._double:
                    {
                        dataManager.instance.setDouble(data.key, double.Parse(data.value));
                        break;
                    }
                case globalDataType._int:
                    {
                        dataManager.instance.setInt(data.key, int.Parse(data.value));
                        break;
                    }
                case globalDataType._switch:
                    {
                        dataManager.instance.setSwitch(data.key, bool.Parse(data.value));
                        break;
                    }
                default: break;
            }
        }

        public void deleteData(string fileName)
        {
            saveData data = readFile(fileName);
            for (int i = 0; i < data.dataDics.Count; i++)
            {
                deleteFile(data.dataDics[i]);
            }
            for (int i = 0; i < data.gameDics.Count; i++)
            {
                deleteFile(data.gameDics[i]);
            }

            deleteFile(fileName);
        }

        public void loadData(string fileName)
        {
            saveData data = readFile(fileName);

            gameManager.instance.PlayTime += data.playTime;

            dataManager.instance.clear();
            for (int i = 0; i < data.dataDics.Count; i++)
            {
                string name = data.dataDics[i];
                string content = File.ReadAllText(Application.dataPath + @"/Save/" + name + ".json");
                string key = name.Replace(fileName + "_data_", "");
                dataDictionary d = new dataDictionary(checkType(false, key,content));                
                dataManager.instance.loadDataDictionary(key, d);
            }
            for (int i = 0; i < data.gameDics.Count; i++)
            {
                string name = data.gameDics[i];
                string content = File.ReadAllText(Application.dataPath + @"/Save/" + name + ".json");
                string key = name.Replace(fileName + "_game_", "");
                gameDictionary d = new gameDictionary(checkType(false,key,content));
                dataManager.instance.loadGameDictionary(key, d);
            }

            gameManager.instance.loadIndependentSwitchs(data.independentSwitches);

            gameManager.instance.loadTeam(data.team);
        }

        public void saveData(string fileName)
        {
            saveData data = new saveData();
            data.fileName = fileName;
            data.version = Application.version;
            data.playTime = gameManager.instance.PlayTime;

            data.dataDics = new List<string>();
            var d = dataManager.instance.getDataDictionary();
            List<string> keys = new List<string>(d.Keys);
            List<object> values = new List<object>(d.Values);
            for (int i = 0; i < keys.Count; i++)
            {
                string dataDic = fileName + "_data_" + keys[i];
                data.dataDics.Add(dataDic);
                writeFile(dataDic, checkType(true,keys[i], values[i]));
            }

            data.gameDics = new List<string>();
            d = dataManager.instance.getGameDictionary();
            keys = new List<string>(d.Keys);
            values = new List<object>(d.Values);
            for (int i = 0; i < keys.Count; i++)
            {
                string gameDic = fileName + "_game_" + keys[i];
                data.gameDics.Add(gameDic);
                writeFile(gameDic, checkType(true, keys[i],values[i]));
            }

            data.independentSwitches = gameManager.instance.IndependentSwitches;

            data.mapName = scenesManager.instance.NowScene;
            data.team = gameManager.instance.Team;
            var player = ControlManager.instance.NowPlayer;
            data.position = new vector3d(player.transform.position);

            // 2d独有
            var p = player.GetComponent<ActorInterface>();
            if(GetControlMode()==controlMode.control_2D)
            {
            #if use2D
                var a = turnTool.get(((ActorMoveInterface2D)p.getMove()).getTurn());
                data.turn = new vector3d(a.x, a.y);
            #endif
            }
            else
            {
                // 3d独有
                data.turn = new vector3d(player.transform.rotation.eulerAngles);
            }

            writeFile(fileName, data);
        }

        public Dictionary<string, object> checkType(bool toSaveType, string key, object value)
        {
            Dictionary<string, object> list = new Dictionary<string, object>();
            if (toSaveType)
            {
                Dictionary<string, object> dic = (Dictionary<string, object>)value;               
                var keys = new List<string>(dic.Keys);
                var values = new List<object>(dic.Values);
                for (int i = 0; i < values.Count; i++)
                {
                    if (key == dictionaryName.vec3s)
                    {
                        var v = (Vector3)values[i];
                        list.Add(keys[i], new vector3d(v.x, v.y, v.z));
                    }
                    else
                    {
                        list.Add(keys[i], values[i]);
                    }
                }
            }
            else
            {
                if (key == dictionaryName.vec3s)
                {
                    Dictionary<string, vector3d> dic = JsonMapper.ToObject<Dictionary<string, vector3d>>((string)value);
                    var keys = new List<string>(dic.Keys);
                    var values = new List<vector3d>(dic.Values);
                    for (int i = 0; i < values.Count; i++)
                    {
                        list.Add(keys[i], values[i].toVec3());
                    }
                }
                else
                {
                    list = JsonMapper.ToObject<Dictionary<string, object>>((string)value);
                }
            }

            return list;
        }

        // =================================== 用户输入处理 ===================================

        private void checkWindow()
        {
            var w = ControlManager.instance.NowWindow;
            if (w != null)
            {
                var window = w.GetComponent<keyboardMenuInterface>();
                if (ControlManager.instance.Inputs.Contains(keyInput.upOnce))
                {
                    window.up();
                }
                else if (ControlManager.instance.Inputs.Contains(keyInput.downOnce))
                {
                    window.down();
                }
                else if (ControlManager.instance.Inputs.Contains(keyInput.leftOnce))
                {
                    window.left();
                }
                else if (ControlManager.instance.Inputs.Contains(keyInput.rightOnce))
                {
                    window.right();
                }

                if (ControlManager.instance.Inputs.Contains(keyInput.confirm))
                {
                    window.doOption();
                }
                else if (ControlManager.instance.Inputs.Contains(keyInput.cancel))
                {
                    window.cancel();
                }
            }
        }

        public void callMenu()
        {
            audioManager.instance.playSE(dataManager.instance.findAudioInList("确定"));
            UIManager.instance.showMenuPanel();
        }

        // 角色对输入做出反应
        private void checkPlayer()
        {
            var p = ControlManager.instance.NowPlayer;
            if (p != null)
            {
                var player = p.GetComponent<ActorInterface>();
                bool run = false;
                if (gameManager.instance.Setting.alwaysRun)
                {
                    run = true;
                    if (ControlManager.instance.Inputs.Contains(keyInput.shift))
                        run = false;
                }
                else
                {
                    run = false;
                    if (ControlManager.instance.Inputs.Contains(keyInput.shift))
                        run = true;
                }
                bool rush = ControlManager.instance.Inputs.Contains(keyInput.ctrl);

                ActorMoveInterface m = player.getMove();
                if (ControlManager.instance.Inputs.Contains(keyInput.up))
                {
                    m.Move(new Vector2(0, 1), run, rush);
                }
                else if (ControlManager.instance.Inputs.Contains(keyInput.down))
                {
                    m.Move(new Vector2(0, -1), run, rush);
                }
                else if (ControlManager.instance.Inputs.Contains(keyInput.left))
                {
                    m.Move(new Vector2(-1, 0), run, rush);
                }
                else if (ControlManager.instance.Inputs.Contains(keyInput.right))
                {
                    m.Move(new Vector2(1, 0), run, rush);
                }
                else
                {
                    m.Move(Vector2.zero, run, rush);
                }

                if (ControlManager.instance.Inputs.Contains(keyInput.jump))
                {
                    m.Jump();
                }

                if (ControlManager.instance.Inputs.Contains(keyInput.confirm))
                {

                }
                else if (ControlManager.instance.Inputs.Contains(keyInput.cancel))
                {
                    if(UIManager.instance.canCallMenu)
                    {
                        m.Move(Vector2.zero, run, rush);
                        callMenu();
                    }
                }

                ActorBattleInterface b = player.getBattle();
                if(b!=null)
                {
                    if (ControlManager.instance.MouseInputs.Contains(mouseInput.leftDown)|| ControlManager.instance.MouseInputs.Contains(mouseInput.left))
                    {
                        b.Attack();
                    }
                }

                player.CameraRotate(ControlManager.instance.MouseX, ControlManager.instance.MouseY);
                player.CameraScale(ControlManager.instance.MouseScrollWheel);
            }
        }

        public void changeStateCallbaks(nowState ns)
        {
            if (ns == nowState.move)
            {
                if (scenesManager.instance.NowScene != scenesManager.firstScene)
                    UIManager.instance.showGameUI();
                if(gameManager.instance.GameSetting.canAttack)
                    UIManager.instance.showPivot();
                else
                    UIManager.instance.hidePivot();
            }
            else
            {
                UIManager.instance.hidePivot();
                UIManager.instance.hideGameUI();
            }
        }

        // =================================== 场景转换 ===================================

        public void backToMain()
        {
            changeSceneDo _csd = new changeSceneDo();
            LoadLevel(scenesManager.firstScene, _csd, true);
        }

        public void LoadLevel(string level, changeSceneDo _csd, bool stopMusic = true)
        {
            canEventsDo = false;
            canInput = false;
            ControlManager.instance.Inputs.Clear();
            ControlManager.instance.MouseInputs.Clear();
            ControlManager.instance.NowMode = gameManager.instance.StartSettings[gameManager.instance.nowStartSetting].startMode;
            UIManager.instance.beBlack(1.0f, true);
            UIManager.instance.hideTextPanel();
            audioManager.instance.resetForNewScene(stopMusic);

            csd = _csd;

            scenesManager.instance.loadScene(level, Mask);
        }

        public void startGame()
        {
            changeSceneDo _csd = new changeSceneDo(
                gameManager.instance.StartSettings[gameManager.instance.nowStartSetting].startActor[0],
                gameManager.instance.StartSettings[gameManager.instance.nowStartSetting].StartPos,
                gameManager.instance.StartSettings[gameManager.instance.nowStartSetting].StartRotate,
                gameManager.instance.StartSettings[gameManager.instance.nowStartSetting].StartTurn);

            List<globalData> datas = gameManager.instance.StartSettings[gameManager.instance.nowStartSetting].datas;
            for (int i = 0; i < datas.Count; i++)
            {
                setGlobalDataToGame(datas[i]);
            }

            for (int i=0;i< gameManager.instance.StartSettings[gameManager.instance.nowStartSetting].startActor.Count;i++)
                inTeam(gameManager.instance.StartSettings[gameManager.instance.nowStartSetting].startActor[i]);

            LoadLevel(gameManager.instance.StartSettings[gameManager.instance.nowStartSetting].StartScene, _csd);
        }

        // =================================== 场景转换后处理 ===================================

        public void afterLoadScene()
        {
            resetForNewScene(csd);

            if(csd.changeType == changeSceneType.fromBlack)
            {
                UIManager.instance.beWhite(1.0f, true);
            }
            else if (csd.changeType == changeSceneType.fromWhite)
            {
                UIManager.instance.beWhite(1.0f, false);
            }

            if(scenesManager.instance.NowScene == scenesManager.firstScene)
            {
                UIMenu = GameObject.Find("Canvas").transform.GetChild(0).gameObject;
                UIMenu.SetActive(true);
            }
            StartCoroutine(StartDo());
        }

        public IEnumerator StartDo()
        {
            yield return new WaitForSeconds(1.0f);
            Mask.SetActive(false);
            canEventsDo = true;
            yield return new WaitForSeconds(0.1f);
            canInput = true;
        }

        public void resetForNewScene(changeSceneDo csd)
        {
            if (csd.actorId != -1)
            {
                var g = gameManager.instance.SystemSetting.PlayerInfos[csd.actorId].playerPrefab;                
                GameObject go = Instantiate(g) as GameObject;
                go.transform.position = csd.pos;

                // 2d独有
                ActorInterface a = go.GetComponent<ActorInterface>();
                if (GetControlMode()==controlMode.control_2D)
                {
#if use2D
                    ActorInterface2D a_ = (ActorInterface2D)a;
                    var v = turnTool.get(csd.turn);
                    ((ActorMoveInterface2D)a.getMove()).Rotating(v.x,v.y,true);
                    var list = getTeam();
                    for (int i = 1; i < list.Count; i++)
                    {
                        g = Instantiate(gameManager.instance.SystemSetting.PlayerInfos[list[i]].playerPrefab);
                        a_.addFollow(g);
                    }
                    a_.changeFollowsPos(csd.turn, csd.pos);
#endif
                }
                else
                {
                    // 3d独有
                    a = go.GetComponent<ActorInterface>();
                    go.transform.rotation.SetEulerRotation(csd.rotate.x, csd.rotate.y, csd.rotate.z);
                }

                ControlManager.instance.startUserControl(go);
            }
            else
                ControlManager.instance.changeState(nowState.window);
        }


        // =================================== 文件操作 ===================================

        public saveData readFile(string fileName)
        {
            string filePath = Application.dataPath + @"/Save/" + fileName + ".json";
            if (File.Exists(filePath))
            {
                string txt = File.ReadAllText(filePath);
                return JsonMapper.ToObject<saveData>(txt);
            }
            else
                return null;

        }

        public void writeFile(string fileName, object data)
        {
            string filePath = Application.dataPath + @"/Save/" + fileName + ".json";
            if (false == System.IO.Directory.Exists(Application.dataPath + @"/Save/"))
            {
                //创建文件夹
                System.IO.Directory.CreateDirectory(Application.dataPath + @"/Save/");
            }
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
        }

        public void deleteFile(string fileName)
        {
            string filePath = Application.dataPath + @"/Save/" + fileName + ".json";
            File.Delete(filePath);
        }

        // =================================== 其他 ===================================

        public HashsAndTags getHat()
        {
            return hat;
        }

        public bool useItem(string id, commonEventCallback c = null)
        {
            var it = dataManager.instance.findItemInList(id);
            if (it == null)
                return false;
            if (it.getData<int>(itemProperty.times) == -1)
                return false;
            if (it.getData<int>(itemProperty.times) != -2)
                dataManager.instance.removeItem(id, 1);

            switch (id)
            {
                default:break;
            }

            int num = it.getData<int>(itemProperty.eventNum);
            var list = it.getData<List<string>>(itemProperty.commonEvent);
            for (int i = 0; i < num; i++)
            {
                gameManager.instance.startPublicEvent(list[i], nowState.window,c);
            }

            return true;
        }

        public bool getCanEventsDo()
        {
            return canEventsDo;
        }

        // =================================== gameManager ===================================
        public void setNowStartSetting(int i)
        {
            gameManager.instance.nowStartSetting = i;
        }

        public systemSetting getSystemSetting()
        {
            return gameManager.instance.SystemSetting;
        }

        public gameSetting getGameSetting()
        {
            return gameManager.instance.GameSetting;
        }

        public int getOriSize()
        {
            return gameManager.instance.OriSize;
        }

        public settings getSetting()
        {
            return gameManager.instance.Setting;
        }

        public void changeLanguage(bool add)
        {
            gameManager.instance.changeLanguage(add);
        }

        public void changeBool(string key)
        {
            gameManager.instance.changeBool(key);
        }

        public void addWindowSize(bool add)
        {
            gameManager.instance.addWindowSize(add);
            ControlManager.instance.NowWindow.GetComponent<keyboardMenuInterface>().changeSize();
        }

        public void saveSetting()
        {
            gameManager.instance.saveSetting();
        }

        public void addToSettingCallbacks(changeSettingCallbaks cb)
        {
            gameManager.instance.callbacks += cb;
        }

        public void startPublicEvent(string name,nowState ns, commonEventCallback c =null, Component cm = null)
        {
            gameManager.instance.startPublicEvent(name, ns,c,cm);
        }

        public bool[] getIndependentSwitchs(string key)
        {
            return gameManager.instance.getIndependentSwitchs(key);
        }

        public void setIndependentSwitchs(string key, int index, bool ind)
        {
            gameManager.instance.setIndependentSwitchs(key, index, ind);
        }

        public void inTeam(int id)
        {
            gameManager.instance.inTeamIfNotContain(id);
        }

        public void outTeam(int id)
        {
            gameManager.instance.outTeamIfContain(id);
        }

        public List<int> getTeam()
        {
            return gameManager.instance.Team;
        }

        // =================================== dataManager ===================================

        public eventStruct getCommonEvent(string name)
        {
            return dataManager.instance.getCommonEvent(name);
        }

        public int getTotalItemNum(itemType type)
        {
            return dataManager.instance.getTotalNumInList(dictionaryName.items ,(int)type);
        }

        public int getTotalQuestNum(questStatus type)
        {
            return dataManager.instance.getTotalNumInList(dictionaryName.quests, (int)type);
        }

        public ListNodeInterface getItemInfo(string key)
        {
            return dataManager.instance.findItemInList(key);
        }

        public ListNodeInterface getQuestInfo(string key)
        {
            return dataManager.instance.findQuestInList(key);
        }

        public int getInt(string key)
        {
            return dataManager.instance.getInt(key);
        }

        public double getDouble(string key)
        {
            return dataManager.instance.getDouble( key);
        }

        public bool getSwitch(string key)
        {
            return dataManager.instance.getSwitch(key);
        }

        public Vector3 getVec3(string key)
        {
            return dataManager.instance.getVec3(key);
        }

        public void setInt(string key,int v)
        {
            dataManager.instance.setInt(key,v);
        }

        public void setDouble(string key,double v)
        {
            dataManager.instance.setDouble(key,v);
        }

        public void setSwitch(string key,bool v)
        {
            dataManager.instance.setSwitch(key,v);
        }

        public void setVec3(string key, Vector3 v)
        {
            dataManager.instance.setVec3(key, v);
        }

        public void getItem(string id, int num)
        {
            dataManager.instance.addItem(id, num);
        }

        public void removeItem(string id, int num)
        {
            dataManager.instance.removeItem(id, num);
        }
        public void addQuest(string id)
        {
            dataManager.instance.addQuest(id);
        }

        public void changeQuest(string id, questStatus type)
        {
            dataManager.instance.changeQuest(id, type);
        }
        public Dictionary<string, int> getItems()
        {
            return dataManager.instance.getListInGameDictionary<int>(dictionaryName.items);
        }

        public Dictionary<string, int> getQuests()
        {
            return dataManager.instance.getListInGameDictionary<int>(dictionaryName.quests);
        }

        public GameObject findPrefab(string id)
        {
            return dataManager.instance.findPrefabInList(id);
        }

        public AudioClip findAudio(string id)
        {
            return dataManager.instance.findAudioInList(id);
        }

        public float findPitch(string id)
        {
            return dataManager.instance.findPitchInList(id);
        }

        public Sprite findImg(string id)
        {
            return dataManager.instance.findImgInList(id);
        }

        public string findText(string id, language lang)
        {
            return dataManager.instance.findTextInList(id,lang);
        }

        // =================================== ControlManager ===================================

        public controlMode GetControlMode()
        {
            return ControlManager.instance.NowMode;
        }

        public nowState getNowState()
        {
            return ControlManager.instance.NowState;
        }

        public GameObject getPlayer()
        {
            return ControlManager.instance.NowPlayer;
        }

        public List<keyInput> getInputs()
        {
            return ControlManager.instance.Inputs;
        }

        public List<mouseInput> getMouseInputs()
        {
            return ControlManager.instance.MouseInputs;
        }

        public void startUserControl(GameObject _player = null)
        {
            ControlManager.instance.startUserControl(_player);
        }

        public void stopUserControl()
        {
            ControlManager.instance.stopUserControl();
        }

        public void changeState(nowState state, GameObject _object = null)
        {
            ControlManager.instance.changeState(state, _object);
        }

        // =================================== UIManager ===================================

        public void showGameUI()
        {
            UIManager.instance.showGameUI();
        }

        public void hideGameUI()
        {
            UIManager.instance.hideGameUI();
        }

        public void showVideoPanel(string name, Func call)
        {
            UnityEngine.Video.VideoClip clip = dataManager.instance.findVideoInList(name);
            UIManager.instance.showVideoPanel(clip, call);
        }

        public void enableAttack()
        {
            UIManager.instance.showPivot();
            gameManager.instance.GameSetting.canAttack = true;
        }

        public void disableAttack()
        {
            UIManager.instance.hidePivot();
            gameManager.instance.GameSetting.canAttack = false;
        }

        public bool getShowFinish()
        {
            return UIManager.instance.ShowFinish;
        }

        public bool getCanHind()
        {
            return UIManager.instance.CanHide;
        }

        public GameObject getMainMenu()
        {
            return UIManager.instance.getMainMenu();
        }

        public void setMainMenu(GameObject menu)
        {
            UIManager.instance.setMainMenu(menu);
        }

        public void showChoosePanel(string text, Func yes, Func no, nowState ns, GameObject mainPanel = null, bool black = false)
        {
            UIManager.instance.showChoosePanel(text, yes, no, ns, mainPanel, black);
        }

        public void showEventChoosePanel(string maintext, Func[] choices, string[] texts, nowState nowstate, GameObject mainPanel = null, bool black = false)
        {
            UIManager.instance.showEventChoosePanel(maintext, choices, texts, nowstate, mainPanel, black);
        }

        public GameObject showAnyPanel(GameObject _panel, Vector2 _pos, bool mainUI, nowState ns)
        {
            return UIManager.instance.showAnyPanel(_panel, _pos, mainUI, ns);
        }

        public GameObject showPicPanel(string index, Vector2 _pos, Sprite sp, float a)
        {
            return UIManager.instance.showPicPanel(index, _pos, sp, a);
        }

        public GameObject showButton(string index, Vector2 _pos, GameObject btn, Func call)
        {
            return UIManager.instance.showButton(index, _pos, btn, call);
        }

        public void showHint(string text)
        {
            UIManager.instance.showHint(text, gameManager.instance.Setting.windowSize, gameManager.instance.OriSize);
        }

        public void showTextPanel(Component _object, string name, string text, Sprite sprite, AudioClip se = null, float p = 1.0f, bool _new = false)
        {
            UIManager.instance.showTextPanel(_object, name, text, sprite, se, p, _new);
        }

        public void showSpecialTextPanel(Component _object, string text, Vector2 pos, AudioClip se = null, float p = 1.0f)
        {
            UIManager.instance.showSpecialTextPanel(_object, text, pos, se, p);
        }

        public void showSavePanel(Func call = null)
        {
            UIManager.instance.showSavePanel(call);
        }

        public GameObject showCanDoHint(Vector3 _pos, float scale = 1)
        {
            return UIManager.instance.showCanDoHint(_pos,scale);
        }

        public GameObject showFlash(Color c, float time)
        {
            return UIManager.instance.showFlash(c, time);
        }

        public void hideTextPanel()
        {
            UIManager.instance.hideTextPanel();
        }

        public void hidePicPanel(string index)
        {
            UIManager.instance.hidePicPanel(index);
        }

        public void hideButton(string index)
        {
            UIManager.instance.hideButton(index);
        }

        public void movePicPanel(string index, Vector2 pos, float a, float time)
        {
            UIManager.instance.movePicPanel(index, pos, a, time);
        }

        public void beWhite(float time, bool black)
        {
            UIManager.instance.beWhite(time, black);
        }

        public void beBlack(float time, bool black)
        {
            UIManager.instance.beBlack(time, black);
        }

        public bool checkTextRequet(Component _object)
        {
            return UIManager.instance.checkTextRequet(_object);
        }

        public void skip()
        {
            UIManager.instance.skip();
        }


        // =================================== audioManager ===================================

        public void playSE(string name, float pitch = 1.0f, bool loop = false)
        {
            AudioClip clip = dataManager.instance.findAudioInList(name);
            playSE(clip, pitch,loop);
        }

        public void playMusic(string name)
        {
            AudioClip clip = dataManager.instance.findAudioInList(name);
            playMusic(clip);
        }

        public void playBGS(string name)
        {
            AudioClip clip = dataManager.instance.findAudioInList(name);
            audioManager.instance.playBGS(clip);
        }

        private void playSE(AudioClip clip, float pitch = 1.0f, bool loop = false)
        {
            audioManager.instance.playSE(clip,pitch,loop);
        }

        private void playMusic(AudioClip clip)
        {
            audioManager.instance.playMusic(clip);
        }

        private void playBGS(AudioClip clip)
        {
            audioManager.instance.playBGS(clip);
        }

        public void stopMusic(bool now = false)
        {
            audioManager.instance.stopMusic(now);
        }

        public void stopBGS(bool now = false)
        {
            audioManager.instance.stopBGS(now);
        }

        public bool reduceBGSVolume(float num)
        {
            return audioManager.instance.reduceBGSVolume(num);
        }
        public bool reduceMusicVolume(float num)
        {
            return audioManager.instance.reduceMusicVolume(num);
        }

        public void changeMusicValue(float num)
        {
            audioManager.instance.changeMusicValue(num);
            gameManager.instance.Setting.musicValue += num;
        }

        public void changeSEValue(float num)
        {
            audioManager.instance.changeSEValue(num);
            gameManager.instance.Setting.SEValue += num;
        }

        public void setMusicValue(float v)
        {
            audioManager.instance.MusicValue = v;
            gameManager.instance.Setting.musicValue = v;
        }

        public void setSEValue(float v)
        {
            audioManager.instance.SEValue = v;
            gameManager.instance.Setting.SEValue = v;
        }


        // =================================== scenesManager ===================================

        public string getNowScene()
        {
            return scenesManager.instance.NowScene;
        }
    }
}
