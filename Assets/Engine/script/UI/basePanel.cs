using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basePanel : MonoBehaviour
{
    private Vector3 startScale = new Vector3(1, 1, 1);
    private Vector3 startPosition = new Vector3(0, 0, 0);
    protected AudioSource _audio=null;

    private void Awake()
    {
        startScale = transform.localScale;
        startPosition = new Vector3(0, 0, transform.localPosition.z);
    }

    private void Start()
    {
        _audio = gameObject.GetComponent<AudioSource>();
        init();
    }

    private void OnEnable()
    {
        changeSize();
        ableInit();
    }

    virtual protected void init() { }
    virtual protected void ableInit() { }

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
                transform.localScale = startScale / gameManager.instance.oriSize * gameManager.instance.setting.windowSize;
                transform.localPosition = startPosition / gameManager.instance.oriSize * gameManager.instance.setting.windowSize;
            }
        }
    }

    protected void playSE(AudioClip clip)
    {
        if (_audio != null)
        {
            _audio.volume = gameManager.instance.setting.SEValue;
            _audio.clip = clip;
            _audio.Play();
        }
    }
}
