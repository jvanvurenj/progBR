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
    [SerializeField]
    private float inputRate = 1;
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
    private GameObject lightningPrefab;
    [SerializeField]
    private GameObject soulPrefab;
    [SerializeField]
    private GameObject frostPrefab;

    [SerializeField]
    private GameObject NetPrefab;
    private Camera playerCamera;
    private float timer;
    private float skill1timer;
    private float skill2timer;
    private float skill3timer;
    private float inputtimer;
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
        skill1timer +=Time.deltaTime;
        skill2timer +=Time.deltaTime;
        skill3timer +=Time.deltaTime;
        inputtimer +=Time.deltaTime;
        if (timer >= fireRate)
        {
            if (Input.GetButton("Fire1"))
            {
                timer = 0f;
                Fire();
            }
        }
        if (Input.GetKeyDown("1")){
            if (inputtimer>=inputRate){
                if (gameObject.GetComponent<HealthManager>().ManageSkill()){
                //ASSIGN  ATTACK SKILLS AND STUFF
                    inputtimer = 0f;
                }
            }
            else{
                if(skill1timer >= skill1fireRate){
                    //FireAttackSkill();
                    skill1timer = 0f;
                }
            }
            
        }
        if (Input.GetKeyDown("2")){
            if (inputtimer>=inputRate){
                if (gameObject.GetComponent<HealthManager>().ManageSkill()){
                //ASSIGN  ATTACK SKILLS AND STUFF
                    inputtimer = 0f;
                }
            }
            else{
                if(skill2timer >= skill2fireRate){
                    //FireAttackSkill();
                    skill2timer = 0f;
                }
            }
            
        }
        if (Input.GetKeyDown("3")){
            if (inputtimer>=inputRate){
                if (gameObject.GetComponent<HealthManager>().ManageSkill()){
                //ASSIGN  ATTACK SKILLS AND STUFF
                    inputtimer = 0f;
                }
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
        int r = Random.Range(0, 4);
        switch (r)
        {
            case (0):
                //_animator.SetTrigger("Attack");
                CmdAnimateAttack("Attack");
                CmdFireBall();
                break;
            case (1):
                //_animator.SetTrigger("Attack2");
                CmdAnimateAttack("Attack2");
                CmdLightning();
                break;
            case (2):
                //_animator.SetTrigger("Attack3");
                CmdAnimateAttack("Attack3");
                CmdSoul();
                break;
            case (3):
                //_animator.SetTrigger("Attack4");
                CmdAnimateAttack("Attack3");
                CmdFrost();
                break;
            default:
                //_animator.SetTrigger("Attack");
                CmdAnimateAttack("Attack");
                CmdFireBall();
                break;
        }
        
        
 
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

    [Command]
    public void CmdLightning()
    {

        GameObject spawnedLB = Instantiate(lightningPrefab, firePoint.transform.position, firePoint.transform.rotation);
        spawnedLB.GetComponent<DestroyOnHit>().projectileOwner = this.gameObject;
        spawnedLB.GetComponent<DestroyOnHit>().AddDmg(damageModifer);
        spawnedLB.GetComponent<Rigidbody>().AddForce(spawnedLB.transform.forward * projectileSpeed);
        NetworkServer.Spawn(spawnedLB);
        return;
    }
    [Command]
    public void CmdSoul()
    {

        GameObject spawnedSB = Instantiate(soulPrefab, firePoint.transform.position, firePoint.transform.rotation);
        spawnedSB.GetComponent<DestroyOnHit>().projectileOwner = this.gameObject;
        spawnedSB.GetComponent<DestroyOnHit>().AddDmg(damageModifer);
        spawnedSB.GetComponent<Rigidbody>().AddForce(spawnedSB.transform.forward * projectileSpeed);
        NetworkServer.Spawn(spawnedSB);
        return;
    }
    [Command]
    public void CmdFrost()
    {

        GameObject spawnedFFB = Instantiate(frostPrefab, firePoint.transform.position, firePoint.transform.rotation);
        spawnedFFB.GetComponent<DestroyOnHit>().projectileOwner = this.gameObject;
        spawnedFFB.GetComponent<DestroyOnHit>().AddDmg(damageModifer);
        spawnedFFB.GetComponent<Rigidbody>().AddForce(spawnedFFB.transform.forward * projectileSpeed);
        NetworkServer.Spawn(spawnedFFB);
        return;
    }
}
