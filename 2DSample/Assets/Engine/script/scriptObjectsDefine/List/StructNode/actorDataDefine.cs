using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BaseData
{
    // 事件基类
    [System.Serializable]
    public class actorComponentData : myListStructNode
    {
        public actorComponentType type;
        public actorComponentType _type
        {
            get
            {
                return type;
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
        public bool bool3;
        public bool _bool3
        {
            get
            {
                return bool3;
            }
        }
        public bool bool4;
        public bool _bool4
        {
            get
            {
                return bool4;
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
        public float float3;
        public float _float3
        {
            get
            {
                return float3;
            }
        }
        public float float4;
        public float _float4
        {
            get
            {
                return float4;
            }
        }

        public Sprite img1;
        public Sprite _img1
        {
            get
            {
                return img1;
            }
        }

        public Sprite img2;
        public Sprite _img2
        {
            get
            {
                return img2;
            }
        }
        public Sprite img3;
        public Sprite _img3
        {
            get
            {
                return img3;
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


        public actorComponentData()
        {
            bool1 = false;
            bool2 = false;
            bool3 = false;
            bool4 = false;
        }

        public static string dataTypeToName(actorComponentType _type)
        {
            string t = string.Empty;
            switch (_type)
            {
                case actorComponentType.basicMove: t = "基础移动"; break;
                case actorComponentType.basicBattle: t = "基础战斗"; break;
                case actorComponentType.followBehaviour: t = "跟随组件"; break;
                default: t = "错误，未找到"; break;
            }
            return t;
        }
    }

    [CreateAssetMenu]
    public class actorDataDefine : myList<actorComponentData>
    {
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(actorDataDefine))]
    public class actorDataInspector : myListInspector<actorComponentData>
    {
        protected override void showAddBtn(List<actorComponentData> list)
        {
            if (GUILayout.Button("添加组件"))
            {
                var window = EditorWindow.GetWindow(typeof(ActorDataTypeWin), true, "请选择组件类型") as ActorDataTypeWin;
                window.maxSize = new Vector2(400, window.maxSize.y);
                window.e = delegate (int t)
                {
                    var a = new actorComponentData();
                    a.type = (actorComponentType)t;
                    list.Add(a);
                };
                window.Show();
            }
        }

        protected override void drawInspector(int i, actorComponentData e)
        {

            e.type = (actorComponentType)EditorGUILayout.EnumPopup(actorComponentData.dataTypeToName(e.type), e.type);
            if (e.type == actorComponentType.basicMove)
            {
                e.float1 = EditorGUILayout.FloatField("行走速度", e.float1);
                e.str1 = EditorGUILayout.TextField("行走音效", e.str1);
                e.bool1 = EditorGUILayout.Toggle("可以奔跑", e.bool1);
                if (e.bool1)
                {
                    e.float2 = EditorGUILayout.FloatField("奔跑速度", e.float2);
                    e.str2 = EditorGUILayout.TextField("奔跑音效", e.str2);
                }
                e.bool2 = EditorGUILayout.Toggle("可以冲刺", e.bool2);
                if (e.bool2)
                {
                    e.float3 = EditorGUILayout.FloatField("冲刺距离", e.float3);
                    e.float4 = EditorGUILayout.FloatField("冲刺时间", e.float4);
                }
                e.bool3 = EditorGUILayout.Toggle("采用静止动画", e.bool3);
                if (!e.bool3)
                {
                    e.img1 = (Sprite)EditorGUILayout.ObjectField("静止图像-左", e.img1, typeof(Sprite));
                    e.img2 = (Sprite)EditorGUILayout.ObjectField("静止图像-上", e.img2, typeof(Sprite));
                    e.img3 = (Sprite)EditorGUILayout.ObjectField("静止图像-下", e.img3, typeof(Sprite));
                }
                e.bool4 = EditorGUILayout.Toggle("动画素材默认向左", e.bool4);
                e.turn = (turn)EditorGUILayout.EnumPopup("默认朝向", e.turn);
            }
            else if (e.type == actorComponentType.basicBattle)
            {

            }
            else if (e.type == actorComponentType.followBehaviour)
            {
                e.bool1 = EditorGUILayout.Toggle("可以奔跑跟随", e.bool1);
            }
        }
    }
#endif
}

