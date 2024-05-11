//using DG.Tweening;
//using EPOOutline;
//using System.Collections;
//using System.Collections.Generic;
//using System.Diagnostics.Eventing.Reader;
//using UnityEngine;
//using UnityEngine.UI;

//[RequireComponent(typeof(Camera))]
//public class CameraManager : MonoSingleTon<CameraManager>, IBaseScript
//{
//    [SerializeField, DisplayOnly] private Camera mainCamera;
//    [SerializeField, Range(0, 3), Header("相机当前状态"), DisplayOnly] private int cameraState = 0;
//    /// <summary>
//    /// 0:不能移动   1:自由移动   2:围绕固定建筑   3:DoTween移动中  4:只能绕自身旋转  5:跟随目标,仅缩放  6:跟随漫游角色  7:跟随设备   9:DoLocalPath移动中
//    /// </summary>
//    public int CameraState
//    {
//        get { return cameraState; }
//        set
//        {
//            cameraState = value;
//            MessageManager.Broadcast<int>(GameEventType.CameraState, cameraState);
//        }
//    }

//    public Camera MainCamera
//    {
//        get
//        {
//            if (mainCamera == null)
//            {
//                mainCamera = GetComponent<Camera>();
//            }
//            return mainCamera;
//        }
//    }



//    #region 5.22 初始化过场动画相机
//    /// <summary>
//    /// 过场动画完成事件
//    /// </summary>
//    System.Action aniCompleteAction;

//    Tweener tweener1 = null;
//    Tweener tweener2 = null;
//    /// <summary>
//    /// 初始化跟随相机
//    /// </summary>
//    public void InitAnimationCamera2(FollowCameraData _followCameraData, System.Action completeAction = null)
//    {
//        CameraState = 3;
//        followCameraData = _followCameraData;
//        if (followCameraData != null)
//            followCameraDefaultMinDistance = followCameraData.minDistance;
//        else
//            followCameraDefaultMinDistance = -1f;
//        cameraTargetPos = GameObject.Find(followCameraData.cameraTargetPath).transform;
//        cameraLookatTarget = GameObject.Find(followCameraData.cameraLookatPath).transform;
//        cameraTargetPos.LookAt(cameraLookatTarget);

//        maskUI = UIManager.Instance.ShowUI<MaskUI>(GameData.MaskUI, 9, false);


//        if (tweener1 != null) tweener1.Kill();
//        if (tweener2 != null) tweener2.Kill();
//        tweener1 = this.transform.DOMove(cameraTargetPos.position, followCameraData.doTweenMoveTime).OnComplete(() =>
//        {
//            log.LogFormat("初始化相机到开始位置  开始进行过场动画");
//            //锁定完成 切换到状态8  进行过场动画
//            CameraState = 8;

//            tweener1.Kill();
//            tweener2.Kill();
//        });
//        tweener2 = this.transform.DORotate(cameraTargetPos.eulerAngles, followCameraData.doTweenMoveTime);
//        ////tweener1.SetEase(Ease.Linear);
//        ////tweener2.SetEase(Ease.Linear);
//        ////关闭动画自动销毁 
//        //tweener1.SetAutoKill(true);
//        //tweener2.SetAutoKill(true);
//    }

//    public void MoveCameraAniEndPos(Transform cameraAniEndPos, float durationTime)
//    {
//        CameraState = 3;
//        log.LogFormat("移动到结束点 --------  " + CameraState);

//        maskUI = UIManager.Instance.ShowUI<MaskUI>(GameData.MaskUI, 9, false);

//        Tweener tweener1 = this.transform.DOMove(cameraAniEndPos.position, durationTime).OnComplete(() =>
//        {
//            maskUI.ShowPanel(false);

//            CameraState = 2;
//        });
//        Tweener tweener2 = this.transform.DORotate(cameraAniEndPos.eulerAngles, durationTime);
//        //tweener1.SetEase(Ease.Linear);
//        //tweener2.SetEase(Ease.Linear);
//        //关闭动画自动销毁 
//        tweener1.SetAutoKill(true);
//        tweener2.SetAutoKill(true);

//    }



//    Vector3[] aniPathPoints;
//    Vector3[] aniPathRots;
//    /// <summary>
//    /// 初始化过场动画相机
//    /// </summary>
//    public void InitAnimationCamera(FollowCameraData _followCameraData, System.Action completeAction = null)
//    {
//        followCameraData = _followCameraData;
//        if (followCameraData != null)
//            followCameraDefaultMinDistance = followCameraData.minDistance;
//        else
//            followCameraDefaultMinDistance = -1f;

//        if (followCameraData == null)
//        {
//            return;
//        }
//        if (followCameraData.isCameraAnimation == false || string.IsNullOrEmpty(followCameraData.cameraAniEndPos))
//        {
//            return;
//        }

//        CameraState = 3;
//        aniCompleteAction = completeAction;
//        cameraLookatTarget = GameObject.Find(followCameraData.cameraAniEndPos).transform;
//        List<Transform> cameraPathList = UnityTools.GetChildrenComponents<Transform>(cameraLookatTarget.gameObject);
//        int count = cameraPathList.Count;
//        aniPathPoints = new Vector3[count];
//        aniPathRots = new Vector3[count];
//        for (int i = 0; i < count; i++)
//        {
//            //先看向目标点
//            cameraPathList[i].LookAt(cameraLookatTarget);

//            aniPathPoints[i] = cameraPathList[i].position;
//            aniPathRots[i] = cameraPathList[i].eulerAngles;
//        }

//        if (aniPathPoints.Length != 0)
//        {
//            float movetime = GetMoveTime(aniPathPoints, followCameraData.animationTime);
//            // 先移动到 第一个目标点
//            DoMove(aniPathPoints[0], aniPathRots[0], movetime, () =>
//            {
//                CameraState = 9;
//                DoTweenPathMove(aniPathPoints, followCameraData.animationTime);
//            });
//        }

//    }
//    float GetMoveTime(Vector3[] aniPathPoints, float durationTime)
//    {
//        float disSum = 0;
//        for (int i = 0; i < aniPathPoints.Length - 1 - 1; i++)
//        {
//            disSum += Vector3.Distance(aniPathPoints[i], aniPathPoints[i + 1]);
//        }

//        //float speed = disSum / durationTime;


//        float dis2 = Vector3.Distance(aniPathPoints[0], transform.position);

//        float movetime = (dis2 * durationTime) / disSum;

//        log.LogFormat("disSum:{0}  durationTime:{1}  dis2:{2}  movetime:{3}", disSum, durationTime, dis2, movetime);
//        return movetime;
//    }

//    void DoMove(Vector3 pos, Vector3 rot, float durationTime, System.Action action = null)
//    {
//        Tweener tweener1 = transform.DOMove(pos, durationTime).OnComplete(() =>
//        {
//            action?.Invoke();
//        });
//        Tweener tweener2 = this.transform.DORotate(rot, durationTime);
//        tweener1.SetEase(Ease.Linear);
//        tweener2.SetEase(Ease.Linear);
//        tweener1.SetAutoKill(true);
//        tweener2.SetAutoKill(true);
//    }

