using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions
{
    public abstract class startEvents : startAction
    {
        private eventTools data = new eventTools();

        protected settings getSetting()
        {
            return data.getSetting();
        }

        protected HashsAndTags getHat()
        {
            return data.getHat();
        }

        protected eventStruct getCommonEvent(string index)
        {
            return data.getCommonEvent(index);
        }
    }
}
