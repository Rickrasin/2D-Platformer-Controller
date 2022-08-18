using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    
    [SerializeField] protected SO_WeaponData weaponData;

    protected float startTime;

    protected Animator baseAnimator;
    protected Animator weaponAnimator;

    protected PlayerAttackState state;

    protected int attackCounter;
    protected bool canReset;

    protected virtual void Awake()
    {
        baseAnimator = transform.Find("Base").GetComponent<Animator>();
        weaponAnimator = transform.Find("Weapon").GetComponent<Animator>();

        startTime = Time.time;
        gameObject.SetActive(false);
    }

    public virtual void EnterWeapon()
    {
        gameObject.SetActive(true);
        


        if (attackCounter >= weaponData.amountOfAttacks)
        {
            attackCounter = 0;
            canReset = false;

        }

        #region Check Reset
        //Check can Reset Attack

        if (attackCounter > 0)
        {
            canReset = true;
        } else if(attackCounter == 1)
        {
            canReset = true;
        }else if(attackCounter < 2)
        {
            canReset = true;
        } else if(attackCounter == 0)
        {
            canReset = false;
        } else if(attackCounter == 2)
        {
            canReset = false;
        }
        #endregion


        baseAnimator.SetBool("attack", true);
        weaponAnimator.SetBool("attack", true);

        baseAnimator.SetInteger("attackCounter", attackCounter);
        weaponAnimator.SetInteger("attackCounter", attackCounter);

    }

    

    public virtual void ExitWeapon()
    {

        baseAnimator.SetBool("attack", false);
        weaponAnimator.SetBool("attack", false);

        attackCounter++;

        gameObject.SetActive(false);

    }

    #region Animation Triggers

    public virtual void AnimationFinishTrigger()
    {
        state.AnimationFinishTrigger();
        

    }

    public virtual void AnimationStartMovementTrigger()
    {
        state.SetPlayerVelocity(weaponData.MovementSpeed[attackCounter]);
    }

    public virtual void AnimationStopMovementTrigger()
    {
        state.SetPlayerVelocity(0f);
    }

    public virtual void AnimationTurnOffFlipTrigger()
    {
        state.SetFlipCheck(false);
    }

    public virtual void AnimationTurnOnFlipTrigger()
    {
        state.SetFlipCheck(true);
    }

    public virtual void AnimationActionTrigger()
    {

    }

    #endregion

    public void SetCanReset(bool value) => canReset = value;

    public void resetAttackCount() => attackCounter = 0;



    public void InitializeWeapon(PlayerAttackState state)
    {
        this.state = state;
    }
}
