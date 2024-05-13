using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BehaviorDesigner.Runtime.Tasks.Movement.Patrol__2;


public class DetectionAttackDis : Conditional
{
    public SharedGameObject targetObj;
    public SharedFloat attackDis = 2;
    float distance = 0;


    private NPCState currentState;
    private bool stateChanged = true; // ��־λ������ȷ��״̬�ı�ֻ��ӡһ��


    public override void OnStart()
    {
        base.OnStart();
    }


    public override TaskStatus OnUpdate()
    {
        if (targetObj.Value == null) return TaskStatus.Failure;


        distance = Vector3.Distance(this.gameObject.transform.position, targetObj.Value.transform.position);
        if (distance > attackDis.Value)
        {
            //Debug.LogFormat("DetectionAttackDis --> ������������ --- ");
            return TaskStatus.Failure;
        }

        return TaskStatus.Success;
    }
}
