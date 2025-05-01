using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//注意，ScriptableObject的数据会在编译后自动清空

namespace BaseData
{
    // id为搜索主键
    public class myTreeNode : TreeNode
    {
        public List<string> childrenID = new List<string>();
        public List<string> parentsID = new List<string>();

        public void toInterface()
        {
            node = new DataNode(this);                
        }
    }

    //树模板
    public abstract class myTree<T> : dataTree where T : myTreeNode, new()
    {
        public string id;

        // 本质是利用public 的list参数会被自动保存，故数据结构只能采用public list
        [HideInInspector]
        public List<T> nodes;

        public myTree()
        {
            T t = new T();
            t.id = "root";
            nodes = new List<T>();
            nodes.Add(t);

            toInterface();

            toInterfaceFunc = toInterface;
        }

        public T getById(string id)
        {
            for(int i=0;i<nodes.Count;i++)
            {
                if (nodes[i].id == id)
                    return nodes[i];
            }
            return null;
        }

        public void removeById(string id)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].id == id)
                    nodes.RemoveAt(i);
            }
        }

        public void toInterface()
        {
            for(int i=0;i< nodes.Count;i++)
            {
                for(int j=0;j< nodes[i].childrenID.Count;j++)
                {
                    nodes[i].addChild(getById(nodes[i].childrenID[j]));
                    getById(nodes[i].childrenID[j]).addParent(nodes[i]);
                }
                nodes[i].toInterface();
            }

            root = nodes[0];
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(myTree<>))]
    public abstract class myTreeInspector<T> : Editor where T : myTreeNode, new()
    {
        protected Editor cacheEditor;
        protected string index = "";
        protected myTree<T> tree;

        protected bool checkIdIsParent(T t, string id)
        {
            var list = t.parentsID;
            bool re = false;
            foreach(var i in list)
            {
                if (i == id)
                    return true;
                else
                    re = (re || checkIdIsParent(tree.getById(i), id));
            }
            return re;
        }

        protected T checkIdExist(string id)
        {
            return tree.getById(id);
        }

        protected abstract void drawInspector(T e);

        protected virtual void showRemoveBtn(T e, string index)
        {
            if(e.childrenID.Count>0)
            {
                if (GUILayout.Button("删除子节点"))
                {
                    if (!string.IsNullOrEmpty(index))
                    {                       
                        if(e.childrenID.Contains(index))
                        {
                            var child = tree.getById(index);
                            if (child.parentsID.Count == 1)
                            {
                                var list = child.childrenID;
                                for (int i = 0; i < list.Count; i++)
                                {
                                    tree.getById(list[i]).parentsID.Add(e.id);
                                    tree.getById(e.id).childrenID.Add(list[i]);
                                    tree.getById(list[i]).parentsID.Remove(child.id);
                                }
                                tree.removeById(index);
                            }

                            tree.getById(e.id).childrenID.Remove(index);
                            var n = tree.getById(index);
                            if(n!=null)
                                n.parentsID.Remove(e.id);
                        }
                    }
                    else
                    {
                        Debug.LogError("id不能为空！");
                    }
                }
            }
        }

        protected virtual void showAddBtn(T e,string id)
        {
            if (GUILayout.Button("添加子节点"))
            {
                if (!string.IsNullOrEmpty(id))
                {
                    if(e.id!=id && !checkIdIsParent(e, id))
                    {
                        T t = checkIdExist(id);
                        if (t == null)
                        {
                            t = new T();
                            tree.nodes.Add(t);
                        }

                        t.parentsID.Add(e.id);
                        e.childrenID.Add(t.id);

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

        protected virtual void showEditWin()
        {
            if (GUILayout.Button("查看树结构"))
            {
                var window = EditorWindow.GetWindow(typeof(EditTreeStructWin<T>), true, "树结构") as EditTreeStructWin<T>;
                window.Init(tree);
                window.Show();
            }
        }

        protected virtual void showParents(T e)
        {
            string txt = "";
            var list = e.parentsID;
            for (int i = 0; i < list.Count; i++)
            {
                txt += list[i] + " ";
            }
            GUILayout.Label("父节点id: " + txt);
        }

        protected virtual void showChildren(T e)
        {
            string txt = "";
            var list = e.childrenID;
            for (int i = 0; i < list.Count; i++)
            {
                txt += list[i] + " ";
            }
            GUILayout.Label("子节点id: " + txt);

            index = EditorGUILayout.TextField("操作子节点的id: ", index);
        }

        protected void draw(T e)
        {
            if (e != null)
            {
                GUILayout.Space(10);
                GUILayout.Label("节点id: " + e.id);
                showParents(e);
                showChildren(e);
                EditorGUILayout.BeginHorizontal();

                showRemoveBtn(e, index);
                showAddBtn(e, index);

                EditorGUILayout.EndHorizontal();
                GUILayout.Space(10);

                drawInspector(e);
            }
        }

        public override void OnInspectorGUI()
        {
            // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
            serializedObject.Update();

            //显示eventlist的默认UI
            base.OnInspectorGUI();

            GUILayout.Space(20);
            tree = ((myTree<T>)target);

            GUILayout.Label("节点总数： "+tree.nodes.Count);

            GUILayout.Space(10);
            showEditWin();
            GUILayout.Space(10);

            for (int i = 0; i < tree.nodes.Count; i++)
            {
                draw(tree.nodes[i]);
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
