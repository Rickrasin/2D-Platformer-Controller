using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{

    #region Other Variables
    public bool debugStateName;
    #endregion


    protected Movement Movement { get => movement ?? Core.GetCoreComponent(ref movement); }

    private Movement movement;
    

    public FiniteStateMachine stateMachine;

    //Data
    public D_Entity entityData;


    public bool isright { get; private set; } //TODO:


    public Animator anim { get; private set; }


    public GameObject playerGO { get; private set; }

    public GameObject enemyGO { get; private set; }

    public AnimationToStateMachine atsm { get; private set; }

    public int lastDamageDirection { get; private set; }

    public Core Core { get; private set; }


    [SerializeField]
    private Transform wallCheck;

    [SerializeField]
    private Transform ledgeCheck;

    [SerializeField]
    private Transform playerCheck;
    [SerializeField]
    private Transform groundCheck;

    private float currentHealth;
    private float currentStunResistance;
    private float lastDamageTime;

    private Vector2 velocityWorkspace;

    protected bool isStunned;
    protected bool isDead;


    

    public virtual void Awake()
    {
        Core = GetComponentInChildren<Core>();

        isright = true;

        currentHealth = entityData.maxHealth;
        currentStunResistance = entityData.stunResistance;



        playerGO = GameObject.FindGameObjectWithTag("Player");

        anim = GetComponent<Animator>();
        atsm = GetComponent<AnimationToStateMachine>();

        stateMachine = new FiniteStateMachine();

    }

    public virtual void Update()
    {
        Core.LogicUpdate();
        stateMachine.currentState.LogicUpdate();

        anim.SetFloat("yVelocity", Movement.RB.velocity.y);


        if (Time.time >= lastDamageTime + entityData.stunRecoveryTime)
        {
            ResetStunResistance();
        }


    }

    public virtual void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();

    }





    public virtual bool CheckPlayerInMinAgroRange()
    {
        return Physics2D.OverlapCircle(playerCheck.position, entityData.minAgroDistance, entityData.whatIsPlayer) && canPlayerViewX() && canPlayerViewY();
    }


    public virtual bool CheckPlayerInMaxAgroRange()
    {
        return Physics2D.OverlapCircle(playerCheck.position, entityData.maxAgroDistance, entityData.whatIsPlayer) && canPlayerViewX() && canPlayerViewY();

    }



    public virtual bool CheckPlayerInCloseRangeAction()
    {
        return Physics2D.Raycast(playerCheck.position, transform.right, entityData.closeRangeActionDistance, entityData.whatIsPlayer);
    }


    public virtual bool canPlayerViewX()
    {

        float EnemyPosX = playerCheck.transform.position.x;


        if (Movement.FacingDirection == 1)
        {

            isright = true;

        }
        else if (Movement.FacingDirection == -1)
        {

            isright = false;

        }


        if (isright) //Dire��o
        {
            if (playerGO.transform.position.x > EnemyPosX) // DIREITA
            {
                return true;
            }
        }
        if (!isright) // Direção
        {
            if (playerGO.transform.position.x < EnemyPosX) // ESQUERDA
            {
                return true;
            }
        }

        return false;


    }

    public virtual bool canPlayerViewY()
    {
        float EnemyPosX = playerCheck.transform.position.x;
        float EnemyPosY = playerCheck.transform.position.y;

        if (playerGO.transform.position.y > EnemyPosY) //ACIMA
        {



            return true;

        }
        else if (playerGO.transform.position.y < EnemyPosY) //ABAIXO
        {
            return false;
        }
        return false;

    }


    public virtual void DamageHop(float velocity)
    {
        velocityWorkspace.Set(Movement.RB.velocity.x, velocity);
        Movement.RB.velocity = velocityWorkspace;
    }

    public virtual void ResetStunResistance()
    {
        isStunned = false;
        currentStunResistance = entityData.stunResistance;
    }




    public virtual void OnDrawGizmos()
    {

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(groundCheck.position, 0.3f);


    }
}



