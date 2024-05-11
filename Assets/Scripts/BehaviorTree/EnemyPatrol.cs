using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : Action
{
    public SharedGameObjectList patrolPoints;
    public SharedFloat patrolSpeed = 2.0f;
    private int currentPointIndex = 0;
    private CharacterController controller;
    private bool isWaiting = false;
    private bool isRotating = false;
    private float waitTimer = 0f;

    public float closeEnoughDistance = 1f;
    public float waitTime = 2.0f;
    public float rotationSpeed = 5.0f;
    private Quaternion targetRotation;



    public override void OnStart()
    {
        base.OnStart();

        controller = GetComponent<CharacterController>();

        if (patrolPoints.Value.Count > 0)
        {
            currentPointIndex = Random.Range(0, patrolPoints.Value.Count);
            RotateTowards(patrolPoints.Value[currentPointIndex].transform);
            //Debug.Log($"开始向第一个巡逻点（点{currentPointIndex + 1}）移动。");
        }

        int id = this.gameObject.GetComponent<PlayerCtrl>().Id; 
        MessageManager.Broadcast<int, int>(GameEventType.EnemyStateChange, 1, id);
    }

    public override TaskStatus OnUpdate()
    {
        if (isWaiting)
        {
            HandleWaiting();
        }
        else if (!isRotating)
        {
            PatrolBetweenPoints();
        }
        else
        {
            SmoothRotate();
        }
        return TaskStatus.Running;
    }

    private void PatrolBetweenPoints()
    {
        if (patrolPoints.Value.Count == 0)
            return;

        Transform targetPoint = patrolPoints.Value[currentPointIndex].transform;
        Vector3 moveDirection = (targetPoint.position - transform.position).normalized;
        controller.Move(moveDirection * patrolSpeed.Value * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPoint.position) < closeEnoughDistance && !isWaiting && !isRotating)
        {
            waitTime = Random.Range(2f, 5f);
            waitTimer = waitTime;
            isWaiting = true;
            //Debug.Log($"到达巡逻点 {currentPointIndex + 1}，开始等待。");

            int id = this.gameObject.GetComponent<PlayerCtrl>().Id;
            MessageManager.Broadcast<int, int>(GameEventType.EnemyStateChange, 1, id);
        }
    }

    private void HandleWaiting()
    {
        if (waitTimer > 0)
        {
            waitTimer -= Time.deltaTime;
        }
        else if (isWaiting)
        {
            isWaiting = false;
            int oldPointIndex = currentPointIndex;
            while (currentPointIndex == oldPointIndex)
            {
                currentPointIndex = Random.Range(0, patrolPoints.Value.Count);
            }
            RotateTowards(patrolPoints.Value[currentPointIndex].transform);
            //Debug.Log($"等待结束，开始旋转到新的巡逻点（点{currentPointIndex + 1}）。");
        }
    }

    private void RotateTowards(Transform target)
    {
        targetRotation = Quaternion.LookRotation(target.position - transform.position);
        isRotating = true;
    }

    private void SmoothRotate()
    {
        if (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
        else
        {
            transform.rotation = targetRotation;
            isRotating = false;
            //Debug.Log("旋转完成。");

            int id = this.gameObject.GetComponent<PlayerCtrl>().Id;
            MessageManager.Broadcast<int, int>(GameEventType.EnemyStateChange, 2, id);
        }
    }
}
