using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerAbilityState
{
    private Weapon weapon;


    private int xInput;

    private float velocityToSet;
    private bool setVelocity;
    private bool shouldCheckFlip;


    public PlayerAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoCheck()
    {
        base.DoCheck();
    }
    public override void Enter()
    {
        base.Enter();


        core.Movement.SetVelocityZero();
        setVelocity = false;

        weapon.EnterWeapon();
    }

    public override void Exit()
    {
        base.Exit();

        weapon.ExitWeapon();

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    
        xInput = player.InputHandler.NormInputX;

        if (shouldCheckFlip) { 
        core.Movement.CheckIfShouldFlip(xInput);
        }
        if (setVelocity)
        {
            core.Movement.SetVelocityX(velocityToSet * player.Core.Movement.FacingDirection);
        }
    }

    public void SetWeapon(Weapon weapon)
    {
        this.weapon = weapon;
        weapon.InitializeWeapon(this);
    }

    public void SetPlayerVelocity(float velocity)
    {
        core.Movement.SetVelocityX(velocity * player.Core.Movement.FacingDirection);

        

        velocityToSet = velocity;
        setVelocity = true;
    }

    public void SetFlipCheck(bool value)
    {
        shouldCheckFlip = value;
    }

    public void ResetAttackCount() //Reseta ataque
    {


        if (Time.time >= startTime + 0.5f)
        {

            weapon.resetAttackCount();
            weapon.SetCanReset(false);

        }
    }

    



    #region Animation Triggers

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        isAbilityDone = true;
    }



    #endregion
}