//    void DoTweenPathMove(Vector3[] pathPoints, float durationTime)
//    {
//        //transform.DoLocalPath(pathPoints, durationTime, PathType.Linear, PathMode.Full3D);
//        DG.Tweening.Plugins.Core.PathCore.Path m_DoTweenPath = new DG.Tweening.Plugins.Core.PathCore.Path(DG.Tweening.PathType.CatmullRom, pathPoints, 20, Color.red);
//        DG.Tweening.Core.TweenerCore<Vector3, DG.Tweening.Plugins.Core.PathCore.Path, DG.Tweening.Plugins.Options.PathOptions> tweenerCore =
//             transform.DOLocalPath(m_DoTweenPath, durationTime, PathMode.Full3D);
//        tweenerCore.onWaypointChange = DOLocalPath_WaypointChange;
//        tweenerCore.onPause = DOLocalPathPause;
//        tweenerCore.onComplete = DOLocalPathComplete;
//        tweenerCore.onKill = DOLocalPathKill;
//        tweenerCore.SetAutoKill(true);

//        //UnityTools.DelayDoTween(1, () =>
//        //{
//        //    log.LogFormat("长度:{0}  时间:{1}  速度:{2} ", tweenerCore.PathLength(), durationTime, (tweenerCore.PathLength() / durationTime));
//        //});

//    }

//    #region  DOLocalPath 移动回调
//    private void DOLocalPath_WaypointChange(int value)
//    {
//        Debug.LogFormat("DOLocalPath_WaypointChange  路径点变化: " + value);
//    }


//    void DOLocalPathComplete()
//    {
//        Debug.LogFormat("DOLocalPathComplete  移动完成!!!  ");

//        // 过场动画完成后 隐藏遮罩 固定相机看向的目标点  切换到状态2
//        maskUI.ShowPanel(false);
//        CameraState = 2;

//        aniCompleteAction?.Invoke();
//    }
//    void DOLocalPathPause()
//    {
//        Debug.LogFormat("DOLocalPathPause  移动暂停!!!  ");
//    }
//    void DOLocalPathKill()
//    {
//        Debug.LogFormat("DOLocalPathPause  移动关闭!!!  ");
//    }
//    #endregion

//    #endregion


//    #region  相机位置 状态 初始化
//    // ----- 2:围绕固定建筑 -----
//    MaskUI maskUI;
//    private FollowCameraData followCameraData;
//    private float followCameraDefaultMinDistance = -1f;
//    /// <summary>
//    /// 相机移动目标点
//    /// </summary>
//    public Transform cameraTargetPos;
//    /// <summary>
//    /// 相机看向的目标点
//    /// </summary>
//    public Transform cameraLookatTarget;

//    /// <summary>
//    /// 初始化跟随相机
//    /// </summary>
//    public void InitFollowCamera(FollowCameraData _followCameraData)
//    {
//        log.LogFormat("围绕固定建筑 -------  ");
//        CameraState = 3;
//        followCameraData = _followCameraData;
//        if (followCameraData != null)
//            followCameraDefaultMinDistance = followCameraData.minDistance;
//        else
//            followCameraDefaultMinDistance = -1f;
//        cameraTargetPos = GameObject.Find(followCameraData.cameraTargetPath).transform;
//        cameraLookatTarget = GameObject.Find(followCameraData.cameraLookatPath).transform;
//        cameraTargetPos.LookAt(cameraLookatTarget);

//        maskUI = UIManager.Instance.ShowUI<MaskUI>(GameData.MaskUI, 9, false);

//        if (string.IsNullOrEmpty(followCameraData.cameraOriginPosPath) == false)
//        {
//            Transform cameraOriginPos = GameObject.Find(followCameraData.cameraOriginPosPath).transform;
//            this.transform.position = cameraOriginPos.position;
//            this.transform.eulerAngles = cameraOriginPos.eulerAngles;
//        }


//        Tweener tweener1 = this.transform.DOMove(cameraTargetPos.position, followCameraData.doTweenMoveTime).OnComplete(() =>
//        {
//            maskUI.ShowPanel(false);

//            //锁定完成 切换到状态2
//            CameraState = 2;
//        });
//        Tweener tweener2 = this.transform.DORotate(cameraTargetPos.eulerAngles, followCameraData.doTweenMoveTime);

//        //tweener1.SetEase(Ease.Linear);
//        //tweener2.SetEase(Ease.Linear);
//        //关闭动画自动销毁 
//        tweener1.SetAutoKill(true);
//        tweener2.SetAutoKill(true);
//    }


//    // ----- 1:自由移动 -----

//    RoamCameraData roamCameraData;
//    /// <summary>
//    /// 初始化漫游相机
//    /// </summary>
//    public void InitRoamCamera(RoamCameraData _roamCameraData)
//    {
//        CameraState = 3;
//        roamCameraData = _roamCameraData;
//        Transform cameraTargetPos = GameObject.Find(roamCameraData.cameraTargetPath).transform;
//        moveSpeed = roamCameraData.moveSpeed;

//        if (roamCameraData.isCompleteOperation)
//        {
//            maskUI = UIManager.Instance.ShowUI<MaskUI>(GameData.MaskUI, 9, false);
//        }

//        Tweener tweener1 = this.transform.DOMove(cameraTargetPos.position, roamCameraData.doTweenMoveTime).OnComplete(() =>
//        {
//            if (roamCameraData.isCompleteOperation) maskUI.ShowPanel(false);

//            //锁定完成 切换到状态1
//            CameraState = 1;
//        });
//        Tweener tweener2 = this.transform.DORotate(cameraTargetPos.eulerAngles, roamCameraData.doTweenMoveTime);

//        //tweener1.SetEase(Ease.Linear);
//        //tweener2.SetEase(Ease.Linear);
//        tweener1.SetAutoKill(true);
//        tweener2.SetAutoKill(true);
//    }


//    // ----- 4:只能绕自身旋转 ----- 
//    /// <summary>
//    /// 旋转相机 仅旋转相机 (景观视图)
//    /// </summary>
//    public void InitRotateCamera(RoamCameraData _roamCameraData)
//    {
//        CameraState = 3;
//        roamCameraData = _roamCameraData;
//        Transform cameraTargetPos = GameObject.Find(roamCameraData.cameraTargetPath).transform;
//        moveSpeed = roamCameraData.moveSpeed;

//        if (string.IsNullOrEmpty(roamCameraData.cameraOriginPosPath) == false)
//        {
//            Transform cameraOriginPos = GameObject.Find(roamCameraData.cameraOriginPosPath).transform;
//            this.transform.position = cameraOriginPos.position;
//            this.transform.eulerAngles = cameraOriginPos.eulerAngles;
//        }

//        maskUI = UIManager.Instance.ShowUI<MaskUI>(GameData.MaskUI, 9, false);

//        Tweener tweener1 = this.transform.DOMove(cameraTargetPos.position, roamCameraData.doTweenMoveTime).OnComplete(() =>
//        {
//            if (roamCameraData.isCompleteOperation) maskUI.ShowPanel(false);

//            CameraState = 4;
//        });
//        Tweener tweener2 = this.transform.DORotate(cameraTargetPos.eulerAngles, roamCameraData.doTweenMoveTime);

//        //tweener1.SetEase(Ease.Linear);
//        //tweener2.SetEase(Ease.Linear);
//        tweener1.SetAutoKill(true);
//        tweener2.SetAutoKill(true);
//    }


