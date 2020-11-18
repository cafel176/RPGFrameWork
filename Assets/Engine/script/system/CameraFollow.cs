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

    [HideInInspector]
    public GameObject player = null;

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
