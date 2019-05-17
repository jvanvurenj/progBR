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
    public int playerLevel = 1; // Start off with 1 for now.
    public int skillLevels = 1;
    [SerializeField]
    private float xpPerKill = .99f;
    [SerializeField]
    private float playerHealth = 100;
    private float startingHP;
    [SerializeField]
    private float dmgIncreaseAmt = .5f;


    private Transform startingPosition;
    public bool isAlive = true;
    public Image hpBar;
    public Image xpBar;


    private void Start()
    {
        startingHP = playerHealth;
        lvl.text = playerLevel.ToString();
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
    public void CmdDeactivate(){RpcDeactivate();}

    [ClientRpc]
    public void RpcDeactivate(){this.gameObject.SetActive(false);}


    // Example of skill level up, Increase damage or speed for now.
    private void LevelUp(int s)
    {
        if (!isLocalPlayer) { return; }
        if (skillLevels <= 0) { return; }
        skillLevels -= 1;
        switch (s)
        {
            case 0:
                // Increase damage.
                GetComponent<WeaponManager>().damageModifer += dmgIncreaseAmt;
                break;
            case 1:
                // Increase damage.
                GetComponent<PlayerMovement>().speed += 2;
                break;
            default:
                break;
        }
    }

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

    public void TakeDamage(float amt, GameObject sender)
    {

       
        playerHealth -= amt;
        hpBar.fillAmount = playerHealth / startingHP;
        CmdHpBar(playerHealth/startingHP);
        

        if(playerHealth <= 0)
        {
            sender.GetComponent<HealthManager>().GainExperience(xpPerKill);
            CmdDeactivate();
            this.gameObject.SetActive(false);

        }
    }

}
