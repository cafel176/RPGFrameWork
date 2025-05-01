using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace BaseData
{
    public abstract class myListNode : ListNode
    {
        public void toInterface()
        {
            node = new DataNode(this);
        }
    }

    // name为搜索主键
    public class myListVarNode<T> : myListNode
    {
        public T value;
        public T _value
        {
            get
            {
                return value;
            }
        }
    }

    // id为搜索主键
    public class myListStructNode : myListNode
    {
        public string name;
        public string _name
        {
            get
            {
                return name;
            }
        }

        public string text;
        public string _text
        {
            get
            {
                return text;
            }
        }

        public Sprite img;
        public Sprite _img
        {
            get
            {
                return img;
            }
        }
    }

    public abstract class myList<R> : dataList where R : myListNode
    {
        public string id;
        [HideInInspector]
        public List<R> datalist;

        public myList()
        {
            datalist = new List<R>();
            toInterfaceFunc = toInterface;
        }

        public R getNode(string id)
        {
            for (int i = 0; i < list.Count; i++)
            {
                R t = (R)list[i];
                if (t.id == id)
                    return t;
            }
            return null;
        }

        public void toInterface()
        {
            list.Clear();
            for (int i = 0;i<datalist.Count;i++)
            {
                list.Add(datalist[i]);
                ((R)list[i]).toInterface();
            }              
        }
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(myList<>))]
    public abstract class myListInspector<R> : Editor where R : myListNode, new()
    {
        protected int index = 0;
        protected int showPage = 1;
        protected int showNum = 50;

        protected virtual void showAddBtn(List<R> list)
        {
            if (GUILayout.Button("添加元素"))
            {
                list.Insert(0, new R());
            }
        }

        protected virtual void showRemoveBtn(List<R> list)
        {
            if (GUILayout.Button("确认删除"))
            {
                if (index >= 0 && index < list.Count)
                    list.RemoveAt(index);
            }
        }

        protected virtual void drawParam(myList<R> e)
        {
            
        }

        protected abstract void drawInspector(int i, R e);

        public override void OnInspectorGUI()
        {
            // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
            serializedObject.Update();

            base.OnInspectorGUI();

            myList<R> data = (myList<R>)target;
            var list = data.datalist;

            drawParam(data);
            GUILayout.Space(20);
            showAddBtn(list);
            EditorGUILayout.BeginHorizontal();
            index = EditorGUILayout.IntField("删除元素，编号: ", index);
            showRemoveBtn(list);
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(20);

            GUILayout.Label("编辑列表，元素总数: " + list.Count + "  " + data.getList().Count);
            int num = list.Count > showPage * showNum ? showPage * showNum : list.Count;
            for (int i = 0; i < num; i++)
            {
                GUILayout.Space(10);
                var e = list[i];
                if (e != null)
                {
                    GUILayout.Label("元素 " + i + ":");
                    drawInspector(i,e);
                }
            }

            if (list.Count > showPage * showNum)
            {
                GUILayout.Space(10);
                if (GUILayout.Button("显示更多"))
                {
                    showPage++;
                    OnInspectorGUI();
                    return;
                }
            }

       
            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }

            // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
            serializedObject.ApplyModifiedProperties();
        }
    }

#endif
}
