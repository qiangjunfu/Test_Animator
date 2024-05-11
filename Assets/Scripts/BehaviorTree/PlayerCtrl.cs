using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    [SerializeField] int id = 1;
    [SerializeField ] Actions actions ;
    [SerializeField] private string enemyState;

    public int Id { get => id; set => id = value; }



    #region MyRegion
    private void OnEnable()
    {
        MessageManager.AddListener<int, int>(GameEventType.EnemyStateChange, OnEnemyStateChange);

        //if (actions == null) actions = GetComponent<Actions>();
        //actions.AddAnimationEvent(actions.GetAnimator(), "Fire SniperRifle", "StartFireEvent", 0);
    }
    private void OnDisable()
    {
        MessageManager.RemoveListener<int, int>(GameEventType.EnemyStateChange, OnEnemyStateChange);

        //actions.CleanAllEvent(actions.GetAnimator());
    }

    private void OnEnemyStateChange(int arg1, int arg2)
    {
        // 1:Stay 
        // 2:Walk 
        // 3:Run 
        // 4:Sitting 
        // 5:Jump 
        // 6:Aiming 
        // 7:Attack 
        // 8:Damage 
        // 9:Death Reset 
        if (arg2 != id ) return;
        if (actions == null) { actions = GetComponent<Actions>(); }


        string aniName = "";
        switch (arg1)
        {
            case 1:
                aniName = "Stay";
                actions.Stay();
                break;
            case 2:
                aniName = "Walk";
                actions.Walk();
                break;
            case 3:
                aniName = "Run";
                actions.Run();
                break;
            case 4:
                aniName = "Sitting";
                break;
            case 5:
                aniName = "Jump";
                break;
            case 6:
                aniName = "Aiming";
                break;
            case 7:
                aniName = "Attack";
                break;
            case 8:
                aniName = "Damage";
                break;
            case 9:
                aniName = "Death Reset";
                break;
            default:
                break;
        }

        enemyState = aniName;
    }

    private void StartFireEvent()
    {
        // Debug.Log("开始播放 Attack动画  selfid: " + data.selfid );

        //// 射击
        //if (weapon != null && fireCount >0 ) weapon.Shoot();

        //fireCount++;
    }
    #endregion
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
