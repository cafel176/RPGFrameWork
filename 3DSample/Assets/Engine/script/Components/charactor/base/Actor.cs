using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actors
{
    [RequireComponent(typeof(AudioSource))]
    public abstract class Actor : basicComponent, ActorInterface
    {
        protected AudioSource _audio;
        public AudioSource AudioSource
        {
            get
            {
                return _audio;
            }
        }

        protected ActorMove actorMove;
        protected ActorBattle actorBattle;

        protected actorTools data = new actorTools();

        [SerializeField]
        protected dataList components;

        protected List<ActorComponent> behaviours = new List<ActorComponent>();

        override protected void onAwake()
        {
            _audio = gameObject.GetComponent<AudioSource>();
            _audio.volume = data.getSetting().SEValue;

            addComponents();
        }

        protected virtual void addComponents()
        {
            components.init();
            var list = components.getList();
            for (int i = 0; i < list.Count; i++)
            {
                addComponent(list[i]);
            }
        }

        private void Update()
        {
            doEveryFrame();
        }

        public override void doEveryFrame()
        {
            if (behaviours != null)
                foreach (var b in behaviours)
                    b.doEveryFrame();
        }

        public T getActorComponent<T>() where T : ActorComponent
        {
            return gameObject.GetComponent<T>();
        }

        public void playSE(string name, bool loop = false)
        {
            if (_audio != null && gameObject.activeInHierarchy)
            {
                _audio.loop = loop;
                _audio.volume = data.getSetting().SEValue;
                _audio.clip = data.findAudio(name);
                if (!_audio.isPlaying)
                    _audio.Play();
            }
        }

        public void stopSE()
        {
            if (_audio != null)
            {
                _audio.loop = false;
                _audio.Stop();
            }
        }

        public gameSetting getGameSetting()
        {
            return data.getGameSetting();
        }

        public GameObject spawnPrefab(string name)
        {
            return GameObject.Instantiate(data.findPrefab(name)) as GameObject;
        }

        public ActorMoveInterface getMove()
        {
            return actorMove;
        }

        public ActorBattleInterface getBattle()
        {
            return actorBattle;
        }

        // =============================================== ½Ó¿Úº¯Êý ===============================================

        protected abstract void addComponent(ListNodeInterface component);

        public abstract controlMode getMode();
        public abstract void CameraRotate(float _mouseX, float _mouseY);
        public abstract void CameraScale(float scale);
    }
}
