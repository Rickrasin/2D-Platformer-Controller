using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : CoreComponent
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;


    protected override void Awake()
    {
        base.Awake();

        currentHealth = maxHealth;
    }

    public void DecreaseHealth(float amount)
    {
        currentHealth -= amount;

        //Event: Eu devo chamar aqui o Evento de morte

        if(currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log(core.transform.parent.name + ": Health is zero!!");
        }
    }

    public void IncreaseHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0,maxHealth);
    }
}
