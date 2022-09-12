using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{


    [SerializeField]
    protected float gravity;
    [SerializeField]
    protected float damageRadius;

    protected float speed;
    protected bool isGravityOn;

    protected Rigidbody2D rb;



    // Start is called before the first frame update
    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    public virtual void FixedUpdate()
    {

    }
}
