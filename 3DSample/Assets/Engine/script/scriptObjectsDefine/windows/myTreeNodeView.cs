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

    public class myTreeNodeView<T> : Node where T : myTreeNode, new()
    {
        protected T e;
        public T data
        {
            get{return e;}
        }

        public myTreeNodeView(myTreeGraphView<T> graph,T node, Node parent = null)
        { }

        public myTreeNodeView(myTreeGraphView<T> graph, object data)
        { }

        protected void addChild(string label = "вс╫з╣Ц")
        {
            var outPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(Port));
            outPort.portName = label;
            outputContainer.Add(outPort);
        }
    }

#endif
}
