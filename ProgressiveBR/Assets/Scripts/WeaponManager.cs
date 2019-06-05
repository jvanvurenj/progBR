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
    private float shieldAmount = 50;

    
    [SerializeField]
    private float inputRate = 1;
    // Animator for player controller
    [SerializeField]
    Animator _animator;
    // Firepoint locations for instantiating prefabs
    [SerializeField]
    private GameObject firePoint;

    [SerializeField]
    private GameObject attackIcon1;
    [SerializeField]
    private GameObject attackIcon2;
    [SerializeField]
    private GameObject attackIcon3;

    [SerializeField]
    private GameObject defenseIcon1;
    [SerializeField]
    private GameObject defenseIcon2;
    [SerializeField]
    private GameObject defenseIcon3;

    [SerializeField]
    private GameObject movementIcon1;
    [SerializeField]
    private GameObject movementIcon2;
    [SerializeField]
    private GameObject movementIcon3;



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
    private GameObject mysticPrefab;
    [SerializeField]
    private GameObject waterDropPrefab;
    [SerializeField]
    private GameObject waterZonePrefab;
    [SerializeField]
    private GameObject shadowBallPrefab;
    [SerializeField]
    private GameObject acidBallPrefab;

    [SerializeField]
    private GameObject speedBuffEffect;
    [SerializeField]
    private GameObject teleportEffect;

    [SerializeField]
    private GameObject iceWall;


    private Camera playerCamera;
    private float timer;
    private float skill1timer;
    private float skill2timer;
    private float skill3timer;
    private bool isAlive = true;


    //SKILL MAPPING
    // Attack: 1 = lightning (fast, lots of damage, big cooldown) 2 = Double shot (fires two fireballs, same damage, low cooldown) 3. Water drop (Falls from the sky at the location of the mouse. takes maybe 3 seconds to land, but does a ton of damage. big cooldown) 
    // Defense: 1. Shield (Use it, get a shield of 50 health for 2 seconds. Cooldown of maybe 6 seconds?) 2. Wall. Spawns a wall that lasts for 3 seconds. Cooldown of 10 seconds. 3. Invincible. Unable to take damage for 4 seconds. Cooldown of like 15 seconds
    // Movement: 1. Dash. Quick burst in direction of the mouse. Almost instantaneous. 2. Speed Boost. Increased movement speed for 5 seconds. Cooldown of 10 seconds. 3. Teleport. Moves the player randomly to one of the spawn locations on the map. Cooldown of 20 seconds.

    private int attackTag = 0;
    private int defenseTag = 0;
    private int movementTag = 0;

    private int[] skill1fireRate = new int[4]{0, 10, 4, 10}; 
    private int[] skill2fireRate = new int[4]{0, 6, 10, 15};
    private int[] skill3fireRate = new int[4]{0, 6, 10, 15};



    [Command]
    public void CmdAnimateAttack(string att) { RpcAnimateAttack(att); }

    [ClientRpc]
    public void RpcAnimateAttack(string att) { _animator.SetTrigger(att); }

    private void Start()
    {
        skill1timer = 20.0f;
        skill2timer = 20.0f;
        skill3timer = 20.0f;
        timer = 20.0f;
        playerCamera = GetComponent<PlayerMovement>().MyCamera;
    }


    void Update()
    {

        if (!isLocalPlayer || !GetComponent<PlayerMovement>().isEnabled || GetComponent<HealthManager>().returnHealth() <= 0) { return; }

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
            if (gameObject.GetComponent<HealthManager>().ManageSkill())
            {
                attackTag = Random.Range(1,4);
                //SetAttackIcon();
                skill1timer = 20f;
            }
            else{
                if(skill1timer >= skill1fireRate[attackTag])
                {
                    AttackSkill();
                    skill1timer = 0f;
                }
                
            }
            SetAttackIcon();

        }
        if (Input.GetKeyDown("2"))
        {

            if (gameObject.GetComponent<HealthManager>().ManageSkill()){
                defenseTag = Random.Range(1,4);
                //SetDefenseIcon();
                skill2timer = 20f;
            }
            else
            {
                if(skill2timer >= skill2fireRate[defenseTag]){
                    DefenseSkill();
                    skill2timer = 0f;
                }
            }
            SetDefenseIcon();
        }
        if (Input.GetKeyDown("3")){
            if (gameObject.GetComponent<HealthManager>().ManageSkill()){
                movementTag = Random.Range(2,4);
                //SetMovementIcon();
                skill3timer = 20f;
            }
    
            else{
                if(skill3timer >= skill3fireRate[movementTag]){
                    MovementSkill();
                    skill3timer = 0f;
                }
            }
            SetMovementIcon();
        }


        
    }

    private void SetAttackIcon()
    {
        attackIcon1.SetActive(false);
        attackIcon2.SetActive(false);
        attackIcon3.SetActive(false);
        switch (attackTag)
        {
            case (1):
                attackIcon1.SetActive(true);
                break;
            case (2):
                attackIcon2.SetActive(true);
                break;
            case (3):
                attackIcon3.SetActive(true);
                break;
            default:
                break;
        }
    }

    private void SetDefenseIcon()
    {
        defenseIcon1.SetActive(false);
        defenseIcon2.SetActive(false);
        defenseIcon3.SetActive(false);
        switch (defenseTag)
        {
            case (1):
                defenseIcon1.SetActive(true);
                break;
            case (2):
                defenseIcon2.SetActive(true);
                break;
            case (3):
                defenseIcon3.SetActive(true);
                break;
            default:
                break;
        }
    }

    private void SetMovementIcon()
    {
        movementIcon1.SetActive(false);
        movementIcon2.SetActive(false);
        movementIcon3.SetActive(false);
        switch (movementTag)
        {
            case (1):
                movementIcon1.SetActive(true);
                break;
            case (2):
                movementIcon2.SetActive(true);
                break;
            case (3):
                movementIcon3.SetActive(true);
                break;
            default:
                break;
        }
    }

    private void DefenseSkill(){
        switch (defenseTag)
        {
            case (1): // 2  second shield.
                gameObject.GetComponent<HealthManager>().GainShield(shieldAmount, 2);
                break;
            case (2): // Spawn a temporary wall
                Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Vector3 targetPostition = new Vector3(hit.point.x, this.transform.position.y, hit.point.z);
                    CmdAnimateAttack("Attack2");
                    CmdFrostWall(targetPostition);
                }
                break;
            case (3): // "Invincible" for 6 seconds just make the shield unbreakable by making it very large.
                gameObject.GetComponent<HealthManager>().GainShield(9999, 6);
                break;
            default:
                break;
        }
    }

    private void AttackSkill()
    {
        
        switch (attackTag)
        {
            case (1):
                CmdAnimateAttack("Attack");
                CmdLightning(); // 50 Dmg, half the weight so twice as fast.
                break;
            case (2):
                CmdAnimateAttack("Attack2");
                CmdDoubleFireBall();
                break;
            // 75 DMG falls from the sky of mouse position click.
            case (3):
                Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Vector3 targetPostition = new Vector3(hit.point.x, this.transform.position.y + 10, hit.point.z);
                    CmdAnimateAttack("Attack3");
                    CmdWaterDrop(targetPostition);
                }
                break;
            default:
                break;
        }

       
    }
    private void MovementSkill(){
        switch (movementTag)
        {
            case (1):;
                break;

            case (2):
                CmdSpeedBuff();
                gameObject.GetComponent<PlayerMovement>().SpeedBoost();
                break;
            case (3): // Teleport to random spawn position, and play a teleport animation
                Teleport();
                break;
            default:
                break;
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
        int r = Random.Range(0, 6);
        switch (r)
        {
            case (0):
                CmdAnimateAttack("Attack");
                CmdFireBall();
                break;
            case (1):
                CmdAnimateAttack("Attack2");
                CmdMystic();
                break;
            case (2):
                CmdAnimateAttack("Attack3");
                CmdSoul();
                break;
            case (3):
                CmdAnimateAttack("Attack3");
                CmdFrost();
                break;
            case (4):
                CmdAnimateAttack("Attack4");
                CmdShadowBall();
                break;
            case (5):
                CmdAnimateAttack("Attack5");
                CmdAcidBall();
                break;
            default:
                CmdAnimateAttack("Attack");
                CmdFireBall();
                break;
        }
        
        
 
    }
    
    public void SetProjectileSpeed(int speed)
    {
        projectileSpeed = speed;
    }


    public void Teleport()
    {
        GameObject[] SpawnPoints = GameObject.FindGameObjectsWithTag("Spawn");
        int r = Random.Range(0, SpawnPoints.Length);
        CmdTeleAnim();
        gameObject.transform.position = SpawnPoints[r].transform.position;
        CmdTeleport(SpawnPoints[r]);
        return;
    }

    [Command]
    public void CmdTeleport(GameObject obj) {RpcTeleport(obj); }
    [ClientRpc]
    public void RpcTeleport(GameObject obj) { gameObject.transform.position = obj.transform.position;}


    [Command]
    public void CmdTeleAnim()
    {
        GameObject teleportAnimation = Instantiate(teleportEffect, transform.position, Quaternion.identity);
        NetworkServer.Spawn(teleportAnimation);
        return;
    }

    [Command]
    public void CmdArrow()
    {

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
    public void CmdShadowBall()
    {

        GameObject spawnedFB = Instantiate(shadowBallPrefab, firePoint.transform.position, firePoint.transform.rotation);
        spawnedFB.GetComponent<DestroyOnHit>().projectileOwner = this.gameObject;
        spawnedFB.GetComponent<DestroyOnHit>().AddDmg(damageModifer);
        spawnedFB.GetComponent<Rigidbody>().AddForce(spawnedFB.transform.forward * projectileSpeed);
        NetworkServer.Spawn(spawnedFB);
        return;
    }

    [Command]
    public void CmdAcidBall()
    {

        GameObject spawnedFB = Instantiate(acidBallPrefab, firePoint.transform.position, firePoint.transform.rotation);
        spawnedFB.GetComponent<DestroyOnHit>().projectileOwner = this.gameObject;
        spawnedFB.GetComponent<DestroyOnHit>().AddDmg(damageModifer);
        spawnedFB.GetComponent<Rigidbody>().AddForce(spawnedFB.transform.forward * projectileSpeed);
        NetworkServer.Spawn(spawnedFB);
        return;
    }

    [Command]
    public void CmdDoubleFireBall()
    {

        GameObject spawnedFB = Instantiate(fireballPrefab, firePoint.transform.position+new Vector3(0.0f,.51f, 0.0f), firePoint.transform.rotation*Quaternion.Euler(Vector3.up * 10));
        spawnedFB.GetComponent<DestroyOnHit>().projectileOwner = this.gameObject;
        spawnedFB.GetComponent<DestroyOnHit>().AddDmg(damageModifer);
        spawnedFB.GetComponent<Rigidbody>().AddForce(spawnedFB.transform.forward * projectileSpeed);
        NetworkServer.Spawn(spawnedFB);

        GameObject spawnedFB2 = Instantiate(fireballPrefab, firePoint.transform.position+new Vector3(0.0f,-.51f, 0.0f), firePoint.transform.rotation*Quaternion.Euler(Vector3.up * (-10)));
        spawnedFB2.GetComponent<DestroyOnHit>().projectileOwner = this.gameObject;
        spawnedFB2.GetComponent<DestroyOnHit>().AddDmg(damageModifer);
        spawnedFB2.GetComponent<Rigidbody>().AddForce(spawnedFB2.transform.forward * projectileSpeed);
        NetworkServer.Spawn(spawnedFB2);
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

    [Command]
    public void CmdMystic()
    {

        GameObject spawnedFFB = Instantiate(mysticPrefab, firePoint.transform.position, firePoint.transform.rotation);
        spawnedFFB.GetComponent<DestroyOnHit>().projectileOwner = this.gameObject;
        spawnedFFB.GetComponent<DestroyOnHit>().AddDmg(damageModifer);
        spawnedFFB.GetComponent<Rigidbody>().AddForce(spawnedFFB.transform.forward * projectileSpeed);
        NetworkServer.Spawn(spawnedFFB);
        return;
    }

    [Command]
    public void CmdSpeedBuff()
    {
        GameObject spawnedBuff = Instantiate(speedBuffEffect, transform.position, Quaternion.identity);
        NetworkServer.Spawn(spawnedBuff);
        return;
    }


    [Command]
    public void CmdWaterDrop(Vector3 targetPostition)
    {
        // Get position of mouse
        Vector3 ZonePos = new Vector3(targetPostition.x, targetPostition.y-10, targetPostition.z);
        GameObject WaterDrop = Instantiate(waterDropPrefab, targetPostition, Quaternion.identity);
        GameObject WaterZone = Instantiate(waterZonePrefab, ZonePos, Quaternion.identity);
        WaterDrop.GetComponent<DestroyOnHit>().projectileOwner = this.gameObject;
        WaterDrop.GetComponent<DestroyOnHit>().AddDmg(damageModifer);
        Destroy(WaterZone, 1.5f);
        NetworkServer.Spawn(WaterZone);
        NetworkServer.Spawn(WaterDrop);
        
        return;
    }

    [Command]
    public void CmdFrostWall(Vector3 targetPostition)
    {
        GameObject IceWALL = Instantiate(iceWall, targetPostition, transform.rotation);
        Destroy(IceWALL, 3);
        NetworkServer.Spawn(IceWALL);

        return;
    }


    // To supress animation errors.
    public void FootR() { }
    public void FootL() { }


}
