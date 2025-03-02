using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
/*
                        Punch : ����(�ָ�) Ŭ����

                - RayCast�� ����ؼ� �������� ����
*/
public class Punch : Weapon
{
    private float attackRange = 1f;
    private Vector3 boxSize = new Vector3(0.8f, 2f, 0.8f);
    private Vector3 attackOrigin;                             
    private Vector3 attackDir;
    
    private void Awake()
    {
        // ���� Ÿ�� ����
        type = WeaponType.None;
    }

    public override void Attack()
    {
        attackOrigin = GameManager.Instance.player.transform.position + GameManager.Instance.player.transform.up;
        attackDir = GameManager.Instance.player.transform.forward;
        

        RaycastHit[] hits = Physics.BoxCastAll(
            attackOrigin,                       // �߽���ġ : �÷��̾�
            boxSize,                            // �ڽ�ũ��
            attackDir,                          // ���ݹ���     
            Quaternion.identity,                // ȸ��X
            attackRange,                        // �����ִ�Ÿ�
            LayerMask.GetMask("Monster")
            );
        
        // �Ѹ����� ����
        if(hits.Length > 0)
        {
            Monster monster = hits[0].collider.GetComponent<Monster>();
            if (monster != null)
            {
                monster.GetDamaged(DataManager.Instance.GetPlayerData().Damage);
            }
        }
    }

    private  void OnTriggerEnter(Collider other)
    {
        
    }

    public override void SetHitBox(bool isEnabled)
    {
        
    }

    public override void SetEffect(bool isEnabled)
    {

    }
    // ���ݹ��� �ð�ȭ
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackOrigin, boxSize);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(attackOrigin, attackOrigin + attackDir * attackRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(attackOrigin + attackDir * attackRange, boxSize);
    }
}
