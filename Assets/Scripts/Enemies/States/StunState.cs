using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : State
{

    protected D_StunState stateData;

    protected bool isStunTImeOver;
    protected bool isGrounded;
    protected bool isMovementStopped;
    protected bool performCloseRangeAction;
    protected bool isPlayerInMinAgroRange;

    public StunState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_StunState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isGrounded = core.CollisionSenses.Ground;
        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Entrei");

        isStunTImeOver = false;
        isMovementStopped = false;
        core.Movement.SetVelocity(stateData.stunKnockbackSpeed, stateData.stunKnockbackAngle, entity.lastDamageDirection);
    }

    public override void Exit()
    {
        base.Exit();
        entity.ResetStunResistance();
        Debug.Log("Sa�");

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(Time.time >= startTime + stateData.stunTime)
        {
            isStunTImeOver = true;
        }

        if(isGrounded && Time.time >= startTime + stateData.stunKnockbackTime && !isMovementStopped)
        {
            isMovementStopped = true;
            core.Movement.SetVelocityX(0f);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}