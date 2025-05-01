using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions
{
    public abstract class RunAction : basicComponent
    {
        public abstract void interact(eventStruct e);
    }
}
