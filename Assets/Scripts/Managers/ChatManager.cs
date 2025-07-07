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
    [SerializeField] TMP_InputField inputField;             // 채팅 입력칸
    [SerializeField] TMP_Text chatContent;                  // 채팅 표시칸
    [SerializeField] ScrollRect scrollRect;

    private Queue<string> messageQueue = new();
    private int maxMessages = 10;
    private bool isInputActive = false;                     // InputField 활성화여부

    private void Start()
    {
        AppendMessage($"<color=green>{PhotonNetwork.NickName} Joined the chat room.</color>");
        inputField.onSubmit.AddListener(_ => SendChatMessage());
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            if(!isInputActive)
            {
                isInputActive = true;
                inputField.ActivateInputField();
            }
            else
            {
                if(!string.IsNullOrWhiteSpace(inputField.text))
                {

                }
                else
                {
                    isInputActive = false;
                    inputField.DeactivateInputField();
                    EventSystem.current.SetSelectedGameObject(null);        // 포커스해제
                }
            }
        }

    }
    private void SendChatMessage()
    {
        if(!string.IsNullOrEmpty(inputField.text))
        {
            string message = PhotonNetwork.NickName + ": " + inputField.text;
            photonView.RPC("ReceiveMessage", RpcTarget.All, message);
            inputField.text = "";

            
        }
    }

    [PunRPC]
    void ReceiveMessage(string message)
    {
        AppendMessage(message);
    }

    void AppendMessage(string message)
    {
        messageQueue.Enqueue(message);
        if(messageQueue.Count > maxMessages)
        {
            messageQueue.Dequeue();
        }

        chatContent.text = string.Join("\n", messageQueue);

        ScrollToBottom();
    }

    // 새로운 채팅은 맨아래로
    void ScrollToBottom()
    {
        Canvas.ForceUpdateCanvases();

        scrollRect.verticalNormalizedPosition = 0f;
    }
}
