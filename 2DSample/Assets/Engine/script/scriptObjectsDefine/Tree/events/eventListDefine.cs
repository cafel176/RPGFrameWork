using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

namespace BaseData
{
    // 天气
    public enum weather
    {
        sunny,
        rain,
        snow
    }

    // 事件基类
    [System.Serializable]
    public class myEvent : myTreeNode
    {
        public Transform helper;

        public eventType type;
        public eventType _type
        {
            get
            {
                return type;
            }
        }
        public calValue howToCal;
        public calValue _howToCal
        {
            get
            {
                return howToCal;
            }
        }
        public IndependentSwitch independentSwitch;
        public IndependentSwitch _independentSwitch
        {
            get
            {
                return independentSwitch;
            }
        }
        public compare compare;
        public compare _compare
        {
            get
            {
                return compare;
            }
        }
        public turn turn;
        public turn _turn
        {
            get
            {
                return turn;
            }
        }
        public weather weather;
        public weather _weather
        {
            get
            {
                return weather;
            }
        }
        public findType findType;
        public findType _findType
        {
            get
            {
                return findType;
            }
        }
        public questStatus questStatus;
        public questStatus _questStatus
        {
            get
            {
                return questStatus;
            }
        }
        public conditionType conditionType;
        public conditionType _conditionType
        {
            get
            {
                return conditionType;
            }
        }

        //设置名字和文本
        public string str1;
        public string _str1
        {
            get
            {
                return str1;
            }
        }
        public string str2;
        public string _str2
        {
            get
            {
                return str2;
            }
        }
        public string str3;
        public string _str3
        {
            get
            {
                return str3;
            }
        }
        public string str4;
        public string _str4
        {
            get
            {
                return str4;
            }
        }

        public bool bool1;
        public bool _bool1
        {
            get
            {
                return bool1;
            }
        }
        public bool bool2;
        public bool _bool2
        {
            get
            {
                return bool2;
            }
        }

        public float float1;
        public float _float1
        {
            get
            {
                return float1;
            }
        }
        public float float2;
        public float _float2
        {
            get
            {
                return float2;
            }
        }

        public int int1;
        public int _int1
        {
            get
            {
                return int1;
            }
        }

        public Vector3 vec1;
        public Vector3 _vec1
        {
            get
            {
                return vec1;
            }
        }
        public Vector3 vec2;
        public Vector3 _vec2
        {
            get
            {
                return vec2;
            }
        }

        public Color color1;
        public Color _color1
        {
            get
            {
                return color1;
            }
        }

        public myEvent()
        {
            type = eventType.none;
            bool1 = false;
            bool2 = false;
        }

        public static string eventTypeToName(eventType _type)
        {
            string t = string.Empty;
            switch (_type)
            {
                case eventType.text: t = "显示文本框文字"; break;
                case eventType.textSpecial: t = "显示特殊文字"; break;
                case eventType.beBlack: t = "黑屏"; break;
                case eventType.beWhite: t = "白屏"; break;
                case eventType.changeGlobalVector3: t = "3维向量操作"; break;
                case eventType.changeGlobalDouble: t = "浮点变量操作"; break;
                case eventType.changeGlobalInt: t = "整数变量操作"; break;
                case eventType.changeGlobalSwith: t = "开关操作"; break;
                case eventType.changeIndependentSwitch: t = "独立开关操作"; break;
                case eventType.changeScene: t = "场景移动"; break;
                case eventType.commonEvent: t = "公共事件"; break;
                case eventType.debugLog: t = "控制台输出"; break;
                case eventType.hidePic: t = "隐藏图片"; break;
                case eventType.movePic: t = "移动图片"; break;
                case eventType.playBgm: t = "播放BGM"; break;
                case eventType.playBgs: t = "播放BGS"; break;
                case eventType.playSE: t = "播放SE"; break;
                case eventType.showPic: t = "显示图片"; break;
                case eventType.startUserControl: t = "开启角色控制"; break;
                case eventType.stopBgm: t = "停止BGM"; break;
                case eventType.stopBgs: t = "停止BGS"; break;
                case eventType.stopUserControl: t = "关闭角色控制"; break;
                case eventType.wait: t = "等待"; break;
                case eventType.playAnima: t = "播放动画"; break;
                case eventType.moveCamera: t = "移动相机"; break;
                case eventType.sbMoveToSw: t = "设置移动路线"; break;
                case eventType.flashScreen: t = "闪烁屏幕"; break;
                case eventType.shakeScreen: t = "震动屏幕"; break;
                case eventType.changeWeather: t = "改变天气"; break;
                case eventType.showSavePanel: t = "打开存档界面"; break;
                case eventType.changeItem: t = "增减道具"; break;
                case eventType.changeThingPos: t = "设置物体位置"; break;
                case eventType.changeTurn: t = "设置人物朝向"; break;
                case eventType.changeThingActive: t = "设置物体可见性"; break;
                case eventType.startQuest: t = "任务开始"; break;
                case eventType.changeQuest: t = "更改任务状态"; break;
                case eventType.If: t = "如果"; break;
                case eventType.setFollow: t = "设置跟随"; break;
                case eventType.changeTeam: t = "队伍变更"; break;
                default: t = "错误，未找到"; break;
            }
            return t;
        }
    }

