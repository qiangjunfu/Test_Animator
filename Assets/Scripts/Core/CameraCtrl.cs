using System.Collections;
using System.Collections.Generic;
using UnityEngine;


  //if (Input.GetMouseButton(0))
public class CameraCtrl : MonoBehaviour
{
  
    public float moveSpeed = 10f;          // 相机移动速度
    public float rotationSpeed = 100f;     // 相机旋转速度
    public float zoomSpeed = 10f;          // 相机缩放速度
    public float zoomDistanceMin = 1f;     // 相机最小距离
    public float zoomDistanceMax = 10f;    // 相机最大距离
    public LayerMask collisionLayer;       // 碰撞层级
    public float collisionRadius = 0.5f;   // 碰撞检测半径

    private Vector3 targetPosition;        // 相机目标位置
    private Quaternion targetRotation;     // 相机目标旋转
    private float targetZoomDistance = 5f; // 相机目标缩放距离

    void Start()
    {
        targetPosition = transform.position;
        targetRotation = transform.rotation;
    }

    void Update()
    {
        // 处理相机移动
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
        targetPosition += transform.TransformDirection(direction) * moveSpeed * Time.deltaTime;

        // 处理相机旋转
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        targetRotation *= Quaternion.Euler(-mouseY * rotationSpeed * Time.deltaTime, mouseX * rotationSpeed * Time.deltaTime, 0f);

        // 处理相机缩放
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        targetZoomDistance = Mathf.Clamp(targetZoomDistance - scrollWheel * zoomSpeed, zoomDistanceMin, zoomDistanceMax);

        // 碰撞检测
        RaycastHit hit;
        if (Physics.SphereCast(targetPosition, collisionRadius, -transform.forward, out hit, targetZoomDistance, collisionLayer))
        {
            targetZoomDistance = hit.distance;
        }

        // 更新相机位置和旋转
        transform.position = targetPosition - transform.forward * targetZoomDistance;
        transform.rotation = targetRotation;
    }
}
