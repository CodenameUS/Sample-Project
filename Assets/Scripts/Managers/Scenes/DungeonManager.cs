using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

/*
                            << DungeonManager >>

        - 던전(Multi)의 이벤트 관리자(싱글톤)
        
        - 씬 입장시 플레이어 위치설정, 몬스터생성(SpawnMonster())

        - 모든몬스터 처치시 보스몬스터 생성처리(SpawnBossMonster())
            - 보스몬스터 죽음 이벤트 구독 => DungeonClear() 실행
            - 보스몬스터 등장시 컷신 플레이(PlayCutScene())

        - 보스몬스터 처치시 죽음 이벤트 호출
            - 클리어UI 활성화 및 보상획득 가능 => 보상획득 후 탈출 포탈활성화
            - 포탈을 통해 Viliage 씬으로 이동

 */

public class DungeonManager : MonoBehaviour
{
    [SerializeField] private Transform startingPoint;               // 플레이어 시작 위치
    [SerializeField] private Transform bossSpawnPoint;              // 보스 등장 위치
    [SerializeField] private Transform monsters;                    // 일반 몬스터 그룹
    [SerializeField] private Transform cutScenePlayerPos;           // 컷씬 출력시 플레이어 위치

    [SerializeField] private TimelineAsset[] timelineAsset;         // 타임라인 에셋
    [SerializeField] private GameObject cutSceneObj;                // 컷씬 카메라 오브젝트
    [SerializeField] private GameObject bossMonsterPrefab;          // 보스몬스터 프리팹
    [SerializeField] private GameObject portal;                     // 출구포탈 오브젝트

    [Header("#UI")]
    [SerializeField] private GameObject clearUI;                    // 클리어 UI 오브젝트

    private bool bossSpawned = false;                               // 보스 등장여부
    public bool isCutScenePlaying = false;                          // 컷씬 진행 여부

    private PlayableDirector pd;
    private BossMonster boss;

    private static DungeonManager instance;
    public static DungeonManager Instance => instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        GameManager.Instance.isLoading = false;

        pd = GetComponent<PlayableDirector>();

        // 이벤트 연결 
        pd.played += OnCutsceneStarted;
        pd.stopped += OnCutsceneEnded;

        if (startingPoint != null)
            GameManager.Instance.player.transform.position = startingPoint.position;

        SpawnMonster();
    }

    private void Update()
    {
        if (bossSpawned)
            return;

        if(monsters.childCount == 0)
        {
            SpawnBossMonster();
        }
    }

    // 몬스터 소환
    private void SpawnMonster()
    {
        for(int i = 0;i<monsters.childCount;i++)
        {
            monsters.GetChild(i).gameObject.SetActive(true);
        }
    }

    // 보스몬스터 소환
    private void SpawnBossMonster()
    {
        bossSpawned = true;

        Instantiate(bossMonsterPrefab, bossSpawnPoint);

        // 보스몬스터 정보 가져오기
        boss = GetComponentInChildren<BossMonster>();
        // 보스 죽음 이벤트 등록
        boss.OnBossDied += OnBossDied;
        // 컷씬 출력
        cutSceneObj.gameObject.SetActive(false);
        pd.Play(timelineAsset[0]);
    }
    
    // 보스 죽음
    private void OnBossDied()
    {
        StartCoroutine(DungeonClear());
    }

    // 던전 클리어 UI활성화
    private IEnumerator DungeonClear()
    {
        // 딜레이
        yield return new WaitForSeconds(3f);

        clearUI.SetActive(true);
    }

    // 컷씬 시작(플레이어 움직임 제어)
    private void OnCutsceneStarted(PlayableDirector director)
    {
        GameManager.Instance.player.isCutscenePlaying = true;
        isCutScenePlaying = true;

        // 플레이어 위치 설정
        GameManager.Instance.player.transform.position = cutScenePlayerPos.position;
        GameManager.Instance.player.transform.LookAt(boss.transform);

        Debug.Log("컷씬 시작");
    }

    // 컷씬 끝(플레이어 움직임 제어해제)
    private void OnCutsceneEnded(PlayableDirector director)
    {
        GameManager.Instance.player.isCutscenePlaying = false;
        isCutScenePlaying = false;
    }

    // 던전 클리어 UI 닫기 및 보상획득
    public void GetRewardsAndSetActiveFalse()
    {
        clearUI.SetActive(false);
        DataManager.Instance.GetPlayerData().UseGold(-500);
        portal.SetActive(true);
    }
}
