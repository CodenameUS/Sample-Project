using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ResourceManager : MonoBehaviour
{
    private Dictionary<string, Sprite> cashedSpriteDictionary = new Dictionary<string, Sprite>();

    // 어드레서블 Sprite 경로
    private const string assetRef = "Assets/Sprites/";           

    void Start()
    {
       
    }

 
    public void LoadIcon(string spriteName, System.Action<Sprite> onLoaded)
    {
        // 캐시에 이미 있을 때는 캐싱
        if (cashedSpriteDictionary.TryGetValue(spriteName, out var cachedSprite))
        {
            onLoaded?.Invoke(cachedSprite);
            return;
        }

        // 어드레서블에서 Sprite 로드(이름으로)
        Addressables.LoadAssetAsync<Sprite>(assetRef + spriteName).Completed += handle =>
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

}
