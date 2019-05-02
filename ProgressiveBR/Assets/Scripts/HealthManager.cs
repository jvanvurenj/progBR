using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HealthManager : NetworkBehaviour
{

    // Starting health can be set to whatever.
    [SerializeField]
    private float playerHealth = 100;
    public bool isAlive = true;

    public void TakeDamage(float amt)
    {
        
        playerHealth -= amt;
        if(playerHealth <= 0)
        {
            // Perform other logics like giving points to whoever killed this person
            // Network operations needed to remove prefabs , etc
            Destroy(this.gameObject);
        }
    }

}
