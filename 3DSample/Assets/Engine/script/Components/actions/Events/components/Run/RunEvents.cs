using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions
{
    public abstract class RunEvents : RunAction
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

        protected nowState getNowState()
        {
            return data.getNowState();
        }

        protected List<keyInput> getInputs()
        {
            return data.getInputs();
        }

        protected bool getShowFinish()
        {
            return data.getShowFinish();
        }

        protected bool getCanHind()
        {
            return data.getCanHind();
        }

        protected void skip()
        {
            data.skip();
        }

        protected bool checkTextRequet(Component g)
        {
            return data.checkTextRequet(g);
        }

        protected void hideTextPanel()
        {
            data.hideTextPanel();
        }

        protected void stopUserControl()
        {
            data.stopUserControl();
        }
    }
}


