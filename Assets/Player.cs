using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    [SerializeField]
    Vector3 movement = new Vector3();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    [Client]
    void Update()
    {
        if(!hasAuthority) return;
        if(!Input.GetKeyDown(KeyCode.Space)){return;}
        CmdMove();
    }
    [Command]
    void CmdMove()
    {
        RpcMove();
    }
    [ClientRpc]
    void RpcMove() => transform.Translate(movement);

    
}
