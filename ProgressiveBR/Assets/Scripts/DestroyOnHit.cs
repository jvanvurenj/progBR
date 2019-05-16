using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DestroyOnHit : NetworkBehaviour
{

    [SerializeField]
    private float damage = 0;

    // Who shot this projectile
    public GameObject projectileOwner;
    public GameObject impactEffect;

    void Start()
    {
        // Just incase.
        Destroy(gameObject, 3f);
    }

    public void AddDmg(float d)
    {
        damage = damage+d;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<HealthManager>().TakeDamage(damage, projectileOwner);
        }
        
        if(impactEffect != null)
        {
            CmdImpact();
        }
        // We also can Instantiate a prefab here on the collider hitpoint such as an explosion.
        // Delete after performing all needed steps.
        Destroy(this.gameObject);
    }

    [Command]
    public void CmdImpact()
    {

        GameObject explosion = Instantiate(impactEffect, transform.position, Quaternion.identity);
        NetworkServer.Spawn(explosion);
        Destroy(explosion, 1f);
        return;
    }
}
