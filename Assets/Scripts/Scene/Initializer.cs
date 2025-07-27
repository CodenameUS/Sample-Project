using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
                            << Initializer >>

        - 게임시작시 ViliageScene 로드 및 포커싱

 */

public class Initializer : MonoBehaviour
{
    [SerializeField] private GameObject sceneCover;         // 씬커버(게임시작시 검은색 화면)

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
