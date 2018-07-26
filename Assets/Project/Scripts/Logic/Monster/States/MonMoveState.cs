using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonMoveState : MonBaseState {
    public override bool OnEnter(SMEvent smEvent)
    {
        monCtrl.PlayAnim("monsterIdle");
        return base.OnEnter(smEvent);
    }

    public override bool OnUpdate(SMEvent smEvent)
    {
        if (monCtrl.IsHungry()) {
            if (monCtrl.isHasTarget())
            {
                monCtrl.FollowTarget();
                if (monCtrl.IsReachTarget()) {
                    monCtrl.stateMachine.DoEvent("attack");
                }
            }
            else {
                monCtrl.GetTarget();
            }
        }
        else
            monCtrl.Wander();
        return base.OnUpdate(smEvent);
    }
}
