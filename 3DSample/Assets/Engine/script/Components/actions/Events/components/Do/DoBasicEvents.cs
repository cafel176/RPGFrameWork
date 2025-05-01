using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//使用须知
/* 事件管理器共有函数的工具类，包含若干不卡进程函数
 * setPlayerMove来让角色静止来阻隔残留输入
 * startUserControl切换到玩家操作模式，即结束自动事件
 * stopUserControl停止玩家操作，即开始自动事件
 */


namespace Actions
{
    public class message
    {
        public string name, text;
        public message(string n, string t)
        {
            name = n; text = t;
        }

        public message(string t)
        {
            name = ""; text = t;
        }
    }

    public abstract class DoBasicEvents : DoAction
    {
        private eventTools data = new eventTools();

        protected EventAction a;

        protected override void onStart()
        {
            a = gameObject.GetComponent<EventAction>();
        }

        protected void pushNow(bool auto = false)
        {
            a.pushNow(auto);
        }

        protected void inTeam(int id)
        {
            data.inTeam(id);
        }

        //角色离队
        protected void outTeam(int id)
        {
            data.outTeam(id);
        }

        protected List<int> getTeam()
        {
            return data.getTeam();
        }

        protected systemSetting getSystemSetting()
        {
            return data.getSystemSetting();
        }

        protected settings getSetting()
        {
            return data.getSetting();
        }

        protected HashsAndTags getHat()
        {
            return data.getHat();
        }

        protected void startPublicEvent(string name, nowState ns)
        {
            data.startPublicEvent(name,ns, delegate () { pushNow(true); }, this);
        }

        protected GameObject findPrefab(string key)
        {
            return data.findPrefab(key);
        }

        //=====================卡进程函数===============================
        /* 开始时设置changeCanDo(false;
         * 协程结束时设置changeCanDo(true;
         * 都可推进流程
         */

        //显示文字，卡进程
        protected void showTextPanel(Component c, string name, string text, Sprite sprite, AudioClip se = null, float p = 1.0f, bool _new = false)
        {
            changeCanDo(true);
            a.showTextPanel(c, name, text, sprite, se, p, _new);
        }

        protected void showTextPanel(Component c, message m, Sprite sprite, AudioClip se = null, float p = 1.0f, bool _new = false)
        {
            changeCanDo(true);
            a.showTextPanel(c, m.name, m.text, sprite, se, p, _new);
        }

        protected void showSpecialTextPanel(Component c, string text, Vector2 pos, AudioClip se = null, float p = 1.0f)
        {
            changeCanDo(true);
            a.showSpecialTextPanel(c, text, pos, se, p);
        }

        protected void showSpecialTextPanel(Component c, message m, Vector2 pos, AudioClip se = null, float p = 1.0f)
        {
            changeCanDo(true);
            a.showSpecialTextPanel(c, m.text, pos, se, p);
        }

        //用于启用协程，启用的协程必须以showTextPanel为结尾
        protected void myStartCoroutine(IEnumerator i)
        {
            a.stopUserControl();
            changeCanDo(false);
            StartCoroutine(i);
        }

        //=====================卡进程函数===============================
        /* 开始时设置changeCanDo(false;
         * 协程结束时设置changeCanDo(true;
         * 都可推进流程
         */

        //淡入，卡进程
        protected void beBlack(float time, bool push = true, bool black = true)
        {
            changeCanDo(false);
            data.beBlack(time, black);
            a.changeState(nowState.auto);
            StartCoroutine(wait(time, push));
        }

        //淡出，卡进程
        protected void beWhite(float time, bool push = true, bool black = true)
        {
            changeCanDo(false);
            data.beWhite(time, black);
            a.changeState(nowState.auto);
            StartCoroutine(wait(time, push));
        }

        //等待，卡进程
        protected void waitForTime(float time, bool push = true)
        {
            changeCanDo(false);
            a.changeState(nowState.auto);
            StartCoroutine(wait(time, push));
        }

        IEnumerator wait(float time, bool push)
        {
            yield return new WaitForSeconds(time);
            if (push)
                pushNow(true);
            changeCanDo(true);
        }

        //显示文字提示，卡进程
        protected void showHint(string text, bool wait = true)
        {
            showHint(new message(text), wait);
        }

        protected void showHint(message m, bool wait = true)
        {
            changeCanDo(false);
            a.showHint(m.text);
            if (wait)
            {
                a.changeState(nowState.auto);
                StartCoroutine(hint());
            }
        }

        IEnumerator hint()
        {
            yield return new WaitForSeconds(2f);
            pushNow(true);
            changeCanDo(true);
        }

        protected void CameraMoveToSw(Vector3 move, float time)
        {
            a.stopUserControl();
            changeCanDo(false);
            GameObject c = Camera.main.gameObject;
            StartCoroutine(CameraToSw(c, move, time));
        }

        IEnumerator CameraToSw(GameObject sb, Vector3 move, float time)
        {
            Vector3 target = move;
            var cf = sb.GetComponent<CameraBase>();
            cf.Active = false;
            cf.MoveTo(target, time);
            yield return new WaitForSeconds(time + 1.0f);
            pushNow(true);
            changeCanDo(true);
        }

        protected void ShakeCamera(float hard, float time)
        {
            GameObject c = Camera.main.gameObject;
            var cf = c.GetComponent<CameraBase>();
            cf.Active = false;
            cf.shake(hard, time);
        }

