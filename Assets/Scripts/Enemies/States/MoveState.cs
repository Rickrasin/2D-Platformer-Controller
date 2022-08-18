using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    protected D_MoveState stateData;

    protected bool isDetectingWall;
    protected bool isDetectingLedge;

    protected bool IsPlayerMinAgroRange;

    protected bool isPlayerCheckVisibleY;
    protected bool isViewPlayer;

    public MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData) : base(entity, stateMachine, animBoolName)
    {

        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isDetectingLedge = core.CollisionSenses.LedgeVertical;
        isDetectingWall = core.CollisionSenses.WallFront;
        IsPlayerMinAgroRange = entity.CheckPlayerInMinAgroRange();
        isPlayerCheckVisibleY = entity.canPlayerViewY();
    }

    public override void Enter()
    {
        base.Enter();

        core.Movement.SetVelocityX(stateData.movementSpeed * core.Movement.FacingDirection);



    }

    public override void Exit()
    {
        base.Exit();

        
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        core.Movement.SetVelocityX(stateData.movementSpeed * core.Movement.FacingDirection);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        


    }
}
