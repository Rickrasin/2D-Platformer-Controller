using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAdvanced : MonoBehaviour
{

    //public static float move; // 1 Direita / -1 Esquerda // Variável que inverte o collider do ataque

    [Header("Player stats")]
    [Space(5)]

    [SerializeField] private int health;



    [Header("Player Combat")]
    [Space(5)]
    [SerializeField] private float knockbackDuration;
    [SerializeField] private Vector2 knockbackSpeed;


    private bool knockback;
    private float knockbackStartTime;






    [Header("Move")]
    [Space(5)]

    //Andar

    [SerializeField] private float runMaxSpeed;
    [SerializeField] private float runAccel;
    [SerializeField] private float runDeccel;
    [SerializeField] private float velPower;

    private Vector2 moveInput;
    private bool isFacingRight = true;

    [Space(5)]
    [SerializeField]  private float accelInAir;
    [SerializeField]  private float deccelInAir;

    [Space(5)]

    [SerializeField] [Range(.5f, 2f)] private float accelPower;
    [SerializeField] [Range(.5f, 2f)] private float stopPower;
    [SerializeField] [Range(.5f, 2f)] private float turnPower;

    [Space(5)]

    //Outros
    [SerializeField] private bool doKeepRunMomentum;
    private bool canFlip = true;
    private int FacingDirection = 1;





    [Header("Jump")]
    [Space(5)]

    [Space(10)]

    [SerializeField] private float jumpForce;
    [SerializeField] [Range(0, 1)] public float jumpCutMultiplier;
    [SerializeField] [Range(0, 0.5f)] public float jumpBufferTime;
    
    private bool _isJumping;
    private float _lastOnGroundTime;
    private float _lastPressedJumpTime;


    [Header("Gravity")]
    [Space(5)]

    [SerializeField] private float gravityScale;
    [SerializeField] private float fallGravityMultiplier;
    [SerializeField] private float quickFallGravityMult;





    [Header("Physics")]
    [Space(5)]

    [SerializeField] [Range(0, 0.5f)] public float coyoteTime;

    [Space(10)]

    

    [Space(10)]

    [SerializeField] private float knockbackY;
    [SerializeField] private float knockbackX;

    [Header("Drag")]
    [Space(5)]
    [SerializeField] private float dragAmount; //drag in air
    [SerializeField] private float frictionAmount; //drag on ground

    [Header("Colisions")]
    [Space(5)] 
    //Ground
    [SerializeField] private float groundLength = 0.6f;
    [SerializeField] private Vector3 groundColliderOffset;


    [Space(10)]

    [SerializeField] private float wallLength = 0.6f;
    [SerializeField] private Vector3 wallColliderOffset;
    [Space(5)]
    [SerializeField] private LayerMask _layerGround;

    private bool onWall = false;
    private bool onGround = false;


    [Header("RayCast Origin")]
    [SerializeField] private GameObject feetPos;
    [SerializeField] private GameObject bodyPos;



    [Header("Components")]
    [Space(5)]

    [SerializeField] private Rigidbody2D playerRb;
     private Animator animator;


    private PlayerCombatController PC;





    private void Awake()
    {
       
    }

    private void Start()
    {
        //Pegar Componentes
        playerRb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        PC = GetComponent<PlayerCombatController>();



        SetGravityScale(gravityScale);
        isFacingRight = true;
    }


    void Update()
    {

        #region anims 

        animator.SetFloat("Horizontal", Mathf.Abs(playerRb.velocity.x));
        animator.SetFloat("Vertical", (playerRb.velocity.y));
        animator.SetBool("OnGround", onGround);
        #endregion

        //Detecta o chão Por meio de 2 Raycast
        #region  RayCasters
        checkSurroundings();

        #endregion

        #region Utilities
        Timers();
        //Flip
        checkMovementDirection();

        CheckKnockback();



        #endregion

        #region Inputs
        if (!knockback)
        {
            //Inputs
            CheckInput();
            JumpCheck();
        }

        #endregion


        #region Physics
        if (!knockback)
        {
            GravityPhysics();
        }
        #endregion


        


        // Cooldown de Recuperação 





    }


    void FixedUpdate()
    {
        //Physics
      
        //Correr
        if(!knockback) { 
        Run(1);
        Drag(frictionAmount);

        }


    }



    #region Utilities
    private void CheckInput()
    {


        //Move
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // Jump
        if (Input.GetButtonDown("Jump"))
        {
            _lastPressedJumpTime = jumpBufferTime;
        }

        if (Input.GetButtonUp("Jump"))
        {
            if (CanJumpCut())
                JumpCut();
        }

        
    }

    private void Timers()
    {
        //ground Check

        //Timers
        _lastOnGroundTime -= Time.deltaTime;
        _lastPressedJumpTime -= Time.deltaTime;



        if (onGround)
        {
            _lastOnGroundTime = coyoteTime;
        }
    }

    public void DisableFlip()
    {
        canFlip = false;
    }

    public void EnableFlip()
    {
        canFlip = true;
    }

    void Flip()
    {
        
        if(canFlip && !knockback)
        {
            FacingDirection *= -1;
            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }

    }

    private void checkMovementDirection()
    {
        if(isFacingRight && moveInput.x < 0)
        {
            Flip();
        } else if(!isFacingRight && moveInput.x > 0)
        {
            Flip();
        }
    }

    public int GetFacingDirection()
    {
        return FacingDirection;
    }

    private void CheckDirectionToFace(bool isMovingRight)
    {
        if (isMovingRight != isFacingRight)
            Flip();
    }

    private void checkSurroundings()
    {
        onGround = Physics2D.Raycast(feetPos.transform.position + groundColliderOffset, Vector2.down, groundLength, _layerGround) || Physics2D.Raycast(feetPos.transform.position - groundColliderOffset, Vector2.down, groundLength, _layerGround);
        onWall = Physics2D.Raycast(bodyPos.transform.position + wallColliderOffset, Vector2.left, wallLength, _layerGround) || Physics2D.Raycast(bodyPos.transform.position - wallColliderOffset, Vector2.right, wallLength, _layerGround);
    }
    #endregion

    #region Move

    private void Run(float lerpAmount)
    {
        float targetSpeed = moveInput.x * runMaxSpeed; //calcule a direção em que queremos nos mover e nossa velocidade desejada
        float speedDif = targetSpeed - playerRb.velocity.x; //calcular a diferença entre a velocidade atual e a velocidade desejada

        #region Acelleration Rate
        float accelRate;

        //obtém um valor de aceleração baseado em se estamos acelerando (inclui curvas) ou tentando desacelerar (parar). Além de aplicar um multiplicador se estivermos no ar

        if (_lastOnGroundTime > 0 && !knockback)
        {
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAccel : runDeccel;

        } 
        else
        {

            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAccel * accelInAir : runDeccel * deccelInAir;
        }

        //se queremos correr, mas já estamos indo mais rápido que a velocidade máxima de corrida
        if (((playerRb.velocity.x > targetSpeed && targetSpeed > 0.01f) || (playerRb.velocity.x < targetSpeed && targetSpeed < -0.01f)) && doKeepRunMomentum)
        {
            accelRate = 0; //evitar que qualquer desaceleração aconteça, ou em outras palavras, conservar o momento atual
        }

        #endregion

        #region Velocity Power
        float velPower;
        if (Mathf.Abs(targetSpeed) < 0.01f)
        {
            velPower = stopPower;
        }
        else if (Mathf.Abs(playerRb.velocity.x) > 0 && (Mathf.Sign(targetSpeed) != Mathf.Sign(playerRb.velocity.x))) {

            velPower = turnPower;

        } else
        {

            velPower = accelPower;

        }
        #endregion

        //aplica a aceleração à diferença de velocidade, então é aumentada para uma potência definida para que a aceleração aumente com velocidades mais altas, finalmente multiplica por sinal para preservar a direção
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
        movement = Mathf.Lerp(playerRb.velocity.x, movement, lerpAmount); //lerp para que possamos evitar que o Run reduza a velocidade do jogador imediatamente, em algumas situações, por exemplo, salto na parede, dash

        playerRb.AddForce(movement * Vector2.right); //aplica força força ao corpo rígido, multiplicando por Vector2.right para que afete apenas o eixo X

        //Flip
        if (moveInput.x != 0) { 
            CheckDirectionToFace(moveInput.x > 0);
        }

    }


    #endregion

    #region Jump

    private void Jump()
    {
        //garante que não possamos chamar um salto várias vezes de um toque
        _lastPressedJumpTime = 0;
        _lastOnGroundTime = 0;

        //Perform Jump
        float force = jumpForce;
        if (playerRb.velocity.y < 0)
        {
            force -= playerRb.velocity.y;
        }



        playerRb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }

    private void JumpCut()
    {
        //aplica força para baixo quando o botão de salto é liberado. Permitindo que o jogador controle a altura do salto
        playerRb.AddForce(Vector2.down * playerRb.velocity.y * (1 - jumpCutMultiplier), ForceMode2D.Impulse);
    }

    private void JumpCheck()
    {
        //Jump checks

            //Jump
        if (_isJumping && playerRb.velocity.y < 0)
        {
            _isJumping = false;
        }


            //Jump Check
        if (CanJump() && _lastPressedJumpTime > 0)
        {
           


                _isJumping = true;
                Jump();
           
        }

    }

    private bool CanJump()
    {
        return _lastOnGroundTime > 0 && !_isJumping;
    }

    private bool CanJumpCut()
    {
        return _isJumping && playerRb.velocity.y > 0;
    }


    #endregion


    #region gravity

    private void SetGravityScale(float scale)
    {
        playerRb.gravityScale = scale;
    }

    #endregion

    #region Physics


    private void Drag(float amount)
    {
        Vector2 force = amount * playerRb.velocity.normalized;

        force.x = Mathf.Min(Mathf.Abs(playerRb.velocity.x), Mathf.Abs(force.x)); // garante que apenas atrasamos o jogador, se o jogador estiver indo muito devagar, apenas aplicamos uma força para detê-lo
        force.y = Mathf.Min(Mathf.Abs(playerRb.velocity.y), Mathf.Abs(force.y));
        force.x *= Mathf.Sign(playerRb.velocity.x); //encontra a direção para aplicar a força
        force.y *= Mathf.Sign(playerRb.velocity.y);

        playerRb.AddForce(-force, ForceMode2D.Impulse); //applies force against movement direction
    }


    
    

    private void GravityPhysics()
    {
        if(playerRb.velocity.y >= 0) {

            SetGravityScale(gravityScale);
        }
        else if (moveInput.y < 0) {
            SetGravityScale(gravityScale * quickFallGravityMult);
        }
        else { 
            SetGravityScale(gravityScale * fallGravityMultiplier);
        }
    }

    #endregion


    #region Combat 

    //Cancelamento de dano no meio do Dash pro Knockback

    public void Knockback(int direction)
    {
        knockback = true;

        animator.SetBool("Hurt", true);
        knockbackStartTime = Time.time;

        playerRb.velocity = new Vector2(knockbackSpeed.x * direction, knockbackSpeed.y);

    }

   

    private void CheckKnockback()
    {
        if(Time.time >= knockbackStartTime + knockbackDuration && knockback)
        {
            animator.SetBool("Hurt", false);

            knockback = false;
            
            playerRb.velocity = new Vector2(0.0f, playerRb.velocity.y);
        }
    }
    //Checar se está no meio do knockback e retornar verdadeiro caso esteja.
    public bool checkKnockbackTime()
    {
        if (knockback) { return true; } else { return false; }
    }


    #endregion

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;

        //Gizmos.DrawWireSphere(attackHit.position, attackRange);
       

        Gizmos.color = Color.cyan;
        //Ground

        Gizmos.DrawLine(feetPos.transform.position + groundColliderOffset, feetPos.transform.position + groundColliderOffset  + Vector3.down * groundLength);
        Gizmos.DrawLine(feetPos.transform.position - groundColliderOffset, feetPos.transform.position - groundColliderOffset + Vector3.down * groundLength);

        Gizmos.color = Color.blue;

        //Wall
       //Gizmos.DrawLine(bodyPos.transform.position + wallColliderOffset, bodyPos.transform.position + wallColliderOffset + Vector3.left * wallLength);
        //Gizmos.DrawLine(bodyPos.transform.position - wallColliderOffset, bodyPos.transform.position - wallColliderOffset + Vector3.right * wallLength);


    }
}
