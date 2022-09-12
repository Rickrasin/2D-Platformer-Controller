using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : Projectile
{

    GameObject target;
    public float bulletSpeed;

    // Start is called before the first frame update
    public override void Start()
    {
        speed = bulletSpeed;


        base.Start();

        target = GameObject.FindGameObjectWithTag("Player");
        Vector2 moveDir = (target.transform.position - transform.position).normalized * bulletSpeed;
        rb.velocity = new Vector2(moveDir.x, moveDir.y);
        Destroy(this.gameObject, 2);
    }

}
