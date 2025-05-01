using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actors
{
    [RequireComponent(typeof(BoxCollider))]
    public class FootTrigger : MonoBehaviour
    {
        private bool canJumpNow = false;
        public bool CanJumpNow
        {
            get
            {
                return canJumpNow;
            }
        }

        private List<Collider> objs = new List<Collider>();

        private void OnTriggerExit(Collider other)
        {
            if (objs.Contains(other) && other.tag != HashsAndTags.player)
            {
                objs.Remove(other);
                if(objs.Count==0)
                    canJumpNow = false;
            }
                
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!objs.Contains(other) && other.tag != HashsAndTags.player)
            {
                objs.Add(other);
                canJumpNow = true;
            }               
        }

        private void OnTriggerStay(Collider other)
        {
            if (!objs.Contains(other) && other.tag!=HashsAndTags.player)
            {
                objs.Add(other);
                canJumpNow = true;
            }
                
        }
    }
}
