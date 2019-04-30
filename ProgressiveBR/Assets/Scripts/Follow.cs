using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Follow : NetworkBehaviour
{
    public Transform player;
    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        transform.position = player.transform.position + new Vector3(1,0,0);
    }
}
