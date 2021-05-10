using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToMainMenu : MonoBehaviour
{
    NetworkManagerLobby networkManager;
    void Start()
    {
        networkManager = FindObjectOfType<NetworkManagerLobby>();
    }

    public void ChangeSceneToMainMenu()
    {
        networkManager.StopClient();
        networkManager.StopServer();
        networkManager.ServerChangeScene(networkManager.menuScene);
    }
}
