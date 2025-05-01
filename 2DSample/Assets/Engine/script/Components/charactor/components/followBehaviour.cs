using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actors
{
    public class followBehaviour : ActorComponent
    {
        protected bool canDo = false;
        public bool CanDo
        {
            get
            {
                return canDo;
            }
        }

        protected List<followBehaviour> follows = new List<followBehaviour>();
        public List<followBehaviour> Follows
        {
            get
            {
                return follows;
            }
        }

        protected int followIndex = 0;
        protected bool canRunFollow = false;


        protected GameObject player;

        override public void doEveryFrame()
        {
            base.doEveryFrame();

            if (!useComponent)
                return;
            
            if (canDo)
            {
                Vector2 now = transform.position;
                Vector2 pos = player.transform.position;
                if (canRunFollow && Vector2.Distance(pos, now) > getSystemSetting().runFollowDis)
                    getMove().MoveTo(pos, true);
                else if (Vector2.Distance(pos, now) > getSystemSetting().followEpsilon)
                    getMove().MoveTo(pos, false);
                else
                {
                    getMove().Move(Vector2.zero);
                    getMove().StopMoveTo();
                }
            }
            else
            {
                getRigidbody().velocity = Vector3.zero;
            }
        }

        protected override void setData(ListNodeInterface n)
        {
            canRunFollow = n.getData<bool>(structProperty.bool1);
        }

        public void changeState(bool canDo, GameObject _object = null)
        {
            this.canDo = canDo;
            gameObject.layer = LayerMask.NameToLayer(HashsAndTags.npc);
            if (canDo)
            {
                if (_object != null)
                {
                    player = _object;
                }
                gameObject.layer = LayerMask.NameToLayer(HashsAndTags.follow);
                getMove().RayMask = (1 << LayerMask.NameToLayer(HashsAndTags.Collider));
            }
            else
            {
                gameObject.layer = LayerMask.NameToLayer(HashsAndTags.npc);
                getMove().RayMask = ((1 << LayerMask.NameToLayer(HashsAndTags.Collider)) | (1 << LayerMask.NameToLayer(HashsAndTags.player)));
                getMove().Target = transform.position;
            }
        }


        private void OnEnable()
        {
            int num = follows.Count;
            for (int i = 0; i < num; i++)
            {
                if (follows[i].gameObject != null)
                {
                    follows[i].gameObject.SetActive(true);
                }
                else
                    removeFollow(i);
            }
        }

        private void OnDisable()
        {
            int num = follows.Count;
            for (int i = 0; i < num; i++)
            {
                if (follows[i].gameObject != null)
                    follows[i].gameObject.SetActive(false);
                else
                    removeFollow(i);
            }
        }

        private void OnDestroy()
        {
            int num = follows.Count;
            for (int i = 0; i < num; i++)
            {
                removeFollow(i);
            }

            if (player != null)
                player.gameObject.GetComponent<followBehaviour>().removeFollow(followIndex);
        }


        public void addFollow(GameObject npc)
        {
            var f = npc.GetComponent<followBehaviour>();
            if (f != null)
            {
                follows.Add(f);
                f.setUseable(true);
                f.changeState(true, this.gameObject);
                f.followIndex = follows.Count - 1;
            }

        }

        public void removeFollow(GameObject npc)
        {
            var f = npc.GetComponent<followBehaviour>();
            if(f!=null)
            {
                f.setUseable(false);
                removeFollow(follows.IndexOf(f));
            }              
        }

        public void removeFollow(int index)
        {
            if (follows[index] != null)
                follows[index].changeState(false);
            follows.RemoveAt(index);
        }

        public void changeFollowsPos(turn turn, Vector2 t, string step = "", string runStep = "")
        {
            int num = follows.Count;
            var f = turnTool.get(turn);
            Vector2 e = new Vector2(f.x, f.y);
            for (int i = 0; i < num; i++)
            {
                follows[i].gameObject.transform.position = new Vector3(t.x - e.x * 0.1f, t.y - e.y * 0.1f, transform.position.z);
                follows[i].getMove().changeTurn(getMove().Turn);
                if (follows[i].getMove().Step != "")
                {
                    follows[i].getMove().Step = step;
                    follows[i].getMove().RunStep = runStep;
                }
            }
        }
    }
}
