using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerStats : MonoBehaviour
{
    [SerializeField] private Slider hpBar;
    [SerializeField] private Slider mpBar;

    [SerializeField] private Text hpText;
    [SerializeField] private Text mpText;

    private PlayerData playerData;

    private void Start()
    {
        playerData = DataManager.Instance.GetPlayerData();
        SetHMpText();
        SetHMpAmount();
    }

    private void Update()
    {
        SetHMpText();
        SetHMpAmount();
    }

    // Hp, Mp 텍스트 표기 형식
    private void SetHMpText()
    {
        hpText.text = playerData.CurHp + " / " + playerData.MaxHp;
        mpText.text = playerData.CurMp + " / " + playerData.MaxMp;
    }

    // 슬라이더 Value 조절
    private void SetHMpAmount()
    {
        float hpFillAmount = (float)playerData.CurHp / playerData.MaxHp;
        hpBar.value = hpFillAmount;
        float mpFillAmount = (float)playerData.CurMp / playerData.MaxMp;
        mpBar.value = mpFillAmount;
    }
}
