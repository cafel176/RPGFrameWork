using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions
{
    public class eventTools : dataComponent
    {
        //显示文字，卡进程
        public void showTextPanel(Component g, string name, string text, Sprite sprite, AudioClip se = null, float p = 1.0f, bool _new = false)
        {
            MI.showTextPanel(g, name, text, sprite, se, p, _new);
        }

        public void showTextPanel(Component g, message m, Sprite sprite, AudioClip se = null, float p = 1.0f, bool _new = false)
        {
            MI.showTextPanel(g, m.name, m.text, sprite, se, p, _new);
        }

        public void showSpecialTextPanel(Component g, string text, Vector2 pos, AudioClip se = null, float p = 1.0f)
        {
            MI.showSpecialTextPanel(g, text, pos, se, p);
        }

        public void showSpecialTextPanel(Component g, message m, Vector2 pos, AudioClip se = null, float p = 1.0f)
        {
            MI.showSpecialTextPanel(g, m.text, pos, se, p);
        }

        public void showHint(string text, bool wait = true)
        {
            MI.showHint(text);
        }

        public void showHint(message m, bool wait = true)
        {
            MI.showHint(m.text);
        }

        public GameObject showAnyPanel(GameObject _panel, Vector2 _pos, bool mainUI = false)
        {
            return MI.showAnyPanel(_panel, _pos, mainUI, nowState.move);
        }

        public void startUserControl(GameObject _player = null)
        {
            MI.startUserControl(_player);
        }

        //停止玩家控制，不卡进程
        public void stopUserControl()
        {
            MI.stopUserControl();
        }

        public void loadLevel(string level, changeSceneDo csd, bool stopMusic = true)
        {
            MI.LoadLevel(level, csd, stopMusic);
        }

        public void playMusic(AudioClip clip)
        {
            MI.playMusic(clip);
        }

        public void playBGS(AudioClip clip)
        {
            MI.playBGS(clip);
        }

        public void playSE(AudioClip clip, float pitch = 1.0f, bool loop = false)
        {
            MI.playSE(clip, pitch, loop);
        }

        public List<keyInput> getInputs()
        {
            return MI.getInputs();
        }

        public void addToSettingCallbacks(changeSettingCallbaks cb)
        {
            MI.addToSettingCallbacks(cb);
        }

        public void addToStateCallbacks(changeStateCallbaks cb)
        {
            MI.addToStateCallbacks(cb);
        }

        public nowState getNowState()
        {
            return MI.getNowState();
        }

        public bool getShowFinish()
        {
            return MI.getShowFinish();
        }

        public bool getCanEventsDo()
        {
            return MI.getCanEventsDo();
        }

        public bool getCanHind()
        {
            return MI.getCanHind();
        }

        public bool checkTextRequet(Component g)
        {
            return MI.checkTextRequet(g);
        }

        public void skip()
        {
            MI.skip();
        }

        public void hideTextPanel()
        {
            MI.hideTextPanel();
        }

        public eventStruct getCommonEvent(string name)
        {
            return MI.getCommonEvent(name);
        }

        public void startPublicEvent(string name,nowState ns, commonEventCallback c = null, Component cm = null)
        {
            MI.startPublicEvent(name,ns,c,cm);
        }

        public bool[] getIndependentSwitchs(string name)
        {
            return MI.getIndependentSwitchs(name);
        }

        public void setIndependentSwitchs(string key, int index, bool ind)
        {
            MI.setIndependentSwitchs(key, index, ind);
        }

        public GameObject showCanDoHint(Vector3 pos)
        {
            return MI.showCanDoHint(pos);
        }

        public string getNowScene()
        {
            return MI.getNowScene();
        }

        public void stopMusic()
        {
            MI.stopMusic();
        }

        public void stopBGS()
        {
            MI.stopBGS();
        }

        public GameObject showPicPanel(string index, Vector2 _pos, Sprite sp, float a)
        {
            return MI.showPicPanel(index, _pos, sp, a);
        }

        public void hidePicPanel(string index)
        {
            MI.hidePicPanel(index);
        }

        public void movePicPanel(string index, Vector2 pos, float a, float time)
        {
            MI.movePicPanel(index, pos, a, time);
        }

        public List<int> getTeam()
        {
            return MI.getTeam();
        }

        public void showSavePanel()
        {
            MI.showSavePanel();
        }

        public GameObject showFlash(Color c, float time)
        {
            return MI.showFlash(c, time);
        }

        public void inTeam(int id)
        {
            MI.inTeam(id);
        }

        //角色离队
        public void outTeam(int id)
        {
            MI.outTeam(id);
        }

        //获得道具，不卡进程
        public void getItem(string i, int num)
        {
            MI.getItem(i, num);
        }

        public void useItem(string i)
        {
            MI.useItem(i);
        }

        public void removeItem(string i, int num)
        {
            MI.removeItem(i, num);
        }

        public string findText(string key, language lang)
        {
            return MI.findText(key, lang);
        }

        public GameObject findPrefab(string key)
        {
            return MI.findPrefab(key);
        }

        public Sprite findImg(string key)
        {
            return MI.findImg(key);
        }

        public AudioClip findAudio(string key)
        {
            return MI.findAudio(key);
        }

        public float findPitch(string key)
        {
            return MI.findPitch(key);
        }

        public void setInt(string key, int v)
        {
            MI.setInt(key, v);
        }

        public void setDouble(string key, double v)
        {
            MI.setDouble(key, v);
        }

        public void setSwitch(string key, bool v)
        {
            MI.setSwitch(key, v);
        }

        public void setVec3(string key, Vector3 v)
        {
            MI.setVec3(key, v);
        }

        public int getInt(string key)
        {
            return MI.getInt(key);
        }

        public double getDouble(string key)
        {
            return MI.getDouble(key);
        }

        public bool getSwitch(string key)
        {
            return MI.getSwitch(key);
        }

        public Vector3 getVec3(string key)
        {
            return MI.getVec3(key);
        }

        public ListNodeInterface getItemInfo(string key)
        {
            return MI.getItemInfo(key);
        }

        public void addQuest(string id)
        {
            MI.addQuest(id);
        }

        public void changeQuest(string id, questStatus type)
        {
            MI.changeQuest(id, type);
        }

    }
}
