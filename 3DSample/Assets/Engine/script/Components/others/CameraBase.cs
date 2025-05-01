using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBase : basicComponent
{
    public bool Active = false;
    public float followSpeed = 20;

    public float everyShakeTime = 0.1f;

    [HideInInspector]
    public GameObject player = null;

    protected Vector3 target, move = Vector3.zero;
    // ÕðÆÁÐ§¹û
    protected Vector3 deltaPos = Vector3.zero, oriPosition;
    protected float shakeTime = 1.0f, timer = 0, timer2 = 0, shakeHard = 1.0f;

    //Õð¶¯ÆÁÄ»
    public void shake(float hard, float time)
    {
        oriPosition = transform.localPosition;
        shakeHard = hard;
        shakeTime = time;
        deltaPos = new Vector3(0.1f, 0, 0);
    }

    public void MoveTo(Vector2 t, float time)
    {
        Vector3 now = transform.localPosition;
        target = new Vector3(t.x, t.y, now.z);
        move = (target - now) / time;
    }

    public override void doEveryFrame()
    {
        if (move != Vector3.zero)
        {
            transform.localPosition += move * Time.deltaTime;
            if (Vector3.Distance(transform.localPosition, target) < 0.05f)
            {
                transform.localPosition = target;
                move = Vector3.zero;
            }
        }
        if (deltaPos != Vector3.zero)
        {
            timer += Time.deltaTime;
            timer2 += Time.deltaTime;

            transform.localPosition += deltaPos * shakeHard / everyShakeTime * Time.deltaTime;
            if (timer2 > everyShakeTime)
            {
                deltaPos *= -1;
                timer2 = 0;
            }

            if (timer >= shakeTime)
            {
                deltaPos = Vector3.zero;
                timer = 0;
                timer2 = 0;
                transform.localPosition = oriPosition;
            }
        }
    }

    private void Update()
    {
        doEveryFrame();
    }
}
