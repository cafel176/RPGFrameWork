using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseData
{

#if UNITY_EDITOR

    public class EventTreeStructWin : EditTreeStructWin<myEvent>
    {
        protected override myTreeGraphView<myEvent> getGraphView(myTree<myEvent> tree)
        {
            return new EventTreeGraphView(this, tree);
        }
    }

#endif
}
