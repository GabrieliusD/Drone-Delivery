using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;
public class RoundSystem : NetworkBehaviour
{
    public Animator animator = null;

    NetworkManagerLobby room;

    NetworkManagerLobby Room
    {
        get
        {
            if (room != null) return room;
            return room = NetworkManager.singleton as NetworkManagerLobby;
        }
    }

    public void CountdownEnd()
    {
        animator.enabled = false;
    }

    public override void OnStartServer()
    {
        NetworkManagerLobby.OnServerStopped += CleanUpServer;
        NetworkManagerLobby.OnServerReadied += CheckToStartRound;
    }

    [ServerCallback]
    private void OnDestroy()
    {
        CleanUpServer();
    }
    [Server]
    void CleanUpServer()
    {
        NetworkManagerLobby.OnServerStopped -= CleanUpServer;
        NetworkManagerLobby.OnServerReadied -= CheckToStartRound;
    }


    [ServerCallback]
    public void StartRound()
    {
        RpcStartRound();
    }
    [Server]
    private void CheckToStartRound(NetworkConnection conn)
    {
        if(Room.GamePlayers.Count(x => x.connectionToClient.isReady) != Room.GamePlayers.Count) return;

        animator.enabled = true;

        RpcStartCountdown();

    }

    [ClientRpc]
    void RpcStartCountdown()
    {
        animator.enabled = true;
    }
    [ClientRpc]
    private void RpcStartRound()
    {
        InputManager.Remove(ActionMapNames.Player);
        Debug.Log("Start");
    }
}
