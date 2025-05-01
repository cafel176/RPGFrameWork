using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
#endif

namespace BaseData
{
#if UNITY_EDITOR

    public class myTreeGraphView<T> : GraphView where T : myTreeNode, new()
    {
        protected EditorWindow editorWindow;
        protected myTree<T> tree;
        protected Node root;

        protected class TempGridBackground : GridBackground { }
        protected int offset = 400;

        public myTreeGraphView(EditorWindow editorWindow_, myTree<T> tree_)
        {
            editorWindow = editorWindow_;
            tree = tree_;

            GridBackground gridBackground = new TempGridBackground();
            gridBackground.name = "GridBackground";
            Insert(0, gridBackground);

            //按照父级的宽高全屏填充
            this.StretchToParentSize();
            //滚轮缩放
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            //graphview窗口内容的拖动
            this.AddManipulator(new ContentDragger());
            //选中Node移动功能
            this.AddManipulator(new SelectionDragger());
            //多个node框选功能
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ClickSelector());

            root = addNode("root",0, 0);

            addMenuProvider();
        }

        protected virtual void addMenuProvider()
        {
            var menuWindowProvider = ScriptableObject.CreateInstance<myTreeSearchMenuWindowProvider<T>>();
            menuWindowProvider.OnSelectEntryHandler = OnMenuSelectEntry;
            
            nodeCreationRequest += context =>
            {
                SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), menuWindowProvider);
            };
        }

        protected T getElements(string id)
        {
            foreach (T e in tree.nodes)
                if (e.getId() == id)
                    return e;

            return null;
        }

        protected Node addNode(string id,int x,int y, Node parent = null)
        {
            T t = getElements(id);
            Node n = getNewNode(t, parent);
            n.SetPosition(new Rect(new Vector2(x, y), n.GetPosition().size));
            AddElement(n);

            for(int i=0;i<t.childrenID.Count;i++)
            {
                addNode(t.childrenID[i],x+offset,y+i*offset ,n);
            }

            return n;
        }

        protected virtual Node getNewNode(T t, Node parent)
        {
            return new myTreeNodeView<T>(this,t, parent);
        }

        protected virtual bool OnMenuSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            AddElement(new myTreeNodeView<T>(this,searchTreeEntry.userData));
            
            return true;
        }

        public override List<Port> GetCompatiblePorts(Port startAnchor, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();
            foreach (var port in ports.ToList())
            {
                if (startAnchor.node == port.node ||
                    startAnchor.direction == port.direction ||
                    startAnchor.portType != port.portType)
                {
                    continue;
                }
                
                compatiblePorts.Add(port);
            }
            return compatiblePorts;
        }


        public void Save()
        {
            List<T> re = new List<T>();

            saveNode(re, root);

            tree.nodes = re;
        }

        protected void saveNode(List<T> list, Node node)
        {
            myTreeNodeView<T> n = (myTreeNodeView<T>)node;
            T t = n.data;
            t.childrenID.Clear();
            list.Add(t);

            var children = node.outputContainer.Children();
            foreach (var child in children)
            {
                var other = ((Port)child).connections;
                foreach (var o in other)
                {
                    saveNode(list, o.input.node);

                    T t_ = ((myTreeNodeView<T>)o.input.node).data;
                    t_.parentsID.Clear();
                    t_.parentsID.Add(t.getId());
                    t.childrenID.Add(t_.getId());
                }
            }

            afterSaveNode(t);
        }

        protected virtual void afterSaveNode(T t)
        {

        }
    }

#endif
}

