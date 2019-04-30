using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnHit : MonoBehaviour
{
    void Start()
    {
        var time = 1;
        Destroy(gameObject, time);
    }
}
