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

    void Start()
    {
        // Just incase.
        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<HealthManager>().TakeDamage(damage, projectileOwner);
        }
        // We also can Instantiate a prefab here on the collider hitpoint such as an explosion.
        // Delete after performing all needed steps.
        Destroy(this.gameObject);
    }
}