//    // ----- 5:跟随目标,仅缩放 -----
//    /// <summary>
//    /// 跟随设备相机 仅缩放
//    /// </summary>
//    public void InitDeviceCamera(FollowCameraData _followCameraData)
//    {
//        Debug.Log("InitDeviceCamera()    _followCameraData = " + _followCameraData.cameraLookatPath);
//        CameraState = 3;
//        followCameraData = _followCameraData;
//        if (followCameraData != null)
//            followCameraDefaultMinDistance = followCameraData.minDistance;
//        else
//            followCameraDefaultMinDistance = -1f;
//        cameraTargetPos = GameObject.Find(followCameraData.cameraTargetPath).transform;
//        cameraLookatTarget = GameObject.Find(followCameraData.cameraLookatPath).transform;
//        cameraTargetPos.LookAt(cameraLookatTarget);

//        MaskUI maskUI = UIManager.Instance.ShowUI<MaskUI>(GameData.MaskUI, 9, false);

//        if (string.IsNullOrEmpty(followCameraData.cameraOriginPosPath) == false)
//        {
//            Transform cameraOriginPos = GameObject.Find(followCameraData.cameraOriginPosPath).transform;
//            this.transform.position = cameraOriginPos.position;
//            this.transform.eulerAngles = cameraOriginPos.eulerAngles;
//        }


//        Tweener tweener1 = this.transform.DOMove(cameraTargetPos.position, followCameraData.doTweenMoveTime).OnComplete(() =>
//        {
//            maskUI.ShowPanel(false);

//            CameraState = 5;
//        });
//        Tweener tweener2 = this.transform.DORotate(cameraTargetPos.eulerAngles, followCameraData.doTweenMoveTime);

//        //tweener1.SetEase(Ease.Linear);
//        //tweener2.SetEase(Ease.Linear);
//        //关闭动画自动销毁 
//        tweener1.SetAutoKill(true);
//        tweener2.SetAutoKill(true);
//    }


//    // ----- 6:跟随漫游角色 -----
//    ManYouData manYouData = null;
//    public void InitManYouPlayerCamera(ManYouData _manYouData)
//    {
//        // 不在移动 由player控制
//        CameraState = 6;
//        manYouData = _manYouData;

//        Vector3 pos = UnityTools.ArrayToVector3(manYouData.originPos);
//        Vector3 rot = UnityTools.ArrayToVector3(manYouData.originRot);
//        this.transform.position = pos;
//        this.transform.eulerAngles = rot;
//    }

//    public void ResetCameraViewForceWithCardInfo(float tweenTime, System.Action focusFinishedAction, Transform targetModel = null)
//    {
//        log.Log("开始卡片运镜!!!");
//        //【抖动】
//        //transform.DOShakePosition(1f);
//        //【旋转 + 推拉】
//        float maxDistance = 400f;
//        float minDistance = 360f;
//        float midDistance = 350f;
//        float defaultDistance = 0f, defaultOffsetDistance = 0f;
//        float cardWinOffsetHight = 0f;
//        GameObject centerModel = null;//中心模型
//        string currentScene = SceneLoadManager.Instance.GetSceneName();
//        switch (currentScene)
//        {
//            case GameData.MainScene:
//                centerModel = ModelsMapManager.Instance.GetMappedModelByBuildingId("6");
//                //maxDistance = 500f;
//                //minDistance = 460f;
//                //midDistance = 450f;

//                //改为近距离聚焦拆分楼层
//                //maxDistance = 250f;
//                //minDistance = 230f;
//                //midDistance = 225f;
//                //距离更近些
//                maxDistance = 150f;
//                minDistance = 130f;
//                midDistance = 125f;

//                cardWinOffsetHight = 20f;//卡片聚焦时,相机高度偏移
//                defaultOffsetDistance = 25f;//重置聚焦时，距离模型扎点的偏移距离     聚焦距离【5】
//                if (followCameraData != null)
//                {
//                    log.Log("相机默认最小距离followCameraData.minDistance = " + followCameraData.minDistance);
//                    followCameraData.minDistance = 10f;
//                    log.Log("相机最小距离followCameraData.minDistance = " + followCameraData.minDistance);
//                }
//                break;
//            case GameData.FenCengScene:
//                centerModel = ModelsMapManager.Instance.GetMappedModelBySubFloor();
//                maxDistance = 200f;
//                minDistance = 180f;
//                midDistance = 175f;

//                defaultOffsetDistance = 5f;//重置聚焦时，距离模型扎点的偏移距离     聚焦距离【5】
//                break;
//            case GameData.CoolingRoom:
//                CoolingRoomManager coolingManager = FindObjectOfType<CoolingRoomManager>();
//                if (coolingManager != null)
//                {
//                    centerModel = coolingManager.centerModel.gameObject;
//                    //距离更近些
//                    maxDistance = 150f;
//                    minDistance = 130f;
//                    midDistance = 125f;

//                    cardWinOffsetHight = 5f;//10//卡片聚焦时,相机高度偏移
//                    defaultOffsetDistance = 17f;//25//重置聚焦时，距离模型扎点的偏移距离     聚焦距离【5】
//                    if (followCameraData != null)
//                    {
//                        log.Log("相机默认最小距离followCameraData.minDistance = " + followCameraData.minDistance);
//                        followCameraData.minDistance = 10f;
//                        log.Log("相机默认推拉速度followCameraData.scrollSpeed = " + followCameraData.scrollSpeed);
//                        followCameraData.scrollSpeed = 5f;
//                        log.Log("相机最小距离followCameraData.minDistance = " + followCameraData.minDistance);
//                    }
//                }
//                break;
//            default:
//                break;
//        }

//        //GameObject building6 = ModelsMapManager.Instance.GetMappedModelByBuildingId("6");
//        if (centerModel != null)
//        {
//            //中心点及朝向
//            Vector3 buildingCenter = centerModel.transform.position;
//            buildingCenter = new Vector3(buildingCenter.x, 0f, buildingCenter.z);
//            Vector3 dir = Vector3.zero;
//            //矫正相机焦点
//            cameraLookatTarget = centerModel.transform;
//            Vector3 targetPos = Vector3.zero;

//            //判断是目标模型(非楼栋)运镜还是随机运镜
//            bool isTargetModelMove = true;
//            if (targetModel != null)
//            {
//                //目标模型是否为楼栋
//                if ((targetModel.parent != null && targetModel.parent.name == "Floors") || targetModel.name.EndsWith("_centor"))
//                {
//                    isTargetModelMove = false;
//                }
//                else
//                {
//                    isTargetModelMove = true;
//                }
//            }
//            else
//            {
//                isTargetModelMove = false;
//            }

//            //设定运镜参数
//            if (isTargetModelMove)
//            {
//                log.Log("聚焦模型,运镜到目标模型位置   targetModel.name = " + targetModel.name);
//                dir = targetModel.position - buildingCenter;
//                defaultDistance = Vector3.Distance(targetModel.position, buildingCenter);
//                //强制修正聚焦距离
//                midDistance = defaultDistance + defaultOffsetDistance;
//                targetPos = buildingCenter + dir.normalized * midDistance + Vector3.up * cardWinOffsetHight;

//                log.Log("聚焦模型,运镜到目标模型位置   targetPos = " + targetPos.ToString());

