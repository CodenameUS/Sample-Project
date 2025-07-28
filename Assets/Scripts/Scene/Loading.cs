using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
                            << Loading >>

        - 씬 변경 클래스
        
        - 로딩바 시각화(LoadSceneProgress())
            - 다음씬 준비가 완료되면(90%) 로딩바 100%까지는 Fake 로딩
 */

public class Loading : MonoBehaviour
{
    [SerializeField] private Image progressBar;             // 로딩바 UI
    [SerializeField] private Text tipText;                  // 게임팁 Text UI

    private static string nextScene;                        // 로딩될 다음씬 이름
    private static string prevSceneName;                    // 기존씬 이름

    string[] gameTips =
    {
        "플레이어가 사망하면 마을로 돌아갑니다.",
        "포션으로 체력을 보충할 수 있습니다.",
        "더 좋은 장비는 던전을 클리어하는데 큰 도움이 됩니다."
    };

    private void Start()
    {
        GameManager.Instance.isLoading = true;
        StartCoroutine(LoadSceneProgress());
        ShowGameTips();
    }

    // 로딩씬 불러오기
    public static void LoadNextScene(string sceneName)
    {
        prevSceneName = SceneManager.GetActiveScene().name;
        nextScene = sceneName;
        SceneManager.LoadSceneAsync("Loading", LoadSceneMode.Additive);
    }

    // 로딩바 구현
    private IEnumerator LoadSceneProgress()
    {
        // 로딩씬을 ActiveScene으로 설정
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Loading"));

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);
        op.allowSceneActivation = false;
        
        float timer = 0f;

        // 로딩게이지 표시
        while(!op.isDone)
        {
            yield return null;

            if(op.progress < 0.9f)
            {
                progressBar.rectTransform.sizeDelta = new Vector2(op.progress * 1920f, 80f);
            }
            // 90% 로딩 이후로 Fake 로딩
            else
            {
                timer += Time.unscaledDeltaTime;
                progressBar.rectTransform.sizeDelta = new Vector2(1728f + timer * 20f, 80f);
                if(progressBar.rectTransform.sizeDelta.x >= 1920f)
                {
                    op.allowSceneActivation = true;
                }
            }
        }

        // 로딩 완료 후 전환
        yield return null;

        // 로드될 다음 씬을 ActiveScene으로 설정
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(nextScene));

        // 이전 씬 언로드
        if(prevSceneName != "PersistentScene" && prevSceneName != "Loading")
        {
            SceneManager.UnloadSceneAsync(prevSceneName);
        }

        // 로딩 씬 언로드
        SceneManager.UnloadSceneAsync("Loading");
    }

    // 로딩중 게임팁 출력
    private void ShowGameTips()
    {
        int ran = Random.Range(0, gameTips.Length);

        tipText.text = "Game Tip. " + gameTips[ran];
    }
}
