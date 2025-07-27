using UnityEngine;

/*
                            << SceneBGMInfo >>

        - 씬입장시 현재 씬의 BGM 플레이
 */

public class SceneBGMInfo : MonoBehaviour
{
    [Tooltip("BGM Key")]
    [SerializeField] private string bgmKey;         // 실행할 BGM 키

    private void Start()
    {
        if(!string.IsNullOrEmpty(bgmKey))
        {
            AudioManager.Instance.PlayBGM(bgmKey);
        }
    }
}
