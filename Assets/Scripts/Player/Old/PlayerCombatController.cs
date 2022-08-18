using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    [SerializeField]
    private bool combatEnabled;
    [Space(10)]
    [SerializeField]
    private float inputTimer, attack1Radius, attack1Damage;
    [SerializeField]
    private float stunDamageAmount = 1f;
    [SerializeField]
    private Transform attack1HitBoxPos;
    [SerializeField]
    private LayerMask whatIsdamageable;
    

    private bool gotInput, isAttacking, isFirstAttack;


    private float lastInputTime = Mathf.NegativeInfinity;

    private AttackDetails attackDetails;

    private Animator anim;

    private PlayerAdvanced PA;
    private PlayerStats PS;

    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("canAttack", combatEnabled);
        PA = GetComponent<PlayerAdvanced>();
        PS = GetComponent<PlayerStats>();


    }

    private void Update()
    {
        CheckCombatInput();
        CheckAttacks();

        //Bloquear ataque no HURT
        if(PA.checkKnockbackTime())
        {
            finishAttack1();
        } 
    }

    private void CheckCombatInput()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            if (combatEnabled)
            {
                //Attempt Combat
                gotInput = true;
                lastInputTime = Time.time;
            }
        }
    }


    #region Checar os ataques
    private void CheckAttacks()
    {
        if(gotInput )
        {

            //Perform Attack1
            if (!isAttacking)
            {

                gotInput = false;
                isAttacking = true;
                isFirstAttack = !isFirstAttack;
                anim.SetBool("attack1", true);
                anim.SetBool("firstAttack", isFirstAttack);
                anim.SetBool("isAttacking", isAttacking);

            }
        }

        if(Time.time >= lastInputTime + inputTimer)
        {
            //Wait for new Input
            gotInput = false;
        }

        
    }

    #endregion
    private void CheckAttackHitBox()
    {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attack1HitBoxPos.position, attack1Radius, whatIsdamageable);

        attackDetails.damageAmount = attack1Damage;
        attackDetails.position = transform.position;
        attackDetails.stunDamageAmount = stunDamageAmount;


        foreach (Collider2D collider in detectedObjects)
        {

            collider.transform.parent.SendMessage("Damage", attackDetails);
            //Instantiate hit particle
            
        }


    }

    private void Damage(AttackDetails attackDetails) 
    {
        int direction;


        PS.DecreaseHealth(attackDetails.damageAmount);

        //O dano do player tem que usar attackDetails[0] aqui

        if(attackDetails.position.x < transform.position.x)
        {
            direction = 1;
        } else
        {
            direction = -1;
        }

        PA.Knockback(direction);
    }

    #region Finaliza Ataque
    private void finishAttack1()
    {
        isAttacking = false;
        anim.SetBool("isAttacking", isAttacking);
        anim.SetBool("attack1", false);
    }
    #endregion

   
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attack1HitBoxPos.position, attack1Radius);
    }
}

