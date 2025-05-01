using UnityEngine;
using System.Reflection;
using System;
using System.Collections.Generic;

namespace BaseData
{
    public class DataNode
    {
        private PropertyInfo[] infos;
        private object[] datas;

        public DataNode(object o)
        {
            // 只有set get封装过的public变量才会被识别到
            infos = o.GetType().GetProperties();
            datas = new object[infos.Length];
            for (int i = 0; i < datas.Length; i++)
            {
                datas[i] = infos[i].GetValue(o);
            }
        }

        public Type getType(string name)
        {
            // foreach内没法增减列表成员
            foreach (var i in infos)
            {
                if (i.Name == name)
                    return i.GetType();
            }
            return null;
        }

        public object getData(string name)
        {
            for (int i = 0; i < infos.Length; i++)
            {
                if (infos[i].Name == name)
                    return datas[i];
            }
            return null;
        }

        public T getData<T>(string name)
        {
            return (T)getData(name);
        }
    }

    public class DataBaseNode : dataNodeInterface
    {
        protected DataNode node = null;

        public Type getType(string name)
        {
            return node.getType(name);
        }

        public object getData(string name)
        {
            return node.getData(name);
        }

        public T getData<T>(string name)
        {
            return node.getData<T>(name);
        }
    }

    public class ListNode : DataBaseNode, ListNodeInterface
    {
        public string id;

        public string getId()
        {
            return id;
        }

        public int getNodeType()
        {
            return (int)node.getData(propertyName.type);
        }
    }

    public class TreeNode : ListNode, TreeNodeInterface
    {
        protected List<TreeNodeInterface> children = new List<TreeNodeInterface>();
        protected List<TreeNodeInterface> parents = new List<TreeNodeInterface>();

        public TreeNodeInterface getChild(string id)
        {
            foreach(var child in children)
                if(child.getId()==id)
                    return child;

            return null;
        }

        public void addChild(TreeNodeInterface c)
        {
            if (!children.Contains(c))
            {
                children.Add(c);
            }
        }

        public bool removeChild(TreeNodeInterface c)
        {
            if (children.Contains(c))
            {
                return children.Remove(c);
            }

            return true;
        }

        public List<TreeNodeInterface> getChildren()
        {
            return children;
        }



        public TreeNodeInterface getParent(string id)
        {
            foreach (var parent in parents)
                if (parent.getId() == id)
                    return parent;

            return null;
        }

        public void addParent(TreeNodeInterface c)
        {
            if (!parents.Contains(c))
            {
                parents.Add(c);
            }
        }

        public bool removeParent(TreeNodeInterface c)
        {
            if (parents.Contains(c))
            {
                return parents.Remove(c);
            }

            return true;
        }

        public List<TreeNodeInterface> getParents()
        {
            return parents;
        }


    }
}
