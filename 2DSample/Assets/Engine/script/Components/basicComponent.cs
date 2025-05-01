using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicComponent : MonoBehaviour
{
    private void Awake()
    {
        onAwake();
    }

    private void Start()
    {
        onStart();
    }

    virtual protected void onAwake()
    {       
    }

    virtual protected void onStart()
    {
    }

    virtual public void doEveryFrame()
    {

    }
}
