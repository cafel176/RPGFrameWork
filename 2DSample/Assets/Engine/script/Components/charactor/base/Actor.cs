using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Actors
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(AudioSource))]
    public class Actor : basicComponent, ActorInterface
    {
        protected Rigidbody2D _rigidbody;
        public Rigidbody2D Rigidbody
        {
            get
            {
                return _rigidbody;
            }
        }

        protected Animator anima;
        public Animator Animator
        {
            get
            {
                return anima;
            }
        }

        protected SpriteRenderer render;
        public SpriteRenderer Renderer
        {
            get
            {
                return render;
            }
        }
        protected AudioSource _audio;
        public AudioSource AudioSource
        {
            get
            {
                return _audio;
            }
        }


        protected basicMove basicMove;
        public basicMove BasicMove
        {
            get
            {
                return basicMove;
            }
        }

        protected basicBattle basicBattle;
        public basicBattle BasicBattle
        {
            get
            {
                return basicBattle;
            }
        }

        [SerializeField]
        protected dataList components;

        protected List<ActorComponent> behaviours = new List<ActorComponent>();

        private actorTools data = new actorTools();

        private void Update()
        {
            doEveryFrame();
        }

        override protected void onAwake()
        {
            _rigidbody = gameObject.GetComponent<Rigidbody2D>();
            anima = gameObject.GetComponent<Animator>();
            render = gameObject.GetComponent<SpriteRenderer>();
            _audio = gameObject.GetComponent<AudioSource>();
            _audio.volume = data.getSetting().SEValue;

            addComponents();
        }

        protected void addComponents()
        {
            components.init();
            var list = components.getList();
            for (int i = 0; i < list.Count; i++)
            {
                actorComponentType type = list[i].getData<actorComponentType>(structProperty.type);
                ActorComponent a = null;
                switch(type)
                {
                    case actorComponentType.basicMove:
                        a = basicMove = gameObject.AddComponent<basicMove>();
                        if (a != null)
                        {
                            a.actor = this;
                            behaviours.Add(a);
                            a.setData<basicMove>(list[i]);
                        }                       
                        break;
                    case actorComponentType.basicBattle:
                        a = basicBattle = gameObject.AddComponent<basicBattle>();
                        if (a != null)
                        {
                            a.actor = this;
                            behaviours.Add(a);
                            a.setData<basicBattle>(list[i]);
                        }
                        break;
                    case actorComponentType.followBehaviour:
                        a = gameObject.AddComponent<followBehaviour>();
                        if (a != null)
                        {
                            a.actor = this;
                            behaviours.Add(a);
                            a.setData<followBehaviour>(list[i]);
                            a.setUseable(false);
                        }
                        break;
                    default:break;
                }

            }
        }

        override public void doEveryFrame()
        {
            if (behaviours != null)
                foreach (var b in behaviours)
                    b.doEveryFrame();
        }

        public T getActorComponent<T>() where T : ActorComponent
        {
            return gameObject.GetComponent<T>();
        }

// =============================================== 接口函数 ===============================================

        public void Move(Vector2 vec, bool run = false, bool _rush = false)
        {
            if(basicMove!=null)
                basicMove.Move(vec, run, _rush);
        }

        public void changeTurn(turn turn)
        {
            if (basicMove != null)
                basicMove.changeTurn(turn);
        }

        public turn getTurn()
        {
            if (basicMove != null)
                return basicMove.Turn;
            return turn.all;
        }

        public void MoveTo(Vector2 pos, bool run = false)
        {
            if (basicMove != null)
                basicMove.MoveTo(pos,run);
        }

        public void setStep(string clip)
        {
            if (basicMove != null)
                basicMove.Step = clip;
        }

        public void setRunStep(string clip)
        {
            if (basicMove != null)
                basicMove.RunStep = clip;
        }

        public void changeFollowsPos(turn e, Vector2 t, string step = "", string runStep = "")
        {
            var f = getActorComponent<followBehaviour>();
            if (f != null)
                f.changeFollowsPos(e, t, step, runStep);
        }

        public void addFollow(GameObject npc)
        {
            if (npc == null)
                return;

            var f = getActorComponent<followBehaviour>();
            if (f != null)
            {
                f.addFollow(npc);
            }
        }

        public void removeFollow(GameObject npc)
        {
            if (npc == null)
                return;

            var f = getActorComponent<followBehaviour>();
            if (f != null)
                f.removeFollow(npc);
        }
    }
}
