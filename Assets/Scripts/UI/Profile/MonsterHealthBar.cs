using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private Monster monster;
    private Transform cam;

    private void Start()
    {
        cam = Camera.main.transform;
        monster = GetComponentInParent<Monster>();

        SetHpAmount();
    }

    private void Update()
    {
        // UI�� ī�޶� �������� �ٶ󺸵���
        transform.LookAt(transform.position + cam.rotation * Vector3.forward, cam.rotation * Vector3.up);
        SetHpAmount();
    }

    // ���� ü�� ǥ��
    private void SetHpAmount()
    {
        float hpFillAmount = (float)monster.curHp / monster.maxHp;
        slider.value = hpFillAmount;
    }
}
