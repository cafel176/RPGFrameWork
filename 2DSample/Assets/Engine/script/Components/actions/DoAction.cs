using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions
{
    public abstract class DoAction : basicComponent
    {
        //控制事件是否可推进
        protected bool canDoSth = true;
        public bool CanDoSth
        {
            get
            {
                return canDoSth;
            }
        }

        public abstract void doSth(TreeNodeInterface toDo);

        protected void changeCanDo(bool can)
        {
            canDoSth = can;
        }
    }
}
