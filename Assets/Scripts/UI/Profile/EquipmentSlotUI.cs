using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlotUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;           // ������ ������ �̹���
    [SerializeField] private Image highlightImage;      // ���̶���Ʈ �̹���

    private EquipmentUI equipmentUI;

    private GameObject highlightGo;
    private GameObject iconGo;

    private RectTransform highlightRect;

    private float maxHighlightAlpha = 0.5f;             // ���̶���Ʈ �̹��� �ִ� ���İ�
    private float currentHighlightAlpha = 0f;           // ���� ���̶���Ʈ �̹��� ���İ�
    private float highlightFadeDuration = 0.2f;         // ���̶���Ʈ �ҿ�ð�

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

    // ���̶���Ʈ �̹����� ��/�ϴ����� ǥ��
    public void SetHighlightOnTop(bool value)
    {
        if (value)
            highlightRect.SetAsLastSibling();
        else
            highlightRect.SetAsFirstSibling();
    }

    // ���� ���̶���Ʈ ǥ�� �� ����
    public void Highlight(bool show)
    {
        if (show)
            StartCoroutine(nameof(HighlightFadeIn));
        else
            StartCoroutine(nameof(HighlightFadeOut));
    }

    #region ** Coroutines **
    // ���̶���Ʈ Fade-in
    private IEnumerator HighlightFadeIn()
    {
        // �������� fade-out ���߱�
        StopCoroutine(nameof(HighlightFadeOut));

        // ���̶���Ʈ �̹��� Ȱ��ȭ
        highlightGo.SetActive(true);

        float timer = maxHighlightAlpha / highlightFadeDuration;

        // ���̶���Ʈ �̹��� ���İ��� ������ ������Ű��
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

    // ���̶���Ʈ Fade-out
    private IEnumerator HighlightFadeOut()
    {
        StopCoroutine(nameof(HighlightFadeIn));

        float timer = maxHighlightAlpha / highlightFadeDuration;

        // ���̶���Ʈ �̹��� ���İ��� ������ ���ҽ�Ű��
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
