using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
            - 게임시작시 MainScene 로드 및 포커싱
 
 */
public class PersistentInitializer : MonoBehaviour
{
    [SerializeField] private GameObject sceneCover;

    private void Start()
    {
        StartCoroutine(InitGame());
    }

    private IEnumerator InitGame()
    {
        // Additive로 MainScene 불러오기
        yield return SceneManager.LoadSceneAsync("Viliage", LoadSceneMode.Additive);
        
        // MainScene 포커싱
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Viliage"));

        sceneCover.SetActive(false);
    }
}
