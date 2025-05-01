using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class MainSceneData : basePanelData
    {
        public void showSplach(logoSetting[] logos, Func call)
        {
            MI.showSplach(logos,call);
        }

        public void startGame()
        {
            MI.startGame();
        }
    }
}
