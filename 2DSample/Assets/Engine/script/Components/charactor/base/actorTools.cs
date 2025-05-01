using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actors
{
    public class actorTools : dataComponent
    {
        public AudioClip findAudio(string name)
        {
            return MI.findAudio(name); 
        }
    }
}
