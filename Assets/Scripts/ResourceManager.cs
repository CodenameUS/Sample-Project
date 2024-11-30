using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ResourceManager : MonoBehaviour
{
    private Dictionary<string, Sprite> cashedSpriteDictionary = new Dictionary<string, Sprite>();

    // ��巹���� Sprite ���
    private const string assetRef = "Assets/Sprites/";           

    void Start()
    {
       
    }

 
    public void LoadIcon(string spriteName, System.Action<Sprite> onLoaded)
    {
        // ĳ�ÿ� �̹� ���� ���� ĳ��
        if (cashedSpriteDictionary.TryGetValue(spriteName, out var cachedSprite))
        {
            onLoaded?.Invoke(cachedSprite);
            return;
        }

        // ��巹������ Sprite �ε�(�̸�����)
        Addressables.LoadAssetAsync<Sprite>(assetRef + spriteName).Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                Sprite loadedSprite = handle.Result;
                cashedSpriteDictionary[spriteName] = loadedSprite;   // ĳ�ÿ� ��������Ʈ ����
                onLoaded?.Invoke(loadedSprite);
            }
            else
            {
                Debug.LogError($"���� Sprite�� �������µ� ������. { spriteName }");
                onLoaded?.Invoke(null);
            }
        };
    }

}