//                //矫正相机焦点
//                cameraLookatTarget = targetModel;
//                //开始运镜
//                transform.DOMove(targetPos, tweenTime + 0.1f).OnComplete(() =>
//                {
//                    CameraState = 2;
//                    focusFinishedAction?.Invoke();
//                });
//                //焦点注视
//                //transform.DODynamicLookAt(targetModel.position, tweenTime);
//                GameObject tempAngleAgent = new GameObject("tempAngle");
//                tempAngleAgent.transform.position = targetPos;
//                tempAngleAgent.transform.LookAt(targetModel.position);
//                Vector3 targetAngle = tempAngleAgent.transform.eulerAngles;
//                DestroyImmediate(tempAngleAgent);
//                transform.DORotate(targetAngle, tweenTime);
//            }
//            else
//            {
//                log.LogWarning("无法定位模型,随机运镜!!!");
//                //向量旋转一定角度
//                //Vector3 buildingCenter = centerModel.transform.position;
//                //buildingCenter = new Vector3(buildingCenter.x, 0f, buildingCenter.z);
//                dir = transform.position - buildingCenter;
//                Vector3 rotDir = Quaternion.Euler(new Vector3(0f, Random.Range(-18, 18) * 20f, 0f)) * dir;
//                Vector3 rotPos = buildingCenter + rotDir;
//                //强制修正聚焦距离
//                //defaultDistance = Vector3.Distance(rotPos, buildingCenter);
//                //midDistance = defaultDistance + defaultOffsetDistance;

//                //推拉一定距离
//                float cameraDistance = Vector3.Distance(rotPos, buildingCenter);

//                if (cameraDistance > maxDistance)
//                {
//                    targetPos = (rotPos - buildingCenter).normalized * midDistance;
//                }
//                else if (cameraDistance < minDistance)
//                {
//                    targetPos = (rotPos - buildingCenter).normalized * (maxDistance + 10f);
//                }
//                else
//                {
//                    targetPos = (rotPos - buildingCenter).normalized * midDistance;
//                }
//                targetPos += buildingCenter;


//                //开始运镜
//                transform.DOMove(targetPos, tweenTime + 0.1f).OnComplete(() =>
//                {
//                    CameraState = 2;
//                    focusFinishedAction?.Invoke();
//                });
//                //焦点注视
//                //transform.DODynamicLookAt(centerModel.transform.position, tweenTime);
//                GameObject tempAngleAgent = new GameObject("tempAngle");
//                tempAngleAgent.transform.position = targetPos;
//                tempAngleAgent.transform.LookAt(centerModel.transform.position);
//                Vector3 targetAngle = tempAngleAgent.transform.eulerAngles;
//                DestroyImmediate(tempAngleAgent);
//                transform.DORotate(targetAngle, tweenTime);

//            }
//            log.Log("注视点：cameraLookatTarget.name = " + cameraLookatTarget.name);
//            /*
//            //开始运镜
//            transform.DOMove(targetPos, tweenTime + 0.1f).OnComplete(() =>
//            {
//                //CameraState = 2;
//                //focusFinishedAction?.Invoke();
//            });
//            //焦点注视
//            transform.DODynamicLookAt(centerModel.transform.position, tweenTime);
//            */
//        }
//        else
//        {
//            log.Log("建筑中心点异常!!!");
//        }
//    }
//    public void ResetCameraViewForce(Vector3 targetCamPos, Vector3 targetCamAngle, float tweenTime = -1f)
//    {
//        if (tweenTime < 0)
//            tweenTime = 0.2f;
//        transform.DOMove(targetCamPos, tweenTime);
//        transform.DORotate(targetCamAngle, tweenTime);
//    }
//    public void ResetCameraViewForceWithCamTargetPos(Transform cameraLookatTarget, Vector3 targetCamPos, Vector3 targetCamAngle, float tweenTime = -1f)
//    {
//        ResetCameraLookatTarget(cameraLookatTarget);
//        ResetCameraViewForce(targetCamPos, targetCamAngle, tweenTime);
//    }

//    public Transform ResetCameraLookatTarget(Transform cameraLookatTarget)
//    {
//        Transform preCameraLookatTarget = this.cameraLookatTarget;
//        this.cameraLookatTarget = cameraLookatTarget;
//        cameraState = 2;//强制改为可旋转模式
//        return preCameraLookatTarget;
//    }
//    /// <summary>
//    /// 重置相机推拉速度即距离区间
//    /// </summary>
//    /// <param name="scrollSpeed">滚轮间距，负值无影响</param>
//    /// <param name="minDistance">最小距离，负值无影响</param>
//    /// <param name="maxDistance">最大距离，负值无影响</param>
//    public void ResetCameraDistanceInterval(float scrollSpeed, float minDistance, float maxDistance)
//    {
//        log.Log("重置 场景相机 距离区间 minDistance = " + minDistance + "     maxDistance = " + maxDistance + "       followCameraData = null---> " + (followCameraData == null));
//        if (followCameraData == null)
//            return;
//        if (scrollSpeed >= 0f)
//            followCameraData.scrollSpeed = 1f;
//        if (minDistance >= 0f)
//            followCameraData.minDistance = minDistance;
//        if (maxDistance >= 0f)
//            followCameraData.maxDistance = maxDistance;
//    }
//    /// <summary>
//    /// 点击图标或列表项时，会聚焦模型，重置最小距离为5；退出聚焦模式时，需要重置为默认的最小距离
//    /// </summary>
//    public void ResetFlollowCameraMinDistance()
//    {
//        if (followCameraDefaultMinDistance > 0f)
//        {
//            followCameraData.minDistance = followCameraDefaultMinDistance;
//        }
//        else
//        {
//            followCameraData.minDistance = 100f;
//        }
//    }
//    #endregion


//    #region 初始化相机位置 状态  

//    Vector3 targetPos;
//    GameObject cameraLookatTargetObj;
//    int x = 1;
//    int y = 1;
//    int z = 1;
//    public float beishu = 1;
//    public float cameraFlow = 2; 
//    /// <summary>
//    /// 初始化跟随设备相机 
//    /// </summary>
//    public void InitFollowCamera_Device(FollowCameraData _followCameraData, LocalDeviceInfo localDeviceInfo)
//    {
//        log.LogFormat("初始化跟随设备相机: " + localDeviceInfo.name);
//        CameraState = 3;
//        followCameraData = _followCameraData;
//        if (followCameraData != null)
//            followCameraDefaultMinDistance = followCameraData.minDistance;
//        else
//            followCameraDefaultMinDistance = -1f;

//        MaskUI maskUI = UIManager.Instance.ShowUI<MaskUI>(GameData.MaskUI, 9, false);


//        targetPos = new Vector3(localDeviceInfo.posX, localDeviceInfo.posY, localDeviceInfo.posZ);
//        if (cameraLookatTargetObj == null) cameraLookatTargetObj = new GameObject();
//        cameraLookatTargetObj.transform.position = targetPos;
//        cameraLookatTarget = cameraLookatTargetObj.transform;
//        //Vector3 cameraPos = GetFollowCamerPos(localDeviceInfo);  不再用射线检测
//        Vector3 cameraPos = GetFollowCamerPos2(localDeviceInfo);
//        GameObject cameraObj = new GameObject();
//        cameraObj.transform.position = cameraPos;
//        cameraObj.transform.LookAt(cameraLookatTarget);


//        Tweener tweener1 = this.transform.DOMove(cameraObj.transform.position, followCameraData.doTweenMoveTime).OnComplete(() =>
//        {
//            maskUI.ShowPanel(false);
//            DestroyImmediate(cameraObj);

