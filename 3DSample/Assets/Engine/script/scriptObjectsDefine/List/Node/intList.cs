using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BaseData
{
    [System.Serializable]
    public class VarIntDictionary : myListVarNode<int>
    {
    }

    [CreateAssetMenu]
    public class intList : myList<VarIntDictionary>
    {
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(intList))]
    public class intInspector : myListInspector<VarIntDictionary>
    {
        protected override void drawInspector(int i, VarIntDictionary e)
        {

            e.id = EditorGUILayout.TextField("变量名", e.id);
            e.value = EditorGUILayout.IntField("值", e.value);
        }
    }
#endif
}