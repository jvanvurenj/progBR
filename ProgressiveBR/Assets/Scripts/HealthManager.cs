using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HealthManager : NetworkBehaviour
{
    private float playerExperience = 0;
    // To be used later.
    public TextMesh lvl;
    public TextMesh skilltxt;

    [SyncVar]
    public int playerLevel = 1;
    [SyncVar]
    public int skillLevels = 1;

    [SyncVar]
    public float xpPerKill = 1f;
    [SyncVar]
    private float playerHealth = 100;

    [SyncVar]
    public float playerShield = 0;

    private float startingHP;
    [SerializeField]
    private float dmgIncreaseAmt = .5f;

    public Animator anim;


    private Transform startingPosition;
    public bool isAlive = true;

    public Image hpBar;
    public Image xpBar;

    // The spell prefab
    public GameObject playerShieldSpell;
    public GameObject playerInvincibleSpell;

    private void Start()
    {
        startingHP = playerHealth;
        lvl.text = playerLevel.ToString();
        CmdLvl(playerLevel);
        if (!isLocalPlayer)
        {
            return;
        }

        skilltxt.text = "Level up! Press\n1. Attack Skill\n2. Defense Skill\n 3. Movement Skill";
        CmdSkillUp();
    }

    

    [Command]
    public void CmdHpBar(float amt){RpcHpBar(amt);}

    [ClientRpc]
    public void RpcHpBar(float amt){hpBar.fillAmount = amt;}

    [Command]
    public void CmdXpBar(float amt){RpcXpBar(amt);}

    [ClientRpc]
    public void RpcXpBar(float amt){xpBar.fillAmount = amt;}

    [Command]
    public void CmdLvl(int level) { RpcLvl(level); }

    [ClientRpc]
    public void RpcLvl(int level) { lvl.text = level.ToString(); }


    [Command]
    public void CmdDmgShield(float amt)
    {
        playerShield -= amt;
        RpcDmgShield(amt);
    }

    [ClientRpc]
    public void RpcDmgShield(float amt)
    {
        playerShield -= amt;
        if (playerShield <= 0)
        {
            CmdDeactivateShield();
        }
    }

    [Command]
    public void CmdDeactivateShield() {
        playerShield = 0;
        RpcDeactivateShield(); }

    [ClientRpc]
    public void RpcDeactivateShield() {
        playerShield = 0;
        playerShieldSpell.SetActive(false);
        playerInvincibleSpell.SetActive(false);
    }

    [Command]
    public void CmdActivateShield(float amt)
    {
        playerShield += amt;
        RpcActivateShield(amt); }


    [ClientRpc]
    public void RpcActivateShield(float amt) {
        playerShield += amt;

        if (amt > 50)
        {
            playerInvincibleSpell.SetActive(true);
        }
        else
        {
            playerShieldSpell.SetActive(true);
        }
        
    }


    [Command]
    public void CmdDeactivate(){RpcDeactivate();}

    [ClientRpc]
    public void RpcDeactivate(){this.gameObject.SetActive(false);}


    [Command]
    public void CmdSkillUp(){ RpcSkillUp(); }

    [ClientRpc]
    public void RpcSkillUp(){ 
        skilltxt.text = "Level up! Press\n1. Attack Skill\n2. Defense Skill\n 3. Movement Skill";
    }

    [Command]
    public void CmdSkillDown(){ RpcSkillDown(); }

    [ClientRpc]
    public void RpcSkillDown(){
        //if (skillLevels <= 1){
        skilltxt.text = "";
        //}

    }


    public bool ManageSkill(){
        skilltxt.text = "";
        CmdSkillDown();
        if (skillLevels > 0){
            skillLevels -= 1;
            if (skillLevels>0)
            {
                skilltxt.text = "Level up! Press\n1. Attack Skill\n2. Defense Skill\n 3. Movement Skill";
                CmdSkillUp();
            }
            
            return true;
        }
        return false;
    }

    [Command]
    public void CmdAnimateDeath() { RpcAnimateDeath(); }

    [ClientRpc]
    public void RpcAnimateDeath() { anim.SetTrigger("Death"); }


    public void GainExperience(float amt)
    {
        
        playerExperience += amt;
        if(playerExperience >= 1f)
        {
            playerExperience = 0;
            playerLevel += 1;
            skillLevels += 1;
            lvl.text = playerLevel.ToString();
            CmdLvl(playerLevel);
            skilltxt.text = "Level up! Press\n1. Attack Skill\n2. Defense Skill\n 3. Movement Skill";
            CmdSkillUp();
            // Give level up stuff
        }
        xpBar.fillAmount = playerExperience;
        CmdXpBar(playerExperience);
    }


    public float returnHealth()
    {
        return playerHealth;
    }

    public void GainHealth(float amt)
    {

        playerHealth += amt;
        if(playerHealth > 100) { playerHealth = 100; }
        hpBar.fillAmount = playerHealth / startingHP;
        CmdHpBar(playerHealth / startingHP);

    }

    public void GainShield(float amt, float t)
    {

        //playerShieldSpell.SetActive(true);
        CmdActivateShield(amt); // Display shield
        StartCoroutine(DeactivateShield(t));

    }


    IEnumerator DeactivateShield(float t)
    {
        yield return new WaitForSeconds(t);
        playerShield = 0;
        // playerShieldSpell.SetActive(false);
        CmdDeactivateShield(); // Remove shield
    }

    IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(2f);
        this.gameObject.SetActive(false);
        CmdDeactivate();
    }

    public void TakeDamage(float amt, GameObject sender)
    {

        if(sender == this.gameObject)
        {
            return;
        }
       
        // Already dead
        if(playerHealth == 0)
        {
            return;
        }

        if(playerShield > 0)
        {
            float absorbed = playerShield - amt;
            if (absorbed <= 0) // Shield broke
            {
                CmdDeactivateShield();
                amt = amt - playerShield;
                playerShield = 0;
            }
            else // Fully absorbed
            {
                CmdDmgShield(amt);
                //playerShield -= amt;
                amt = 0;
            }
        }

        playerHealth -= amt;
        hpBar.fillAmount = playerHealth / startingHP;
        CmdHpBar(playerHealth/startingHP);
        

        if(playerHealth <= 0)
        {
            GetComponent<PlayerMovement>().isEnabled = false;
            anim.SetTrigger("Death");
            CmdAnimateDeath();
            sender.GetComponent<HealthManager>().GainExperience(xpPerKill);
            StartCoroutine(Deactivate());
            gameObject.GetComponent<PlayerMovement>().Spectate(sender);

        }
    }

}
