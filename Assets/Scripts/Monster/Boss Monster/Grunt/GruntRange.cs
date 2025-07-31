using Photon.Pun;
using UnityEngine;

/*
                            << GruntRange >>

        - Grunt 생성시 첫 타깃플레이어를 지정하기위한 Collider 이벤트
        
        - 플레이어가 Range에 접근시 타깃플레이어로 지정
 */

public class GruntRange : MonoBehaviour
{
    [SerializeField] private BoxCollider scanRange;         // 적 탐지 범위
    private Grunt parent;

    private void Awake()
    {
        parent = GetComponentInParent<Grunt>();
        scanRange.enabled = false;
    }

    private void Update()
    {
        if(GameManager.Instance.isMultiPlaying)
        {
            if (!scanRange.enabled && !MultiDungeonManager.Instance.isCutScenePlaying)
                scanRange.enabled = true;
        }
        else
        {
            if (!scanRange.enabled && !DungeonManager.Instance.isCutScenePlaying)
                scanRange.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (!GameManager.Instance.isMultiPlaying)
            parent.TargetPlayer = GameManager.Instance.player;
        else if (PhotonNetwork.IsMasterClient)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            int viewID = player.GetComponent<PhotonView>().ViewID;
            parent.photonView.RPC(nameof(parent.SetTargetPlayer), RpcTarget.All, viewID);
        }
    }
}