    // 事件列表
    [CreateAssetMenu]
    public class eventListDefine : myTree<myEvent>, EventListInterface
    {
        // float误差
        [SerializeField]
        private static float floatEpsilon = 1e-3f;

        // 事件页条件
        public conditionDictionary[] conditions;

        // 事件页触发方式
        [SerializeField]
        private start howToStart;

        // 独立开关
        private bool[] independentSwitchs = new bool[] { false, false, false, false,};

        public void setIndependentSwitchs(bool[] ind)
        {
            if(ind!=null)
                independentSwitchs = ind;
        }

        public bool[] getIndependentSwitchs()
        {
            return independentSwitchs;
        }

        public start getHowToStart()
        {
            return howToStart;
        }

        // 检查条件是否满足
        public bool checkEventConditions()
        {
            bool checkBool = true;
            for (int i = 0; i < conditions.Length; i++)
            {
                bool b = checkCondition(conditions[i], independentSwitchs);

                if (conditions[i].thisConnect == connect.or)
                    checkBool = (checkBool || b);
                else
                    checkBool = (checkBool && b);
            }
            return checkBool;
        }

        public bool checkCondition(conditionDictionary condition, bool[] independentSwitchs)
        {
            bool b = true;
            var MI = MIFactory.getMI();

            if (condition.type == conditionType.Switch)
            {
                bool a = MI.getSwitch(condition.name);
                if (condition.compare == compare.equal)
                {
                    if (Convert.ToBoolean(condition.value) == a)
                        b = true;
                    else
                        b = false;
                }
                else
                {
                    if (Convert.ToBoolean(condition.value) != a)
                        b = true;
                    else
                        b = false;
                }
            }
            else if (condition.type == conditionType.IndependentSwitch)
            {
                bool a = false;
                switch (condition.name.ToUpper())
                {
                    case "A": a = independentSwitchs[0]; break;
                    case "B": a = independentSwitchs[1]; break;
                    case "C": a = independentSwitchs[2]; break;
                    case "D": a = independentSwitchs[3]; break;
                    default: break;
                }
                if (condition.compare == compare.equal)
                {
                    if (Convert.ToBoolean(condition.value) == a)
                        b = true;
                    else
                        b = false;
                }
                else
                {
                    if (Convert.ToBoolean(condition.value) != a)
                        b = true;
                    else
                        b = false;
                }
            }
            else if (condition.type == conditionType.Int)
            {
                int a = MI.getInt(condition.name);
                if (condition.compare == compare.equal)
                {
                    if (Convert.ToInt32(condition.value) == a)
                        b = true;
                    else
                        b = false;
                }
                else if (condition.compare == compare.larger)
                {
                    if (a > Convert.ToInt32(condition.value))
                        b = true;
                    else
                        b = false;
                }
                else if (condition.compare == compare.less)
                {
                    if (a < Convert.ToInt32(condition.value))
                        b = true;
                    else
                        b = false;
                }
                else
                {
                    if (Convert.ToInt32(condition.value) != a)
                        b = true;
                    else
                        b = false;
                }
            }
            else
            {
                double a = MI.getDouble(condition.name);
                if (condition.compare == compare.equal)
                {
                    if (Math.Abs(Convert.ToDouble(condition.value) - a) < floatEpsilon)
                        b = true;
                    else
                        b = false;
                }
                else if (condition.compare == compare.larger)
                {
                    if (a > Convert.ToDouble(condition.value))
                        b = true;
                    else
                        b = false;
                }
                else if (condition.compare == compare.less)
                {
                    if (a < Convert.ToDouble(condition.value))
                        b = true;
                    else
                        b = false;
                }
                else
                {
                    if (Math.Abs(Convert.ToDouble(condition.value) - a) > floatEpsilon)
                        b = true;
                    else
                        b = false;
                }
            }

            return b;
        }

    }

#if UNITY_EDITOR
    [CustomEditor(typeof(eventListDefine))]
    public class eventInspector : myTreeInspector<myEvent>
    {
        protected override void showRemoveBtn(myEvent e, string id)
        {
            if (e.childrenID.Count > 0)
            {
                base.showRemoveBtn(e, id);
            }
        }

