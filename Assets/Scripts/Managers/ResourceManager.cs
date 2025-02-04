using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

/*
                     ResourceManager

                - �̱���      
                - LoadIcon() : ������ �̸����� ��巹������ ������ �ε� �� ĳ��
 */

public class ResourceManager : MonoBehaviour
{
    private static ResourceManager instance;

    private Dictionary<string, Sprite> cashedSpriteDictionary = new Dictionary<string, Sprite>();

    // ��巹���� Sprite ���
    private const string assetRef = "Assets/Sprites/";

    public static ResourceManager Instance
    {
        get
        {
            if (instance == null)
            {
                // �ν��Ͻ��� �����ϴ��� �ѹ� �� üũ
                instance = FindObjectOfType<ResourceManager>();

                // ���ٸ�
                if (instance == null)
                {
                    GameObject obj = new GameObject("ResourceManager");
                    instance = obj.AddComponent<ResourceManager>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ������ ������ �ҷ�����
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