using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DestroyOnHit : NetworkBehaviour
{
    void Start()
    {
        // Just incase.
        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Here we would check if we collied with a player.
        // If (other.tag == "Player"), we apply logic such as damage their hitpoints.
        // We also can Instantiate a prefab here on the collider hitpoint such as an explosion.

        // Delete after performing all needed steps.
        Destroy(this.gameObject);
    }
}
