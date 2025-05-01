using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions
{
    public abstract class startAction : basicComponent
    {
        // 自己返回自己会导致堆栈溢出
        protected bool canDoSth = false;
        public bool CanDoSth
        {
            get
            {
                return canDoSth;
            }
        }

        virtual public bool checkStart(start howToStart)
        {
            return true;
        }

        public abstract void start(string index);
    }
}
