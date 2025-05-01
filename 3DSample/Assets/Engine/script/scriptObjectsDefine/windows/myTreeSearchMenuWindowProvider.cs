using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
#endif

namespace BaseData
{
#if UNITY_EDITOR

    public class myTreeSearchMenuWindowProvider<T> : ScriptableObject, ISearchWindowProvider where T : myTreeNode, new()
    {
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var entries = new List<SearchTreeEntry>();
            entries.Add(new SearchTreeGroupEntry(new GUIContent("新建节点")));//添加了一个一级菜单

            menuDetails(entries);

            return entries;
        }

        protected virtual void menuDetails(List<SearchTreeEntry> entries)
        { }

        public delegate bool SerchMenuWindowOnSelectEntryDelegate(SearchTreeEntry searchTreeEntry, SearchWindowContext context);//声明一个delegate类

        public SerchMenuWindowOnSelectEntryDelegate OnSelectEntryHandler;//delegate回调方法

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            if (OnSelectEntryHandler == null)
            {
                return false;
            }
            return OnSelectEntryHandler(searchTreeEntry, context);
        }

    }

#endif
}
