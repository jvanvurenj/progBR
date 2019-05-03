using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HealthManager : NetworkBehaviour
{

    private float playerExperience = 0;

    // To be used later.
    private int playerLevel = 0;

    [SerializeField]
    private float xpPerKill = .25f;

    [SerializeField]
    private float playerHealth = 100;
    private float startingHP;
    public bool isAlive = true;
    public Image hpBar;
    public Image xpBar;

    private void Start()
    {
        startingHP = playerHealth;
    }

    [Command]
    public void CmdHpBar(float amt)
    {
        RpcHpBar(amt);
    }

    [ClientRpc]
    public void RpcHpBar(float amt)
    {
        
        hpBar.fillAmount = amt;
       

    }

    [Command]
    public void CmdXpBar(float amt)
    {
        RpcXpBar(amt);
    }

    [ClientRpc]
    public void RpcXpBar(float amt)
    {
        
        xpBar.fillAmount = amt;

    }


    public void GainExperience(float amt)
    {
        playerExperience += amt;
        if(playerExperience >= 1f)
        {
            playerExperience = 0;
            playerLevel += 1;
            // Give level up stuff
        }
        xpBar.fillAmount = playerExperience;
        CmdXpBar(playerExperience);
    }


    public void TakeDamage(float amt, GameObject sender)
    {

       
        playerHealth -= amt;
        hpBar.fillAmount = playerHealth / startingHP;
        CmdHpBar(playerHealth/startingHP);
        

        if(playerHealth <= 0)
        {
            // Perform other logics like giving points to whoever killed this person
            // Network operations needed to remove prefabs , etc
            // 1/4 a leve
            sender.GetComponent<HealthManager>().GainExperience(xpPerKill);

            Destroy(this.gameObject);
        }
    }

}
