using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DetectionDamage : Conditional
{
    public SharedInt fleeHP;
    public SharedInt hp = 100 ;
    public SharedGameObject targetObj;

    public override void OnStart()
    {
        base.OnStart();

    }

    public override TaskStatus OnUpdate()
    {
        //if (targetObj.Value == null) return TaskStatus.Failure;

        if (hp.Value < fleeHP.Value)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;

    }

}
