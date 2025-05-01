using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions
{
    public class startEventTrigger : startEvents
    {
        private EventTrigger a;

        protected override void onStart()
        {
            a = gameObject.GetComponent<EventTrigger>();
        }

        protected virtual EventTrigger getA()
        {
            return a;
        }

        override public void start(string index)
        {
            canDoSth = true;
            var a_ = getA();
            a_.setCanDo(true);
            a_.showCanDo();
        }

        override public bool checkStart(start howToStart)
        {
            return checkIn();
        }

        protected virtual bool checkIn()
        {
            if (a.Player == null)
                return false;

            SphereCollider c = gameObject.GetComponent<SphereCollider>();
            var pos = a.Player.transform.position;
            var p = transform.TransformPoint(c.center);
            bool can = Vector3.Distance(pos, p) < c.radius;
            return can;
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.tag == HashsAndTags.player)
            {
                a.Player = collision.gameObject;

                start("");
            }
        }

        private void OnTriggerStay(Collider collision)
        {
            if (collision.tag == HashsAndTags.player)
            {
                a.Player = collision.gameObject;

                start("");
            }
        }

        private void OnTriggerExit(Collider collision)
        {
            if (collision.tag == HashsAndTags.player)
            {
                canDoSth = false;
                a.setCanDo(true);
                a.hideCanDo();
                StartCoroutine(aaa());
            }
        }

        protected IEnumerator aaa()
        {
            yield return new WaitForSeconds(0.11f);
            var a_ = getA();
            if (a_.getNowState() == nowState.text && a_.checkTextRequet(this))
            {
                a_.hideTextPanel();
                a_.startUserControl();
            }
        }
    }
}
