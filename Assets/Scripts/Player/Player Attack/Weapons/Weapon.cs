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
  
    // ���� ���� On/Off
    public abstract void SetHitBox(bool isEnabled);

    // ����Ʈ On/Off
    public abstract void SetEffect(bool isEnabled);
}
