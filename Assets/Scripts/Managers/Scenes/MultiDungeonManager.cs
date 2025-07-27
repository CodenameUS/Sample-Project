using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;

/*
                            << MultiDungeonManager >>

        - 던전(Multi)의 이벤트 관리자(싱글톤)
        
        - 씬 입장시 각 플레이어 생성(SpawnPlayer()), 데이터로딩(Init()), 몬스터생성(SpawnMonster())

        - 모든몬스터 처치시 보스몬스터 생성처리(SpawnBossMonster())
            - 보스몬스터 죽음 이벤트 구독 => DungeonClear() 실행
            - 보스몬스터 등장 컷신 플레이(PlayCutScene())

        - 보스몬스터 처치시 죽음 이벤트 호출
            - 클리어UI 활성화 및 보상획득 가능 => 보상획득 후 탈출 포탈생성
            - 포탈을 통해 Viliage 씬으로 이동
 */

public class MultiDungeonManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform[] playerSpawnPositions;              // 플레이어 스폰위치
    [SerializeField] private Transform[] monsterSpawnPositions;             // 몬스터 스폰위치
    [SerializeField] private Transform bossSpawnPosition;                   // 보스몬스터 스폰위치
    [SerializeField] private Transform portalSpawnPosition;                 // 포탈 스폰위치
    [SerializeField] private GameObject cutSceneObj;                        // 컷씬 카메라 오브젝트
    [SerializeField] private GameObject portalPrefab;                       // 포탈 프리팹
    [SerializeField] private TimelineAsset[] timelineAsset;                 // 타임라인 에셋
    [SerializeField] private GameObject clearUI;

    public int currentMonsterCount = -1;                    // 현재 몬스터 수(기본 -1)
    private GameObject boss;                                // 보스몬스터 오브젝트
    private PlayableDirector pd;                            

    private bool bossSpawned = false;                       // 보스 등장여부
    public bool isCutScenePlaying = false;                  // 컷씬 진행 여부

    private static MultiDungeonManager instance;
    public static MultiDungeonManager Instance => instance;


    #region ** UnityEvents **
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

        SpawnPlayer();
        SpawnMonster();
        Init();
    }

    private void Update()
    {
        if(currentMonsterCount == 0 && PhotonNetwork.IsMasterClient && !bossSpawned)
        {
            SpawnBossMonster();
        }
    }

    #endregion

    // 플레이어 오브젝트 생성
    private void SpawnPlayer()
    {
        var localPlayerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;                    // 플레이어 넘버
        var spawnPosition = playerSpawnPositions[localPlayerIndex];                          // 플레이어 위치 설정

        PhotonNetwork.Instantiate("Player/MultiPlayer", spawnPosition.position, spawnPosition.rotation); 
    }

    // 초기화
    private void Init()
    {
        // 데이터
        GameManager.Instance.FindPlayerObject();
        GameManager.Instance.FindCameraObject();
        DataManager.Instance.LoadPlayerData();
        EquipmentUI.Instance.LoadEquipmentSlotData();

        pd = GetComponent<PlayableDirector>();
        pd.played += OnCutSceneStarted;
        pd.stopped += OnCutSceneEnded;
    }

    // 몬스터 생성
    private void SpawnMonster()
    {
        // 마스터 클라이언트만 
        if (!PhotonNetwork.IsMasterClient) return;

        // 몬스터 소환
        for(int i = 0;i<monsterSpawnPositions.Length;i++)
        {
            PhotonNetwork.Instantiate("Monsters/TurtleShell_Multi",
                monsterSpawnPositions[i].position, monsterSpawnPositions[i].rotation);

            currentMonsterCount = i + 1;
        }
    }

    // 보스몬스터 생성
    private void SpawnBossMonster()
    {
        // 마스터 클라이언트만 
        if (!PhotonNetwork.IsMasterClient) return;

        bossSpawned = true;

        // 보스몬스터 소환
        boss = PhotonNetwork.Instantiate("Monsters/Boss_Grunt_Multi", bossSpawnPosition.position, bossSpawnPosition.rotation);

        // 죽음 이벤트 등록
        boss.GetComponent<BossMonster>().OnBossDied += OnBossDied;

        // 컷씬 플레이
        photonView.RPC(nameof(PlayCutScene), RpcTarget.All);
    }

    // 보스 죽음 이벤트
    [PunRPC]
    private void OnBossDied()
    {
        StartCoroutine(DungeonClear());
    }

    // 던전 클리어
    private IEnumerator DungeonClear()
    {
        // 딜레이
        yield return new WaitForSeconds(3f);

        clearUI.SetActive(true);
    }

    [PunRPC]
    public void PlayCutScene()
    {
        cutSceneObj.gameObject.SetActive(false);
        pd.Play(timelineAsset[0]);
    }

    // 컷씬 시작
    public void OnCutSceneStarted(PlayableDirector director)
    {
        GameManager.Instance.player.isCutscenePlaying = true;
        isCutScenePlaying = true;

        GameManager.Instance.player.transform.position = playerSpawnPositions[PhotonNetwork.LocalPlayer.ActorNumber - 1].position;
        GameManager.Instance.player.transform.LookAt(bossSpawnPosition);
    }

    // 컷씬 끝
    public void OnCutSceneEnded(PlayableDirector director)
    {
        GameManager.Instance.player.isCutscenePlaying = false;
        isCutScenePlaying = false;
    }

    // 던전 클리어 UI 닫기 및 보상획득
    public void GetRewardsAndSetActiveFalse()
    {
        clearUI.SetActive(false);
        DataManager.Instance.GetPlayerData().UseGold(-500);
        GameObject portal = Instantiate(portalPrefab, portalSpawnPosition.position, portalSpawnPosition.rotation);
        portal.GetComponent<SceneChange>().nextScene = "Viliage";
    }
}
