using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actors
{
    public class actorTools : dataComponent
    {
        public GameObject findPrefab(string name)
        {
            return MI.findPrefab(name);
        }

        public AudioClip findAudio(string name)
        {
            return MI.findAudio(name); 
        }

        public gameSetting getGameSetting()
        {
            return MI.getGameSetting();
        }

        public controlMode GetControlMode()
        {
            return MI.GetControlMode();
        }
    }
}
