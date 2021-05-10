using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class MainMenu : MonoBehaviour
{
    [Header("UI")]
    public NetworkManagerLobby networkManager = null;
    public GameObject landingPagePanel = null;

    public void HostLobby()
    {
        if(networkManager == null) networkManager = FindObjectOfType<NetworkManagerLobby>();
        networkManager.StartHost();
        landingPagePanel.SetActive(false);
    }
}
