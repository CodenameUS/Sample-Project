using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

/*
                     ResourceManager

                - �̱���      
                - LoadIcon() : ������ �̸����� ��巹������ ������ �ε� �� ĳ��
 */

public class ResourceManager : Singleton<ResourceManager>
{ 
    private Dictionary<string, Sprite> cashedSpriteDictionary = new Dictionary<string, Sprite>();

    // ��巹���� Sprite ���
    private const string spriteRef = "Assets/Sprites/";

    // ��巹���� ���� Prefab ���
    private const string wPrefabRef = "Assets/Prefabs/WeaponPrefabs/";

    protected override void Awake()
    {
        base.Awake();
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
        Addressables.LoadAssetAsync<Sprite>(spriteRef + spriteName).Completed += handle =>
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

    // ���� ������ �ҷ�����
    public void LoadWeaponPrefab(string prefabName, System.Action<GameObject> onLoaded)
    {
        Addressables.LoadAssetAsync<GameObject>(wPrefabRef + prefabName).Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject loadedPrefab = handle.Result;
                onLoaded?.Invoke(loadedPrefab);
            }
            else
            {
                Debug.Log($"���� Prefab�� �������µ� ������.{ prefabName }");
                onLoaded?.Invoke(null);
            }
        };
    }

}