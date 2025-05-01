using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detectHit : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag==HashsAndTags.playerBullet)
        {
            MIFactory.getMI().setSwitch("hit", true);
        }
    }
}
