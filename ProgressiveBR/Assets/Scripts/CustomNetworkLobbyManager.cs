using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class CustomNetworkLobbyManager : NetworkLobbyManager
{
    // Generate map randomly when all players are ready
    public override void OnLobbyServerPlayersReady()
    {
        string[] scenes = new string[5] { "ProgressiveBR", "SnowBR", "DesertBR", "UrbanBR", "IslandBR" };
        int random = Random.Range(0, 5);
        playScene = scenes[random];
        ServerChangeScene(playScene);
    }

}


