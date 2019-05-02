using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
//comment for commit
public class prefabSpawn : NetworkBehaviour
{
    

    // Prefabs for projectiles
    [SerializeField]
    private GameObject arrowPrefab;

 
    // Start is called before the first frame update
    [Command]
    public void CmdArrow(GameObject firePoint, int projectileSpeed )
    {
        GameObject spawnedArrow = Instantiate(arrowPrefab, firePoint.transform.position, firePoint.transform.rotation);
        spawnedArrow.GetComponent<Rigidbody>().AddForce(spawnedArrow.transform.forward * projectileSpeed);
        NetworkServer.Spawn(spawnedArrow);
        Destroy(this.gameObject);
    }
}
