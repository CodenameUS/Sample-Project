using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
                    Monster Ŭ������ ������ ���� �����͸� ����

        - Monster Status & Flags

        - ������Ʈ �ʱ�ȭ

        - ���� �Լ�
            - ReadyToAttack : ���ݰ��ɿ��θ� ��Ÿ���� isAttackReady �÷��׸� True��
            - DeactiveGameObject : ���� ������Ʈ�� ��Ȱ��ȭ
 */

public class Monster : MonoBehaviour
{
    #region ** Monster Status **
    [Header("#Monster Stats")]
    public int maxHp;
    public int curHp;
    public float speed;
    public float maxDistance;                       // �÷��̾���� �Ÿ�(�����ϱ����� �ִ�Ÿ�)
    public float idleThreshold;                     // ������ ó�� ��ġ���� ����
    public float attackDelay;                       // ���ݼӵ�
    #endregion

    #region ** Flags **
    [HideInInspector]
    public bool isReset;                            // �������� �����ߴ��� ����
    [HideInInspector]
    public bool isAttackReady;                      // ���� ���� ����
    [HideInInspector]
    public bool isDead;                             // �׾����� ����
    #endregion

    #region ** Private Fields **
    private Vector3 startPosition;                  // ������ ù ��ġ
    private PlayerController targetPlayer;          // Ÿ�� �÷��̾�
    private BoxCollider scanRangeCol;               // �÷��̾� Ž������
    private BoxCollider hitBoxCol;                  // ���� ��Ʈ�ڽ�
    private Animator anim;                          // ���� �ִϸ�����
    private NavMeshAgent nav;                       // ���� �׺���̼�
    #endregion

    #region ** Properties **
    public Animator Anim => anim;
    public NavMeshAgent Nav => nav;
    public PlayerController Target => targetPlayer;
    public Vector3 StartPosition => startPosition;
    public BoxCollider HitBox => hitBoxCol;
    #endregion
    
    protected void Awake()
    {
        Init();
    }
    
    private void Init()
    {
        targetPlayer = GameManager.Instance.player;
        startPosition = transform.position;
        isAttackReady = true;

        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        hitBoxCol = GetComponent<BoxCollider>();
        scanRangeCol = GetComponent<BoxCollider>();
    }


    // ���� ���ɻ��·� ��ȯ
    public void ReadyToAttack()
    {
        isAttackReady = true;
    }

    // ����
    public void DeactiveGameObject()
    {
        gameObject.SetActive(false);
    }
}
