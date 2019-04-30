using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerAtt : NetworkBehaviour
{
    public Transform character; //main character
    public Transform bullet; // the bullet prefab
    GameObject spawnPt; // holds the spawn point object
    private Rigidbody RB;

    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (Input.GetMouseButtonDown(1))
        { // only do anything when the button is pressed:
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.DrawLine(character.position, hit.point);

                Transform projectile = Instantiate(bullet, character.transform.position+new Vector3(1,0,0), Quaternion.identity) as Transform;
                // turn the projectile to hit.point
                projectile.LookAt(hit.point);
                // accelerate it
                RB = projectile.GetComponent<Rigidbody>();
                RB.velocity = projectile.forward * 100;
            }
        }
    }
}
