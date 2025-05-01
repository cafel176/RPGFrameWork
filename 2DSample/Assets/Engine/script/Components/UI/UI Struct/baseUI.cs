using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public abstract class baseUI : basicComponent
    {
        virtual protected void ableInit() { }

        private void OnEnable()
        {
            ableInit();
        }
    }
}
