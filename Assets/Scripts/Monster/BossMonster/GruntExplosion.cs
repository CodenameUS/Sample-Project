using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntExplosion : MonoBehaviour
{
    public float damage = 10f;
    public float knocebackForce = 80f;
    public float duration = 0.2f;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("플레이어 피격");

            DataManager.Instance.GetPlayerData().GetDamaged(damage);
        }

        Rigidbody rigid = other.GetComponent<Rigidbody>();
        if(rigid != null)
        {
            Vector3 knockbackDir = (other.transform.position - transform.position).normalized;
            rigid.AddForce(knockbackDir * knocebackForce, ForceMode.Impulse);
        }
    }
}
