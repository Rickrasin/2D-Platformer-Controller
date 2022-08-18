using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{

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

        anim.SetFloat("yVelocity", Core.Movement.RB.velocity.y);


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
        return Physics2D.Raycast(playerCheck.position, transform.right,entityData.closeRangeActionDistance, entityData.whatIsPlayer);
    }
   

    public virtual bool canPlayerViewX()
    {
        
        float EnemyPosX = playerCheck.transform.position.x;


        if (Core.Movement.FacingDirection == 1)
        {
            
            isright = true;

        }
        else if (Core.Movement.FacingDirection == -1)
        {
            
            isright = false;

        }


        if (isright) //Direção
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

        } else if(playerGO.transform.position.y < EnemyPosY) //ABAIXO
        {
            return false;
        }
        return false;
        
    }

    
    public virtual void DamageHop(float velocity)
    {
        velocityWorkspace.Set(Core.Movement.RB.velocity.x, velocity);
        Core.Movement.RB.velocity = velocityWorkspace;
    }

    public virtual void ResetStunResistance()
    {
        isStunned = false;
        currentStunResistance = entityData.stunResistance;
    }

    public virtual void Damage(AttackDetails attackDetails)
    {
        lastDamageTime = Time.time;
        currentHealth -= attackDetails.damageAmount;
        currentStunResistance -= attackDetails.stunDamageAmount;

        DamageHop(entityData.damageHopSpeed);

        Instantiate(entityData.hitParticle, transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));

        if(attackDetails.position.x > transform.position.x)
        {
            lastDamageDirection = -1;
        } else
        {
            lastDamageDirection = 1;
        }

        if(currentStunResistance <= 0)
        {
            isStunned = true;
        }

        if(currentHealth <= 0)
        {
            isDead = true;
        }
    }


    public virtual void OnDrawGizmos()
    {
        if (Core != null)
        {
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * Core.Movement.FacingDirection * Core.CollisionSenses.WallCheckDistance));

            Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + (Vector3)(Vector2.down * Core.CollisionSenses.LedgeCheckDistance));

            //Gizmos.color = Color.black;
            //Gizmos.DrawWireSphere(playerCheck.position, entityData.playerCheckRadius);

            Gizmos.color = Color.white;

            //Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * facingDireciton * entityData.closeRangeActionDistance), 0.2f);
            //Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * facingDireciton * entityData.minAgroDistance), 0.2f);
            //Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * facingDireciton * entityData.maxAgroDistance), 0.2f);

            Gizmos.color = Color.black;

            //Gizmos.DrawWireSphere(playerCheck.position, entityData.closeRangeActionDistance);

            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(playerCheck.position, entityData.minAgroDistance);
            Gizmos.color = Color.green;

            Gizmos.DrawWireSphere(playerCheck.position, entityData.maxAgroDistance);
        }
       




    }
}



