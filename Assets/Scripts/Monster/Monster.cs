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


    protected PlayerController targetPlayer;        // Ÿ�� �÷��̾�
    private BoxCollider hitBoxCol;                  // ���� ��Ʈ�ڽ�
    private Animator anim;                           // ���� �ִϸ�����

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
