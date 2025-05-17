using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    [SerializeField] private Image progressBar;
    [SerializeField] private Text tipText;

    
    private static string nextScene;
    private static string prevSceneName;

    string[] gameTips =
    {
        "�÷��̾ ����ϸ� ������ ���ư��ϴ�.",
        "�������� ü���� ������ �� �ֽ��ϴ�.",
        "�� ���� ���� ������ Ŭ�����ϴµ� ū ������ �˴ϴ�."
    };

    private void Start()
    {
        StartCoroutine(LoadSceneProgress());
        ShowGameTips();
    }

    // �ε��� �ҷ�����
    public static void LoadNextScene(string sceneName)
    {
        prevSceneName = SceneManager.GetActiveScene().name;
        nextScene = sceneName;
        SceneManager.LoadSceneAsync("Loading", LoadSceneMode.Additive);
    }

    private IEnumerator LoadSceneProgress()
    {
        // �ε����� ActiveScene���� ����
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Loading"));

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);
        op.allowSceneActivation = false;
        
        float timer = 0f;

        while(!op.isDone)
        {
            yield return null;

            if(op.progress < 0.9f)
            {
                progressBar.rectTransform.sizeDelta = new Vector2(op.progress * 1920f, 80f);
            }
            // 90% �ε� ���ķ� Fake �ε�
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

        // �ε� �Ϸ� �� ��ȯ
        yield return null;

        // �ε�� ���� ���� ActiveScene���� ����
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(nextScene));

        // ���� �� ��ε�(PersistentScene ����)
        if(prevSceneName != "PersistentScene" && prevSceneName != "Loading")
        {
            SceneManager.UnloadSceneAsync(prevSceneName);
        }

        // �ε����� ��ε�
        SceneManager.UnloadSceneAsync("Loading");
    }

    private void ShowGameTips()
    {
        int ran = Random.Range(0, gameTips.Length);

        tipText.text = "Game Tip. " + gameTips[ran];
    }
}
