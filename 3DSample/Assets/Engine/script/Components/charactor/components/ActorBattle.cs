using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actors
{
    public abstract class ActorBattle : ActorComponent, ActorBattleInterface
    {
        protected float attackTime = 0.3f;
        protected float attackTimer = 0;

        protected override void setData(ListNodeInterface n)
        {
            attackTime = n.getData<float>(structProperty.float1);
        }

        override public void doEveryFrame()
        {
            attackTimer += Time.deltaTime;
        }

        public abstract void Attack();
    }
}
