using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 1;
    public float runSpeed = 2;
    public float rushDis = 1;
    public float rushTime = 0.3f;

    public Sprite[] idle = new Sprite[3];//0左1上2下
    public float distanceEpsilon = 1e-2f;
    public List<npc> follows = new List<npc>();
    public bool left = true;
    public bool canRun = true;
    public bool useIdleAnima = false;
    public AudioClip step;
    public AudioClip runStep;

    [HideInInspector]
    public Vector2 turn = new Vector2(0, -1);

    protected float moveEpsilon = 1e-3f;
    protected Rigidbody2D _rigidbody;
    protected Animator anima;
    protected HashsAndTags hat;
    protected SpriteRenderer render;
    protected AudioSource _audio;
    protected Vector2 target, start;
    protected bool startMove = false;
    protected bool runMove = false;
    protected int rayMask;
    protected float rushTimer = 0;

    void Awake()
    {
        rushTimer = rushTime;
        hat = gameManager.instance.hat;
        _rigidbody = gameObject.GetComponent<Rigidbody2D>();
        anima = gameObject.GetComponent<Animator>();
        render = gameObject.GetComponent<SpriteRenderer>();
        _audio = gameObject.GetComponent<AudioSource>();
        _audio.volume = gameManager.instance.setting.SEValue;

        if (!useIdleAnima)
        {
            anima.enabled = false;
            if (turn.y > moveEpsilon)
                render.sprite = idle[1];
            else if (turn.y < -moveEpsilon)
                render.sprite = idle[2];
            else
            {
                if (turn.x > moveEpsilon)
                    render.flipX = left;
                else if (turn.x < -moveEpsilon)
                    render.flipX = !left;
                render.sprite = idle[0];
            }
        }
        else
        {
            playSE(step, true);
            anima.enabled = true;
            if (turn.y > moveEpsilon)
                anima.SetFloat(hat.idleAnima, 1);
            else if (turn.y < -moveEpsilon)
                anima.SetFloat(hat.idleAnima, 2);
            else
            {
                if (turn.x > moveEpsilon)
                    render.flipX = left;
                else if (turn.x < -moveEpsilon)
                    render.flipX = !left;
                anima.SetFloat(hat.idleAnima, 0);
            }
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
        rayMask = (1 << LayerMask.NameToLayer("collider")) | (1 << LayerMask.NameToLayer("npc"));

        init();
    }

    virtual protected void init() { }
    virtual protected void doSth() { }

    private void Update()
    {
        rushTimer += Time.deltaTime;
        doSth();
        if (startMove)
        {
            Vector2 now = transform.position;
            Vector2 temp = target - now;
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
                        _rigidbody.mass = 100000;
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
                        _rigidbody.mass = 100000;
                    }
                }
            }
        }
    }

    private void OnEnable()
    {
        int num = follows.Count;
        for (int i = 0; i < num; i++)
        {
            if (follows[i].gameObject != null)
            {
                follows[i].gameObject.SetActive(true);
            }
            else
                removeFollow(i);
        }

        AudioListener a = gameObject.GetComponent<AudioListener>();
        if (a != null)
        {
            Camera.main.GetComponent<AudioListener>().enabled = false;
            a.enabled = true;
        }
    }

    private void OnDisable()
    {
        int num = follows.Count;
        for (int i = 0; i < num; i++)
        {
            if (follows[i].gameObject != null)
                follows[i].gameObject.SetActive(false);
            else
                removeFollow(i);
        }

        AudioListener a = gameObject.GetComponent<AudioListener>();
        if (a != null)
        {
            a.enabled = false;
            if (Camera.main != null)
                Camera.main.GetComponent<AudioListener>().enabled = true;
        }
    }

    private void OnDestroy()
    {
        int num = follows.Count;
        for (int i = 0; i < num; i++)
        {
            removeFollow(i);
        }
    }

    public void changeTurn(Vector2 turn)
    {
        Move(turn);
        Move(Vector2.zero);
    }

    public void rush()
    {
        transform.position += new Vector3(turn.x, turn.y, 0) * rushDis;
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
    }

    public void Move(Vector2 vec, bool run = false, bool _rush = false)
    {
        bool rush = false;
        if (rushTimer > rushTime)
        {
            rushTimer = 0;
            rush = _rush;
        }
        else
            rush = false;
        //移动
        if (Vector2.Distance(Vector2.zero, vec) > 1.5 * moveEpsilon)
        {
            _rigidbody.mass = 10;
            //控制方向
            if (Mathf.Abs(vec.x) > moveEpsilon)
            {
                if (vec.x > moveEpsilon)
                {
                    render.flipX = left;
                    turn = new Vector2(1, 0);
                }
                else if (vec.x < -moveEpsilon)
                {
                    render.flipX = !left;
                    turn = new Vector2(-1, 0);
                }
            }
            else
            {
                //render.flipX = false;
                if (vec.y > moveEpsilon)
                {
                    turn = new Vector2(0, 1);
                }
                else if (vec.y < -moveEpsilon)
                {
                    turn = new Vector2(0, -1);
                }
            }
            //控制速度
            //if (!rush)
            _rigidbody.velocity = turn * 0.5f * (canRun && run ? runSpeed : speed);
            anima.enabled = true;
            anima.SetBool(hat.runX, false);
            anima.SetBool(hat.runY, false);
            anima.SetBool(hat.Xspeed, false);
            anima.SetBool(hat.YspeedUp, false);
            anima.SetBool(hat.YspeedDown, false);

            if (Mathf.Abs(turn.x) > moveEpsilon)
            {
                anima.SetBool(hat.Xspeed, true);
                if (rush)
                {
                    anima.SetTrigger(hat.rushX);
                }
                else if (canRun && run)
                {
                    anima.SetBool(hat.runX, true);
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
                anima.SetBool(hat.YspeedUp, true);
                if (rush)
                {
                    anima.SetTrigger(hat.rushY);
                }
                else if (canRun && run)
                {
                    anima.SetBool(hat.runY, true);
                    playSE(runStep, true);
                }
                else
                    playSE(step, true);
            }
            else if (turn.y < -moveEpsilon)
            {
                anima.SetBool(hat.YspeedDown, true);
                if (rush)
                {
                    anima.SetTrigger(hat.rushY);
                }
                else if (canRun && run)
                {
                    anima.SetBool(hat.runY, true);
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
            _rigidbody.mass = 100000;
            _rigidbody.velocity = Vector2.zero;
            if (!useIdleAnima)
            {
                stopSE();
                anima.enabled = false;
                if (turn.y > moveEpsilon)
                    render.sprite = idle[1];
                else if (turn.y < -moveEpsilon)
                    render.sprite = idle[2];
                else
                {
                    if (turn.x > moveEpsilon)
                        render.flipX = left;
                    else if (turn.x < -moveEpsilon)
                        render.flipX = !left;
                    render.sprite = idle[0];
                }
            }
            else
            {
                playSE(step, true);
                anima.enabled = true;
                anima.SetBool(hat.runX, false);
                anima.SetBool(hat.runY, false);
                anima.SetBool(hat.Xspeed, false);
                anima.SetBool(hat.YspeedUp, false);
                anima.SetBool(hat.YspeedDown, false);
                if (turn.y > moveEpsilon)
                    anima.SetFloat(hat.idleAnima, 1);
                else if (turn.y < -moveEpsilon)
                    anima.SetFloat(hat.idleAnima, 2);
                else
                {
                    if (turn.x > moveEpsilon)
                        render.flipX = left;
                    else if (turn.x < -moveEpsilon)
                        render.flipX = !left;
                    anima.SetFloat(hat.idleAnima, 0);
                }
            }
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
    }

    public void MoveTo(Vector2 pos, bool run = false)
    {
        target = pos;
        start = transform.position;
        startMove = true;
        _rigidbody.mass = 10;
        runMove = run;
    }

    public void addFollow(GameObject npc)
    {
        follows.Add(npc.GetComponent<npc>());
        npc.GetComponent<npc>().changeState(npcState.follow, this.gameObject);
        npc.GetComponent<npc>().followIndex = follows.Count - 1;
    }

    public void removeFollow(int index)
    {
        if (follows[index] != null)
            follows[index].changeState(npcState.none);
        follows.RemoveAt(index);
    }

    protected void playSE(AudioClip clip, bool loop = false)
    {
        if (_audio != null && gameObject.activeInHierarchy)
        {
            _audio.loop = loop;
            _audio.volume = gameManager.instance.setting.SEValue;
            _audio.clip = clip;
            if (!_audio.isPlaying)
                _audio.Play();
        }
    }

    protected void stopSE()
    {
        if (_audio != null)
        {
            _audio.loop = false;
            _audio.Stop();
        }
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
            npc a = gameObject.GetComponent<npc>();
            if (a != null && a.nowState == npcState.follow)
                xfirst = true;
            else if (Mathf.Abs(pos.x - start.x) > Mathf.Abs(pos.y - start.y))
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
}
