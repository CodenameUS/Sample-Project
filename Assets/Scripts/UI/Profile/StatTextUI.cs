using UnityEngine;
using UnityEngine.UI;

/*
                            << StatTextUI >>

        - 장비창의 캐릭터 능력치 정보 UI 관리
 */

public class StatTextUI : MonoBehaviour
{
    [Header("Connected Texts")]
    [SerializeField] private Text damageText;           // 공격력 텍스트 UI
    [SerializeField] private Text hpText;               // 체력 텍스트 UI
    [SerializeField] private Text speedText;            // 이동속도 텍스트 UI
    [SerializeField] private Text defenseText;          // 방어력 텍스트 UI

    private void Update()
    {
        damageText.text = string.Format("{0}", Mathf.FloorToInt(DataManager.Instance.GetPlayerData().Damage));
        hpText.text = string.Format("{0}", Mathf.FloorToInt(DataManager.Instance.GetPlayerData().CurHp));
        speedText.text = string.Format("{0}%", Mathf.RoundToInt(DataManager.Instance.GetPlayerData().Speed));
        defenseText.text = string.Format("{0}", Mathf.FloorToInt(DataManager.Instance.GetPlayerData().Defense));
    }
}