//            CameraState = 2;
//        });
//        Tweener tweener2 = this.transform.DORotate(cameraObj.transform.eulerAngles, followCameraData.doTweenMoveTime);

//        //tweener1.SetEase(Ease.Linear);
//        //tweener2.SetEase(Ease.Linear);
//        //关闭动画自动销毁 
//        tweener1.SetAutoKill(true);
//        tweener2.SetAutoKill(true);
//    }


//    #region  射线检测 获取相机可视位置
//    Vector3 GetFollowCamerPos2(LocalDeviceInfo localDeviceInfo) 
//    {
//        Vector3 camerPos = Vector3.zero;

//        Vector3 targetPos = new Vector3(localDeviceInfo.posX, localDeviceInfo.posY, localDeviceInfo.posZ);
//        if (beishu < localDeviceInfo.width)
//        {
//            beishu = localDeviceInfo.width;
//        }
//        if (beishu < localDeviceInfo.height)
//        {
//            beishu = localDeviceInfo.height;
//        }
//        if (beishu < localDeviceInfo.length)
//        {
//            beishu = localDeviceInfo.length;
//        }
//        beishu = beishu >= 5 ? beishu : 5;
//        camerPos = targetPos + new Vector3(0, beishu, 0);

//        return camerPos;
//    }


//    Vector3 GetFollowCamerPos(LocalDeviceInfo localDeviceInfo)
//    {
//        if (localDeviceInfo == null) return Vector3.zero;

//        //// 目标物体边界盒

//        if (beishu < localDeviceInfo.width)
//        {
//            beishu = localDeviceInfo.width;
//        }
//        if (beishu < localDeviceInfo.height)
//        {
//            beishu = localDeviceInfo.height;
//        }
//        if (beishu < localDeviceInfo.length)
//        {
//            beishu = localDeviceInfo.length;
//        }

//        #region MyRegion
//        //////// 前方位置
//        //////Vector3 forwardPos = targetPos + new Vector3(0, y, z) * beishu;
//        //////Vector3 v1 = targetPos + new Vector3(x * 0.5f, y, z) * beishu;
//        //////Vector3 v2 = targetPos + new Vector3(-x * 0.5f, y, z) * beishu;
//        //////// 后方位置         
//        //////Vector3 backPos = targetPos + new Vector3(0, y, -z) * beishu;
//        //////Vector3 v3 = targetPos + new Vector3(x * 0.5f, y, -z) * beishu;
//        //////Vector3 v4 = targetPos + new Vector3(-x * 0.5f, y, -z) * beishu;

//        //////// 左方位置
//        //////Vector3 leftPos = targetPos + new Vector3(-x, y, 0) * beishu;

//        //////// 右方位置 
//        //////Vector3 rightPos = targetPos + new Vector3(x, y, 0) * beishu;

//        ////// 前方位置
//        ////Vector3 forwardPos = targetPos + new Vector3(0, 0, z) * beishu;
//        ////Vector3 v1 = targetPos + new Vector3(x * 0.5f, 0, z) * beishu;
//        ////Vector3 v2 = targetPos + new Vector3(-x * 0.5f, 0, z) * beishu;
//        ////// 后方位置         
//        ////Vector3 backPos = targetPos + new Vector3(0, 0, -z) * beishu;
//        ////Vector3 v3 = targetPos + new Vector3(x * 0.5f, 0, -z) * beishu;
//        ////Vector3 v4 = targetPos + new Vector3(-x * 0.5f, 0, -z) * beishu;

//        ////// 左方位置
//        ////Vector3 leftPos = targetPos + new Vector3(-x, 0, 0) * beishu;

//        ////// 右方位置 
//        ////Vector3 rightPos = targetPos + new Vector3(x, 0, 0) * beishu;




//        ////// 检测5个位置是否可见目标物体
//        //////Vector3 finalPos = CheckPositions(forwardPos, backPos, leftPos, rightPos, upPos);
//        ////Vector3 finalPos = CheckPositions(forwardPos, backPos, leftPos, rightPos, v1, v2, v3, v4);

//        ////return finalPos + new Vector3(0, y, 0) * beishu;


//        #endregion


//        //y = (int )(beishu > 2 ? 2 : beishu);
//        //y = (int)localDeviceInfo.height;

//        // 前方位置
//        Vector3 forwardPos = targetPos + new Vector3(0, y, z) * beishu * cameraFlow;
//        Vector3 v1 = targetPos + new Vector3(x * 0.5f, y, z) * beishu * cameraFlow;
//        Vector3 v2 = targetPos + new Vector3(-x * 0.5f, y, z) * beishu * cameraFlow;
//        // 后方位置         
//        Vector3 backPos = targetPos + new Vector3(0, y, -z) * beishu * cameraFlow;
//        Vector3 v3 = targetPos + new Vector3(x * 0.5f, y, -z) * beishu * cameraFlow;
//        Vector3 v4 = targetPos + new Vector3(-x * 0.5f, y, -z) * beishu * cameraFlow;

//        // 左方位置
//        Vector3 leftPos = targetPos + new Vector3(-x, y, 0) * beishu * cameraFlow;

//        // 右方位置 
//        Vector3 rightPos = targetPos + new Vector3(x, y, 0) * beishu * cameraFlow;




//        // 检测5个位置是否可见目标物体
//        //Vector3 finalPos = CheckPositions(forwardPos, backPos, leftPos, rightPos, upPos);
//        Vector3[] v3s = new Vector3[] { forwardPos, backPos, leftPos, rightPos, v1, v2, v3, v4 };
//        string[] dirInfos = new string[] { "forwardPos", "backPos", " leftPos", "rightPos", " v1", " v2", "v3","v4"};
//        Vector3 finalPos = CheckPositions(v3s , dirInfos ); 
//        log.LogFormat("选中最终位置:  {0} ", finalPos);
//        return finalPos;
//    }

//    /// <summary>
//    /// 检测位置是否可见目标物体 
//    /// </summary>
//    Vector3 CheckPositions( Vector3[] positions , string[] dirInfos)
//    {
//        //foreach (Vector3 pos in positions)
//        //{
//        //    // 使用射线检测pos是否可见target
//        //    if (!IsTargetVisible(pos))
//        //        return pos;   // 找到第一个可见位置并返回
//        //}
//        for (int i = 0; i < positions .Length ; i++)
//        {
//            // 使用射线检测pos是否可见target
//            if (!IsTargetVisible(positions[i] , dirInfos[i] ))
//                // 找到第一个可见位置并返回
//                return positions[i];    
//        }

//        return positions[positions.Length - 1];
//    }

//    /// <summary>
//    /// 射线检测
//    /// </summary>
//    bool IsTargetVisible(Vector3 cameraPos  ,  string dirInfo)
//    {
//        // 从相机出发的射线
//        //Ray ray = new Ray(origin, targetPos - origin);
//        //Ray ray = new Ray(targetPos, targetPos - cameraPos);
//        Ray ray = new Ray(targetPos, cameraPos - targetPos);

//        // 射线检测返回的信息
//        RaycastHit hit;

//        // 射线检测所有遮挡层 
//        int mask = LayerMask.GetMask("Wall");

