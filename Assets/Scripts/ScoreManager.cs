using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;


public class ScoreManager : NetworkBehaviour
{
    public struct PlayerScore 
    {
        public string name;
        public int points;
    }

    public PlayerScore[] playerScore = new PlayerScore[4];
    public Transform GameOverScreen;
    [SyncVar(hook=nameof(UpdateText))]public float GameTime;
    [SyncVar]public bool GameOver;

    NetworkManagerLobby networkManager;
    List<NetworkGamePlayerLobby> playerLobbies;
    public Text TimerText;

    public Text[] scoreBoardText;
    public Text WinnerText;
    void Start()
    {

            networkManager = FindObjectOfType<NetworkManagerLobby>();
            playerLobbies = networkManager.GamePlayers;
            int count = 0;
            foreach (var item in playerLobbies)
            {
                scoreBoardText[count].gameObject.SetActive(true);
                scoreBoardText[count].text = item.DisplayName + " Score: 0";
                playerScore[count].name = item.DisplayName;
                count++;
            }
            UpdateScoreBoard();
        
    }
    public void setScore(int index, int points, string name)
    {
        playerScore[index].points = points; 
        playerScore[index].name = name;
    }
    public void UpdateScoreBoard()
    {
        int count = 0;
        foreach (var item in playerScore)
        {
            scoreBoardText[count].text = item.name + " | Score: " + item.points;
            count++;
        }
    }
    
    PlayerScore FindHighestScore()
    {
        int highestScore = 0;
        PlayerScore ps = new PlayerScore();
        foreach (var item in playerScore)
        {
            if(item.points > highestScore)
            {
                highestScore = item.points;
                ps.name = item.name;
                ps.points = item.points;
            }
        }
        return ps;
    }

    void Update()
    {
        if(isServer)
        {
            GameTime -= Time.deltaTime;
            if(GameTime <= 0)
            {
                GameOver = true;
            }
        }
        UpdateScoreBoard();
        if(GameOver)
        {
            GameOverScreen.gameObject.SetActive(true);
            PlayerScore ps = FindHighestScore();
            WinnerText.text = $"The Winner is: {ps.name} with the score of: {ps.points}";
        }
    }

    void UpdateText(float oldValue, float newValue)
    {
        TimerText.text = ((int)newValue).ToString();
    }
}