        protected override void showAddBtn(myEvent e,string id)
        {
            if (e.childrenID.Count < 2)
            {
                if (GUILayout.Button("添加子节点"))
                {
                    if (!string.IsNullOrEmpty(id))
                    {
                        if (e.id != id && !checkIdIsParent(e, id))
                        {
                            string w = id;
                            myEvent t = checkIdExist(w);
                            if (t == null)
                            {
                                var window = EditorWindow.GetWindow(typeof(ChooseEventTypeWin), true, "请选择事件类型") as ChooseEventTypeWin;
                                window.maxSize = new Vector2(400, window.maxSize.y);

                                window.e = delegate (int type)
                                {
                                    t = new myEvent();
                                    t.id = w;
                                    t.type = (eventType)type;

                                    tree.nodes.Add(t);
                                    t.parentsID.Add(e.id);
                                    e.childrenID.Add(t.id);

                                    window.Close();
                                };
                                window.Show();
                            }
                            else
                            {
                                t.parentsID.Add(e.id);
                                e.childrenID.Add(t.id);
                            }
                        }
                        else
                        {
                            Debug.LogError("id不能与本或父节点相同！");
                        }
                    }
                    else
                    {
                        Debug.LogError("id不能为空！");
                    }
                }
            }
        }

        protected override void drawInspector(myEvent e)
        {

            e.type = (eventType)EditorGUILayout.EnumPopup(myEvent.eventTypeToName(e.type), e.type);
            if (e.type == eventType.text)
            {
                e.str1 = EditorGUILayout.TextField("名字", e.str1);
                e.str2 = EditorGUILayout.TextField("文本", e.str2);
                e.str3 = EditorGUILayout.TextField("立绘", e.str3);
                e.bool1 = EditorGUILayout.Toggle("是否显示立绘", e.bool1);
                e.str4 = EditorGUILayout.TextField("文字音效", e.str4);
            }
            else if (e.type == eventType.textSpecial)// 会覆盖黑屏白屏
            {
                e.str1 = EditorGUILayout.TextField("文本", e.str1);
                e.vec1 = EditorGUILayout.Vector2Field("位置坐标", e.vec1);
                EditorGUILayout.BeginHorizontal();
                e.helper = (Transform)EditorGUILayout.ObjectField(e.helper, typeof(Transform));
                if (GUILayout.Button("获取"))
                {
                    e.vec1 = e.helper.position;
                    e.helper = null;
                }
                EditorGUILayout.EndHorizontal();

                e.str2 = EditorGUILayout.TextField("文字音效", e.str2);
            }
            else if (e.type == eventType.beBlack || e.type == eventType.beWhite || e.type == eventType.wait)
            {
                e.float1 = EditorGUILayout.FloatField("时间", e.float1);
            }
            else if (e.type == eventType.changeGlobalInt)
            {
                e.str1 = EditorGUILayout.TextField("变量名", e.str1);
                e.howToCal = (calValue)EditorGUILayout.EnumPopup("计算方式", e.howToCal);
                e.int1 = EditorGUILayout.IntField("值", e.int1);
            }
            else if (e.type == eventType.changeGlobalDouble)
            {
                e.str1 = EditorGUILayout.TextField("变量名", e.str1);
                e.howToCal = (calValue)EditorGUILayout.EnumPopup("计算方式", e.howToCal);
                e.float1 = EditorGUILayout.FloatField("值", e.float1);
            }
            else if (e.type == eventType.changeGlobalVector3)
            {
                e.str1 = EditorGUILayout.TextField("变量名", e.str1);
                e.howToCal = (calValue)EditorGUILayout.EnumPopup("计算方式", e.howToCal);
                e.vec1 = EditorGUILayout.Vector3Field("值", e.vec1);
            }
            else if (e.type == eventType.changeGlobalSwith)
            {
                e.str1 = EditorGUILayout.TextField("变量名", e.str1);
                e.bool1 = EditorGUILayout.Toggle("值", e.bool1);
            }
            else if (e.type == eventType.debugLog)
            {
                e.str1 = EditorGUILayout.TextField("文本", e.str1);
            }
            else if (e.type == eventType.playBgm)
            {
                e.str1 = EditorGUILayout.TextField("BGM名称", e.str1);
            }
            else if (e.type == eventType.playBgs)
            {
                e.str1 = EditorGUILayout.TextField("BGS名称", e.str1);
            }
            else if (e.type == eventType.playSE)
            {
                e.str1 = EditorGUILayout.TextField("SE名称", e.str1);
            }
            else if (e.type == eventType.stopBgm)
            {
                e.float1 = EditorGUILayout.FloatField("时间", e.float1);
            }
            else if (e.type == eventType.stopBgs)
            {
                e.float1 = EditorGUILayout.FloatField("时间", e.float1);
            }
            else if (e.type == eventType.showPic)
            {
                e.str1 = EditorGUILayout.TextField("图片编号", e.str1);
                e.str2 = EditorGUILayout.TextField("图片名", e.str2);
                e.vec1 = EditorGUILayout.Vector2Field("位置坐标", e.vec1);
                EditorGUILayout.BeginHorizontal();
                e.helper = (Transform)EditorGUILayout.ObjectField(e.helper, typeof(Transform));
                if (GUILayout.Button("获取"))
                {
                    e.vec1 = e.helper.position;
                    e.helper = null;
                }
                EditorGUILayout.EndHorizontal();

                e.float1 = EditorGUILayout.FloatField("透明度", e.float1);
            }
            else if (e.type == eventType.movePic)
            {
                e.str1 = EditorGUILayout.TextField("图片编号", e.str1);

                e.vec1 = EditorGUILayout.Vector2Field("移动目标位置", e.vec1);
                EditorGUILayout.BeginHorizontal();
                e.helper = (Transform)EditorGUILayout.ObjectField(e.helper, typeof(Transform));
                if (GUILayout.Button("获取"))
                {
                    e.vec1 = e.helper.position;
                    e.helper = null;
                }
                EditorGUILayout.EndHorizontal();

                e.float1 = EditorGUILayout.FloatField("透明度", e.float1);
                e.float2 = EditorGUILayout.FloatField("时间", e.float2);
                e.bool1 = EditorGUILayout.Toggle("卡进程", e.bool1);
            }
            else if (e.type == eventType.hidePic)
            {
                e.str1 = EditorGUILayout.TextField("图片编号", e.str1);
            }
            else if (e.type == eventType.changeScene)
            {
                e.str1 = EditorGUILayout.TextField("场景名", e.str1);
                e.vec1 = EditorGUILayout.Vector2Field("位置", e.vec1);
                EditorGUILayout.BeginHorizontal();
                e.helper = (Transform)EditorGUILayout.ObjectField(e.helper, typeof(Transform));
                if (GUILayout.Button("获取"))
                {
                    e.vec1 = e.helper.position;
                    e.helper = null;
                }
                EditorGUILayout.EndHorizontal();

                e.turn = (turn)EditorGUILayout.EnumPopup("新的朝向", e.turn);
            }
            else if (e.type == eventType.changeIndependentSwitch)
            {
                e.independentSwitch = (IndependentSwitch)EditorGUILayout.EnumPopup("编号", e.independentSwitch);
                e.bool1 = EditorGUILayout.Toggle("开关", e.bool1);
            }
            else if (e.type == eventType.playAnima)
            {
                e.str1 = EditorGUILayout.TextField("动画名", e.str1);

                e.vec1 = EditorGUILayout.Vector2Field("位置", e.vec1);
                EditorGUILayout.BeginHorizontal();
                e.helper = (Transform)EditorGUILayout.ObjectField(e.helper, typeof(Transform));
                if (GUILayout.Button("获取"))
                {
                    e.vec1 = e.helper.position;
                    e.helper = null;
                }
                EditorGUILayout.EndHorizontal();

                e.bool1 = EditorGUILayout.Toggle("卡进程", e.bool1);
            }
            else if (e.type == eventType.moveCamera)
            {
                e.vec1 = EditorGUILayout.Vector2Field("移动目标位置", e.vec1);
                EditorGUILayout.BeginHorizontal();
                e.helper = (Transform)EditorGUILayout.ObjectField(e.helper, typeof(Transform));
                if (GUILayout.Button("获取"))
                {
                    e.vec1 = e.helper.position;
                    e.helper = null;
                }
                EditorGUILayout.EndHorizontal();

                e.float1 = EditorGUILayout.FloatField("时间", e.float1);
            }
            else if (e.type == eventType.sbMoveToSw)
            {
                e.findType = (findType)EditorGUILayout.EnumPopup("查找类型", e.findType);
                e.str1 = EditorGUILayout.TextField("文本", e.str1);
                e.vec1 = EditorGUILayout.Vector2Field("移动目标位置", e.vec1);
                EditorGUILayout.BeginHorizontal();
                e.helper = (Transform)EditorGUILayout.ObjectField(e.helper, typeof(Transform));
                if (GUILayout.Button("获取"))
                {
                    e.vec1 = e.helper.position;
                    e.helper = null;
                }
                EditorGUILayout.EndHorizontal();

                e.turn = (turn)EditorGUILayout.EnumPopup("新的朝向", e.turn);
            }
            else if (e.type == eventType.changeTurn)
            {
                e.findType = (findType)EditorGUILayout.EnumPopup("查找类型", e.findType);
                e.str1 = EditorGUILayout.TextField("文本", e.str1);
                e.turn = (turn)EditorGUILayout.EnumPopup("新的朝向", e.turn);
            }
            else if (e.type == eventType.flashScreen)
            {
                e.color1 = EditorGUILayout.ColorField("颜色", e.color1);
                e.float1 = EditorGUILayout.FloatField("时间", e.float1);
                e.bool1 = EditorGUILayout.Toggle("卡进程", e.bool1);
            }
            else if (e.type == eventType.shakeScreen)
            {
                e.float1 = EditorGUILayout.FloatField("时间", e.float1);
                e.float2 = EditorGUILayout.FloatField("强度", e.float2);
                e.bool1 = EditorGUILayout.Toggle("卡进程", e.bool1);
            }
            else if (e.type == eventType.changeWeather)
            {
                e.weather = (weather)EditorGUILayout.EnumPopup("天气", e.weather);
                e.float1 = EditorGUILayout.FloatField("强度", e.float1);
            }
            else if (e.type == eventType.changeItem)
            {
                e.str1 = EditorGUILayout.TextField("道具id", e.str1);
                e.int1 = EditorGUILayout.IntField("数量", e.int1);
            }
            else if (e.type == eventType.changeThingPos)
            {
                e.findType = (findType)EditorGUILayout.EnumPopup("查找类型", e.findType);
                e.str1 = EditorGUILayout.TextField("文本", e.str1);
                e.vec1 = EditorGUILayout.Vector2Field("设置位置", e.vec1);
                EditorGUILayout.BeginHorizontal();
                e.helper = (Transform)EditorGUILayout.ObjectField(e.helper, typeof(Transform));
                if (GUILayout.Button("获取"))
                {
                    e.vec1 = e.helper.position;
                    e.helper = null;
                }
                EditorGUILayout.EndHorizontal();
            }
            else if (e.type == eventType.changeThingActive)
            {
                e.findType = (findType)EditorGUILayout.EnumPopup("查找类型", e.findType);
                e.str1 = EditorGUILayout.TextField("文本", e.str1);
                e.bool1 = EditorGUILayout.Toggle("可见性", e.bool1);
            }
            else if (e.type == eventType.startQuest)
            {
                e.str1 = EditorGUILayout.TextField("任务id", e.str1);
            }
            else if (e.type == eventType.changeQuest)
            {
                e.str1 = EditorGUILayout.TextField("任务id", e.str1);
                e.questStatus = (questStatus)EditorGUILayout.EnumPopup("任务状态", e.questStatus);
            }
            else if (e.type == eventType.If)
            {
                EditorGUILayout.LabelField("条件: ");
                e.conditionType = (conditionType)EditorGUILayout.EnumPopup("条件类型", e.conditionType);
                e.str1 = EditorGUILayout.TextField("变量名", e.str1);
                e.compare = (compare)EditorGUILayout.EnumPopup("比较类型", e.compare);
                e.str2 = EditorGUILayout.TextField("值", e.str2);
                EditorGUILayout.LabelField("分支: ");
                e.str3 = EditorGUILayout.TextField("符合，子节点id", e.str3);
                e.str4 = EditorGUILayout.TextField("不符合，子节点id", e.str4);
            }
            else if (e.type == eventType.setFollow)
            {
                e.findType = (findType)EditorGUILayout.EnumPopup("查找类型", e.findType);
                e.str1 = EditorGUILayout.TextField("文本", e.str1);
                e.int1 = EditorGUILayout.IntField("对应PlayerPrefab编号", e.int1);
                e.bool1 = EditorGUILayout.Toggle("是否跟随", e.bool1);
            }
            else if (e.type == eventType.changeTeam)
            {
                e.int1 = EditorGUILayout.IntField("角色编号", e.int1);
                e.bool1 = EditorGUILayout.Toggle("入队出队", e.bool1);
            }
            else if (e.type == eventType.commonEvent)
            {
                e.str1 = EditorGUILayout.TextField("事件名", e.str1);
            }
        }
    }
#endif

}