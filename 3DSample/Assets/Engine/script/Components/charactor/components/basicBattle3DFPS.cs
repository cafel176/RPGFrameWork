using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actors
{
    public class basicBattle3DFPS : ActorBattle
    {
        private float attackForce = 5;
        private string bullet;

        private Camera _camera;

        override protected void onAwake()
        {
            _camera = gameObject.GetComponentInChildren<Camera>();
        }

        protected override void setData(ListNodeInterface n)
        {
            base.setData(n);

            attackForce = n.getData<float>(structProperty.float2);
            bullet = n.getData<string>(structProperty.str1);
        }

        public override void Attack()
        {
            if(a.getGameSetting().canAttack && attackTimer>=attackTime)
            {
                attackTimer = 0;

                GameObject go = a.spawnPrefab(bullet);
                Ray r = _camera.ScreenPointToRay(new Vector2(Screen.width * 0.5f, Screen.height * 0.5f));
                go.transform.position = r.origin;
                go.GetComponent<Rigidbody>().velocity = r.direction * attackForce;
            }
        }


    }
}
