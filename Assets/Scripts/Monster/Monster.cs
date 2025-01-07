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
    public float speed;
    public float maxDistance;                       // �÷��̾���� �Ÿ�(�����ϱ����� �ִ�Ÿ�)
    public float idleThreshold;                     // ������ ó�� ��ġ���� ����
    public float attackDelay;                       // ���ݼӵ�
    #endregion

    private Vector3 startPosition;                  // ������ ù ��ġ

    public bool isReset;                            // �������� �����ߴ��� ����
    public bool isAttackReady;                      // ���� ���� ����

    protected PlayerController targetPlayer;        // Ÿ�� �÷��̾�

    private BoxCollider scanRange;                  // �÷��̾� Ž������
    private BoxCollider hitBoxCol;                  // ���� ��Ʈ�ڽ�
    private Animator anim;                          // ���� �ִϸ�����
    private NavMeshAgent nav;                       // ���� �׺���̼�

    #region ** Properties **

    public Animator Anim => anim;
    public NavMeshAgent Nav => nav;
    public PlayerController Target => targetPlayer;
    public Vector3 StartPosition => startPosition;
    #endregion
    
    protected void Awake()
    {
        Init();
    }
    
    private void Init()
    {
        targetPlayer = GameManager.Instance.player;
        startPosition = transform.position;

        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        hitBoxCol = GetComponent<BoxCollider>();
        scanRange = GetComponent<BoxCollider>();
    }

    // ���� ���ɻ��·� ��ȯ
    public void ReadyToAttack()
    {
        isAttackReady = true;
    }
}
