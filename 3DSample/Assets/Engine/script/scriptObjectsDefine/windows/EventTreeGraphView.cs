using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
#endif

namespace BaseData
{
#if UNITY_EDITOR

    public class EventTreeGraphView : myTreeGraphView<myEvent>
    {
        public EventTreeGraphView(EditorWindow editorWindow, myTree<myEvent> tree_) :base(editorWindow, tree_)
        {

        }

        protected override void addMenuProvider()
        {
            var menuWindowProvider = ScriptableObject.CreateInstance<EventTreeSearchMenuWindowProvider>();
            menuWindowProvider.OnSelectEntryHandler = OnMenuSelectEntry;

            nodeCreationRequest += context =>
            {
                SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), menuWindowProvider);
            };
        }

        protected override Node getNewNode(myEvent t, Node parent)
        {
            return new EventTreeNodeView(this,t, parent);
        }

        protected override bool OnMenuSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            AddElement(new EventTreeNodeView(this,searchTreeEntry.userData));

            return true;
        }

        protected override void afterSaveNode(myEvent t)
        {
            if(t.type==eventType.If)
            {
                int size = t.childrenID.Count;
                if (size > 0)
                    t.str3 = t.childrenID[0];
                if (size > 1)
                    t.str4 = t.childrenID[1];
            }
            else if (t.type == eventType.choicePanel)
            {
                int size = t.childrenID.Count;
                if (size > 0)
                {
                    string[] txts = t.str2.Split('#');
                    if (txts.Length < 2)
                        txts = new string[] { t.str2, "" };
                    t.str2 = txts[0]+"#"+t.childrenID[0];
                }                    
                if (size > 1)
                {
                    string[] txts = t.str3.Split('#');
                    if (txts.Length < 2)
                        txts = new string[] { t.str3, "" };
                    t.str3 = txts[0] + "#" + t.childrenID[1];
                }
                if (size > 2)
                {
                    string[] txts = t.str4.Split('#');
                    if (txts.Length < 2)
                        txts = new string[] { t.str4, "" };
                    t.str4 = txts[0] + "#" + t.childrenID[2];
                }
            }
            else if (t.type == eventType.showButton)
            {
                int size = t.childrenID.Count;
                if (size > 1)
                    t.str3 = t.childrenID[0];
            }
        }
    }

#endif
}
