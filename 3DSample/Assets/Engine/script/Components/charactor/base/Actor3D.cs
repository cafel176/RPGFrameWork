using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actors
{
    [RequireComponent(typeof(Rigidbody))]
    public class Actor3D : Actor
    {
        protected Animator anima;
        public Animator Animator
        {
            get
            {
                return anima;
            }

        }

        private Rigidbody _rigidbody;
        public Rigidbody Rigidbody
        {
            get
            {
                return _rigidbody;
            }
        }

        protected FootTrigger foot;

        private Camera3D _camera;

        override protected void onAwake()
        {
            anima = gameObject.GetComponent<Animator>();
            if(anima==null)
                anima = gameObject.GetComponentInChildren<Animator>();
            foot = gameObject.GetComponentInChildren<FootTrigger>();
            _rigidbody = gameObject.GetComponent<Rigidbody>();
            if(data.GetControlMode()==controlMode.control_3D)
                _camera = Camera.main.GetComponent<Camera3D>();

            base.onAwake();
        }

        protected override void addComponent(ListNodeInterface component)
        {
            actorComponentType type = component.getData<actorComponentType>(structProperty.type);
            ActorComponent a = null;
            switch (type)
            {
                case actorComponentType.basicMove3D:
                    a = actorMove = gameObject.AddComponent<basicMove3D>();
                    if (a != null)
                    {
                        a.actor = this;
                        behaviours.Add(a);
                        a.setData<basicMove3D>(component);
                    }
                    break;
                case actorComponentType.basicBattle3D:
                    a = actorBattle = gameObject.AddComponent<basicBattle3D>();
                    if (a != null)
                    {
                        a.actor = this;
                        behaviours.Add(a);
                        a.setData<basicBattle3D>(component);
                    }
                    break;
                default: break;
            }
        }

        // =============================================== ½Ó¿Úº¯Êý ===============================================

        public override controlMode getMode()
        {
            return controlMode.control_3D;
        }

        public override void CameraRotate(float _mouseX, float _mouseY)
        {
            if (_camera != null)
                _camera.CameraRotate(_mouseX, _mouseY);
        }

        public override void CameraScale(float scale)
        {
            if (_camera != null)
                _camera.CameraScale(scale);
        }
    }
}
