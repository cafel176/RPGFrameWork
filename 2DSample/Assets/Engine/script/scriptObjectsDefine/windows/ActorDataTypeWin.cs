using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BaseData
{
#if UNITY_EDITOR

    public class ActorDataTypeWin : ChooseTypeWin
    {
        protected override void doShow()
        {
            GUILayout.Label("移动组件");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(actorComponentData.dataTypeToName(actorComponentType.basicMove)))
            {
                e((int)actorComponentType.basicMove);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(20);

            GUILayout.Label("战斗组件");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(actorComponentData.dataTypeToName(actorComponentType.basicBattle)))
            {
                e((int)actorComponentType.basicBattle);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(20);

            GUILayout.Label("移动辅助组件");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(actorComponentData.dataTypeToName(actorComponentType.followBehaviour)))
            {
                e((int)actorComponentType.followBehaviour);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(20);
        }
    }

#endif
}
