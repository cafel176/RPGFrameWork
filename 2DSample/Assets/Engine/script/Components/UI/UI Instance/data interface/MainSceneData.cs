using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class MainSceneData : basePanelData
    {
        public void showSplach(GameObject[] logos)
        {
            MI.showSplach(logos);
        }

        public void startGame()
        {
            MI.startGame();
        }
    }
}
