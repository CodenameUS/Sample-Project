using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class GruntExplosion : MonoBehaviourPun
{
    public float damage;
    public float knocebackForce = 7f;
    public float duration = 0.2f;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();

            if (GameManager.Instance.isMultiPlaying)
            {
                player.GetComponent<PhotonView>()?.RPC(nameof(player.GetDamaged), RpcTarget.All, damage);
            }
            else
            {
                player.GetDamaged(damage);
            }
        }

        Rigidbody rigid = other.GetComponent<Rigidbody>();
        if(rigid != null)
        {
            Vector3 knockbackDir = (other.transform.forward).normalized;
            rigid.AddForce(knockbackDir * knocebackForce, ForceMode.Impulse);
        }
    }
}
