using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HealthPack : NetworkBehaviour
{
    public GameObject healthAnimation;
    public float healthGained = 25;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if(other.GetComponent<HealthManager>().returnHealth() < 100f) // Player is damaged.
            {
                other.GetComponent<HealthManager>().GainHealth(healthGained);
                if (healthAnimation != null)
                {
                    CmdImpact();
                    Destroy(this.gameObject);
                }
            }
            
        }

    }

    [Command]
    public void CmdImpact()
    {

        GameObject explosion = Instantiate(healthAnimation, transform.position, Quaternion.identity);
        NetworkServer.Spawn(explosion);
        Destroy(explosion, 1f);
        return;
    }
}