        protected void SbMoveToSw(GameObject sb, Vector3 move, turn turn)
        {
            changeCanDo(false);
            var p = sb.GetComponent<ActorInterface>();
            if (p == null)
            {
                p = sb.transform.GetChild(0).gameObject.GetComponent<ActorInterface>();
                if (p == null)
                    return;
            }
            StartCoroutine(SbToSw(sb, p, move, turn));
        }

        IEnumerator SbToSw(GameObject g, ActorInterface sb, Vector3 move, turn turn)
        {
            Vector3 target = move;
            ActorMoveInterface m = sb.getMove();
            m.MoveTo(target, false);
            Vector3 d = g.transform.position;
            float distanceEpsilon = data.getSystemSetting().distanceEpsilon;
            while (Vector3.Distance(d, target) > 1.5f * distanceEpsilon)
            {
                yield return new WaitForSeconds(1.0f);
                d = g.transform.position;
            }
            #if use2D
            if(data.GetControlMode()==controlMode.control_2D)
            {
                var v = turnTool.get(turn);
                ((ActorMoveInterface2D)m).Rotating(v.x,v.y,true);
            }
#endif

            pushNow(true);
            changeCanDo(true);
        }

        //=====================不卡进程函数===============================
        /* 不推进流程，同一个序号里可以有多个 */

        //移动物体，不卡进程
        protected void changeThingPos(GameObject g, Transform t, string step = "", string runStep = "")
        {
            if (t != null)
            {
                g.transform.position = new Vector3(t.transform.position.x, t.transform.position.y, g.transform.position.z);
                Vector3 v = new Vector3(t.position.x, t.position.y);
                changeThingPos(g, v, step, runStep);
            }
        }

        protected void changeThingPos(GameObject g, Vector3 t, string step = "", string runStep = "")
        {
            g.transform.position = new Vector3(t.x, t.y, g.transform.position.z);
            if (g.tag == HashsAndTags.player)
            {
                ActorInterface a = g.GetComponent<ActorInterface>();
                if (a != null)
                {
                    var m = a.getMove();
                    #if use2D
                    if (data.GetControlMode() == controlMode.control_2D)
                    {
                        ActorMoveInterface2D _m = (ActorMoveInterface2D)m;
                        turn e = _m.getTurn();
                        var v = turnTool.get(e);
                        _m.Rotating(v.x,v.y,true);
                        ActorInterface2D _a = (ActorInterface2D)a;
                        _a.changeFollowsPos(e, t, step, runStep);
                    }
#endif
                    m.Move(Vector3.zero);
                    if (step != null)
                    {
                        m.setStep(step);
                        m.setRunStep(runStep);
                    }                   
                }

            }
        }

        //控制给定的角色开始移动，null则使用原本角色，不卡进程
        protected void startUserControl(GameObject _player = null)
        {
            a.startUserControl(_player);
            if (_player == null)
            {
                a.Player = _player;
                var c = Camera.main.GetComponent<CameraBase>();
                if(c!=null)
                    c.player = a.Player;
            }
        }

        //设置人物朝向，不卡进程
        protected void setActorTurn2D(GameObject _player, turn _turn)
        {
            if (data.GetControlMode() != controlMode.control_2D)
                return;

            #if use2D
            var p = _player.GetComponent<ActorInterface2D>();
            if (p == null)
            {
                p = _player.transform.GetChild(0).gameObject.GetComponent<ActorInterface2D>();
                if (p == null)
                    return;
            }
            ActorMoveInterface2D m = (ActorMoveInterface2D)p.getMove();

            Vector2Int v;
            if (_turn == turn.all)
            {
                v = turnTool.get(m.getTurn());
            }
            else
            {
                v = turnTool.get(_turn);
            }

            m.Rotating(v.x, v.y,true);
            m.Move(Vector3.zero);
#endif
        }

        protected void setActorTurn3D(GameObject _player, float h_,float v_)
        {
            if (data.GetControlMode() == controlMode.control_2D)
                return;

            var p = _player.GetComponent<ActorInterface>();
            if (p == null)
            {
                p = _player.transform.GetChild(0).gameObject.GetComponent<ActorInterface>();
                if (p == null)
                    return;
            }
            ActorMoveInterface m = p.getMove();

            m.Rotating(h_, v_,true);
            m.Move(Vector3.zero);
        }

        protected void setFollow(GameObject g, bool follow)
        {
            var p = a.Player.GetComponent<ActorInterface>();
            if (p != null)
            {
                #if use2D
                if (data.GetControlMode() == controlMode.control_2D)
                {
                    ActorInterface2D _p = (ActorInterface2D)p;
                    if (follow)
                    {
                        _p.addFollow(g);
                    }
                    else
                    {
                        _p.removeFollow(g);
                    }
                }
#endif
            }
        }

        //给某个物体设置某个触发器，不卡进程
        protected void setTrigger(GameObject _object, string hash)
        {
            Animator anima = _object.GetComponent<Animator>();
            anima.SetTrigger(hash);
        }

        //给某个物体设置某个开关，不卡进程
        protected void setBool(GameObject _object, string hash, bool value)
        {
            Animator anima = _object.GetComponent<Animator>();
            anima.SetBool(hash, value);
        }

        protected GameObject spawnPrefab(string name, Vector3 pos)
        {
            var g = findPrefab(name);
            GameObject go = Instantiate(g, pos, g.transform.rotation) as GameObject;

            return go;
        }
    }
}
