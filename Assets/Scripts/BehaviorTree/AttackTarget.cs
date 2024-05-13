using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BehaviorDesigner.Runtime.Tasks.Movement.Patrol__2;


public class AttackTarget : Action
{
    public SharedGameObject target; // ����Ŀ��
    public SharedFloat attackDis = 2;


    private NPCState currentState = NPCState.Attack ;


    public override void OnStart() 
    {
        Debug.LogFormat("��ʼ����: {0} , {1}", currentState, target.Value.name);
        int id = this.gameObject.GetComponent<PlayerCtrl>().Id;
        MessageManager.Broadcast<NPCState, int>(GameEventType.EnemyStateChange, currentState, id);
    }

    public override TaskStatus OnUpdate()
    {
        // ���Ŀ���Ƿ����
        if (target.Value == null ) // || target.Value.GetComponent<Health>().IsDead)
        {
            return TaskStatus.Failure; // Ŀ������������ʧ��
        }

        // ����Ŀ����AI�ľ���
        float distanceToTarget = Vector3.Distance(transform.position, target.Value.transform.position);
        if (distanceToTarget > attackDis.Value )
        {
            return TaskStatus.Failure; // Ŀ�곬��������Χ������ʧ��
        }

        // ���й���
        // Debug.Log("Attacking " + target.Value.name);
        return TaskStatus.Running; // �������ڽ�����
    }
}

