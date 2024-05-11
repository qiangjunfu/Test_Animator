using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackTarget : Action
{
    public SharedGameObject target; // 攻击目标
    public SharedFloat attackDis = 2;


    public override void OnStart() 
    {
        Debug.LogFormat("开始攻击:  " + target.Value.name);
        int id = this.gameObject.GetComponent<PlayerCtrl>().Id;
        MessageManager.Broadcast<int, int>(GameEventType.EnemyStateChange, 7, id);
    }

    public override TaskStatus OnUpdate()
    {
        // 检查目标是否存在
        if (target.Value == null ) // || target.Value.GetComponent<Health>().IsDead)
        {
            return TaskStatus.Failure; // 目标死亡，返回失败
        }

        // 计算目标与AI的距离
        float distanceToTarget = Vector3.Distance(transform.position, target.Value.transform.position);
        if (distanceToTarget > attackDis.Value )
        {
            return TaskStatus.Failure; // 目标超出攻击范围，返回失败
        }

        // 进行攻击
        // Debug.Log("Attacking " + target.Value.name);
        return TaskStatus.Running; // 攻击正在进行中
    }
}

