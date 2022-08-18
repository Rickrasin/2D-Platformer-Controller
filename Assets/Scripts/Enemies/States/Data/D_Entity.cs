using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "newEntityData", menuName = "Data/Entity Data/Base Data")]
public class D_Entity : ScriptableObject
{

    public float playerCheckRadius;


    [Header("Combat")]
    public float maxHealth = 30f;

    public float damageHopSpeed = 3F;
    [Space(10)]

    public float stunResistance = 3f;
    public float stunRecoveryTime = 2f;


    [Header("RayCast Colission")]
    public float wallCheckDistance = 0.1f;
    public float ledgeCheckDistance = 0.5f;
    public float groundCheckRadius = 0.3f;
    [Space(10)]
    public float minAgroDistance = 3f;
    public float maxAgroDistance = 4f;
    [Space(10)]
    public float closeRangeActionDistance = 1f;

    public GameObject hitParticle; 

    public LayerMask whatIsGround;
    public LayerMask whatIsPlayer;


}
