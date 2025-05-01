using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actors
{
    public class basicMove3DFPS : ActorMove
    {
        private Rigidbody _rigidbody;

        override protected void onAwake()
        {
            _rigidbody = gameObject.GetComponent<Rigidbody>();
        }

        public override void Move(Vector2 vec, bool run = false, bool _rush = false)
        {
            //ÒÆ¶¯
            float moveEpsilon = getSystemSetting().moveEpsilon;
            if (Vector2.Distance(Vector2.zero, vec) > 1.5 * moveEpsilon)
            {
                _rigidbody.mass = 10;
                Vector3 t = Vector3.zero;
                if (Mathf.Abs(vec.x) > moveEpsilon)
                {
                    if (vec.x > moveEpsilon)
                    {
                        t = transform.right;
                    }
                    else if (vec.x < -moveEpsilon)
                    {
                        t = -transform.right;
                    }
                }
                else
                {
                    if (vec.y > moveEpsilon)
                    {
                        t = transform.forward;
                    }
                    else if (vec.y < -moveEpsilon)
                    {
                        t = -transform.forward;
                    }
                }
                Vector3 v = t * 0.5f * (canRun && run ? runSpeed : speed);
                _rigidbody.velocity = new Vector3(v.x, _rigidbody.velocity.y,v.z);
            }
            else//¾²Ö¹
            {
                _rigidbody.mass = 100000;
                _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0);
            }
        }

        public override void Jump()
        {
            Vector3 v = _rigidbody.velocity;
            _rigidbody.velocity = new Vector3(v.x, jumpForce, v.z);
        }

        public override void Rotating(float horizontal, float vertical, bool auto = false)
        {

        }
    }
}

