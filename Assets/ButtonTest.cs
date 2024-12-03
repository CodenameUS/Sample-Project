using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTest : MonoBehaviour
{
    // 테스트용 아이템 id
    int testId = 1001;

    [SerializeField] ResourceManager resourceManager;


    void Start()
    {
        // 아이템 id로 데이터 가져오기
        WeaponItemData data = DataManager.Instance.GetDataById(testId);
        if (data == null)
        {
            Debug.LogError($"Item with ID {testId} not found");
            return;
        }

        Text itemNameText = GetComponentInChildren<Text>();
        Image itemIconImage = GetComponent<Image>();

        // 아이템 이름 설정
        itemNameText.text = data.ItemName;

        // 아이콘 데이터 가져오기
        resourceManager.LoadIcon(data.ItemIcon, sprite =>
        {
            // 성공
            if (sprite != null)
            {
                // 아이콘 설정
                itemIconImage.sprite = sprite;
            }
            else
            {
                Debug.Log($"Failed to load icon for item : {data.ItemIcon}");
            }
        });
    }
}
