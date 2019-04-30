using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnHit : MonoBehaviour
{
    void Start()
    {
        var time = .75f;
        Destroy(gameObject, time);
    }
}
