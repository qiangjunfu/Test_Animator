using System.Collections;
using System.Collections.Generic;
using UnityEngine;


  //if (Input.GetMouseButton(0))
public class CameraCtrl : MonoBehaviour
{
  
    public float moveSpeed = 10f;          // ����ƶ��ٶ�
    public float rotationSpeed = 100f;     // �����ת�ٶ�
    public float zoomSpeed = 10f;          // ��������ٶ�
    public float zoomDistanceMin = 1f;     // �����С����
    public float zoomDistanceMax = 10f;    // ���������
    public LayerMask collisionLayer;       // ��ײ�㼶
    public float collisionRadius = 0.5f;   // ��ײ���뾶

    private Vector3 targetPosition;        // ���Ŀ��λ��
    private Quaternion targetRotation;     // ���Ŀ����ת
    private float targetZoomDistance = 5f; // ���Ŀ�����ž���

    void Start()
    {
        targetPosition = transform.position;
        targetRotation = transform.rotation;
    }

    void Update()
    {
        // ��������ƶ�
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
        targetPosition += transform.TransformDirection(direction) * moveSpeed * Time.deltaTime;

        // ���������ת
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        targetRotation *= Quaternion.Euler(-mouseY * rotationSpeed * Time.deltaTime, mouseX * rotationSpeed * Time.deltaTime, 0f);

        // �����������
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        targetZoomDistance = Mathf.Clamp(targetZoomDistance - scrollWheel * zoomSpeed, zoomDistanceMin, zoomDistanceMax);

        // ��ײ���
        RaycastHit hit;
        if (Physics.SphereCast(targetPosition, collisionRadius, -transform.forward, out hit, targetZoomDistance, collisionLayer))
        {
            targetZoomDistance = hit.distance;
        }

        // �������λ�ú���ת
        transform.position = targetPosition - transform.forward * targetZoomDistance;
        transform.rotation = targetRotation;
    }
}
