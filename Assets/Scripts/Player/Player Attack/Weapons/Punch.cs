using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
/*
                        Punch : 무기(주먹) 클래스

                - RayCast를 사용해서 공격판정 구현
*/
public class Punch : Weapon
{
    private float attackRange = 1f;                         // 공격 사거리
    private int maxComboCount = 1;

    private Vector3 boxSize = new Vector3(0.8f, 2f, 0.8f);
    private Vector3 attackOrigin;                             
    private Vector3 attackDir;


    private void Awake()
    {
        // 무기 타입 설정
        type = WeaponType.None;
        soundId = "Punch";
    }

    // 공격 판정
    public override void Attack()
    {
        SetComboCount();

        attackOrigin = GameManager.Instance.player.transform.position + GameManager.Instance.player.transform.up;
        attackDir = GameManager.Instance.player.transform.forward;
        

        RaycastHit[] hits = Physics.BoxCastAll(
            attackOrigin,                       // 중심위치 : 플레이어
            boxSize,                            // 박스크기
            attackDir,                          // 공격방향     
            Quaternion.identity,                // 회전X
            attackRange,                        // 공격최대거리
            LayerMask.GetMask("Monster")
            );
        
        // 한마리만 공격
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

    public override void PlayerSfx()
    {
        AudioManager.Instance.PlaySFX(soundId);
    }

    // ComboCount 설정
    private void SetComboCount()
    {
        if (GameManager.Instance.player.CurComboCount < maxComboCount)
            GameManager.Instance.player.CurComboCount++;
        else
            GameManager.Instance.player.CurComboCount = 0;
    }

    // 공격범위 시각화
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
