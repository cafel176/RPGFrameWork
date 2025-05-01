using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actors
{
    [RequireComponent(typeof(Rigidbody))]
    public class Actor3DFPS : Actor
    {
        private Rigidbody _rigidbody;
        public Rigidbody Rigidbody
        {
            get
            {
                return _rigidbody;
            }
        }

        protected FootTrigger foot;

        private Camera3DFPS _cameraFPS;

        override protected void onAwake()
        {
            foot = gameObject.GetComponentInChildren<FootTrigger>();
            _cameraFPS = gameObject.GetComponentInChildren<Camera3DFPS>();
            _rigidbody = gameObject.GetComponent<Rigidbody>();

            base.onAwake();
        }

        protected override void addComponent(ListNodeInterface component)
        {
            actorComponentType type = component.getData<actorComponentType>(structProperty.type);
            ActorComponent a = null;
            switch (type)
            {
                case actorComponentType.basicMove3DFPS:
                    a = actorMove = gameObject.AddComponent<basicMove3DFPS>();
                    if (a != null)
                    {
                        a.actor = this;
                        behaviours.Add(a);
                        a.setData<basicMove3DFPS>(component);
                    }
                    break;
                case actorComponentType.basicBattle3DFPS:
                    a = actorBattle = gameObject.AddComponent<basicBattle3DFPS>();
                    if (a != null)
                    {
                        a.actor = this;
                        behaviours.Add(a);
                        a.setData<basicBattle3DFPS>(component);
                    }
                    break;
                default: break;
            }
        }

        // =============================================== ½Ó¿Úº¯Êý ===============================================

        public override controlMode getMode()
        {
            return controlMode.control_3DFPS;
        }

        public override void CameraRotate(float _mouseX, float _mouseY)
        {
            if (_cameraFPS != null)
                _cameraFPS.CameraRotate(_mouseX, _mouseY);
        }

        public override void CameraScale(float scale)
        {
            if (_cameraFPS != null)
                _cameraFPS.CameraScale(scale);
        }
    }
}

