using UnityEngine;
using UnityEngine.UI;

/*
                            << BossHpUI >>

        - 보스 체력바 UI 관리
 */


public class BossHpUI : MonoBehaviour
{
    [SerializeField] private Slider hpBar;              // 체력바 슬라이더 
    [SerializeField] private Text hpText;               // 체력 텍스트

    private BossMonster boss;

    private void Awake()
    {
        boss = GetComponentInParent<BossMonster>();
    }

    private void Start()
    {
        SetHpText();
        SetHpAmount();
    }

    private void Update()
    {
        SetHpText();
        SetHpAmount();
    }

    // 현재 체력 텍스트 설정
    private void SetHpText()
    {
        if (boss.curHp <= 0)
            hpText.text = 0 + " / " + (int)boss.maxHp;
        else
        {
            hpText.text = (int)boss.curHp + " / " + (int)boss.maxHp;
        }
    }

    // 현재 체력바 설정
    private void SetHpAmount()
    {
        float hpFillAmount = (float)(boss.curHp / boss.maxHp);
        hpBar.value = hpFillAmount;
    }
}