//        // 射线检测,判断是否打中遮挡墙体
//        float z = cameraPos.z - targetPos.z;
//        //bool isVisible = !Physics.Raycast(ray, out hit, Mathf.Infinity, mask);
//        //bool isVisible = Physics.Raycast(ray, out hit, z * beishu * 2  );
//        bool isVisible = Physics.Raycast(ray, out hit, z * 2);
//        Debug.DrawRay(targetPos, cameraPos - targetPos , Color.red );//绘制射线


//        if (isVisible)
//        {
//            Debug.LogFormat("射线检测到的物体:  name:{0}, point{1}   cameraPos:{2}   dirInfo:{3} ",  hit.collider.gameObject.name , hit.point , cameraPos, dirInfo);
//        }
//        log.LogFormat("是否检测到碰撞体:  isVisible:{0}  cameraPos:{1}   dirInfo:{2} ", isVisible , cameraPos  , dirInfo);
//        // 返回是否可见目标物体
//        return isVisible;
//    }
//    #endregion
//    #endregion

//    #region 无模型相机初始化

//    /// <summary>
//    /// 无模型相机锁定 初始化 (buildId: 1-6)
//    /// </summary>
//    public void InitFollowCamera_Null(int buildId, System.Action moveEndAction = null)
//    {
//        // 找到选中楼栋 , 随机一个相机初始位置 , 相机移动到cameraPos
//        CameraState = 3;

//        //相机目标位置
//        Build build = BuildManager.Instance.GetBuild(buildId);
//        FollowCameraData followCameraData = JsonData.GetFollowCameraData(buildId);
//        if (followCameraData != null)
//            followCameraDefaultMinDistance = followCameraData.minDistance;
//        else
//            followCameraDefaultMinDistance = -1f;
//        cameraLookatTarget = GameObject.Find(followCameraData.cameraLookatPath).transform;


//        //相机初始位置 
//        string uiName = "YunWeiZongLanUI";
//        FollowCameraData followCameraData2 = JsonData.GetFollowCameraData(uiName);
//        Transform cameraOriginPos = GameObject.Find(followCameraData2.cameraTargetPath).transform;
//        GameObject tempLookAt = new GameObject("temp");
//        tempLookAt.transform.position = cameraOriginPos.position - new Vector3(0, 100, 0);
//        tempLookAt.transform.LookAt(cameraLookatTarget);
//        this.transform.position = tempLookAt.transform.position;
//        this.transform.eulerAngles = tempLookAt.transform.eulerAngles;
//        DestroyImmediate(tempLookAt);


//        maskUI = UIManager.Instance.ShowUI<MaskUI>(GameData.MaskUI, 9, false);

//        Tweener tweener1 = transform.DOBlendableMoveBy(transform.forward * 100, 1).OnComplete(() =>
//      {
//          maskUI.ShowPanel(false);
//          moveEndAction?.Invoke();

//          //锁定完成 切换到状态2
//          CameraState = 2;
//      });

//        //tweener1.SetEase(Ease.Linear);
//        tweener1.SetAutoKill(true);
//    }
//    #endregion


//    #region 点击收藏试图
//    Tweener tweener_shoucang1;
//    Tweener tweener_shoucang2;
//    public void MoveCamera_ShouCang(Vector3 pos, Vector3 rot)
//    {
//        if (tweener_shoucang1 != null) tweener_shoucang1.Kill();
//        if (tweener_shoucang2 != null) tweener_shoucang2.Kill();

//        CameraState = 3;
//        maskUI = UIManager.Instance.ShowUI<MaskUI>(GameData.MaskUI, 9, false);

//        tweener_shoucang1 = this.transform.DOMove(pos, 1).OnComplete(() =>
//        {
//            maskUI.ShowPanel(false);

//            CameraState = 2;
//        });
//        tweener_shoucang2 = this.transform.DORotate(rot, 1);

//        //tweener_shoucang1.SetEase(Ease.Linear);
//        //tweener2.SetEase(Ease.Linear);
//        //关闭动画自动销毁 
//        tweener_shoucang1.SetAutoKill(true);
//        tweener2.SetAutoKill(true);
//    }
//    #endregion

//    public void StartFunction()
//    {
//        MessageManager.AddListener<string>(GameEventType.LoadScene, LoadSceneCallback);
//        MessageManager.AddListener<string>(GameEventType.LoadSceneComplete, LoadSceneCompleteCallback);
//    }

//    public void OnDestroyFunction()
//    {
//        MessageManager.AddListener<string>(GameEventType.LoadScene, LoadSceneCallback);
//        MessageManager.RemoveListener<string>(GameEventType.LoadSceneComplete, LoadSceneCompleteCallback);
//    }

//    #region  事件
//    private Outliner outliner = null;
//    private void LoadSceneCallback(string arg1)
//    {
//        CameraState = 0;
//    }
//    private void LoadSceneCompleteCallback(string arg1)
//    {
//        switch (arg1)
//        {
//            case GameData.MainScene:
//                ResetOutlinerBlurIterations(60);
//                break;
//            case GameData.FenCengScene:
//                ResetOutlinerBlurIterations(0);
//                break;
//            default:
//                break;
//        }
//    }
//    /// <summary>
//    /// 切换主场景或子场景 需要 重置边缘光 光晕强度
//    /// </summary>
//    /// <param name="blurIterations"></param>
//    private void ResetOutlinerBlurIterations(int blurIterations)
//    {
//        if (outliner == null)
//            outliner = GetComponent<Outliner>();
//        if (outliner != null)
//        {
//            outliner.BlurIterations = blurIterations;
//        }
//    }
//    #endregion


//    public void ReStartFunction()
//    {
//    }

//    public void UpdateFunction()
//    {
//    }


//    bool isMoveCamera = true;
//    public void LateUpdateFunction()
//    {
//        //InputController.Instance.IsDownMouse0 = Input.GetMouseButtonDown(0);
//        //InputController.Instance.IsMouse0 = Input.GetMouseButton(0);
//        //InputController.Instance.IsUpMouse0 = Input.GetMouseButtonUp(0);

//        //if (InputController.Instance.IsDownMouse0)
//        //{
//        //    if (UnityTools.IsOverUI(UIManager.Instance.GraphicRaycast))
//        //    {
//        //        isMoveCamera = false;
//        //    }
//        //    else
//        //    {
//        //        isMoveCamera = true;
//        //    }
//        //}
//        //if (InputController.Instance.IsUpMouse0)
//        //{
//        //    isMoveCamera = true;
//        //}
//        //if (isMoveCamera == false) return;

//        if (UnityTools.IsOverUI(UIManager.Instance.GraphicRaycast)) return;


//        switch (CameraState)
//        {
//            case 0:
//                break;
//            case 1:
//                输入监听1();
//                RoamMove();
//                RoamRotate2();
//                break;
//            case 2:
//                输入监听2();
//                CamerFollow();
//                break;
//            case 3:
//                break;
//            case 4:
//                输入监听1();
//                //RoamMove();
//                RoamRotate2();
//                break;
//            case 5:
//                输入监听2();
//                CamerFollowZoom();
//                break;
//            case 7:
//                输入监听2();
//                CameraZoom_Device();
//                break;
//            //case 8:
//            //    输入监听8();
//            //    CamerFollow8();
//            //    break;
//            case 9:
//                if (cameraLookatTarget != null)
//                    this.transform.LookAt(cameraLookatTarget.position);
//                break;
//            default:
//                break;
//        }
//    }

//    public void FixedUpdate()
//    {
//        if (CameraState == 8)
//        {
//            输入监听8();
//            CamerFollow8();
//        }

