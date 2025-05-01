using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class loadPanelData : basePanelData
    {
        public GameObject getPlayer()
        {
            return MI.getPlayer();
        }

        public void startGame()
        {
            MI.startGame();
        }

        public PlayerInfo[] getPlayerInfos()
        {
            return MI.getSystemSetting().PlayerInfos;
        }

        public saveData readFile(string file)
        {
            return MI.readFile(file);
        }

        public void loadData(string file)
        {
            MI.loadData(file);
        }

        public void deleteData(string file)
        {
            MI.deleteData(file);
        }

        public void saveData(string fileName)
        {
            MI.saveData(fileName);
        }

        public void deleteFile(string file)
        {
            MI.deleteFile(file);
        }

        public void setNowStartSetting(int i)
        {
            MI.setNowStartSetting(i);
        }
    }
}
