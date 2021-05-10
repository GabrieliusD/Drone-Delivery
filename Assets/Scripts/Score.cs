using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Score : NetworkBehaviour
{
    [SyncVar] public int index;
    [SyncVar] public string PlayerName;
    [SyncVar] public int points;

    ScoreManager scoreManager;
    private void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
    }
    void Update()
    {
        scoreManager.setScore(index, points, PlayerName);
    }
    
}
