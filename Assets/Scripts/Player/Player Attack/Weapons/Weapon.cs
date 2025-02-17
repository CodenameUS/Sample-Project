using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*                  
                    Weapon : ���� ���̽� Ŭ����
            - Attack() : ���� ����� ����
 */
public abstract class Weapon : MonoBehaviour
{
    public WeaponType type;

    // ������ ��
    public abstract void Attack();

    protected abstract void OnTriggerEnter(Collider other);
  
    // ��������
    public abstract void SetHitBox(bool isEnabled);
}
