using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AggressiveWeapon : Weapon
{
    protected SO_AggressiveWeaponData agressiveWeaponData;

    private List<IDamageable> detectedDamageable = new List<IDamageable>();


    protected override void Awake()
    {
        base.Awake();

        if(weaponData.GetType() == typeof(SO_AggressiveWeaponData))
        {
            agressiveWeaponData = (SO_AggressiveWeaponData)weaponData; 
        }
        else
        {
            Debug.LogError("Wrond data for The weapon");
        }
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        checkMeleeAttack();
    }

    private void checkMeleeAttack()
    {
        WeaponAttackDetails details = agressiveWeaponData.AttackDetails[attackCounter];

        foreach (IDamageable item in detectedDamageable.ToList())
        {
            item.Damage(details.damageAmount);
        }
    }

    public void AddToDetected(Collider2D collision)
    {


        IDamageable damageable = collision.GetComponent<IDamageable>();

        if(damageable != null)
        {

            detectedDamageable.Add(damageable);
        }
    }

    public void RemoveFromDetected(Collider2D collision)
    {


        IDamageable damageable = collision.GetComponent<IDamageable>();

        if (damageable != null)
        {


            detectedDamageable.Remove(damageable);
        }
    }
}
