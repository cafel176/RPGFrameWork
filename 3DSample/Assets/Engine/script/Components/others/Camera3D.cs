using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera3D : CameraBase
{
    public Vector3 offset = new Vector3(3, 5, 3);

    public bool canScale = false;
    // 控制旋转速度
    public float rotateSpeed = 2.0f;

    private dataComponent data = new dataComponent();

    protected override void onStart()
    {
        if (player != null)
        {
            transform.position = player.transform.position + offset;
            gameObject.transform.LookAt(player.transform);
        }
    }

    public override void doEveryFrame()
    {
        if(player!=null)
            transform.position = player.transform.position + offset;

        base.doEveryFrame();
    }

    public void CameraRotate(float _mouseX, float _mouseY)
    {
        if (!Active || !data.getMouseInputs().Contains(mouseInput.left) || player==null)
            return;

        transform.RotateAround(player.transform.position, Vector3.up, _mouseX * rotateSpeed);
        transform.RotateAround(player.transform.position, transform.right, -_mouseY * rotateSpeed);
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
