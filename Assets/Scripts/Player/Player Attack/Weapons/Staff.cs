using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*
                    Staff : ����(������) Ŭ����

            - RayCast�� ����ؼ� �������� ����
 */

public class Staff : Weapon
{
    private float attackRange = 3f;             // ���� ����
    private float attackRadius = 3f;            // ���� �ݰ� 

    private Vector3 attackOrigin;
    private Vector3 attackDir;

    private List<RaycastResult> rrList;

    private void Awake()
    {
        type = WeaponType.Staff;
    }

    // ���� ����
    public override void Attack()
    {
        attackOrigin = GameManager.Instance.player.transform.position + GameManager.Instance.player.transform.forward * 2f;
        attackDir = GameManager.Instance.player.transform.forward;
        
        // ���ݹ���
        RaycastHit[] hits = Physics.SphereCastAll(
            attackOrigin,
            attackRadius,
            attackDir,
            attackRange,
            LayerMask.GetMask("Monster")
        );

        // ���ݹ����� ���Ͱ� �������
        foreach(RaycastHit hit in hits)
        {
            Monster monster = hit.collider.GetComponent<Monster>();
            if(monster != null)
            {
                // .. ���Ϳ��� ������
                Debug.Log("���͸� ���߾���.");
                monster.GetDamaged(DataManager.Instance.GetPlayerData().Damage);
            }
        }
    }

    public override void SetHitBox(bool isEnabled)
    {
        
    }

    // ���ݹ��� �ð�ȭ
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackOrigin, attackRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(attackOrigin, attackOrigin + attackDir * attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackOrigin + attackDir * attackRange, attackRadius);
    }
}
