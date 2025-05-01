using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BaseData
{
    [System.Serializable]
    public class DataListDictionary : myListVarNode<dataList>
    {
    }

    [CreateAssetMenu]
    public class DataList : myList<DataListDictionary>
    {
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(DataList))]
    public class DataListInspector : myListInspector<DataListDictionary>
    {
        protected override void drawInspector(int i, DataListDictionary e)
        {
            e.id = EditorGUILayout.TextField("变量名", e.id);
            e.value = (dataList)EditorGUILayout.ObjectField("值", e.value, typeof(dataList));
        }
    }
#endif
}