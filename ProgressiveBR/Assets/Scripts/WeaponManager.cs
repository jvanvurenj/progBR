using System.Collections;
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

    [SerializeField]
    private float skill1fireRate = 4;
    [SerializeField]
    private float skill2fireRate = 4;
    [SerializeField]
    private float skill3fireRate = 4;
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
    private float skill1timer;
    private float skill2timer;
    private float skill3timer;
    private bool isAlive = true;


    private void Start()
    {
        playerCamera = GetComponent<PlayerMovement>().MyCamera;
    }

    void Update()
    {

        if (!isLocalPlayer || !GetComponent<PlayerMovement>().isEnabled) { return; }

        PointToMouse();
        timer += Time.deltaTime;
        skill1timer +=Time.deltaTime;
        skill2timer +=Time.deltaTime;
        skill3timer +=Time.deltaTime;
        if (timer >= fireRate)
        {
            if (Input.GetButton("Fire1"))
            {
                timer = 0f;
                Fire();
            }
        }
        if (Input.GetKeyDown("1")){
            if (gameObject.GetComponent<HealthManager>().ManageSkill()){
                //ASSIGN  ATTACK SKILLS AND STUFF
                skill1timer = 0f;
            }
            else{
                if(skill1timer >= skill1fireRate){
                    //FireAttackSkill();
                    skill1timer = 0f;
                }
            }
            
        }
        if (Input.GetKeyDown("2")){
            if (gameObject.GetComponent<HealthManager>().ManageSkill()){
                //ASSIGN SKILLS AND STUFF
                skill2timer = 0f;
            }
            else{
                if(skill2timer >= skill2fireRate){
                    //FireAttackSkill();
                    skill2timer = 0f;
                }
            }
            
        }
        if (Input.GetKeyDown("3")){
            if (gameObject.GetComponent<HealthManager>().ManageSkill()){
                //ASSIGN SKILLS AND STUFF
                skill3timer = 0f;
            }
            else{
                if(skill3timer >= skill3fireRate){
                    //FireAttackSkill();
                    skill3timer = 0f;
                }
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
        _animator.SetTrigger("Attack");
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
