using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BaseData
{

    [System.Serializable]
    public class switchDictionary : myListVarNode<bool>
    {
    }

    [CreateAssetMenu]
    public class switchList : myList<switchDictionary>
    {
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(switchList))]
    public class switchInspector : myListInspector<switchDictionary>
    {
        protected override void drawInspector(int i, switchDictionary e)
        {

            e.id = EditorGUILayout.TextField("变量名", e.id);
            e.value = EditorGUILayout.Toggle("值", e.value);
        }
    }
#endif
}
