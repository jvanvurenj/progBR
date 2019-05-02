using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;



public class WeaponManager : NetworkBehaviour
{
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

    private Camera playerCamera;

    private float timer;
    private bool isAlive = true;


    private void Start()
    {
        playerCamera = GetComponent<PlayerMovement>().MyCamera;
    }

    void Update()
    {

        if (!isLocalPlayer) { return; }

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
        // Enable animation for player shooting.
        //_animator.SetTrigger("Fire");

        GameObject spawnedArrow = Instantiate(arrowPrefab, firePoint.transform.position, firePoint.transform.rotation);
        spawnedArrow.GetComponent<Rigidbody>().AddForce(spawnedArrow.transform.forward * 150);
        NetworkServer.Spawn(spawnedArrow);

    }
}
