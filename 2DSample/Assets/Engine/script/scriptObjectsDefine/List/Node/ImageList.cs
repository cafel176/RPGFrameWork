using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BaseData
{
    [System.Serializable]
    public class imageDictionary : myListVarNode<Sprite>
    {
    }

    [CreateAssetMenu]
    public class ImageList : myList<imageDictionary>
    {
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ImageList))]
    public class ImgInspector : myListInspector<imageDictionary>
    {
        protected override void drawInspector(int i, imageDictionary e)
        {

            e.id = EditorGUILayout.TextField("图片名", e.id);
            EditorGUILayout.BeginHorizontal();
            e.value = (Sprite)EditorGUILayout.ObjectField("图片",e.value, typeof(Sprite));
            EditorGUILayout.EndHorizontal();
        }
    }
#endif
}
