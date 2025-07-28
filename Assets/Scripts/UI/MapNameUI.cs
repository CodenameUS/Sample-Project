using System.Collections;
using UnityEngine;
using UnityEngine.UI;


/*
                            << MapNameUI >>

        - 씬에 입장할 때 현재 맵이름을 UI로 표시해주는 클래스
            - Fade-In/Out 효과
 */


public class MapNameUI : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    public Text mapNameText;                    // 맵 이름 텍스트 UI
    public string mapName;                      // 맵 이름

    private float duration = 1f;                // Fade 효과가 이루어질때까지의 시간
    private float displayDuration = 2f;         // Fade-In Fade-Out 간의 시간

    private void OnEnable()
    {
        canvasGroup.alpha = 0;
        mapNameText.text = mapName;
        StartCoroutine(FadeRoutine());
    }

    // Fade In -> Fade Out
    private IEnumerator FadeRoutine()
    {
        yield return Fade(0, 1);

        // 완전히 표시된 상태로 displayDuration 만큼 유지
        yield return new WaitForSeconds(displayDuration);

        yield return Fade(1, 0);
        gameObject.SetActive(false);
    }

    // Alpha 값 from -> to 까지 변화
    private IEnumerator Fade(float from, float to)
    {
        float elapsed = 0f;

        while(elapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = to;
    }
}
