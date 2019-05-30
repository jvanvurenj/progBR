﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;



public class WeaponManager : NetworkBehaviour
{

    public float damageModifer = 0f;
    //private GameObject prefabSpawner;
    public int projectileSpeed = 300;
    // How fast the player c
    [SerializeField]
    private float fireRate = 1;
    // Animator for player controller
    [SerializeField]
    Animator _animator;
    // Firepoint locations for instantiating prefabs
    [SerializeField]
    private GameObject firePoint;
 
    // Prefabs for projectiles
    [SerializeField]
    private GameObject arrowPrefab;
    [SerializeField]
    private GameObject fireballPrefab;

    [SerializeField]
    private GameObject NetPrefab;
    private Camera playerCamera;
    private float timer;
    private bool isAlive = true;

    [Command]
    public void CmdAnimateAttack(string att) { RpcAnimateAttack(att); }

    [ClientRpc]
    public void RpcAnimateAttack(string att) { _animator.SetTrigger(att); }

    private void Start()
    {
        playerCamera = GetComponent<PlayerMovement>().MyCamera;
    }

    void Update()
    {

        if (!isLocalPlayer || !GetComponent<PlayerMovement>().isEnabled) { return; }

        PointToMouse();
        timer += Time.deltaTime;
        if (timer >= fireRate)
        {
            if (Input.GetButton("Fire1"))
            {
                timer = 0f;
                Fire();
            }
        }

        
    }

   

    private void PointToMouse()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Debug.DrawLine(firePoint.transform.position, hit.point, Color.red);
            Vector3 targetPostition = new Vector3(hit.point.x, this.transform.position.y, hit.point.z);
            this.transform.LookAt(targetPostition);
        }
    }

    private void Fire()
    {
        int r = Random.Range(0, 2);
        switch (r)
        {
            case (0):
                _animator.SetTrigger("Attack");
                CmdAnimateAttack("Attack");
                break;
            case (1):
                _animator.SetTrigger("Attack2");
                CmdAnimateAttack("Attack2");
                break;
            case (2):
                _animator.SetTrigger("Attack3");
                CmdAnimateAttack("Attack3");
                break;
            default:
                _animator.SetTrigger("Attack");
                CmdAnimateAttack("Attack");
                break;
        }
        
        //CmdArrow();
        CmdFireBall();
 
    }
    
    public void SetProjectileSpeed(int speed)
    {
        projectileSpeed = speed;
    }

    [Command]
    public void CmdArrow()
    {
        //print(isLocalPlayer);
        
        GameObject spawnedArrow = Instantiate(arrowPrefab, firePoint.transform.position, firePoint.transform.rotation);
        spawnedArrow.GetComponent<DestroyOnHit>().projectileOwner = this.gameObject;
        spawnedArrow.GetComponent<DestroyOnHit>().AddDmg(damageModifer);
        spawnedArrow.GetComponent<Rigidbody>().AddForce(spawnedArrow.transform.forward * projectileSpeed);
        NetworkServer.Spawn(spawnedArrow);
        return;
    }

    [Command]
    public void CmdFireBall()
    {

        GameObject spawnedFB = Instantiate(fireballPrefab, firePoint.transform.position, firePoint.transform.rotation);
        spawnedFB.GetComponent<DestroyOnHit>().projectileOwner = this.gameObject;
        spawnedFB.GetComponent<DestroyOnHit>().AddDmg(damageModifer);
        spawnedFB.GetComponent<Rigidbody>().AddForce(spawnedFB.transform.forward * projectileSpeed);
        NetworkServer.Spawn(spawnedFB);
        return;
    }

}
