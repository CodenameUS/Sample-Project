using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SceneBGMInfo : MonoBehaviour
{
    [Tooltip("�� ������ ����� BGM Key")]
    [SerializeField] private string bgmKey;
    [SerializeField] private Transform startingPoint;    

    private void Start()
    {
        if(!string.IsNullOrEmpty(bgmKey))
        {
            AudioManager.Instance.PlayBGM(bgmKey);
        }

        GameManager.Instance.player.transform.position = startingPoint.position;
    }
}
