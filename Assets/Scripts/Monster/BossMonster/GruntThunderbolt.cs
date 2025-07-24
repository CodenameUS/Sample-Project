using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/*
                GruntThunderbolt

            - 보스몬스터 Grunt 스킬 공격
                - 장판형 지속스킬
 */

public class GruntThunderbolt : MonoBehaviourPun
{
    public float damage;
    public float attackInterval = 0.5f;

    private float timer = 0f;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();

            timer += Time.deltaTime;
            if (timer >= 0.5f)
            {
                if(GameManager.Instance.isMultiPlaying)
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
}
