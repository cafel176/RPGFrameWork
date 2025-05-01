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
            if (GUILayout.Button(actorComponentData.dataTypeToName(actorComponentType.basicMove3D)))
            {
                e((int)actorComponentType.basicMove3D);
            }
            else if (GUILayout.Button(actorComponentData.dataTypeToName(actorComponentType.basicMove2D)))
            {
                e((int)actorComponentType.basicMove2D);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(actorComponentData.dataTypeToName(actorComponentType.basicMove3DFPS)))
            {
                e((int)actorComponentType.basicMove3DFPS);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(20);

            GUILayout.Label("战斗组件");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(actorComponentData.dataTypeToName(actorComponentType.basicBattle3D)))
            {
                e((int)actorComponentType.basicBattle3D);
            }
            else if (GUILayout.Button(actorComponentData.dataTypeToName(actorComponentType.basicBattle2D)))
            {
                e((int)actorComponentType.basicBattle2D);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(actorComponentData.dataTypeToName(actorComponentType.basicBattle3DFPS)))
            {
                e((int)actorComponentType.basicBattle3DFPS);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(20);


            GUILayout.Label("移动辅助组件");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(actorComponentData.dataTypeToName(actorComponentType.followBehaviour2D)))
            {
                e((int)actorComponentType.followBehaviour2D);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(20);
        }
    }

#endif
}
