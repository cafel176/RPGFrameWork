using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions
{
    public class VisibleInCamera : MonoBehaviour
    {
        public EventTrigger trigger;

    //当物体进入任何相机的视野内时，触发一次该方法，并且方法是挂载在物体上，与相机无关
        private void OnBecameVisible()
        {
            trigger.IsVisible = true;
        }

        //当物体离开任何相机的视野内时，触发一次该方法，并且方法是挂载在物体上，与相机无关
        private void OnBecameInvisible()
        {
            trigger.IsVisible = false;
        }
    }
}

