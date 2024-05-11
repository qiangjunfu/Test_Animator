using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DetectionAttackDis : Conditional
{
    public SharedGameObject targetObj;
    public SharedFloat attackDis = 2;
    float distance = 0;


    public override void OnAwake()
    {
        
    }


    public override TaskStatus OnUpdate()
    {
        if (targetObj.Value == null)
            return TaskStatus.Failure;

        distance = Vector3.Distance(this.gameObject.transform.position, targetObj.Value.transform.position);
        if (distance > attackDis.Value)
        {
            //Debug.LogFormat("DetectionAttackDis --> ³¬³ö¹¥»÷¾àÀë --- ");
            return TaskStatus.Failure;
        }

        return TaskStatus.Success;
    }
}
