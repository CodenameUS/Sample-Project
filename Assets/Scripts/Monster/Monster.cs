using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    #endregion


    protected PlayerController targetPlayer;        // 타깃 플레이어
    private BoxCollider hitBoxCol;                  // 몬스터 히트박스
    private Animator anim;                           // 몬스터 애니메이터

    public Animator Anim => anim;

    protected void Awake()
    {
        Init();
    }

    private void Init()
    {
        targetPlayer = GameManager.Instance.player;
        anim = GetComponent<Animator>();
        hitBoxCol = GetComponent<BoxCollider>();

        if (anim == null)
            Debug.Log("Null");
    }
}
