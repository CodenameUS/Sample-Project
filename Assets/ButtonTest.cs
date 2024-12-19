using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTest : MonoBehaviour
{
    // �׽�Ʈ�� ������ id
    int testId = 1001;

    [SerializeField] ResourceManager resourceManager;


    void Start()
    {
        // ������ id�� ������ ��������
        WeaponItemData data = DataManager.Instance.GetDataById(testId);
        if (data == null)
        {
            Debug.LogError($"Item with ID {testId} not found");
            return;
        }

        Text itemNameText = GetComponentInChildren<Text>();
        Image itemIconImage = GetComponent<Image>();

        // ������ �̸� ����
        itemNameText.text = data.ItemName;

        // ������ ������ ��������
    }
}
