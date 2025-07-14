using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ChatManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField inputField;             // 채팅 입력란
    [SerializeField] private ScrollRect scrollRect;                 // 채팅 스크롤뷰

    [SerializeField] private GameObject chatLogPrefab;              // 채팅로그 프리팹
    [SerializeField] private Transform contentTransform;            // 채팅로그가 생성될 위치

    private Queue<GameObject> messageQueue = new();
    private int maxMessages = 10;                           // 최대 10개 메세지 표시
    private bool isInputActive = false;                     // 입력모드인지 여부

    private void Start()
    {
        // 플레이어 입장 메세지
        string temp = $"<color=green>{PhotonNetwork.NickName} Joined the chat room.</color>";
        photonView.RPC("ReceiveMessage",RpcTarget.All, temp);

        // 엔터키 입력
        inputField.onSubmit.AddListener(_ => SendChatMessage());
        inputField.DeactivateInputField();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            // 입력모드가 아닐 경우 -> 입력시작
            if(!isInputActive)
            {
                isInputActive = true;
                GameManager.Instance.isChatting = true;
                inputField.ActivateInputField();
            }
            else
            {
                // 입력모드일 때 -> 텍스트가 없으면 포커스해제
                if(string.IsNullOrWhiteSpace(inputField.text))
                {
                    isInputActive = false;
                    GameManager.Instance.isChatting = true;
                    inputField.DeactivateInputField();
                    EventSystem.current.SetSelectedGameObject(null);        // 포커스해제
                }
            }
        }

    }

    // 채팅 전송
    private void SendChatMessage()
    {
        if(!string.IsNullOrEmpty(inputField.text))
        {
            // Player : ~~~
            string message = PhotonNetwork.NickName + ": " + inputField.text;
            photonView.RPC("ReceiveMessage", RpcTarget.All, message);
            inputField.text = "";               // 입력창 초기화

            // 포커스 유지
            inputField.ActivateInputField();
        }
    }

    [PunRPC]
    void ReceiveMessage(string message)
    {
        AppendMessage(message);
    }

    void AppendMessage(string message)
    {
        // 채팅로그 생성
        GameObject go = Instantiate(chatLogPrefab, contentTransform);
        TMP_Text text = go.GetComponent<TMP_Text>();

        text.text = "";
        StartCoroutine(AssignMessageDelayed(text, message));
        
        messageQueue.Enqueue(go);
        if(messageQueue.Count > maxMessages)
        {
            GameObject oldestMessage = messageQueue.Dequeue();
            Destroy(oldestMessage);
        }

        ScrollToBottom();
    }

    // 새로운 채팅으로 포커싱
    void ScrollToBottom()
    {
        StartCoroutine(ScrollToBottomCoroutine());
    }

    private IEnumerator ScrollToBottomCoroutine()
    {
        yield return new WaitForEndOfFrame();

        // 아래로 스크롤
        scrollRect.verticalNormalizedPosition = 0f;

        // Canvas 갱신
        Canvas.ForceUpdateCanvases();
    }

    // 이전 메세지 출력 방지
    private IEnumerator AssignMessageDelayed(TMP_Text text, string message)
    {
        yield return null;
        text.SetText(message);
    }
}
