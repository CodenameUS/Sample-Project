using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlotUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;           // 아이템 아이콘 이미지
    [SerializeField] private Image highlightImage;      // 하이라이트 이미지

    private EquipmentUI equipmentUI;

    private GameObject highlightGo;
    private GameObject iconGo;

    private RectTransform highlightRect;

    private float maxHighlightAlpha = 0.5f;             // 하이라이트 이미지 최대 알파값
    private float currentHighlightAlpha = 0f;           // 현재 하이라이트 이미지 알파값
    private float highlightFadeDuration = 0.2f;         // 하이라이트 소요시간

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        equipmentUI = GetComponentInParent<EquipmentUI>();

        highlightGo = highlightImage.gameObject;
        highlightImage.raycastTarget = false;
        highlightRect = highlightImage.rectTransform;

        highlightGo.SetActive(false);
    }

    // 하이라이트 이미지를 상/하단으로 표시
    public void SetHighlightOnTop(bool value)
    {
        if (value)
            highlightRect.SetAsLastSibling();
        else
            highlightRect.SetAsFirstSibling();
    }

    // 슬롯 하이라이트 표시 및 해제
    public void Highlight(bool show)
    {
        if (show)
            StartCoroutine(nameof(HighlightFadeIn));
        else
            StartCoroutine(nameof(HighlightFadeOut));
    }

    #region ** Coroutines **
    // 하이라이트 Fade-in
    private IEnumerator HighlightFadeIn()
    {
        // 실행중인 fade-out 멈추기
        StopCoroutine(nameof(HighlightFadeOut));

        // 하이라이트 이미지 활성화
        highlightGo.SetActive(true);

        float timer = maxHighlightAlpha / highlightFadeDuration;

        // 하이라이트 이미지 알파값을 서서히 증가시키기
        for (; currentHighlightAlpha <= maxHighlightAlpha; currentHighlightAlpha += timer * Time.deltaTime)
        {
            highlightImage.color = new Color(
                     highlightImage.color.r,
                     highlightImage.color.g,
                     highlightImage.color.b,
                     currentHighlightAlpha
                );

            yield return null;
        }
    }

    // 하이라이트 Fade-out
    private IEnumerator HighlightFadeOut()
    {
        StopCoroutine(nameof(HighlightFadeIn));

        float timer = maxHighlightAlpha / highlightFadeDuration;

        // 하이라이트 이미지 알파값을 서서히 감소시키기
        for (; currentHighlightAlpha >= 0f; currentHighlightAlpha -= timer * Time.deltaTime)
        {
            highlightImage.color = new Color(
                     highlightImage.color.r,
                     highlightImage.color.g,
                     highlightImage.color.b,
                     currentHighlightAlpha
                );

            yield return null;
        }

        highlightGo.SetActive(false);
    }
    #endregion
}
