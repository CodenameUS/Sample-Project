using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    #endregion

    protected BoxCollider hitBoxCol;          // ���� ��Ʈ�ڽ�
    protected Animator anim;                  // ���� �ִϸ�����
    protected PlayerController targetPlayer;        // Ÿ�� �÷��̾�

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        anim = GetComponent<Animator>();
        hitBoxCol = GetComponent<BoxCollider>();

        targetPlayer = GameManager.Instance.player;
    }
}
