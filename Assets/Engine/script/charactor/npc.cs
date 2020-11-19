using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum npcState
{
    none,
    randomMove,
    follow
}

public class npc : Player
{
    //该npc是否会随机移动
    public npcState nowState= npcState.none;

    public float followEpsilon = 0.1f;
    public float runFollowDis = 0.5f;

    public int followIndex = 0;
    public bool canRunFollow = false;

    private GameObject player;

    protected override void init()
    {
        rayMask = (1 << LayerMask.NameToLayer("collider")) | (1 << LayerMask.NameToLayer("player"));
    }

    override protected void doSth()
    {
        if (nowState == npcState.follow)
        {
            Vector2 now = transform.position;
            Vector2 pos = player.transform.position;
            if (canRunFollow && Vector2.Distance(pos, now) > runFollowDis)
                MoveTo(pos, true);
            else if (Vector2.Distance(pos, now) > followEpsilon)
                MoveTo(pos, false);
            else
            {
                Move(Vector2.zero);
                startMove = false;
            }
        }
        else if (nowState == npcState.none)
        {
            _rigidbody.velocity = Vector3.zero;
        }
    }

    public void changeState(npcState state, GameObject _object = null)
    {
        nowState = state;
        gameObject.layer = LayerMask.NameToLayer("npc");
        if (state == npcState.follow)
        {
            if (_object != null)
            {
                player = _object;
            }
            gameObject.layer = LayerMask.NameToLayer("follow");
            rayMask = (1 << LayerMask.NameToLayer("collider"));
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("npc");
            rayMask = (1 << LayerMask.NameToLayer("collider")) | (1 << LayerMask.NameToLayer("player"));
            target = transform.position;
        }
    }

    private void OnDestroy()
    {
        if (nowState == npcState.follow)
        {
            if(player!=null)
                player.gameObject.GetComponent<Player>().removeFollow(followIndex);
        }
    }
}
