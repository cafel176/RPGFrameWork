using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BaseData
{

    [System.Serializable]
    public class itemDictionary : myListStructNode
    {
        public itemType type;
        public itemType _type
        {
            get
            {
                return type;
            }
        }
        // 使用次数，-2表示无限，-1表示不可使用
        public int times;
        public int _times
        {
            get
            {
                return times;
            }
        }
        public List<string> commonEvent;
        public List<string> _commonEvent
        {
            get
            {
                return commonEvent;
            }
        }
        public int eventNum;
        public int _eventNum
        {
            get
            {
                return eventNum; 
            }
        }
    }

    [CreateAssetMenu]
    public class itemListDefine : myList<itemDictionary>
    {
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(itemListDefine))]
    public class itemInspector : myListInspector<itemDictionary>
    {
        protected override void drawInspector(int i, itemDictionary e)
        {

            e.id = EditorGUILayout.TextField("道具id", e.id);
            e.type = (itemType)EditorGUILayout.EnumPopup("道具类型", e.type);
            e.name = EditorGUILayout.TextField("道具名", e.name);
            e.text = EditorGUILayout.TextField("描述文字", e.text);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("图片");
            e.img = (Sprite)EditorGUILayout.ObjectField(e.img, typeof(Sprite));
            EditorGUILayout.EndHorizontal();
            e.times = EditorGUILayout.IntField("使用次数", e.times);

            EditorGUILayout.BeginHorizontal();
            e.eventNum = EditorGUILayout.IntField("事件总数:", e.eventNum);
            bool b = GUILayout.Button("确定");
            EditorGUILayout.EndHorizontal();

            int num = 0;
            if (e.commonEvent == null)
                e.commonEvent = new List<string>();

            num = e.commonEvent.Count;
            if (b)
            {
                if (e.eventNum > num)
                {
                    for (int w = num; w < e.eventNum; w++)
                    {
                        e.commonEvent.Add("");
                    }
                }
                else
                {
                    for (int w = e.eventNum; w < num;)
                    {
                        e.commonEvent.RemoveAt(w);
                        num = e.commonEvent.Count;
                    }
                }
            }

            EditorGUILayout.BeginVertical();
            for (int w = 0; w < num; w++)
            {
                e.commonEvent[w] = EditorGUILayout.TextField("公共事件 " + w, e.commonEvent[w]);
            }
            EditorGUILayout.EndVertical();
        }
    }
#endif
}
