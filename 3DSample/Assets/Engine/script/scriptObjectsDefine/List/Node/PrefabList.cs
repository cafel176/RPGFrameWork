using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BaseData
{

    [System.Serializable]
    public class PrefabDictionary : myListVarNode<GameObject>
    {
    }

    [CreateAssetMenu]
    public class PrefabList : myList<PrefabDictionary>
    {

        public GameObject findPrefab(string id)
        {
            for (int i = 0; i < list.Count; i++)
            {
                PrefabDictionary t = (PrefabDictionary)list[i];
                if (t.id == id)
                    return t.value;
            }
            return null;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(PrefabList))]
    public class PrefabInspector : myListInspector<PrefabDictionary>
    {
        protected override void drawInspector(int i, PrefabDictionary e)
        {

            e.id = EditorGUILayout.TextField("Prefab名", e.id);
            EditorGUILayout.BeginHorizontal();
            e.value = (GameObject)EditorGUILayout.ObjectField("Prefab",e.value, typeof(GameObject));
            EditorGUILayout.EndHorizontal();
        }
    }
#endif
}
