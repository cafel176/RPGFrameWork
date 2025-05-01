using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera3DFPS : CameraBase
{
    public bool canScale = false;
    // 控制旋转速度
    public float rotateSpeed = 2.0f;

    protected override void onStart()
    {
        player = transform.parent.gameObject;
    }

    public void CameraRotate(float _mouseX, float _mouseY)
    {
        if (!Active)
            return;
        
        transform.parent.Rotate(Vector3.up, _mouseX * rotateSpeed, Space.World);
        transform.Rotate(transform.right, -_mouseY * rotateSpeed, Space.World);
    }

    // 滚轮缩放
    public void CameraScale(float scale)
    {
        if (!Active || !canScale)
            return;
        //放大
        if (scale < 0)
        {
            transform.position -= 0.5f * transform.forward;
        }
        //缩小
        if (scale > 0)
        {
            transform.position += 0.5f * transform.forward;
        }
    }
}
