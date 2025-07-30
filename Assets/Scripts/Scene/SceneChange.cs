using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

/*
                            << SceneChange >>

        - 다음 씬으로 이동하기위한 클래스
            - 현재씬 -> 로딩씬 -> 다음씬

        - Portal 오브젝트에 붙여서 사용
 */

public class SceneChange : MonoBehaviourPunCallbacks
{
    [Header("Next Scene Name")]
    public string nextScene;                        // 연결된 씬 이름

    private bool isExiting = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || isExiting)
            return;

        isExiting = true;

        if(GameManager.Instance.isMultiPlaying)
        {
            // 멀티씬 -> 싱글씬
            if (PhotonNetwork.InRoom && PhotonNetwork.NetworkClientState == ClientState.Joined)
            {
                PhotonNetwork.LeaveRoom();
            }
        }
        else
        {
            // 씬 불러오기
            Loading.LoadNextScene(nextScene);
        }
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.Disconnect();
    }

    // 연결종료 -> 다음씬 로딩
    public override void OnDisconnected(DisconnectCause cause)
    {
        Loading.LoadNextScene(nextScene);
    }


}
