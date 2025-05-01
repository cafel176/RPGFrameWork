using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//相机跟随
public class CameraFollow : MonoBehaviour {

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    public bool Active = false;
    public float followSpeed=20;

    public float everyShakeTime = 0.1f;

    [HideInInspector]
    public GameObject player = null;

    private Vector3 target, move = Vector3.zero;
    // 震屏效果
    private Vector3 deltaPos = Vector3.zero,oriPosition;
    private float shakeTime=1.0f,timer = 0,timer2=0,shakeHard=1.0f;

    private void Update()
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
            timer+= Time.deltaTime;
            timer2 += Time.deltaTime;

            transform.localPosition += deltaPos * shakeHard / everyShakeTime * Time.deltaTime;
            if (timer2>everyShakeTime)
            {
                deltaPos *= -1;
                timer2 = 0;
            }

            if (timer>=shakeTime)
            {
                deltaPos = Vector3.zero;
                timer = 0;
                timer2 = 0;
                transform.localPosition = oriPosition;
            }
        }
    }

    //震动屏幕
    public void shake(float hard,float time)
    {
        oriPosition = transform.localPosition;
        shakeHard = hard;
        shakeTime = time;
        deltaPos = new Vector3(0.1f,0,0);
    }

    public void MoveTo(Vector2 t,float time)
    {
        Vector3 now = transform.localPosition;
        target = new Vector3(t.x,t.y,now.z);
        move = (target - now) / time;
    }

    private void LateUpdate()
    {
        if (Active && player!=null)
        {
            Vector2 result = Vector2.Lerp(transform.position, player.transform.position, followSpeed * Time.deltaTime);
            //Vector2 result = player.transform.position;
            if (result.x > minX && result.x < maxX)
            {
                if (result.y > minY && result.y < maxY)
                    transform.position = new Vector3(result.x, result.y, transform.position.z);
                else if (result.y <= minY)
                    transform.position = new Vector3(result.x, minY, transform.position.z);
                else if (result.y >= maxY)
                    transform.position = new Vector3(result.x, maxY, transform.position.z);
            }
            else if (result.x <= minX)
            {
                if (result.y > minY && result.y < maxY)
                    transform.position = new Vector3(minX, result.y, transform.position.z);
                else if (result.y <= minY)
                    transform.position = new Vector3(minX, minY, transform.position.z);
                else if (result.y >= maxY)
                    transform.position = new Vector3(minX, maxY, transform.position.z);
            }
            else if (result.x >= maxX)
            {
                if (result.y > minY && result.y < maxY)
                    transform.position = new Vector3(maxX, result.y, transform.position.z);
                else if (result.y <= minY)
                    transform.position = new Vector3(maxX, minY, transform.position.z);
                else if (result.y >= maxY)
                    transform.position = new Vector3(maxX, maxY, transform.position.z);
            }

        }
    }
}
