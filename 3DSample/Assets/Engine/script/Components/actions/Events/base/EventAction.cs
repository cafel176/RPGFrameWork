using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions
{
    public abstract class EventAction : basicAction
    {
        private eventTools data = new eventTools();

        protected TreeNodeInterface toBeNow = null;
        public TreeNodeInterface ToBeNow
        {
            set
            {
                if (toBeNow == null)
                {
                    toBeNow = value;
                    pushNow(true);
                }                   
            }
        }

        protected GameObject player;
        public GameObject Player
        {
            get
            {
                return player;
            }
            set
            {
                player = value;
            }
        }

        protected bool canDoSth = true;
        public bool CanDoSth
        {
            get
            {
                return canDoSth;
            }
        }

        protected AudioSource _audio = null;
        //获取场景相机
        protected GameObject _camera;

        public virtual void doSth(TreeNodeInterface toDo)
        {
            _do.doSth(toDo);
        }

        public abstract bool checkEnd();

        public abstract void pushNow(bool auto = false);

        public bool checkCondition(eventStruct e)
        {
            conditionDictionary con = new conditionDictionary();
            con.type = e.ThisNow.getData<conditionType>(structProperty.conditionType);
            con.name = e.ThisNow.getData<string>(structProperty.str1);
            con.compare = e.ThisNow.getData<compare>(structProperty.compare);
            con.value = e.ThisNow.getData<string>(structProperty.str2);

            return e.EventListInterface.checkCondition(con, e.EventListInterface.getIndependentSwitchs());
        }

        public void setCanDo(bool can)
        {
            canDoSth = can;
        }



        public void showTextPanel(Component g, string name, string text, Sprite sprite, AudioClip se = null, float p = 1.0f, bool _new = false)
        {
            data.showTextPanel(g, name, text, sprite, se, p, _new);
        }

        public void showTextPanel(Component g, message m, Sprite sprite, AudioClip se = null, float p = 1.0f, bool _new = false)
        {
            data.showTextPanel(g, m.name, m.text, sprite, se, p, _new);
        }

        public void showSpecialTextPanel(Component g, string text, Vector2 pos, AudioClip se = null, float p = 1.0f)
        {
            data.showSpecialTextPanel(g, text, pos, se, p);
        }

        public void showSpecialTextPanel(Component g, message m, Vector2 pos, AudioClip se = null, float p = 1.0f)
        {
            data.showSpecialTextPanel(g, m.text, pos, se, p);
        }

        public void showEventChoosePanel(string maintext, Func[] choices, string[] texts, bool black = false)
        {
            data.showEventChoosePanel(maintext, choices, texts, black);
        }

        public void showHint(string text, bool wait = true)
        {
            data.showHint(text);
        }

        public void showHint(message m, bool wait = true)
        {
            data.showHint(m.text);
        }

        public GameObject showAnyPanel(GameObject _panel, Vector2 _pos, bool mainUI = false)
        {
            return data.showAnyPanel(_panel, _pos, mainUI);
        }

        public void enableAttack()
        {
            data.enableAttack();
        }

        //停止玩家控制，不卡进程
        public void disableAttack()
        {
            data.disableAttack();
        }

        public void startUserControl(GameObject _player = null)
        {
            data.startUserControl(_player);
        }

        //停止玩家控制，不卡进程
        public void stopUserControl()
        {
            data.stopUserControl();
        }

        public void loadLevel(string level, changeSceneDo csd, bool stopMusic = true)
        {
            data.LoadLevel(level, csd, stopMusic);
        }

        public void playMusic(string clip)
        {
            data.playMusic(clip);
        }

        public void playBGS(string clip)
        {
            data.playBGS(clip);
        }

        public void playSE(string clip, float pitch = 1.0f, bool loop = false)
        {
            data.playSE(clip, pitch, loop);
        }

        public void playVideo(string name)
        {
            data.playVideo(name, delegate { pushNow(); });
        }

        public List<keyInput> getInputs()
        {
            return data.getInputs();
        }

        public void addToSettingCallbacks(changeSettingCallbaks cb)
        {
            data.addToSettingCallbacks(cb);
        }

        public nowState getNowState()
        {
            return data.getNowState();
        }

        public bool getCanEventsDo()
        {
            return data.getCanEventsDo();
        }

        public bool getShowFinish()
        {
            return data.getShowFinish();
        }

        public bool getCanHind()
        {
            return data.getCanHind();
        }

        public bool checkTextRequet(Component g)
        {
            return data.checkTextRequet(g);
        }

        public void skip()
        {
            data.skip();
        }

        public void hideTextPanel()
        {
            data.hideTextPanel();
        }

        public eventStruct getCommonEvent(string name)
        {
            return data.getCommonEvent(name);
        }

        public GameObject showCanDoHint(Vector3 pos, float scale)
        {
            return data.showCanDoHint(pos, scale);
        }

        public string getNowScene()
        {
            return data.getNowScene();
        }

        public void stopMusic()
        {
            data.stopMusic();
        }

        public void stopBGS()
        {
            data.stopBGS();
        }

        public GameObject showPicPanel(string index, Vector2 _pos, Sprite sp, float a)
        {
            return data.showPicPanel(index, _pos, sp, a);
        }

        public void hidePicPanel(string index)
        {
            data.hidePicPanel(index);
        }

        public GameObject showButton(string index, Vector2 _pos, string key, Func call)
        {
            return data.showButton(index, _pos, findPrefab(key), call);
        }

        public void hideButton(string index)
        {
            data.hideButton(index);
        }

        public void movePicPanel(string index, Vector2 pos, float a, float time)
        {
            data.movePicPanel(index, pos, a, time);
        }

        public List<int> getTeam()
        {
            return data.getTeam();
        }

        public void showSavePanel(Func call)
        {
            data.showSavePanel(call);
        }

        public GameObject showFlash(Color c, float time)
        {
            return data.showFlash(c, time);
        }
        
        public settings getSetting()
        {
            return data.getSetting();
        }

        public systemSetting getSystemSetting()
        {
            return data.getSystemSetting();
        }

        public HashsAndTags getHat()
        {
            return data.getHat();
        }

        public controlMode GetControlMode()
        {
            return data.GetControlMode();
        }

        public virtual void changeState(nowState ns, GameObject g = null)
        {
            data.changeState(ns, g);
        }

        //角色入队
        public void inTeam(int id)
        {
            data.inTeam(id);
        }

        //角色离队
        public void outTeam(int id)
        {
            data.outTeam(id);
        }

        //获得道具，不卡进程
        public void getItem(string i, int num)
        {
            data.getItem(i, num);
        }

        public void useItem(string i)
        {
            data.useItem(i);
        }

        public void removeItem(string i, int num)
        {
            data.removeItem(i, num);
        }

        public string findText(string key, language lang)
        {
            return data.findText(key, lang);
        }

        public GameObject findPrefab(string key)
        {
            return data.findPrefab(key);
        }

        public Sprite findImg(string key)
        {
            return data.findImg(key);
        }

        public AudioClip findAudio(string key)
        {
            return data.findAudio(key);
        }

        public float findPitch(string key)
        {
            return data.findPitch(key);
        }

        public void setInt(string key, int v)
        {
            data.setInt(key, v);
        }

        public void setDouble(string key, double v)
        {
            data.setDouble(key, v);
        }

        public void setSwitch(string key, bool v)
        {
            data.setSwitch(key, v);
        }

        public void setVec3(string key, Vector3 v)
        {
            data.setVec3(key, v);
        }

        public int getInt(string key)
        {
            return data.getInt(key);
        }

        public double getDouble(string key)
        {
            return data.getDouble(key);
        }

        public bool getSwitch(string key)
        {
            return data.getSwitch(key);
        }

        public Vector3 getVec3(string key)
        {
            return data.getVec3(key);
        }

        public ListNodeInterface getItemInfo(string key)
        {
            return data.getItemInfo(key);
        }

        public void addQuest(string id)
        {
            data.addQuest(id);
        }

        public void changeQuest(string id, questStatus type)
        {
            data.changeQuest(id, type);
        }

        public bool[] getIndependentSwitchs(string name)
        {
            return data.getIndependentSwitchs(name);
        }

        public void setIndependentSwitchs(string key, int index, bool ind)
        {
            data.setIndependentSwitchs(key, index, ind);
        }

        // 让角色停止运动
        public void stopPlayer(GameObject _player)
        {
            _player.GetComponent<ActorInterface>().getMove().Move(Vector2.zero);
        }
    }
}
