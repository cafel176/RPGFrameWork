using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actors
{
    public class basicMove3D : ActorMove
    {
        private Rigidbody _rigidbody;
        private Animator anima;

        protected float turnSmoothing = 5f;   // A smoothing value for turning the player.
        public float TurnSmoothing
        {
            get
            {
                return turnSmoothing;
            }
        }

        protected float speedDampTime = 0.2f;  // The damping for the speed parameter
        public float SpeedDampTime
        {
            get
            {
                return speedDampTime;
            }
        }

        protected Quaternion targetRotation;
        protected bool doRotate = false;

        protected Vector3 turn = Vector3.zero;

        override protected void onAwake()
        {
            _rigidbody = gameObject.GetComponent<Rigidbody>();
            _rigidbody.mass = 100000;
            anima = gameObject.GetComponent<Animator>();
            if (anima == null)
                anima = gameObject.GetComponentInChildren<Animator>();
        }

        override public void doEveryFrame()
        {
            doMoveTo();
            doRotating();
        }

        public override void Move(Vector2 vec, bool run = false, bool _rush = false)
        {
            //移动
            float moveEpsilon = getSystemSetting().moveEpsilon;
            if (Vector2.Distance(Vector2.zero, vec) > 1.5 * moveEpsilon)
            {
                _rigidbody.mass = 10;
                // ... set the players rotation and set the speed parameter to moveSpeed.
                Rotating(-vec.x, -vec.y);
                Vector3 v = new Vector3(-vec.x, 0, -vec.y).normalized * 0.5f * (canRun && run ? runSpeed : speed);
                _rigidbody.velocity = v + new Vector3(0, _rigidbody.velocity.y, 0);

                Vector3 a = transform.eulerAngles;

                // v是z,h是x
                if (vec.y > moveEpsilon)
                {
                    if (vec.x > moveEpsilon)
                    {
                        if (a.y > turn.y)//右转
                        {
                            anima.SetFloat(getHat().speedF, vec.y * 3, speedDampTime, Time.deltaTime);
                            anima.SetFloat(getHat().speedC, -vec.x * 2, speedDampTime, Time.deltaTime);
                        }
                        else if (a.y < turn.y)//左转
                        {
                            anima.SetFloat(getHat().speedF, vec.x * 3, speedDampTime, Time.deltaTime);
                            anima.SetFloat(getHat().speedC, vec.y * 2, speedDampTime, Time.deltaTime);
                        }
                    }
                    else if (vec.x < -moveEpsilon)
                    {
                        if (a.y > turn.y)//右转
                        {
                            anima.SetFloat(getHat().speedF, -vec.x * 3, speedDampTime, Time.deltaTime);
                            anima.SetFloat(getHat().speedC, -vec.y * 2, speedDampTime, Time.deltaTime);
                        }
                        else if (a.y < turn.y)//左转
                        {
                            anima.SetFloat(getHat().speedF, vec.y * 3, speedDampTime, Time.deltaTime);
                            anima.SetFloat(getHat().speedC, -vec.x * 2, speedDampTime, Time.deltaTime);
                        }
                    }
                    else
                    {
                        anima.SetFloat(getHat().speedF, vec.y * 3, speedDampTime, Time.deltaTime);
                        anima.SetFloat(getHat().speedC, 0);
                    }
                }
                else if (vec.y < -moveEpsilon)
                {
                    if (vec.x > moveEpsilon)
                    {
                        if (a.y > turn.y)//右转
                        {
                            anima.SetFloat(getHat().speedF, vec.x * 3, speedDampTime, Time.deltaTime);
                            anima.SetFloat(getHat().speedC, vec.y * 2, speedDampTime, Time.deltaTime);
                        }
                        else if (a.y < turn.y)//左转
                        {
                            anima.SetFloat(getHat().speedF, -vec.y * 3, speedDampTime, Time.deltaTime);
                            anima.SetFloat(getHat().speedC, vec.x * 2, speedDampTime, Time.deltaTime);
                        }
                    }
                    else if (vec.x < -moveEpsilon)
                    {
                        if (a.y > turn.y)//右转
                        {
                            anima.SetFloat(getHat().speedF, -vec.y * 3, speedDampTime, Time.deltaTime);
                            anima.SetFloat(getHat().speedC, vec.x * 2, speedDampTime, Time.deltaTime);
                        }
                        else if (a.y < turn.y)//左转
                        {
                            anima.SetFloat(getHat().speedF, -vec.x * 3, speedDampTime, Time.deltaTime);
                            anima.SetFloat(getHat().speedC, -vec.y * 2, speedDampTime, Time.deltaTime);
                        }
                    }
                    else
                    {
                        anima.SetFloat(getHat().speedF, -vec.y * 3, speedDampTime, Time.deltaTime);
                        anima.SetFloat(getHat().speedC, 0);
                    }
                }
                else
                {
                    anima.SetFloat(getHat().speedF, Mathf.Abs(vec.x) * 3, speedDampTime, Time.deltaTime);
                    anima.SetFloat(getHat().speedC, 0);
                }
                turn = a;
            }
            else // 静止
            {
                _rigidbody.mass = 100000;
                _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0);
                anima.SetFloat(getHat().speedF, 0);
                anima.SetFloat(getHat().speedC, 0);
            }
        }

        public override void Rotating(float horizontal, float vertical, bool auto = false)
        {
            // Create a new vector of the vec.x and vec.y inputs.
            Vector3 targetDirection = new Vector3(horizontal, 0f, vertical);

            // Create a rotation based on this new vector assuming that up is the global y axis.
            targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);

            // Create a rotation that is an increment closer to the target rotation from the player's rotation.
            Quaternion newRotation = Quaternion.Lerp(_rigidbody.rotation, targetRotation, turnSmoothing * Time.deltaTime);

            // Change the players rotation to this new rotation.
            _rigidbody.MoveRotation(newRotation);

            doRotate = auto;
        }

        protected void doRotating()
        {
            if (doRotate)
            {
                Quaternion newRotation = Quaternion.Lerp(_rigidbody.rotation, targetRotation, turnSmoothing * Time.deltaTime);
                _rigidbody.MoveRotation(newRotation);
                // 点乘结果是1，则两个四元数相同
                if (Mathf.Abs(Quaternion.Dot(_rigidbody.rotation, targetRotation)) > 0.95f)
                    doRotate = false;
            }
        }

        protected void doMoveTo()
        {
            if (startMove)
            {
                Vector3 now = transform.position;
                Vector3 temp = target - now;
                float distanceEpsilon = getSystemSetting().distanceEpsilon;
            }
        }

        public override void Jump()
        {
            Vector3 v = _rigidbody.velocity;
            _rigidbody.velocity = new Vector3(v.x, jumpForce, v.z);
        }
    }
}
