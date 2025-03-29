using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerDataDTO
{
    public List<StatusDTO> Status;

    [System.Serializable]
    // 스탯 정보
    public class StatusDTO
    {
        public float maxHp;             // 최대체력
        public float curHp;             // 현재체력
        public float maxMp;             // 최대마나
        public float curMp;             // 현재마나
        public float speed;             // 이동속도
        public float rotateSpeed;       // 회전속도
        public float damage;            // 기본 공격력
        public float defense;           // 기본 방어력
        public int gold;              // 보유 골드
    }

}
