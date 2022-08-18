using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newAggressiveWeaponData", menuName = "Data/Weapon Data/Aggressive Weapon")]

public class SO_AggressiveWeaponData : SO_WeaponData
{

    [SerializeField] private WeaponAttackDetails[] attackDetails;

    public WeaponAttackDetails[] AttackDetails { get => attackDetails; set => attackDetails = value; }

    private void OnEnable()
    {
        amountOfAttacks = attackDetails.Length;

        MovementSpeed = new float[amountOfAttacks];

        for (int i = 0; i < amountOfAttacks; i++)
        {
            MovementSpeed[i] = attackDetails[i].movementSpeed;
        }
    }
}
