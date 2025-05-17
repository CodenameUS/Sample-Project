using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
            - ���ӽ��۽� MainScene �ε� �� ��Ŀ��
 
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
        // Additive�� MainScene �ҷ�����
        yield return SceneManager.LoadSceneAsync("Viliage", LoadSceneMode.Additive);
        
        // MainScene ��Ŀ��
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Viliage"));

        sceneCover.SetActive(false);
    }
}
