using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{

    protected D_IdleState stateData;

    protected bool flipAfterIdle;
    protected bool isIdleTImeOver;
    protected bool IsPlayerInMinAgroRange;

    protected float idleTime;


    public IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        IsPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();

    }

    public override void Enter()
    {
        base.Enter();
        core.Movement.SetVelocityX(0f);
        isIdleTImeOver = false;
        SetRandomIdleTime();

    }

    public override void Exit()
    {
        base.Exit();


        if(flipAfterIdle)
        {
            core.Movement.Flip();
        }

        
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();



        if (Time.time >= startTime + idleTime)
        {
            isIdleTImeOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public void SetFlipAfter(bool flip)
    {
        flipAfterIdle = flip;

    }

    public void SetRandomIdleTime()
    {
        idleTime = Random.Range(stateData.minIdleTime, stateData.maxIdleTime);
    }
}
