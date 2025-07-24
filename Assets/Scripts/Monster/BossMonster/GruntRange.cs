using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GruntRange : MonoBehaviour
{
    [SerializeField] private BoxCollider scanRange;         // Àû Å½Áö ¹üÀ§
    private Grunt parent;

    private void Awake()
    {
        parent = GetComponentInParent<Grunt>();
        scanRange.enabled = false;
    }

    private void Update()
    {
        if (!scanRange.enabled && !MultiDungeonManager.Instance.isCutScenePlaying)
            scanRange.enabled = true;
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
