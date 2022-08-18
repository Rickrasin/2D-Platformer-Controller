using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSenses : CoreComponent
{

    #region checkTransforms

    public Transform GroundCheck 
    { 
        get
        {
            if (groundCheck) { 
                return groundCheck;
            }else { 
            Debug.LogError("No Ground Check on " + Core.transform.parent.name);
            return null;
            }
        }

        private set => groundCheck = value; }

    public Transform WallCheck
    {
        get
        {
            if (wallCheck)
            {
                return wallCheck;
            }else { 
            Debug.LogError("No Wall Check on " + Core.transform.parent.name);
            return null;
            }
        }

        private set => wallCheck = value;
    }

    public Transform LedgeCheckHorizontal
    {
        get
        {
            if (ledgeCheckHorizontal)
            {
                return ledgeCheckHorizontal;
            }
            else
            {
                Debug.LogError("No Ledge Check Horizontal on " + Core.transform.parent.name);
                return null;
            }
        }

        private set => ledgeCheckHorizontal = value;
    }

    public Transform LedgeCheckVertical
    {
        get
        {
            if (ledgeCheckVertical)
            {
                return ledgeCheckVertical;
            }
            else
            {
                Debug.LogError("No Ledge Check Vertical on " + Core.transform.parent.name);
                return null;
            }
        }

        private set => ledgeCheckVertical = value;
    }
    public Transform CeilingCheck
    {
        get
        {
            if (ceilingCheck)
            {
                return ceilingCheck;
            }
            else
            {
                Debug.LogError("No Ceiling Check on " + Core.transform.parent.name);
                return null;
            }
        }

        private set => ceilingCheck = value;
    }
    public float GroundCheckRadius { get => groundCheckRadius; set => groundCheckRadius = value; }
    public float WallCheckDistance { get => wallCheckDistance; set => wallCheckDistance = value; }
    public float LedgeCheckDistance { get => ledgeCheckDistance; set => ledgeCheckDistance = value; }

    public LayerMask WhatIsGround { get => whatIsGround; set => whatIsGround = value; }

    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform ledgeCheckHorizontal;
    [SerializeField] private Transform ledgeCheckVertical;
    [SerializeField] private Transform ceilingCheck;

    [SerializeField] private float groundCheckRadius;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private float ledgeCheckDistance;



    [SerializeField] private LayerMask whatIsGround; 
    #endregion

    public bool Ceiling
    {
        get => Physics2D.OverlapCircle(CeilingCheck.position, groundCheckRadius, whatIsGround);
    }

    public bool Ground
    {
        get => Physics2D.OverlapCircle(GroundCheck.position, groundCheckRadius, whatIsGround);
    }

    public bool WallFront
    {
        get => Physics2D.Raycast(WallCheck.position, Vector2.right * Core.Movement.FacingDirection, wallCheckDistance, whatIsGround);
    }


    public bool WallBack
    {
        get => Physics2D.Raycast(WallCheck.position, Vector2.right * -Core.Movement.FacingDirection, wallCheckDistance, whatIsGround);
    }

    public bool LedgeVertical
    {
        get => Physics2D.Raycast(LedgeCheckVertical.position, Vector2.down, ledgeCheckDistance, whatIsGround);
    }

    public bool LedgeHorizontal
    {
        get => Physics2D.Raycast(LedgeCheckHorizontal.position, Vector2.right * Core.Movement.FacingDirection, ledgeCheckDistance, whatIsGround);
    }
   
}
