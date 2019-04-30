using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DestroyOnHit : NetworkBehaviour
{
    void Start()
    {
        var time = .75f;
        Destroy(gameObject, time);
    }
}
