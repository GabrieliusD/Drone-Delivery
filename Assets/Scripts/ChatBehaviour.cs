using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Events;
using Mirror;
public class ChatBehaviour : NetworkBehaviour
{
    [SerializeField] GameObject chatUI = null;
    [SerializeField] TMP_Text chatText = null;
    [SerializeField] TMP_InputField inputField = null;

    static event Action<string> onMessage;

    public override void OnStartAuthority()
    {
        chatUI.SetActive(true);

        onMessage += HandleNewMessage;
    }

    [ClientCallback]
    void OnDestroy()
    {
        if(!hasAuthority) return;

        onMessage -= HandleNewMessage;
    }

    void HandleNewMessage(string message)
    {
        chatText.text += message;
    }

    [Client]
    public void Send(string message)
    {
        if(!Input.GetKeyDown(KeyCode.Return)) return;
        if(string.IsNullOrWhiteSpace(message)) return;
        CmdSendMessage(inputField.text);
        inputField.text = string.Empty;
    }

    [Command]
    void CmdSendMessage(string message)
    {
        RpcHandleMessage($"[{connectionToClient.connectionId}]: {message}");
    }
    [ClientRpc]
    void RpcHandleMessage(string message)
    {
        onMessage?.Invoke($"\n{message}");
        
    }
}
