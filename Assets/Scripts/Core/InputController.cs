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
    /// 按下鼠标左键
    /// </summary>
    public bool IsDownMouse0 { get => isDownMouse0; set => isDownMouse0 = value; }
    /// <summary>
    /// 按住鼠标左键
    /// </summary>
    public bool IsMouse0 { get => isMouse0; set => isMouse0 = value; }
    /// <summary>
    /// 抬起鼠标左键
    /// </summary>
    public bool IsUpMouse0 { get => isUpMouse0; set => isUpMouse0 = value; }
    /// <summary>
    /// 鼠标滚轮
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
    private Touch oldTouch1;  //上次触摸点1(手指1)
    private Touch oldTouch2;  //上次触摸点2(手指2)
    /// <summary>
    /// 多点触屏检测
    /// </summary>
   public  void MultiTouchDetection()
    { 
        ////漫游时用的鼠标点击检测  取消触屏检测
        //if (isTouchDetection == false )
        //{
        //    return;
        //}


        if (Input.touchCount <= 0)
        {
            return;
        }
        ////单点触摸， 水平上下旋转
        //if ( Input.touchCount == 1 )
        //{
        //    Touch touch = Input.GetTouch(0);
        //    Vector2 deltaPos = touch.deltaPosition;
        //    transform.Rotate(Vector3.down * deltaPos.x, Space.World);//绕Y轴进行旋转
        //    transform.Rotate(Vector3.right * deltaPos.y, Space.World);//绕X轴进行旋转，下面我们还可以写绕Z轴进行旋转
        //}

        if (Input.touchCount == 2)
        {
            //多点触摸, 放大缩小
            Touch newTouch1 = Input.GetTouch(0);
            Touch newTouch2 = Input.GetTouch(1);
            //第2点刚开始接触屏幕, 只记录，不做处理
            if (newTouch2.phase == TouchPhase.Began)
            {
                oldTouch2 = newTouch2;
                oldTouch1 = newTouch1;
                return;
            }
            //计算老的两点距离和新的两点间距离，变大要放大模型，变小要缩放模型
            float oldDistance = Vector2.Distance(oldTouch1.position, oldTouch2.position);
            float newDistance = Vector2.Distance(newTouch1.position, newTouch2.position);
            //两个距离之差，为正表示放大手势， 为负表示缩小手势
            float offset = newDistance - oldDistance;
            //放大因子， 一个像素按 0.01倍来算(100可调整)
            float scaleFactor = offset / 100f;
            //Vector3 localScale = transform.localScale;
            //Vector3 scale = new Vector3(localScale.x + scaleFactor,
            //                            localScale.y + scaleFactor,
            //                            localScale.z + scaleFactor);
            ////在什么情况下进行缩放
            //if (scale.x >= 0.5f && scale.y <= 2f)
            //{
            //    transform.localScale = scale;
            //}
            //记住最新的触摸点，下次使用
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
