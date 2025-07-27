using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
                            << DamageTextManager >>

        - 공격시 데미지 표시

        - Pooling 을 활용해 데미지 폰트 사용 최적화
        - 생성자 : 데이터를 받아 초기화
        - CreateItem() : 초기화된 데이터로 아이템 객체 생성
 */

public class DamageTextManager : Singleton<DamageTextManager>
{
    // 데미지 텍스트 프리팹
    [SerializeField] private GameObject damageTextPrefab;               

    // 최대 풀 사이즈
    private int poolSize = 10;

    private Queue<TextMeshPro> pool = new Queue<TextMeshPro>();

    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    // 데미지 텍스트 생성(풀링 초기화)
    private void Init()
    {
        for(int i = 0;i<poolSize; i++)
        {
            // 데미지 텍스트 생성 및 비활성화
            GameObject obj = Instantiate(damageTextPrefab, transform);
            obj.SetActive(false);
            pool.Enqueue(obj.GetComponent<TextMeshPro>());
        }
    }

    // 데미지 표시
    public void ShowDamage(Transform pos, int damage)
    {
        TextMeshPro damageText = GetDamageText();

        // 위치 설정
        damageText.transform.position = pos.transform.position;
        // 데미지 텍스트 설정
        damageText.text = damage.ToString();

        // Fade-out 효과
        StartCoroutine(FadeOut(damageText));
    }

    // Pool 에서 데미지 텍스트 꺼내오기
    private TextMeshPro GetDamageText()
    {
        if(pool.Count > 0)
        {
            TextMeshPro text = pool.Dequeue();
            text.gameObject.SetActive(true);
            return text;
        }
        else
        {
            GameObject obj = Instantiate(damageTextPrefab, transform);
            return obj.GetComponent<TextMeshPro>();
        }
    }

    // 데미지 Fadeout 효과
    private IEnumerator FadeOut(TextMeshPro text)
    {
        float duration = 1f;                // fade-out 까지 걸리는 시간
        float elapsedTime = 0;
        Vector3 startPos = text.transform.position;
        Color startColor = text.color;

        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            text.transform.position = startPos + new Vector3(0, elapsedTime * 1.5f, 0);
            text.color = new Color(startColor.r, startColor.g, startColor.b, 1 - (elapsedTime / duration));
            yield return null;
        }

        // fade-out 후처리
        text.gameObject.SetActive(false);
        pool.Enqueue(text);
    }
}
