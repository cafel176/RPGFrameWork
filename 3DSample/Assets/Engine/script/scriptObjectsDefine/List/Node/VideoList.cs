using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;

namespace BaseData
{
    [System.Serializable]
    public class videoDictionary : myListVarNode<VideoClip>
    {
        public videoDictionary()
        {
        }
    }

    [CreateAssetMenu]
    public class VideoList : myList<videoDictionary>
    {
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(VideoList))]
    public class VideoInspector : myListInspector<videoDictionary>
    {
        protected override void drawInspector(int i, videoDictionary e)
        {
            e.id = EditorGUILayout.TextField("สำฦตร๛", e.id);
            EditorGUILayout.BeginHorizontal();
            e.value = (VideoClip)EditorGUILayout.ObjectField("สำฦต", e.value, typeof(VideoClip));
            EditorGUILayout.EndHorizontal();
        }
    }
#endif
}
