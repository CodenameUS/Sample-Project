using UnityEngine;
using Photon.Pun;

/*
                            << GruntThunderbolt >>

        - 보스몬스터의 장판형 지속 스킬공격 판정
        
        - 범위(Collider) 안에 있으면 공격간격(0.5초) 마다 데미지 발생
 */

public class GruntThunderbolt : MonoBehaviourPun
{
    public float damage;                        // 공격 데미지
    public float attackInterval = 0.5f;         // 공격 간격 시간

    private float timer = 0f;                   // 공격 간격 시간 계산용 타이머

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerController player = other.GetComponent<PlayerController>();

        timer += Time.deltaTime;

        // 0.5초마다 공격판정
        if (timer >= 0.5f)
        {
            // 데미지 발생
            if (GameManager.Instance.isMultiPlaying)
            {
                PhotonView targetView = other.GetComponent<PhotonView>();
                targetView.RPC("GetDamaged", targetView.Owner, damage * Random.Range(0.1f, 0.4f));
            }
            else
            {
                player.GetDamaged(damage * Random.Range(0.1f, 0.4f));
            }

            timer = 0;
        }
    }
}
