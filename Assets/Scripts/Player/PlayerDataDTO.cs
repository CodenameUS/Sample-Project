using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerDataDTO
{
    public List<StatusDTO> Status;

    [System.Serializable]
    // ���� ����
    public class StatusDTO
    {
        public float maxHp;             // �ִ�ü��
        public float curHp;             // ����ü��
        public float maxMp;             // �ִ븶��
        public float curMp;             // ���縶��
        public float speed;             // �̵��ӵ�
        public float rotateSpeed;       // ȸ���ӵ�
        public float damage;            // �⺻ ���ݷ�
        public float defense;           // �⺻ ����
        public int gold;              // ���� ���
    }

}
