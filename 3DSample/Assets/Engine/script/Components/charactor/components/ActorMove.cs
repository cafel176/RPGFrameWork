using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actors
{
    public abstract class ActorMove : ActorComponent, ActorMoveInterface
    {
        protected float speed = 1;
        public float Speed
        {
            get
            {
                return speed;
            }
        }

        protected float runSpeed = 2;
        public float RunSpeed
        {
            get
            {
                return runSpeed;
            }
        }

        protected float jumpForce = 5;
        public float JumpForce
        {
            get
            {
                return jumpForce;
            }
        }

        protected int rayMask;
        public int RayMask
        {
            set
            {
                rayMask = value;
            }
        }

        protected string step;
        public string Step
        {
            set
            {
                step = value;
            }
            get
            {
                return step;
            }
        }

        protected string runStep;
        public string RunStep
        {
            set
            {
                runStep = value;
            }
            get
            {
                return runStep;
            }
        }
        protected string jumpStep;
        public string JumpStep
        {
            set
            {
                jumpStep = value;
            }
            get
            {
                return jumpStep;
            }
        }

        protected Vector3 target, start;
        public Vector3 Target
        {
            set
            {
                target = value;
            }
        }

        protected bool startMove = false;
        protected bool runMove = false;
        protected bool canRun = true;
        protected bool canJump = true;

        protected override void setData(ListNodeInterface n)
        {
            speed = n.getData<float>(structProperty.float1);
            step = n.getData<string>(structProperty.str1);
            canRun = n.getData<bool>(structProperty.bool1);
            runSpeed = n.getData<float>(structProperty.float2);
            runStep = n.getData<string>(structProperty.str2);
            canJump = n.getData<bool>(structProperty.bool2);
            jumpForce = n.getData<float>(structProperty.float3);
            jumpStep = n.getData<string>(structProperty.str3);
        }

        public void MoveTo(Vector2 pos, bool run = false)
        {
            target = pos;
            start = transform.position;
            startMove = true;
            //_rigidbody.mass = 10;
            runMove = run;
        }

        public void StopMoveTo()
        {
            startMove = false;
        }

        private void OnEnable()
        {
            AudioListener a = gameObject.GetComponent<AudioListener>();
            if (a != null)
            {
                if (Camera.main != null)
                {
                    var c = Camera.main.GetComponent<AudioListener>();
                    if (c != null)
                        c.enabled = false;
                }
                a.enabled = true;
            }
        }

        private void OnDisable()
        {
            AudioListener a = gameObject.GetComponent<AudioListener>();
            if (a != null)
            {
                a.enabled = false;
                if(Camera.main!=null)
                {
                    var c = Camera.main.GetComponent<AudioListener>();
                    if (c != null)
                        c.enabled = true;
                }
            }
        }

        public void setStep(string clip)
        {

        }

        public void setRunStep(string clip)
        {

        }

        public abstract void Rotating(float horizontal, float vertical,bool auto = false);
        public abstract void Move(Vector2 vec, bool run = false, bool _rush = false);
        public abstract void Jump();
    }
}

