using UnityEngine;
using Photon.Pun;

/*
                            << GruntExplosion >>

        - 보스몬스터의 투사체 스킬공격 판정
        
        - 투사체 범위(Collider)에 히트시 데미지 및 넉백
 */

public class GruntExplosion : MonoBehaviourPun
{
    public float damage;
    public float knocebackForce = 7f;
    public float duration = 0.2f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerController player = other.GetComponent<PlayerController>();

        // 데미지 발생
        if (GameManager.Instance.isMultiPlaying)
        {
            PhotonView targetView = other.GetComponent<PhotonView>();
            targetView.RPC("GetDamaged", targetView.Owner, damage);
        }
        else
        {
            player.GetDamaged(damage);
        }

        // 플레이어 넉백
        Rigidbody rigid = other.GetComponent<Rigidbody>();
        if(rigid != null)
        {
            Vector3 knockbackDir = (other.transform.forward).normalized;
            rigid.AddForce(knockbackDir * knocebackForce, ForceMode.Impulse);
        }
    }
}
