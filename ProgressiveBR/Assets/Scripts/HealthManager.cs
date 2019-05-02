using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HealthManager : NetworkBehaviour
{

    // Starting health can be set to whatever.
    [SerializeField]
    private float playerHealth = 100;
    private float startingHP;
    public bool isAlive = true;
    public Image hpBar;

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

    public void TakeDamage(float amt)
    {

       
        playerHealth -= amt;
        hpBar.fillAmount = playerHealth / startingHP;
        CmdHpBar(playerHealth/startingHP);
        

        if(playerHealth <= 0)
        {
            // Perform other logics like giving points to whoever killed this person
            // Network operations needed to remove prefabs , etc
            Destroy(this.gameObject);
        }
    }

}
