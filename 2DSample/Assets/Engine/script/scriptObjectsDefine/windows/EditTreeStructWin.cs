using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BaseData
{

#if UNITY_EDITOR

    public class EditTreeStructWin : EditorWindow
    {
        private Vector2 scrollPosition;

        private void OnGUI()
        {
            drawTree();
        }

        private void drawTree()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        }
    }

#endif
}
