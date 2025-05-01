using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace BaseData
{

#if UNITY_EDITOR
    
    public class EditTreeStructWin<T> : EditorWindow where T : myTreeNode, new()
    {
        protected myTreeGraphView<T> graphView;

        public void Init(myTree<T> tree)
        {
            graphView = getGraphView(tree);
            rootVisualElement.Add(graphView);
        }

        protected virtual myTreeGraphView<T> getGraphView(myTree<T> tree)
        {
            return new myTreeGraphView<T>(this, tree);
        }

        private void OnDisable()
        {
            graphView.Save();
        }
    }

#endif
}
