using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MatchingTextEffect : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI matchingText;
    [SerializeField] private GameObject matchingProgressUI;

    private string baseText = "매칭중";
    private int dotCount = 0;                   // . 갯수(0~3 까지 증가)
    private float interval = 0.5f;              // 텍스트 변화 속도 
    private bool isRunning = true;              // 제어 플래그

    private void Start()
    {
        StartCoroutine(MatchingTextAnim());
    }

    // [ 매칭중 -> 매칭중. -> 매칭중.. -> 매칭중... -> 매칭중 ] 표시
    IEnumerator MatchingTextAnim()
    {
        while(isRunning)
        {
            string dots = new string('.', dotCount);
            matchingText.text = baseText + dots;

            dotCount = (dotCount + 1) % 4;
            yield return new WaitForSeconds(interval);
        }
    }

    // UI 정지
    public void StopAnim()
    {
        isRunning = false;
        matchingProgressUI.SetActive(false);
    }

    // UI 활성화
    public void ActivateUI()
    {
        matchingProgressUI.SetActive(true);
    }
}
