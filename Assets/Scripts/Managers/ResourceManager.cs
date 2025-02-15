using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

/*
                     ResourceManager

                - 싱글톤      
                - LoadIcon() : 아이콘 이름으로 어드레서블에서 아이콘 로드 및 캐싱
 */

public class ResourceManager : Singleton<ResourceManager>
{ 
    private Dictionary<string, Sprite> cashedSpriteDictionary = new Dictionary<string, Sprite>();

    // 어드레서블 Sprite 경로
    private const string spriteRef = "Assets/Sprites/";

    // 어드레서블 무기 Prefab 경로
    private const string wPrefabRef = "Assets/Prefabs/WeaponPrefabs/";

    protected override void Awake()
    {
        base.Awake();
    }

    // 아이콘 데이터 불러오기
    public void LoadIcon(string spriteName, System.Action<Sprite> onLoaded)
    {
        // 캐시에 이미 있을 때는 캐싱
        if (cashedSpriteDictionary.TryGetValue(spriteName, out var cachedSprite))
        {
            onLoaded?.Invoke(cachedSprite);
            return;
        }

        // 어드레서블에서 Sprite 로드(이름으로)
        Addressables.LoadAssetAsync<Sprite>(spriteRef + spriteName).Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                Sprite loadedSprite = handle.Result;
                cashedSpriteDictionary[spriteName] = loadedSprite;   // 캐시에 스프라이트 저장
                onLoaded?.Invoke(loadedSprite);
            }
            else
            {
                Debug.LogError($"다음 Sprite를 가져오는데 실패함. { spriteName }");
                onLoaded?.Invoke(null);
            }
        };
    }

    // 무기 프리팹 불러오기
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
                Debug.Log($"다음 Prefab을 가져오는데 실패함.{ prefabName }");
                onLoaded?.Invoke(null);
            }
        };
    }

}