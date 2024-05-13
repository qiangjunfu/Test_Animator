using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DetectionIsSafety : Conditional
{
    public SharedFloat safeDis;
    public SharedGameObject targetObj;
    float distance = 0;


    public override TaskStatus OnUpdate()
    {
        if (targetObj.Value == null) return TaskStatus.Failure;

        distance = Vector3.Distance(this.gameObject.transform.position, targetObj.Value.transform.position);
        if (distance > safeDis.Value)
        {
            return TaskStatus.Failure;
        }

        return TaskStatus.Success;
    }
}
