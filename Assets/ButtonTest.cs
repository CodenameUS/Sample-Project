using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTest : MonoBehaviour
{
    int testId = 1001;

    [SerializeField] ResourceManager resourceManager;


    void Start()
    {
        WeaponItemData data = DataManager.Instance.GetDataById(testId);
        if (data == null)
        {
            Debug.LogError($"Item with ID {testId} not found");
            return;
        }

        Text itemNameText = GetComponentInChildren<Text>();
        Image itemIconImage = GetComponent<Image>();

        itemNameText.text = data.ItemName;

        resourceManager.LoadIcon(data.ItemIcon, sprite =>
        {
            if (sprite != null)
            {
                itemIconImage.sprite = sprite;
            }
            else
            {
                Debug.Log($"Failed to load icon for item : {data.ItemIcon}");
            }
        });
    }

    public void OnButtonClick()
    {

    }
}