//        //if (CameraState == 8)
//        //{
//        //    输入监听8();
//        //    CamerFollow8();

//        //    count += 1;
//        //    log.LogFormat("111111111   :  {0} ", count);
//        //}
//        //else
//        //{
//        //    count = 0;
//        //}
//    }

//    #region  1 自由移动
//    // 平移
//    [SerializeField, Header("漫游移动速度"), DisplayOnly] float moveSpeed = 1;
//    //[SerializeField] float moveRate = 1;// 按住Shift加速
//    //[SerializeField] float minDistance = 4;// 相机离不可穿过的表面的最小距离（小于等于0时可穿透任何表面）

//    //旋转
//    //[SerializeField, Header("旋转速度")] float rotateSpeed = 1;
//    // public float max_up_angle = 60;    //越大，头抬得越高
//    //public float max_down_angle = -60; //越小，头抬得越低
//    private float current_rotation_H;  //水平旋转结果
//    private float current_rotation_V;  //垂直旋转结果
//                                       //[SerializeField, Header("初始旋转量")] Vector3 originRot = Vector3.zero;


//    // 运动速度和其每个方向的速度分量
//    private Vector3 direction = Vector3.zero;
//    private Vector3 speedForward;
//    private Vector3 speedBack;
//    private Vector3 speedLeft;
//    private Vector3 speedRight;
//    private Vector3 speedUp;
//    private Vector3 speedDown;
//    //缩放
//    private Vector3 speedZoom;
//    //private float zoomSpeed = 10;
//    //平移
//    private Vector3 speedTranslateX;
//    private Vector3 speedTranslateY;
//    private float translateRate = 5;


//    private void 输入监听1()
//    {
//        InputController.Instance.PressKeyW = Input.GetKey(KeyCode.W);
//        InputController.Instance.PressKeyS = Input.GetKey(KeyCode.S);
//        InputController.Instance.PressKeyA = Input.GetKey(KeyCode.A);
//        InputController.Instance.PressKeyD = Input.GetKey(KeyCode.D);
//        InputController.Instance.PressKeyQ = Input.GetKey(KeyCode.Q);
//        InputController.Instance.PressKeyE = Input.GetKey(KeyCode.E);
//        InputController.Instance.PressKeyLeftControl = Input.GetKey(KeyCode.LeftControl);
//        InputController.Instance.PressKeyUpLeftControl = Input.GetKeyUp(KeyCode.LeftControl);

//        InputController.Instance.IsMouse0 = Input.GetMouseButton(0);
//        InputController.Instance.MouseScrollWheel = Input.GetAxis("Mouse ScrollWheel");
//        InputController.Instance.MouseAxisX = Input.GetAxis("Mouse X");
//        InputController.Instance.MouseAxisY = -Input.GetAxis("Mouse Y");

//    }

//    void RoamMove()
//    {
//        GetDirection();
//        // 检测是否离不可穿透表面过近
//        RaycastHit hit;
//        while (Physics.Raycast(transform.position, direction, out hit, roamCameraData.minDistance))
//        {
//            // 消去垂直于不可穿透表面的运动速度分量
//            float angel = Vector3.Angle(direction, hit.normal);
//            float magnitude = Vector3.Magnitude(direction) * Mathf.Cos(Mathf.Deg2Rad * (180 - angel));
//            direction += hit.normal * magnitude;
//        }
//        transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
//    }

//    void RoamRotate2()
//    {
//        if (InputController.Instance.IsMouse0)
//        {

//            var dx = InputController.Instance.MouseAxisX * roamCameraData.rotateSpeed;
//            var dy = InputController.Instance.MouseAxisY * roamCameraData.rotateSpeed * -1;

//            if (Mathf.Abs(dx) > 0 || Mathf.Abs(dy) > 0)
//            {
//                Vector3 angles = transform.rotation.eulerAngles;
//                angles.y += dx;

//                angles.x = Mathf.Repeat(angles.x + 180f, 360f) - 180f;
//                angles.x -= dy;
//                angles.x = UnityTools.ClampAngle(angles.x, roamCameraData.max_down_angle, roamCameraData.max_up_angle);

//                Quaternion targetRotation = default;
//                targetRotation.eulerAngles = new Vector3(angles.x, angles.y, 0);
//                transform.rotation = targetRotation;
//            }
//        }
//    }


//    private void GetDirection()
//    {
//        // 加速移动
//        if (InputController.Instance.PressKeyLeftControl)
//        {
//            moveSpeed += 1;
//            if (moveSpeed > roamCameraData.maxMoveSpeed)
//            {
//                moveSpeed = roamCameraData.maxMoveSpeed;
//            }
//        }
//        if (InputController.Instance.PressKeyUpLeftControl) moveSpeed = roamCameraData.moveSpeed;

//        #region 键盘移动
//        // 复位
//        speedForward = Vector3.zero;
//        speedBack = Vector3.zero;
//        speedLeft = Vector3.zero;
//        speedRight = Vector3.zero;
//        speedUp = Vector3.zero;
//        speedDown = Vector3.zero;
//        speedZoom = Vector3.zero;
//        speedTranslateX = Vector3.zero;
//        speedTranslateY = Vector3.zero;

//        // 获取按键输入
//        if (InputController.Instance.PressKeyW) speedForward = transform.forward;
//        if (InputController.Instance.PressKeyS) speedBack = -transform.forward;
//        if (InputController.Instance.PressKeyA) speedLeft = -transform.right;
//        if (InputController.Instance.PressKeyD) speedRight = transform.right;
//        if (InputController.Instance.PressKeyQ) speedUp = Vector3.up;
//        if (InputController.Instance.PressKeyE) speedDown = Vector3.down;

//        if (InputController.Instance.MouseScrollWheel != 0)
//        {
//            if (InputController.Instance.MouseScrollWheel > 0)
//            {
//                speedZoom = transform.forward * roamCameraData.zoomSpeed;
//            }
//            else
//            {
//                speedZoom = -transform.forward * roamCameraData.zoomSpeed;
//            }
//        }

//        if (Input.GetMouseButton(2))
//        {
//            //float MouseAxisX = Input.GetAxis("Mouse X");
//            //float MouseAxisY = Input.GetAxis("Mouse Y");
//            //if (MouseAxisX > 0)
//            //{
//            //    speedTranslateX = transform.right * translateRate;
//            //}
//            //else
//            //{
//            //    speedTranslateX = -transform.right * translateRate;
//            //}

//            //if (MouseAxisY > 0)
//            //{
//            //    speedTranslateY = Vector3.up * translateRate;
//            //}
//            //else
//            //{
//            //    speedTranslateY = Vector3.down * translateRate;
//            //}
//        }
//        direction = speedForward + speedBack + speedLeft + speedRight + speedUp + speedDown + speedZoom + speedTranslateX + speedTranslateY;
//        #endregion
//    }

//    /// <summary>
//    /// 计算一个Vector3绕旋转中心旋转指定角度后所得到的向量。
//    /// </summary>
//    /// <param name="source">旋转前的源Vector3</param>
//    /// <param name="axis">旋转轴</param>
//    /// <param name="angle">旋转角度</param>
//    /// <returns>旋转后得到的新Vector3</returns>
//    Vector3 V3RotateAround(Vector3 source, Vector3 axis, float angle)
//    {
//        Quaternion q = Quaternion.AngleAxis(angle, axis);// 旋转系数
//        return q * source;// 返回目标点
//    }

