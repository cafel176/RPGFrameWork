using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions
{
    public class startEventTrigger : startEvents
    {
        private EventTrigger a;

        [SerializeField]
        private turn needTurn;
        public turn NeedTurn
        {
            get
            {
                return needTurn;
            }
        }

        protected override void onStart()
        {
            a = gameObject.GetComponent<EventTrigger>();
        }

        override public void start(string index)
        {
            canDoSth = true;
            a.setCanDo(true);
            a.showCanDo();
        }

        override public bool checkStart(start howToStart)
        {
            return (checkTurn(howToStart) && checkIn());
        }

        private bool checkTurn(start howToStart)
        {
            if (needTurn == turn.all)
            {
                if (howToStart == global::start.auto)
                    return true;
                Vector2 l = a.Player.transform.position - new Vector3(0, 0.12f, 0);
                l = a.ArcherPos - l;
                Vector2 k = turnTool.get(a.getActorTurn(a.Player));
                if (l.x * k.x + l.y * k.y > 0)
                    return true;
            }
            if (a.getActorTurn(a.Player)== needTurn)
                return true;
            return false;
        }

        private bool checkIn()
        {
            BoxCollider2D c = gameObject.GetComponent<BoxCollider2D>();
            Vector2 d, d1 = transform.TransformPoint(c.offset), d2 = a.Player.transform.position;
            d2.y -= 0.09f;
            d = d1 - d2;
            float bias = 0.01f;
            if (Mathf.Abs(d.x) < c.size.x + 0.05f + bias && Mathf.Abs(d.y) < c.size.y + 0.04f + bias)
            {
                return true;
            }
            else
            {
                if (canDoSth)
                    canDoSth = false;
                a.hideCanDo();
                return false;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == HashsAndTags.player)
            {
                a.Player = collision.gameObject;

                start("");
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.tag == HashsAndTags.player)
            {
                a.Player = collision.gameObject;

                start("");
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == HashsAndTags.player)
            {
                canDoSth = false;
                a.setCanDo(true);
                a.hideCanDo();
                StartCoroutine(aaa());
            }
        }

        IEnumerator aaa()
        {
            yield return new WaitForSeconds(0.11f);
            if (a.getNowState() == nowState.text && a.checkTextRequet(this))
            {
                a.hideTextPanel();
                a.startUserControl();
            }
        }
    }
}
