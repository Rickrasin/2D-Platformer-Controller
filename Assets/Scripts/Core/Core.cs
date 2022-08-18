using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    public Movement Movement
    {
        get
        {
            if(movement)
            {
            return movement;
            }

            Debug.Log("No Movement Core Component on " + transform.parent.name);
            return null;
        }

        private set{ movement = value; }

    }

    public CollisionSenses CollisionSenses
    {
        get
        {
            if (collisionSenses != null)
            {
                return collisionSenses;
            }

            Debug.Log("No Collision Senses Core Component on " + transform.parent.name);
            return null;
        }

        private set { collisionSenses = value; }
    }
    private Movement movement;
    private CollisionSenses collisionSenses;


    private void Awake()
    {
        Movement = GetComponentInChildren<Movement>();
        CollisionSenses = GetComponentInChildren<CollisionSenses>();

        
    }


    public void LogicUpdate()
    {
        Movement.LogicUpdate();
    }
}
