using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actors
{
    public class basicMove : ActorComponent
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

        protected float rushDis = 1;
        public float RushDis
        {
            get
            {
                return rushDis;
            }
        }

        protected float rushTime = 0.3f;
        public float RushTime
        {
            get
            {
                return rushTime;
            }
        }

        protected Vector2Int turn = new Vector2Int(0, -1);
        public turn Turn
        {
            get
            {
                return turnTool.get(turn);
            }
        }

        protected Vector2 target, start;
        public Vector2 Target
        {
            set
            {
                target = value;
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

        protected Sprite[] idle = new Sprite[3];//0左1上2下

        protected bool left = true;
        protected bool canRun = true;
        protected bool canRush = true;
        protected bool useIdleAnima = false;

        protected float rushTimer = 0;

        protected bool startMove = false;
        protected bool runMove = false;

        override protected void onStart()
        {
            rayMask = (1 << LayerMask.NameToLayer(HashsAndTags.Collider)) | (1 << LayerMask.NameToLayer(HashsAndTags.npc));
            rushTimer = rushTime;
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
            setIdleAnima();
        }

        override public void doEveryFrame()
        {
            if(canRush)
                rushTimer += Time.deltaTime;
            doMoveTo();
        }

        protected override void setData(ListNodeInterface n)
        {
            speed = n.getData<float>(structProperty.float1);
            step = n.getData<string>(structProperty.str1);
            canRun = n.getData<bool>(structProperty.bool1);
            runSpeed = n.getData<float>(structProperty.float2);
            runStep = n.getData<string>(structProperty.str2);
            canRush = n.getData<bool>(structProperty.bool2);
            rushDis = n.getData<float>(structProperty.float3);
            rushTime = n.getData<float>(structProperty.float4);
            useIdleAnima = n.getData<bool>(structProperty.bool3);
            idle[0] = n.getData<Sprite>(structProperty.img1);
            idle[1] = n.getData<Sprite>(structProperty.img2);
            idle[2] = n.getData<Sprite>(structProperty.img3);
            left = n.getData<bool>(structProperty.bool4);
            turn t = n.getData<turn>(structProperty.turn);
            changeTurn(t);
        }

        protected void setIdleAnima()
        {
            float moveEpsilon = getSystemSetting().moveEpsilon;
            if (!useIdleAnima)
            {
                getAnimator().enabled = false;
                if (turn.y > moveEpsilon)
                    getRenderer().sprite = idle[1];
                else if (turn.y < -moveEpsilon)
                    getRenderer().sprite = idle[2];
                else
                {
                    if (turn.x > moveEpsilon)
                        getRenderer().flipX = left;
                    else if (turn.x < -moveEpsilon)
                        getRenderer().flipX = !left;
                    getRenderer().sprite = idle[0];
                }
            }
            else
            {
                playSE(step, true);
                getAnimator().enabled = true;
                if (turn.y > moveEpsilon)
                    getAnimator().SetFloat(getHat().idleAnima, 1);
                else if (turn.y < -moveEpsilon)
                    getAnimator().SetFloat(getHat().idleAnima, 2);
                else
                {
                    if (turn.x > moveEpsilon)
                        getRenderer().flipX = left;
                    else if (turn.x < -moveEpsilon)
                        getRenderer().flipX = !left;
                    getAnimator().SetFloat(getHat().idleAnima, 0);
                }
            }
        }

        virtual public void changeTurn(turn t)
        {
            Move(turnTool.get(t));

            Move(Vector2.zero);
        }

        virtual public void rush()
        {
            if(canRush)
            {
                transform.position += new Vector3(turn.x, turn.y, 0) * rushDis;
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
            }
        }

        virtual public void Move(Vector2 vec, bool run = false, bool _rush = false)
        {
            bool rush = false;
            if (rushTimer > rushTime && canRush)
            {
                rushTimer = 0;
                rush = _rush;
            }
            else
                rush = false;
            //移动
            float moveEpsilon = getSystemSetting().moveEpsilon;
            if (Vector2.Distance(Vector2.zero, vec) > 1.5 * moveEpsilon)
            {
                getRigidbody().mass = 10;
                //控制方向
                if (Mathf.Abs(vec.x) > moveEpsilon)
                {
                    if (vec.x > moveEpsilon)
                    {
                        getRenderer().flipX = left;
                        turn = new Vector2Int(1, 0);
                    }
                    else if (vec.x < -moveEpsilon)
                    {
                        getRenderer().flipX = !left;
                        turn = new Vector2Int(-1, 0);
                    }
                }
                else
                {
                    //getRenderer().flipX = false;
                    if (vec.y > moveEpsilon)
                    {
                        turn = new Vector2Int(0, 1);
                    }
                    else if (vec.y < -moveEpsilon)
                    {
                        turn = new Vector2Int(0, -1);
                    }
                }
                //控制速度
                //if (!rush)
                getRigidbody().velocity = new Vector2(turn.x,turn.y) * 0.5f * (canRun && run ? runSpeed : speed);
                getAnimator().enabled = true;
                getAnimator().SetBool(getHat().runX, false);
                getAnimator().SetBool(getHat().runY, false);
                getAnimator().SetBool(getHat().Xspeed, false);
                getAnimator().SetBool(getHat().YspeedUp, false);
                getAnimator().SetBool(getHat().YspeedDown, false);

                if (Mathf.Abs(turn.x) > moveEpsilon)
                {
                    getAnimator().SetBool(getHat().Xspeed, true);
                    if (rush)
                    {
                        getAnimator().SetTrigger(getHat().rushX);
                    }
                    else if (canRun && run)
                    {
                        getAnimator().SetBool(getHat().runX, true);
                        playSE(runStep, true);
                    }
                    else
                        playSE(step, true);
                }
                else
                {

                }

                if (turn.y > moveEpsilon)
                {
                    getAnimator().SetBool(getHat().YspeedUp, true);
                    if (rush)
                    {
                        getAnimator().SetTrigger(getHat().rushY);
                    }
                    else if (canRun && run)
                    {
                        getAnimator().SetBool(getHat().runY, true);
                        playSE(runStep, true);
                    }
                    else
                        playSE(step, true);
                }
                else if (turn.y < -moveEpsilon)
                {
                    getAnimator().SetBool(getHat().YspeedDown, true);
                    if (rush)
                    {
                        getAnimator().SetTrigger(getHat().rushY);
                    }
                    else if (canRun && run)
                    {
                        getAnimator().SetBool(getHat().runY, true);
                        playSE(runStep, true);
                    }
                    else
                        playSE(step, true);
                }
                else
                {

                }
            }
            else//静止
            {
                getRigidbody().mass = 100000;
                getRigidbody().velocity = Vector2.zero;
                if (!useIdleAnima)
                {
                    stopSE();
                    getAnimator().enabled = false;
                    if (turn.y > moveEpsilon)
                        getRenderer().sprite = idle[1];
                    else if (turn.y < -moveEpsilon)
                        getRenderer().sprite = idle[2];
                    else
                    {
                        if (turn.x > moveEpsilon)
                            getRenderer().flipX = left;
                        else if (turn.x < -moveEpsilon)
                            getRenderer().flipX = !left;
                        getRenderer().sprite = idle[0];
                    }
                }
                else
                {
                    playSE(step, true);
                    getAnimator().enabled = true;
                    getAnimator().SetBool(getHat().runX, false);
                    getAnimator().SetBool(getHat().runY, false);
                    getAnimator().SetBool(getHat().Xspeed, false);
                    getAnimator().SetBool(getHat().YspeedUp, false);
                    getAnimator().SetBool(getHat().YspeedDown, false);
                    if (turn.y > moveEpsilon)
                        getAnimator().SetFloat(getHat().idleAnima, 1);
                    else if (turn.y < -moveEpsilon)
                        getAnimator().SetFloat(getHat().idleAnima, 2);
                    else
                    {
                        if (turn.x > moveEpsilon)
                            getRenderer().flipX = left;
                        else if (turn.x < -moveEpsilon)
                            getRenderer().flipX = !left;
                        getAnimator().SetFloat(getHat().idleAnima, 0);
                    }
                }
            }

            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
        }

        protected void playSE(string name, bool loop = false)
        {
            if (getAudioSource() != null && gameObject.activeInHierarchy)
            {
                getAudioSource().loop = loop;
                getAudioSource().volume = getSetting().SEValue;
                getAudioSource().clip = findAudio(name);
                if (!getAudioSource().isPlaying)
                    getAudioSource().Play();
            }
        }

        protected void stopSE()
        {
            if (getAudioSource() != null)
            {
                getAudioSource().loop = false;
                getAudioSource().Stop();
            }
        }

        protected void doMoveTo()
        {
            if (startMove)
            {
                Vector2 now = transform.position;
                Vector2 temp = target - now;
                float distanceEpsilon = getSystemSetting().distanceEpsilon;
                if (getMoveType(target))
                {
                    if (Mathf.Abs(temp.x) > distanceEpsilon * (runMove ? runSpeed / speed : 1))
                    {
                        Move(new Vector2(temp.x, 0), runMove);
                    }
                    else
                    {
                        transform.position = new Vector3(target.x, transform.position.y, transform.position.z);
                        if (Mathf.Abs(temp.y) > distanceEpsilon * (runMove ? runSpeed / speed : 1))
                        {
                            Move(new Vector2(0, temp.y), runMove);
                        }
                        else
                        {
                            transform.position = new Vector3(target.x, target.y, transform.position.z);
                            Move(Vector2.zero);
                            startMove = false;
                            runMove = false;
                            getRigidbody().mass = 100000;
                        }
                    }
                }
                else
                {
                    if (Mathf.Abs(temp.y) > distanceEpsilon * (runMove ? runSpeed / speed : 1))
                    {
                        Move(new Vector2(0, temp.y), runMove);
                    }
                    else
                    {
                        transform.position = new Vector3(transform.position.x, target.y, transform.position.z);
                        if (Mathf.Abs(temp.x) > distanceEpsilon * (runMove ? runSpeed / speed : 1))
                        {
                            Move(new Vector2(temp.x, 0), runMove);
                        }
                        else
                        {
                            transform.position = new Vector3(target.x, target.y, transform.position.z);
                            Move(Vector2.zero);
                            startMove = false;
                            runMove = false;
                            getRigidbody().mass = 100000;
                        }
                    }
                }
            }
        }

        public void MoveTo(Vector2 pos, bool run = false)
        {
            target = pos;
            start = transform.position;
            startMove = true;
            getRigidbody().mass = 10;
            runMove = run;
        }

        public void StopMoveTo()
        {
            startMove = false;
        }

        protected bool getMoveType(Vector2 pos)
        {
            Vector2 now = transform.position;
            now -= new Vector2(0, 0.09f);
            float ep = 0.05f, ep2 = 0.01f;

            bool xfirst, xC = false, yC = false;
            RaycastHit2D hitx1, hity1, hitx2, hity2, hitx0, hity0;
            hitx0 = Physics2D.Raycast(now, new Vector2((pos - now).x, 0).normalized, 0.05f + ep, rayMask);
            hitx1 = Physics2D.Raycast(now - new Vector2(0, 0.04f + ep2), new Vector2((pos - now).x, 0).normalized, 0.05f + ep, rayMask);
            hitx2 = Physics2D.Raycast(now + new Vector2(0, 0.04f + ep2), new Vector2((pos - now).x, 0).normalized, 0.05f + ep, rayMask);
            hity0 = Physics2D.Raycast(now, new Vector2(0, (pos - now).y - 0.09f).normalized, 0.04f + ep, rayMask);
            hity1 = Physics2D.Raycast(now - new Vector2(0.05f + ep2, 0), new Vector2(0, (pos - now).y - 0.09f).normalized, 0.04f + ep, rayMask);
            hity2 = Physics2D.Raycast(now + new Vector2(0.05f + ep2, 0), new Vector2(0, (pos - now).y - 0.09f).normalized, 0.04f + ep, rayMask);
            if (hitx1 || hitx2 || hitx0)
            {
                xC = true;
            }
            if (hity1 || hity2 || hity0)
            {
                yC = true;
            }

            if (!xC && !yC)
            {
                if (Mathf.Abs(pos.x - start.x) > Mathf.Abs(pos.y - start.y))
                    xfirst = true;
                else
                    xfirst = false;
            }
            else if (xC)
                xfirst = false;
            else if (yC)
                xfirst = true;
            else
            {
                //xy都被阻拦，智能寻路之后完善
                xfirst = true;
            }

            return xfirst;
        }

        private void OnEnable()
        {

            AudioListener a = gameObject.GetComponent<AudioListener>();
            if (a != null)
            {
                Camera.main.GetComponent<AudioListener>().enabled = false;
                a.enabled = true;
            }
        }

        private void OnDisable()
        {
            AudioListener a = gameObject.GetComponent<AudioListener>();
            if (a != null)
            {
                a.enabled = false;
                if (Camera.main != null)
                    Camera.main.GetComponent<AudioListener>().enabled = true;
            }
        }
    }
}
