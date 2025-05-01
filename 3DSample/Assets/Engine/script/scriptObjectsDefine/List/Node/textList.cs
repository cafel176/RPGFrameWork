using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BaseData
{
    [System.Serializable]
    public class stringDictionary : myListVarNode<string>
    {
    }

    [CreateAssetMenu]
    public class textList : myList<stringDictionary>
    {
        public language lang;
        public language _lang
        {
            get
            {
                return lang;
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(textList))]
    public class textInspector : myListInspector<stringDictionary>
    {
        protected override void drawInspector(int i, stringDictionary e)
        {

            e.id = EditorGUILayout.TextField("文本名", e.id);
            var tdata = (textList)target;
            if (tdata.lang == language.chinese)
            {
                GUILayout.Label("中文翻译");
            }
            else if (tdata.lang == language.english)
            {
                GUILayout.Label("English text");
            }
            else  if (tdata.lang == language.japanese)
            {
                GUILayout.Label("日本語の翻訳");
            }
            e.value = EditorGUILayout.TextArea(e.value, GUILayout.Height(40), GUILayout.Width(300));
        }
    }
#endif
}
