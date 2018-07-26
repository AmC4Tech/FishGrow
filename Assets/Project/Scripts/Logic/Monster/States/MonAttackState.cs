using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonAttackState : MonBaseState {

    private float timeCount;
    public override bool OnEnter(SMEvent smEvent)
    {
        timeCount = 0f;
        monCtrl.PlayAnim("monsterAttack");
        monCtrl.AttackTarget();
        return base.OnEnter(smEvent);
    }


    public override bool OnUpdate(SMEvent smEvent)
    {
        if (timeCount >= monCtrl.attackCD) {
            monCtrl.stateMachine.DoEvent("move");
        }
        timeCount += Time.deltaTime;
        return base.OnUpdate(smEvent);
    }
}
