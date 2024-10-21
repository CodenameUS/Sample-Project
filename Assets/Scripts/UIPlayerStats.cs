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

    [SerializeField] private PlayerInfo playerInfo;

    private void Start()
    {
        SetHMpText();
        SetHMpAmount();
    }

    private void Update()
    {
        SetHMpText();
        SetHMpAmount();
        Test();
    }

    void Test()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            playerInfo.curHp -= 100f;
    }

    // Hp, Mp �ؽ�Ʈ ǥ�� ����
    private void SetHMpText()
    {
        hpText.text = playerInfo.curHp + " / " + playerInfo.maxHp;
        mpText.text = playerInfo.curMp + " / " + playerInfo.maxMp;
    }

    // �����̴� Value ����
    private void SetHMpAmount()
    {

        float hpFillAmount = (float)playerInfo.curHp / playerInfo.maxHp;
        hpBar.value = hpFillAmount;
        float mpFillAmount = (float)playerInfo.curMp / playerInfo.maxMp;
        mpBar.value = mpFillAmount;
    }
}
