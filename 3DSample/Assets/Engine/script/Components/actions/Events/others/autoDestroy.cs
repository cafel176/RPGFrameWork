using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions
{
    public class autoDestroy : MonoBehaviour
    {
        public float time = 5f;

        private void Start()
        {
            Destroy(gameObject, time);
        }
    }
}


