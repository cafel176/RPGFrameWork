using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BaseData
{
    [System.Serializable]
    public class questDictionary : myListStructNode
    {
        public questStatus status;
        public questStatus _status
        {
            get
            {
                return status;
            }
        }
        public questType type;
        public questType _type
        {
            get
            {
                return type;
            }
        }
        //设置难度
        public string hard;
        public string _hard
        {
            get
            {
                return hard;
            }
        }
        // 步骤
        public List<string> steps;
        public List<string> _steps
        {
            get
            {
                return steps;
            }
        }
        public int stepNum;
        public int _stepNum
        {
            get
            {
                return stepNum;
            }
        }

        public questDictionary()
        {
            status = questStatus.doing;
            type = questType.main;
        }
    }

    [CreateAssetMenu]
    public class questListDefine : myList<questDictionary>
    {
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(questListDefine))]
    public class questInspector : myListInspector<questDictionary>
    {
        protected override void drawInspector(int i, questDictionary e)
        {

            e.id = EditorGUILayout.TextField("任务id", e.id);
            e.status = (questStatus)EditorGUILayout.EnumPopup("任务状态", e.status);
            e.type = (questType)EditorGUILayout.EnumPopup("任务类型", e.type);
            e.name = EditorGUILayout.TextField("名字", e.name);
            e.hard = EditorGUILayout.TextField("难度", e.hard);
            e.text = EditorGUILayout.TextField("描述文字", e.text);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("图片");
            e.img = (Sprite)EditorGUILayout.ObjectField(e.img, typeof(Sprite));
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            e.stepNum = EditorGUILayout.IntField("步骤总数:", e.stepNum);
            bool b = GUILayout.Button("确定");
            EditorGUILayout.EndHorizontal();

            int num = 0;
            if (e.steps == null)
                e.steps = new List<string>();

            num = e.steps.Count;
            if (b)
            {
                if (e.stepNum > num)
                {
                    for (int w = num; w < e.stepNum; w++)
                    {
                        e.steps.Add("");
                    }
                }
                else
                {
                    for (int w = e.stepNum; w < num;)
                    {
                        e.steps.RemoveAt(w);
                        num = e.steps.Count;
                    }
                }
            }

            EditorGUILayout.BeginVertical();
            for (int w = 0; w < num; w++)
            {
                e.steps[w] = EditorGUILayout.TextField("步骤 " + w, e.steps[w]);
            }
            EditorGUILayout.EndVertical();
        }
    }
#endif
}
