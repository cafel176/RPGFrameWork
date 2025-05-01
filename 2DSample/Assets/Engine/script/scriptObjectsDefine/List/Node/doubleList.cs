using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BaseData
{
    [System.Serializable]
    public class VarDoubleDictionary : myListVarNode<double>
    { 

    }

    [CreateAssetMenu]
    public class doubleList : myList<VarDoubleDictionary>
    {
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(doubleList))]
    public class floatInspector : myListInspector<VarDoubleDictionary>
    {
        protected override void drawInspector(int i, VarDoubleDictionary e)
        {

            e.id = EditorGUILayout.TextField("变量名", e.id);
            e.value = EditorGUILayout.DoubleField("值", e.value);
        }
    }
#endif
}