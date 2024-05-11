using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoSingleTon<InputController> , IBaseScript
{
    [SerializeField] private float horizontal;
    [SerializeField] private float vertical;
    [SerializeField] private float mouseAxisX;
    [SerializeField] private float mouseAxisY;

    [SerializeField] private bool isDownMouse0;
    [SerializeField] private bool isMouse0;
    [SerializeField] private bool isUpMouse0;
    [SerializeField] private float mouseScrollWheel;

    [SerializeField] private bool pressKeyW;
    [SerializeField] private bool pressKeyS;
    [SerializeField] private bool pressKeyA;
    [SerializeField] private bool pressKeyD;
    [SerializeField] private bool pressKeyQ;
    [SerializeField] private bool pressKeyE;
    [SerializeField] private bool pressKeyLeftControl; 
    [SerializeField] private bool pressKeyUpLeftControl; 


    public float Horizontal { get => horizontal; set => horizontal = value; }
    public float Vertical { get => vertical; set => vertical = value; }
    public float MouseAxisX { get => mouseAxisX; set => mouseAxisX = value; }
    public float MouseAxisY { get => mouseAxisY; set => mouseAxisY = value; }
    /// <summary>
    /// ����������
    /// </summary>
    public bool IsDownMouse0 { get => isDownMouse0; set => isDownMouse0 = value; }
    /// <summary>
    /// ��ס������
    /// </summary>
    public bool IsMouse0 { get => isMouse0; set => isMouse0 = value; }
    /// <summary>
    /// ̧��������
    /// </summary>
    public bool IsUpMouse0 { get => isUpMouse0; set => isUpMouse0 = value; }
    /// <summary>
    /// ������
    /// </summary>
    public float MouseScrollWheel { get => mouseScrollWheel; set => mouseScrollWheel = value; }
    
    public bool PressKeyW { get => pressKeyW; set => pressKeyW = value; }
    public bool PressKeyS { get => pressKeyS; set => pressKeyS = value; }
    public bool PressKeyA { get => pressKeyA; set => pressKeyA = value; }
    public bool PressKeyD { get => pressKeyD; set => pressKeyD = value; }
    public bool PressKeyQ { get => pressKeyQ; set => pressKeyQ = value; }
    public bool PressKeyE { get => pressKeyE; set => pressKeyE = value; }
    public bool PressKeyLeftControl { get => pressKeyLeftControl; set => pressKeyLeftControl = value; }
    public bool PressKeyUpLeftControl { get => pressKeyUpLeftControl; set => pressKeyUpLeftControl = value; }


    public void StartFunction()
    {
        Input.multiTouchEnabled = true;
    }


    public void UpdateFunction()
    {
        //Horizontal = Input.GetAxis("Horizontal"); // (-1,1) 
        //Vertical = Input.GetAxis("Vertical");     //  (-1,1)

        //MouseAxisX = Input.GetAxis("Mouse X");
        //MouseAxisY = Input.GetAxis("Mouse Y");


        //IsDownMouse0 = Input.GetMouseButtonDown(0);
        //IsUpMouse0 = Input.GetMouseButtonUp(0);

        //MouseScrollWheel = Input.GetAxis("Mouse ScrollWheel");
        //MultiTouchDetection();
        //log.LogFormat("{0} {1} {2} ", IsDownMouse0, IsUpMouse0, MouseScrollWheel);

    }


    public bool isTouchDetection  = true ;
    private Touch oldTouch1;  //�ϴδ�����1(��ָ1)
    private Touch oldTouch2;  //�ϴδ�����2(��ָ2)
    /// <summary>
    /// ��㴥�����
    /// </summary>
   public  void MultiTouchDetection()
    { 
        ////����ʱ�õ���������  ȡ���������
        //if (isTouchDetection == false )
        //{
        //    return;
        //}


        if (Input.touchCount <= 0)
        {
            return;
        }
        ////���㴥���� ˮƽ������ת
        //if ( Input.touchCount == 1 )
        //{
        //    Touch touch = Input.GetTouch(0);
        //    Vector2 deltaPos = touch.deltaPosition;
        //    transform.Rotate(Vector3.down * deltaPos.x, Space.World);//��Y�������ת
        //    transform.Rotate(Vector3.right * deltaPos.y, Space.World);//��X�������ת���������ǻ�����д��Z�������ת
        //}

        if (Input.touchCount == 2)
        {
            //��㴥��, �Ŵ���С
            Touch newTouch1 = Input.GetTouch(0);
            Touch newTouch2 = Input.GetTouch(1);
            //��2��տ�ʼ�Ӵ���Ļ, ֻ��¼����������
            if (newTouch2.phase == TouchPhase.Began)
            {
                oldTouch2 = newTouch2;
                oldTouch1 = newTouch1;
                return;
            }
            //�����ϵ����������µ��������룬���Ҫ�Ŵ�ģ�ͣ���СҪ����ģ��
            float oldDistance = Vector2.Distance(oldTouch1.position, oldTouch2.position);
            float newDistance = Vector2.Distance(newTouch1.position, newTouch2.position);
            //��������֮�Ϊ����ʾ�Ŵ����ƣ� Ϊ����ʾ��С����
            float offset = newDistance - oldDistance;
            //�Ŵ����ӣ� һ�����ذ� 0.01������(100�ɵ���)
            float scaleFactor = offset / 100f;
            //Vector3 localScale = transform.localScale;
            //Vector3 scale = new Vector3(localScale.x + scaleFactor,
            //                            localScale.y + scaleFactor,
            //                            localScale.z + scaleFactor);
            ////��ʲô����½�������
            //if (scale.x >= 0.5f && scale.y <= 2f)
            //{
            //    transform.localScale = scale;
            //}
            //��ס���µĴ����㣬�´�ʹ��
            oldTouch1 = newTouch1;
            oldTouch2 = newTouch2;

            MouseScrollWheel = scaleFactor;
        }
    }

    public void ReStartFunction()
    {
        throw new System.NotImplementedException();
    }

    public void OnDestroyFunction()
    {
        throw new System.NotImplementedException();
    }
}
