﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;


public class GameManager : NetworkBehaviour
{
    // How many rounds till we stop?
    public static int numRounds = 10;
    public Text Text;
    public int minimumPlayers = 4;
    public int countdownTime = 4;
    private int numConnected;
    bool gameInProgress = false;

    void Start()
    {
        StartCoroutine(WaitForPlayers());
    }


    [Command]
    public void CmdStartGame(){RpcStartGame();}

    [ClientRpc]
    public void RpcStartGame(){StartCoroutine(CountDown(countdownTime));}

    [Command]
    public void CmdLobby() { RpcLobby(); }

    [ClientRpc]
    public void RpcLobby() { StartCoroutine(LobbyCountDown(3)); }

    [Command]
    public void CmdCheckForWin() { RpcCheckForWin(); }

    [ClientRpc]
    public void RpcCheckForWin() { StartCoroutine(CheckForWinner()); }

    [Command]
    public void CmdUnrestrict(){RpcUnrestrict();}

    [ClientRpc]
    public void RpcUnrestrict(){UnRestrictPlayers();}


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
            Text.text = "Waiting for players to connect...";
            waitTime -= 1;
            yield return new WaitForSeconds(1);
        }
        if (isServer)
        {
            CmdStartGame();
            yield return new WaitForSeconds(1);
            gameInProgress = true;
            CmdCheckForWin();
        }
          
    }

    IEnumerator CheckForWinner()
    {
        // Just have the server observe
            // Loop while game is in progress.
    while (gameInProgress)
    {
                // More that one player still alive.
        if (!OnePlayerLeft())
        {
            yield return null;
        }
                // Last player has won, reset round.
        else
        {
            int waitTime = 3;
                if (isServer)
                {
                    CmdLobby();
                }
            while (waitTime  > 0)
            {
                Text.text = "ROUND OVER!\n" + " Returning to lobby in: " + waitTime.ToString();
                waitTime -= 1;
                yield return new WaitForSeconds(1);
            }
            Text.text = "";
            numRounds += 1;
            if (isServer)
            {
                NetworkManager.singleton.ServerChangeScene("Lobby");
            }
            gameInProgress = false;
        }
    }

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
        while(cooldown > 0)
        {
            Text.text = "Starting in: \n" + Mathf.Round(cooldown);
            cooldown -= 1;
            yield return new WaitForSeconds(1f);
        }
        Text.text = "START!";
        yield return new WaitForSeconds(.25f);
        Text.text = "";
        if (isServer)
        {
            CmdUnrestrict();
        }
    }

    IEnumerator LobbyCountDown(int n)
    {
        // Countdown for game to begin
        if (!OnePlayerLeft())
        {
            yield return null;
        }

        int cooldown = n;
        while (cooldown > 0)
        {
            Text.text = "returning to lobby in: \n" + Mathf.Round(cooldown);
            cooldown -= 1;
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(.25f);
        Text.text = "";
        
    }



    // Count the number of players connected to the server, avoiding nulls
    int GetConnectionCount()
    {
        numConnected = 0;
        foreach (NetworkConnection p in NetworkServer.connections)
        {
            if (p != null)
            {
                //print(p.connectionId);
                numConnected++;
            }
        }
        return numConnected;
    }


    // Count the number of players remaining, return true if 1
    private bool OnePlayerLeft()
    {
        GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");
        return (Players.Length == 1);
    }

 

}
