using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;           // ������ ������(�̹���)
    [SerializeField] private Text amountText;           // ������ ����
    [SerializeField] private Image highlightImage;      // ���̶���Ʈ �̹���
    
    private Image slotImage;
    private InventoryUI inventoryUI;

    private RectTransform slotRect;                     // ������ RT
    private RectTransform iconRect;                     // ������ ������ ������ RT
    private RectTransform highlightRect;                // ������ ���̶���Ʈ RT

    private GameObject iconGo;                          
    private GameObject textGo;
    private GameObject highlightGo;


    private float padding = 1f;                         // ���� �� �����ܰ� ���� ���� ����
    private float maxHighlightAlpha = 0.5f;             // ���̶���Ʈ �̹��� ���İ�
    private float currentHighlightAlpha = 0f;           // ���� ���̶���Ʈ �̹��� ���İ�
    private float highlightFadeDuration = 0.2f;         // ���̶���Ʈ �ҿ� �ð�

    private bool isAccessibleSlot = true;               // ���� ���ٰ��� ����
    private bool isAccessibleItem = true;               // ������ ���ٰ��� ����

    // ��Ȱ��ȭ�� ���� ����
    private Color InAccessibleSlotColor = new Color(0.2f, 0.2f, 0.2f, 0.5f);
    // ��Ȱ��ȭ�� ������ ����
    private Color InAccessibleIconColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);

    #region ** PROPERTIES **
    public int Index { get; private set; }              // ���� �ε���
    public bool HasItem => iconImage.sprite != null;    // ���Կ� �������� �ִ��� ����(sprite ���η� Ȯ��)

    public bool IsAccessible => isAccessibleItem && isAccessibleSlot;

    public RectTransform SlotRect => slotRect;

    public RectTransform IconRect => iconRect;
    #endregion

    private void Awake()
    {
        InitComponents();
        InitValues();
    }

    // �ʱ�ȭ
    private void InitComponents()
    {
        inventoryUI = GetComponentInParent<InventoryUI>();

        slotRect = GetComponent<RectTransform>();
        iconRect = iconImage.rectTransform;
        highlightRect = highlightImage.rectTransform;

        iconGo = iconRect.gameObject;
        textGo = amountText.gameObject;
        highlightGo = highlightImage.gameObject;

        slotImage = GetComponent<Image>();
    }

    // �ʱ�ȭ
    private void InitValues()
    {
        // ������ RT ����(Pivot : �߾�, Anchor : Top Left)
        iconRect.pivot = new Vector2(0.5f, 0.5f);
        iconRect.anchorMin = Vector2.zero;
        iconRect.anchorMax = Vector2.one;

        // ������ �е� ����
        iconRect.offsetMin = Vector2.one * (padding);
        iconRect.offsetMax = Vector2.one * (-padding);

        // �����ܰ� ���̶���Ʈ RT�� �����ϰ� ����
        highlightRect.pivot = iconRect.pivot;
        highlightRect.anchorMin = iconRect.anchorMin;
        highlightRect.anchorMax = iconRect.anchorMax;
        highlightRect.offsetMin = iconRect.offsetMin;
        highlightRect.offsetMax = iconRect.offsetMax;

        // ������ �� ���̶���Ʈ �̹����� Ŭ��X
        iconImage.raycastTarget = false;
        highlightImage.raycastTarget = false;

        HideIcon();
        // ���̶���Ʈ ȿ�� ������
        highlightGo.SetActive(false);
    }

    // ������ ������ Ȱ��ȭ
    private void ShowIcon() => iconGo.SetActive(true);
    
    // ������ ������ ��Ȱ��ȭ
    private void HideIcon() => iconGo.SetActive(false);
   
    // ���� �ؽ�Ʈ Ȱ��ȭ
    private void ShowText() => textGo.SetActive(true);
    
    // ���� �ؽ�Ʈ ��Ȱ��ȭ
    private void HideText() => textGo.SetActive(false);


    // ���� �ε��� ����
    public void SetSlotIndex(int index) => Index = index;
    
    // ���� Ȱ��ȭ/��Ȱ��ȭ ���� ����
    public void SetSlotAccessibleState(bool value)
    {
        // ���� ���Ի��°� �����ϰ����ϴ� value�� ������ ����
        if (isAccessibleSlot == value) return;

        // Ȱ��ȭ�� ����
        if(value)
        {
            slotImage.color = Color.black;
        }
        // ��Ȱ��ȭ�� ����
        else
        {
            slotImage.color = InAccessibleSlotColor;
            HideIcon();
            HideText();
        }

        isAccessibleSlot = value;
    }

    // ������ Ȱ��ȭ/��Ȱ��ȭ ���� ����
    public void SetItemAccessibleState(bool value)
    {
        // ���� �����ۻ��°� �����ϰ����ϴ� value�� ������ ����
        if (isAccessibleItem == value) return;

        // Ȱ��ȭ�� ������ ����
        if(value)
        {
            iconImage.color = Color.white;
            amountText.color = Color.white;
        }
        // ��Ȱ��ȭ�� ������ ����
        else
        {
            iconImage.color = InAccessibleIconColor;
            amountText.color = InAccessibleIconColor;
        }

        isAccessibleItem = value;
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

    // ���̶���Ʈ Fade-in
    private IEnumerator HighlightFadeIn()
    {
        // �������� fade-out ���߱�
        StopCoroutine(nameof(HighlightFadeOut));

        // ���̶���Ʈ �̹��� Ȱ��ȭ
        highlightGo.SetActive(true);

        float timer = maxHighlightAlpha / highlightFadeDuration;

        // ���̶���Ʈ �̹��� ���İ��� ������ ������Ű��
        for(; currentHighlightAlpha <= maxHighlightAlpha; currentHighlightAlpha += timer * Time.deltaTime)
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
        for(; currentHighlightAlpha >= 0f; currentHighlightAlpha -= timer * Time.deltaTime)
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
}
