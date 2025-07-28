using UnityEngine;
using UnityEngine.UI;

/*
                            << UIPlayerStats >>

        - 플레이어 체력바 UI 관리
 */

public class UIPlayerStats : MonoBehaviour
{
    [SerializeField] private Slider hpBar;              // 플레이어 체력바 슬라이더 UI
    [SerializeField] private Text hpText;               // 플레이어 체력 텍스트 UI

    private PlayerData playerData;

    private void Start()
    {
        playerData = DataManager.Instance.GetPlayerData();
        SetHpText();
        SetHpAmount();
    }

    private void Update()
    {
        SetHpText();
        SetHpAmount();
    }

    // 체력 텍스트 표시( 현재체력 / 최대체력 )
    private void SetHpText()
    {
        hpText.text = (int)playerData.CurHp + " / " + (int)playerData.MaxHp;
    }

    // 체력바 표시
    private void SetHpAmount()
    {
        float hpFillAmount = (float)(playerData.CurHp / playerData.MaxHp);
        hpBar.value = hpFillAmount;
    }
   
}
