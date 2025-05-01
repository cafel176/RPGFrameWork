using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BaseData
{
    [System.Serializable]
    public class audioDictionary : myListVarNode<AudioClip>
    {
        public float pitch;
        public float _pitch
        {
            get
            {
                return pitch;
            }
        }

        public audioDictionary()
        {
            pitch = 1.0f;
        }
    }

    [CreateAssetMenu]
    public class AudioList : myList<audioDictionary>
    {

        public float findPitch(string id)
        {
            for (int i = 0; i < list.Count; i++)
            {
                audioDictionary t = (audioDictionary)list[i];
                if (t.id == id)
                    return t.pitch;
            }
            return -1;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(AudioList))]
    public class AudioInspector : myListInspector<audioDictionary>
    {
        protected override void drawInspector(int i, audioDictionary e)
        {
            e.id = EditorGUILayout.TextField("音乐/音效名", e.id);
            EditorGUILayout.BeginHorizontal();
            e.value = (AudioClip)EditorGUILayout.ObjectField("音乐/音效",e.value, typeof(AudioClip));
            EditorGUILayout.EndHorizontal();
            e.pitch = EditorGUILayout.FloatField("音调", e.pitch);
        }
    }
#endif
}
