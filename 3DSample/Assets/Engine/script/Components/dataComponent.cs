using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dataComponent
{
    protected managerInterface MI
    {
        get
        {
            return MIFactory.getMI();
        }
    }

    public bool canUse()
    {
        if (MI == null)
            return false;
        else
            return true;
    }

    public HashsAndTags getHat()
    {
        return MI.getHat();
    }

    public settings getSetting()
    {
        return MI.getSetting();
    }

    public systemSetting getSystemSetting()
    {
        return MI.getSystemSetting();
    }

    public void changeState(nowState ns, GameObject g = null)
    {
        MI.changeState(ns, g);
    }

    public void LoadLevel(string level, changeSceneDo csd, bool stopMusic = true)
    {
        MI.LoadLevel(level, csd, stopMusic);
    }

    public void beBlack(float time, bool push = true, bool black = true)
    {
        MI.beBlack(time, black);
    }

    //淡出，卡进程
    public void beWhite(float time, bool push = true, bool black = true)
    {
        MI.beWhite(time, black);
    }

    public List<mouseInput> getMouseInputs()
    {
        return MI.getMouseInputs();
    }

    public List<keyInput> getInputs()
    {
        return MI.getInputs();
    }
}
