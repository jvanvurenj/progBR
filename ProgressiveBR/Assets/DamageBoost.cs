using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DamageBoost : NetworkBehaviour
{
    // Start is called before the first frame update

    float damageBoost = 25f;
    public GameObject anim;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<WeaponManager>().damageModifer += damageBoost;
            if (anim != null)
            {
               CmdImpact();
               Destroy(this.gameObject);
            }

        }

    }

    [Command]
    public void CmdImpact()
    {

        GameObject explosion = Instantiate(anim, transform.position, Quaternion.identity);
        NetworkServer.Spawn(explosion);
        Destroy(explosion, 1f);
        return;
    }
}