//    #endregion


//    #region  2 跟随建筑
//    private void 输入监听2()
//    {
//        //旋转
//        if (Input.GetKey(KeyCode.Mouse0))
//        {
//            InputController.Instance.MouseAxisX = Input.GetAxis("Mouse X");
//            InputController.Instance.MouseAxisY = -Input.GetAxis("Mouse Y");
//        }
//        //else if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
//        //{
//        //    Touch touch = Input.GetTouch(0);
//        //    InputController.Instance.MouseAxisX = touch.deltaPosition.x * 0.04f;
//        //    InputController.Instance.MouseAxisY = touch.deltaPosition.y * 0.04f;
//        //}
//        else
//        {
//            InputController.Instance.MouseAxisX = 0;
//            InputController.Instance.MouseAxisY = 0;
//        }

//        //缩放
//        InputController.Instance.MouseScrollWheel = Input.GetAxis("Mouse ScrollWheel");
//        //InputController.Instance.MultiTouchDetection();
//    }

//    private void CamerFollow()
//    {
//        //旋转
//        if (InputController.Instance.MouseAxisX != 0 || InputController.Instance.MouseAxisY != 0)
//        {
//            transform.RotateAround(cameraLookatTarget.transform.position, Vector3.up, InputController.Instance.MouseAxisX * 5);
//            //transform.RotateAround(targetModel.transform.position, transform.right, mouse_y * 5);

//            //预设角度（当前角度加上将要增加/减少的角度）
//            float rotatedAngle = transform.eulerAngles.x + InputController.Instance.MouseAxisY * followCameraData.rotateSpeed;

//            //旋转角度小于5则设置为5
//            if (rotatedAngle < followCameraData.minRotateAngle)
//            {
//                transform.RotateAround(cameraLookatTarget.position, transform.right, (InputController.Instance.MouseAxisY * followCameraData.rotateSpeed) + (followCameraData.minRotateAngle - rotatedAngle));
//            }
//            else if (rotatedAngle > followCameraData.maxRotateAngle) //旋转角度大于85则设置85
//            {
//                transform.RotateAround(cameraLookatTarget.position, transform.right, (InputController.Instance.MouseAxisY * followCameraData.rotateSpeed) - (rotatedAngle - followCameraData.maxRotateAngle));
//            }
//            else
//            {
//                transform.RotateAround(cameraLookatTarget.position, transform.right, InputController.Instance.MouseAxisY * followCameraData.rotateSpeed);
//            }
//        }

//        if (cameraLookatTarget == null)
//        {
//            log.LogWarning("cameraLookatTarget == null");
//        }
//        if (transform == null)
//        {
//            log.LogWarning("transform == null");
//        }
//        //缩放
//        if (cameraLookatTarget != null)
//        {
//            float dis = Vector3.Distance(cameraLookatTarget.position, transform.position);

//            if (InputController.Instance.MouseScrollWheel > 0 && dis > followCameraData.minDistance)
//            {
//                transform.Translate(Vector3.forward * followCameraData.scrollSpeed);
//            }

//            if (InputController.Instance.MouseScrollWheel < 0 && dis < followCameraData.maxDistance)
//            {
//                transform.Translate(Vector3.forward * -followCameraData.scrollSpeed);
//            }
//        }
//    }
//    #endregion


//    private void CamerFollowZoom()
//    {
//        //缩放
//        float dis = Vector3.Distance(cameraLookatTarget.position, this.transform.position);

//        if (InputController.Instance.MouseScrollWheel > 0 && dis > followCameraData.minDistance)
//        {
//            transform.Translate(Vector3.forward * followCameraData.scrollSpeed);
//        }

//        if (InputController.Instance.MouseScrollWheel < 0 && dis < followCameraData.maxDistance)
//        {
//            transform.Translate(Vector3.forward * -followCameraData.scrollSpeed);
//        }
//    }


//    #region  7 跟随设备
//    private void CameraZoom_Device()
//    {
//        //缩放
//        float dis = Vector3.Distance(targetPos, this.transform.position);

//        if (InputController.Instance.MouseScrollWheel > 0 && dis > followCameraData.minDistance)
//        {
//            transform.Translate(Vector3.forward * followCameraData.scrollSpeed);
//        }

//        if (InputController.Instance.MouseScrollWheel < 0 && dis < followCameraData.maxDistance)
//        {
//            transform.Translate(Vector3.forward * -followCameraData.scrollSpeed);
//        }
//    }
//    #endregion


//    #region  8 相机过场动画
//    [SerializeField, DisplayOnly] float MouseAxisX = 0.2f;
//    [SerializeField, DisplayOnly] float MouseAxisY = -0.02f;
//    [SerializeField, DisplayOnly] float MouseScrollWheel = 0.02f;
//    [SerializeField, DisplayOnly] int count = 0;
//    private void 输入监听8()
//    {
//        if (followCameraData.rotSpeedX_cameraAni != 0)
//        {
//            //log.LogFormat("111111111   :  {0} ", followCameraData.rotSpeedX_cameraAni);
//            MouseAxisX = followCameraData.rotSpeedX_cameraAni;
//            MouseAxisY = followCameraData.rotSpeedY_cameraAni;
//            MouseScrollWheel = followCameraData.scrollSpeed * 0.001f;
//        }
//        //else
//        //{
//        //    log.LogFormat("2222222222   :  {0} ", followCameraData.rotSpeedX_cameraAni);
//        //}

//    }

//    private void CamerFollow8()
//    {
//        //缩放
//        float dis = Vector3.Distance(cameraLookatTarget.position, this.transform.position);

//        if (MouseScrollWheel > 0 && dis > followCameraData.minDistance)
//        {
//            transform.Translate(Vector3.forward * MouseScrollWheel * followCameraData.scrollSpeed);
//        }

//        if (MouseScrollWheel < 0 && dis < followCameraData.maxDistance)
//        {
//            transform.Translate(Vector3.forward * MouseScrollWheel * -followCameraData.scrollSpeed);
//        }


//        //旋转
//        if (MouseAxisX != 0 || MouseAxisY != 0)
//        {
//            transform.RotateAround(cameraLookatTarget.position, Vector3.up, MouseAxisX * 5);
//            //transform.RotateAround(targetModel.transform.position, transform.right, mouse_y * 5);

//            //预设角度（当前角度加上将要增加/减少的角度）
//            float rotatedAngle = transform.eulerAngles.x + MouseAxisY * followCameraData.rotateSpeed;

//            //旋转角度小于5则设置为5
//            if (rotatedAngle < followCameraData.minRotateAngle)
//            {
//                transform.RotateAround(cameraLookatTarget.position, transform.right, (MouseAxisY * followCameraData.rotateSpeed) + (followCameraData.minRotateAngle - rotatedAngle));
//            }
//            else if (rotatedAngle > followCameraData.maxRotateAngle) //旋转角度大于85则设置85
//            {
//                transform.RotateAround(cameraLookatTarget.position, transform.right, (MouseAxisY * followCameraData.rotateSpeed) - (rotatedAngle - followCameraData.maxRotateAngle));
//            }
//            else
//            {
//                transform.RotateAround(cameraLookatTarget.position, transform.right, MouseAxisY * followCameraData.rotateSpeed);
//            }
//        }

//    }

//    #endregion
//}
