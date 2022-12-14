using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatDummyController : MonoBehaviour
{
    [SerializeField]
    private float maxHealth, knockbackSpeedX, knockbackSpeedY, knockbackDuration, knockbackDeathSpeedX, knockbackDeathSpeedY, deathTorque; //Torque ? giro
    [SerializeField]
    private bool applyKnockback;
    [SerializeField]
    private GameObject hitParticle;

    private float currentHealth, knockbackStart;

    private int FacingDirection;
    
    private bool playerOnLeft, knockback;

         

    private PlayerAdvanced pc;
    private GameObject aliveGo, brokenTopGO, brokenBotGO;
    private Rigidbody2D rbAlive, rbBrokenTop, rbBrokenBot;
    private Animator aliveAnim;


    private void Start()
    {
        currentHealth = maxHealth;

        pc = GameObject.Find("Player").GetComponent<PlayerAdvanced>();

        aliveGo = transform.Find("Alive").gameObject;

        brokenTopGO = transform.Find("Broken Top").gameObject;
        brokenBotGO = transform.Find("Broken Bottom").gameObject;

        aliveAnim = aliveGo.GetComponent<Animator>();
        rbAlive = aliveGo.GetComponent<Rigidbody2D>();
        rbBrokenTop = brokenTopGO.GetComponent<Rigidbody2D>();
        rbBrokenBot = brokenBotGO.GetComponent<Rigidbody2D>();

        aliveGo.SetActive(true);
        brokenTopGO.SetActive(false);
        brokenBotGO.SetActive(false);
    }

    private void Update()
    {
        CheckKnockback();
    }

   

    private void Knockback()
    {
        knockback = true;
        knockbackStart = Time.time;
        rbAlive.velocity = new Vector2(knockbackSpeedX * FacingDirection, knockbackSpeedY);
    }

    private void CheckKnockback()
    {
        if(Time.time >= knockbackStart + knockbackDuration && knockback)
        {
            knockback = false;
            rbAlive.velocity = new Vector2(0.0f, rbAlive.velocity.y); 
        }
    }


    private void Die()
    {
        aliveGo.SetActive(false);
        brokenTopGO.SetActive(true);
        brokenBotGO.SetActive(true);

        brokenTopGO.transform.position = aliveGo.transform.position;
        brokenBotGO.transform.position = aliveGo.transform.position;

        if(applyKnockback) { 
        rbBrokenBot.velocity = new Vector2(knockbackSpeedX * FacingDirection, knockbackSpeedY);
        }

        rbBrokenTop.velocity = new Vector2(knockbackDeathSpeedY * FacingDirection, knockbackDeathSpeedY);

        rbBrokenTop.AddTorque(deathTorque * -FacingDirection, ForceMode2D.Impulse);

    }

}
