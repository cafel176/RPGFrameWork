using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actors
{
    public abstract class ActorComponent : basicComponent
    {
        private Actor a;
        public Actor actor
        {
            set
            {
                a = value;
            }
            get
            {
                return actor;
            }
        }

        private actorTools data = new actorTools();

        protected bool useComponent = true;

        public void setData<T>(ListNodeInterface n) where T : ActorComponent
        {
            ((T)this).setData(n);
        }

        protected abstract void setData(ListNodeInterface n);

        public virtual void setUseable(bool able)
        {
            useComponent = able;
        }

        protected AudioClip findAudio(string name)
        {
            return data.findAudio(name);
        }

        protected Rigidbody2D getRigidbody()
        {
            return a.Rigidbody;
        }

        protected Animator getAnimator()
        {
            return a.Animator;
        }

        protected SpriteRenderer getRenderer()
        {
            return a.Renderer;
        }

        protected AudioSource getAudioSource()
        {
            return a.AudioSource;
        }

        protected basicMove getMove()
        {
            return a.BasicMove;
        }

        protected basicBattle getBattle()
        {
            return a.BasicBattle;
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
