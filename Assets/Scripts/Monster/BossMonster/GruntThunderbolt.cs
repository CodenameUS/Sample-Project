using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntThunderbolt : MonoBehaviour
{
    public float damage;
    public float attackInterval = 0.5f;

    private float timer = 0f;

    private void OnTriggerStay(Collider ohter)
    {
        if (ohter.CompareTag("Player"))
        {
            timer += Time.deltaTime;
            if (timer >= 0.5f)
            {
                DataManager.Instance.GetPlayerData().GetDamaged(damage);
                timer = 0;
            }
        }
    }
}
