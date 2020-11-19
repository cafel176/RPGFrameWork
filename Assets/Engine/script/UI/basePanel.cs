using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basePanel : MonoBehaviour
{
    private Vector3 startScale = new Vector3(1, 1, 1);
    private Vector3 startPosition = new Vector3(0, 0, 0);

    private void Awake()
    {
        startScale = transform.localScale;
        startPosition = new Vector3(0, 0, transform.localPosition.z);
    }

    private void OnEnable()
    {
        changeSize();
        ableInit();
    }

    virtual protected void ableInit() { }
    virtual protected void ableDis() { }

    public void changeStartScale(Vector3 v)
    {
        startScale = v;
    }

    public void changeStartPos(float x,float y)
    {
        startPosition = new Vector3(x,y, startPosition.z);
    }

    public void changeSize()
    {
        basePanel menu=null;
        try
        {
            menu = transform.parent.gameObject.GetComponent<basePanel>();
            menu.changeSize();
        }
        catch
        {
            if (menu == null)
            {
                float s = gameManager.instance.setting.windowSize;
                if(gameManager.instance.setting.windowSize == gameManager.instance.maxSize)
                {
                    float w=Screen.width / gameManager.instance.width;
                    float h = Screen.height / gameManager.instance.height;
                    s = (w < h) ? w : h;
                }
                transform.localScale = startScale / gameManager.instance.oriSize * s;
                transform.localPosition = startPosition / gameManager.instance.oriSize * s;
            }
        }
    }
}
