using UnityEngine;

/*
                            << ViliageManager >>

        - Viliage 씬 입장시 플레이어 및 카메라 세팅
        
        - 멀티플레이후 Viliage씬 진입시 미리배치된 플레이어, 카메라 오브젝트 활성화
 */
public class ViliageManager : MonoBehaviour
{
    [SerializeField] private GameObject playerObj;
    [SerializeField] private GameObject cameraObj;
    [SerializeField] private Transform startingPoint;


    private void Start()
    {
        // 로딩끝
        GameManager.Instance.isLoading = false;

        SpawnPlayer();
        Init();

        if (startingPoint != null)
            GameManager.Instance.player.transform.position = startingPoint.position;
    }

    // 플레이어 오브젝트 활성화
    private void SpawnPlayer()
    {
        if (GameManager.Instance.player != null)
            return;

        playerObj.SetActive(true);
        cameraObj.SetActive(true);
    }

    // 데이터 로딩
    private void Init()
    {
        GameManager.Instance.FindPlayerObject();
        GameManager.Instance.FindCameraObject();
        DataManager.Instance.LoadPlayerData();
        EquipmentUI.Instance.LoadEquipmentSlotData();
    }
}
