using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SceneChange : MonoBehaviourPunCallbacks
{
    [Header("Next Scene Name")]
    public string nextScene;                        // ¿¬°áµÈ ¾À ÀÌ¸§

    private bool isExiting = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || isExiting)
            return;

        isExiting = true;

        if(GameManager.Instance.isMultiPlaying)
        {
            // ¸ÖÆ¼¾À -> ½Ì±Û¾À
            if (PhotonNetwork.InRoom && PhotonNetwork.NetworkClientState == ClientState.Joined)
            {
                Debug.Log("LeaveRoom ½ÇÇà");
                PhotonNetwork.LeaveRoom();
            }
        }
        else
        {
            // ¾À ºÒ·¯¿À±â
            Loading.LoadNextScene(nextScene);
        }
    }

    public override void OnLeftRoom()
    {
        Debug.Log("LeftRoom ½ÇÇà");
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnect ½ÇÇà");
        Loading.LoadNextScene(nextScene);
    }


}
