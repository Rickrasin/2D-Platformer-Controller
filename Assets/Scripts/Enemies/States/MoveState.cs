using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{

    private Movement Movement { get =>movement ?? core.GetCoreComponent(ref movement); }
    private CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }

    private Movement movement;
    private CollisionSenses collisionSenses;


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
        
        isDetectingLedge = CollisionSenses.LedgeVertical;
        isDetectingWall = CollisionSenses.WallFront;
        IsPlayerMinAgroRange = entity.CheckPlayerInMinAgroRange();
        isPlayerCheckVisibleY = entity.canPlayerViewY();
    }

    public override void Enter()
    {
        base.Enter();

        Movement.SetVelocityX(stateData.movementSpeed * Movement.FacingDirection);



    }

    public override void Exit()
    {
        base.Exit();


    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        Movement.SetVelocityX(stateData.movementSpeed * Movement.FacingDirection);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();




    }
}
