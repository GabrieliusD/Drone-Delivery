using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class DeliveryPlatform : MonoBehaviour
{
    ScoreManager scoreManager;
    private void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        // if (other.tag == "box")
        // {
        //     string player = other.GetComponent<BoxData>().player;
        //     int points = other.GetComponent<BoxData>().pointAmount;
        //     if (scoreManager == null) scoreManager = FindObjectOfType<ScoreManager>();
        //     scoreManager.CmdIncreasePointsForPlayer(player, points);
        //     Debug.Log("the box was delivered");
        //     scoreManager.CmdDestroyGameObject(other.gameObject);

        // }
    }
}
