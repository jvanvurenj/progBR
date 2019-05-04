using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class GameManager : NetworkBehaviour
{
    public Text Text;
    public int minimumPlayers = 4;
    public int countdownTime = 4;
    private int numConnected;
    bool AllConnected = false;

    void Start()
    {
        StartCoroutine(WaitForPlayers());
    }


    [Command]
    public void CmdStartGame()
    {
        RpcStartGame();
    }

    [ClientRpc]
    public void RpcStartGame()
    {
        StartCoroutine(CountDown(countdownTime));
    }

    [Command]
    public void CmdUnrestrict()
    {
        RpcUnrestrict();
    }

    [ClientRpc]
    public void RpcUnrestrict()
    {
        UnRestrictPlayers();
    }


    IEnumerator WaitForPlayers()
    {
        // Wait to start game until minimum players are connected
        int connected = GetConnectionCount();

        while (isServer && connected < minimumPlayers)
        {
            connected = GetConnectionCount();
            Text.text = "Waiting for players to connect...\n" + connected + " players connected.";
            yield return null;
        }
        // Delay to account for latency lag
        int waitTime = 2;
        while(isServer && waitTime > 0)
        {
            Text.text = "Waiting for all connections...";
            waitTime -= 1;
            yield return new WaitForSeconds(1);
        }
        if (isServer) { CmdStartGame(); }
            
    }

    void UnRestrictPlayers()
    {
        GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject obj in Players)
        {
            obj.GetComponent<PlayerMovement>().isEnabled = true;
        }
    }


    IEnumerator CountDown(int n)
    {
        // Countdown for game to begin
        int cooldown = n;
        while(cooldown >= 0)
        {
            Text.text = "Starting game in " + Mathf.Round(cooldown) + " seconds";
            cooldown -= 1;
            yield return new WaitForSeconds(1f);
        }
        Text.text = "Start!";
        yield return new WaitForSeconds(.25f);
        Text.text = "";
        CmdUnrestrict();

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
        return Players.Length == 1;
    }

 

}
