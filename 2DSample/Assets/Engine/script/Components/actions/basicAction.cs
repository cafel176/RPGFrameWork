using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions
{
    public class basicAction : basicComponent
    {
        [SerializeField]
        protected startAction _start;
        [SerializeField]
        protected RunAction _run;
        [SerializeField]
        protected DoAction _do;
    }
}
