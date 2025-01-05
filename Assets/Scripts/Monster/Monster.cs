using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
            Monster 클래스는 몬스터의 공통데이터를 관리

 */
public class Monster : MonoBehaviour
{
    #region **Monster Stats**
    [Header("#Monster Stats")]
    public int maxHp;
    public int curHp;
    public int maxMp;
    public int curMp;
    public float speed;
    public float maxDistance;
    #endregion

    public GameObject startPosition;
    public bool isReset = false;

    protected PlayerController targetPlayer;        // 타깃 플레이어
    protected BoxCollider scanRange;                // 몬스터 탐지범위

    private BoxCollider hitBoxCol;                  // 몬스터 히트박스
    private Animator anim;                          // 몬스터 애니메이터
    private NavMeshAgent nav;                       // 몬스터 네비게이션

    #region ** Properties **

    public Animator Anim => anim;
    public NavMeshAgent Nav => nav;
    public PlayerController Target => targetPlayer;
    #endregion

    protected void Awake()
    {
        Init();
    }
    
    private void Init()
    {
        targetPlayer = GameManager.Instance.player;
        
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        hitBoxCol = GetComponent<BoxCollider>();
        scanRange = GetComponent<BoxCollider>();
    }
}
