using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actors
{
    public abstract class ActorComponent : basicComponent
    {
        protected Actor a;
        public Actor actor
        {
            set
            {
                a = value;
            }
            get
            {
                return a;
            }
        }

        private actorTools data = new actorTools();

        protected bool useComponent = true;

        public void setData<T>(ListNodeInterface n) where T : ActorComponent
        {
            ((T)this).setData(n);
        }

        protected abstract void setData(ListNodeInterface n);

        protected AudioClip findAudio(string name)
        {
            return data.findAudio(name);
        }

        public virtual void setUseable(bool able)
        {
            useComponent = able;
        }

        protected void playSE(string name, bool loop = false)
        {
            a.playSE(name, loop);
        }

        protected void stopSE()
        {
            a.stopSE();
        }

        protected settings getSetting()
        {
            return data.getSetting();
        }

        protected HashsAndTags getHat()
        {
            return data.getHat();
        }

        protected systemSetting getSystemSetting()
        {
            return data.getSystemSetting();
        }
    }
}
