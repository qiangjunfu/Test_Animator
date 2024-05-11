using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackTarget : Action
{
    public SharedGameObject target; // ����Ŀ��
    public SharedFloat attackDis = 2;


    public override void OnStart() 
    {
        Debug.LogFormat("��ʼ����:  " + target.Value.name);
        int id = this.gameObject.GetComponent<PlayerCtrl>().Id;
        MessageManager.Broadcast<int, int>(GameEventType.EnemyStateChange, 7, id);
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

