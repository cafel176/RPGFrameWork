using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BaseData
{
#if UNITY_EDITOR

    public delegate void AddElement(int type);

    public abstract class ChooseTypeWin : EditorWindow
    {
        protected Vector2 scrollPosition;

        public AddElement e;

        private void OnGUI()
        {
            ShowEditorGUI();
        }

        private void ShowEditorGUI()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            doShow();

            GUILayout.EndScrollView();

            GUIUtility.ExitGUI();
        }

        protected abstract void doShow();
    }

#endif
}

