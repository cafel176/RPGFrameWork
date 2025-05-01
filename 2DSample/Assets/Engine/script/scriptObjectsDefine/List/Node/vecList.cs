using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BaseData
{
    [System.Serializable]
    public class Vec3Dictionary : myListVarNode<Vector3>
    {
    }

    [CreateAssetMenu]
    public class vecList : myList<Vec3Dictionary>
    {
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(vecList))]
    public class vecInspector : myListInspector<Vec3Dictionary>
    {
        protected override void drawInspector(int i, Vec3Dictionary e)
        {

            e.id = EditorGUILayout.TextField("变量名", e.id);
            e.value = EditorGUILayout.Vector3Field("值", e.value);
        }
    }
#endif
}