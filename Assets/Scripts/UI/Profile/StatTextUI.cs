using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatTextUI : MonoBehaviour
{
    [Header("Connected Texts")]
    [SerializeField] private Text DamageText;
    [SerializeField] private Text HpText;
    [SerializeField] private Text SpeedText;
    [SerializeField] private Text ArmorText;

    private void Update()
    {
        DamageText.text = string.Format("{0}", Mathf.FloorToInt(DataManager.Instance.GetPlayerData().Damage));
        HpText.text = string.Format("{0}", Mathf.FloorToInt(DataManager.Instance.GetPlayerData().CurHp));
        SpeedText.text = string.Format("{0}%", Mathf.RoundToInt(DataManager.Instance.GetPlayerData().Speed + 100));
        ArmorText.text = string.Format("{0}", Mathf.FloorToInt(DataManager.Instance.GetPlayerData().Armor));
    }
}