using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
            Monster Ŭ������ ������ ���뵥���͸� ����

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

    protected PlayerController targetPlayer;        // Ÿ�� �÷��̾�
    protected BoxCollider scanRange;                // ���� Ž������

    private BoxCollider hitBoxCol;                  // ���� ��Ʈ�ڽ�
    private Animator anim;                          // ���� �ִϸ�����
    private NavMeshAgent nav;                       // ���� �׺���̼�

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
