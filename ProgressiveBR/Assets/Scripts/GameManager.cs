using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class GameManager : NetworkBehaviour
{
    public Text Text;
    public int minimumPlayers = 4;
    public float countdownTime = 10f;

    private int numConnected;

    //void Start()
    //{
    //    StartCoroutine(GameLoop());
    //}

    void Update()
    {
        // Wait to start game until minimum players are connected
        int connected = GetConnectionCount();
        string waiting;
        if (isServer && connected < minimumPlayers)
        {
            if (connected == 1)
            {
                waiting = "Waiting for players to connect...\n" + connected + " player connected.";
            }
            else
            {
                waiting = "Waiting for players to connect...\n" + connected + " players connected.";
            }
            Text.text = waiting;
        }
      
        if (connected >= minimumPlayers)
        {
            // Countdown for game to begin
            countdownTime -= Time.deltaTime;
            Text.text = "Starting game in " + Mathf.Round(countdownTime) + " seconds";
            if (countdownTime < 0)
            {
                Text.text = "";
            }
            if (OnePlayerLeft())
            {
                Text.text = "GAME OVER!";
            }
        }
    }

    // Count the number of players connected to the server, avoiding nulls
    int GetConnectionCount()
    {
        numConnected = 0;
        foreach (NetworkConnection p in NetworkServer.connections)
        {
            if (p != null)
            {
                numConnected++;
            }
        }
        return numConnected;
    }


    // Count the number of players remaining, return true if 1
    private bool OnePlayerLeft()
    {
        GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");
        return Players.Length == 1 && numConnected > 1;
    }

    /*
    private IEnumerator GameLoop()
    { 
        Text.text = "PLAY!";

        // Keep playing until there is one player left
        while (!OnePlayerLeft())
        {
            yield return null;
        }

        Text.text = "GAME OVER!";
    }

    private IEnumerator Countdown()
    {
        while (countdownTime > 0)
        {
            Text.text = "Starting Game in " + Mathf.Round(countdownTime) + " seconds.";
            yield return new WaitForSeconds(1);
            countdownTime -= 1;
        }
    }
    */



}
