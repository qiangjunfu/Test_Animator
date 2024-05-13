using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    [SerializeField] int id = 1;
    [SerializeField ] Actions actions ;
    [SerializeField] private NPCState enemyState;

    public int Id { get => id; set => id = value; }



    #region MyRegion
    private void OnEnable()
    {
        MessageManager.AddListener<NPCState, int>(GameEventType.EnemyStateChange, OnEnemyStateChange);

        //if (actions == null) actions = GetComponent<Actions>();
        //actions.AddAnimationEvent(actions.GetAnimator(), "Fire SniperRifle", "StartFireEvent", 0);
    }
    private void OnDisable()
    {
        MessageManager.RemoveListener<NPCState, int>(GameEventType.EnemyStateChange, OnEnemyStateChange);

        //actions.CleanAllEvent(actions.GetAnimator());
    }

    private void OnEnemyStateChange(NPCState arg1, int arg2)
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


        enemyState = arg1;
        switch (arg1)
        {
            case NPCState.Stay:
                actions.Stay();
                break;
            case NPCState.Patrol:
                actions.Walk();
                break;
            case NPCState.Pursue:
                actions.Run();
                break;
            case NPCState.Attack:
                actions.Attack();
                break;
            case NPCState.Damage:
                break;
            case NPCState.Flee:
                actions.Run();
                break;
            default:
                break;
        }

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



public enum NPCState
{
    Stay =1 ,
    Patrol ,
    Pursue , 
    Attack , 
    Damage, 
    Flee,

}